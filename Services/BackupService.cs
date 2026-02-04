using System.Diagnostics;
using System.IO.Compression;

namespace Ans.Net10.Psql.Services
{

	public interface IBackupService
	{
		Task InitBackup(string host, string database, string userName, string password, int port);
	}


	public class BackupService
		: IBackupService
	{

		public async Task InitBackup(
			string host,
			string database,
			string userName,
			string password,
			int port)
		{
			try
			{
				var tempFile1 = Path.GetTempFileName();
				var tempPath1 = Path.GetTempPath();
				var backupName1 = $"{host}-{database.Replace(".", "_")}-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}".ToLower();
				var processStartInfo1 = new ProcessStartInfo
				{
					FileName = @"pg_dump",
					Arguments = $"--file={tempFile1} --format=t -n \"public\" --verbose --host={host} --port={port} --username={userName} {database}",
				};
				processStartInfo1.EnvironmentVariables.Add("PGPASSWORD", password);
				var result1 = Process.Start(processStartInfo1);
				result1.WaitForExit();
				result1.Close();

				if (!File.Exists(tempFile1))
					throw new Exception("[BackupService] Backup file does not exist");

				using var zip1 = ZipFile.Open($"{tempPath1}{backupName1}.zip", ZipArchiveMode.Create);
				zip1.CreateEntryFromFile(tempFile1, $"{backupName1}.backup");
				zip1.Dispose();

				var backupArchive1 = new FileInfo($"{tempPath1}{backupName1}.zip");
				//await uploadFile(backupArchive1);
				File.Delete(tempFile1);
				File.Delete($"{tempPath1}{backupName1}.zip");
				//await CleanupBackups(DaysToRetain);
			}
			catch (Exception ex)
			{
				throw new Exception("[BackupService] Error creating backup", ex);
			}
		}
		
	}

}
