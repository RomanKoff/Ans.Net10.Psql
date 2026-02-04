using Ans.Net10.Common;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Ans.Net10.Psql
{

	public static partial class _e
	{

		/* methods */


		[System.Diagnostics.CodeAnalysis.SuppressMessage(
			"Security", "EF1003:Risk of vulnerability to SQL injection.", Justification = "<Pending>")]
		public static void SqlReassignOwned(
			this DbContext context,
			string owner)
		{
			_ = context.Database.ExecuteSqlRaw(
				$"reassign owned by CURRENT_ROLE" +
				$" to \"{owner}\";");
			_ = context.SaveChanges();
			Debug.WriteLine($"SqlReassignOwned(\"{owner}\")");
		}


		[System.Diagnostics.CodeAnalysis.SuppressMessage(
			"Security", "EF1003:Risk of vulnerability to SQL injection.", Justification = "<Pending>")]
		public static void SqlAlterTableOwnerTo(
			this DbContext context,
			string table,
			string owner)
		{
			_ = context.Database.ExecuteSqlRaw(
				$"alter table \"{table}\"" +
				$" owner to \"{owner}\";");
			_ = context.SaveChanges();
			Debug.WriteLine($"SqlAlterTableOwnerTo(\"{table}\", \"{owner}\")");
		}


		/* functions */


		[System.Diagnostics.CodeAnalysis.SuppressMessage(
			"Security", "EF1003:Risk of vulnerability to SQL injection.", Justification = "<Pending>")]
		public static int SqlClearTable(
			this DbContext context,
			string table)
		{
			_ = context.Database.ExecuteSqlRaw(
				$"delete from \"{table}\";" +
				$"select setval(pg_get_serial_sequence('public.\"{table}\"', 'Id'), 1, 'f');");
			var count1 = context.SaveChanges();
			Debug.WriteLine($"SqlClearTable(\"{table}\")");
			return count1;
		}


		public static (string, DbContext) GetTableNameAndContext<T>(
			this DbSet<T> dbSet)
			where T : class
		{
			var context1 = dbSet.GetDbContext();
			var type1 = context1.Model.FindEntityType(typeof(T));
			return (type1.GetTableName(), context1);
		}


		public static int SqlClearTable<T>(
			this DbSet<T> dbSet)
			where T : class
		{
			var (name1, context1) = dbSet.GetTableNameAndContext();
			return context1.SqlClearTable(name1);
		}


		public static int SerialSequenceSetMax<T>(
			this DbSet<T> dbSet)
			where T : class
		{
			var (name1, context1) = dbSet.GetTableNameAndContext();
			return context1.SerialSequenceSetMax(name1);
		}


		public static int Migration<TSource, TTarget>(
			this DbSet<TTarget> target,
			IEnumerable<TSource> source,
			Func<TSource, TTarget> newItem,
			int bufferCount = 999,
			Func<TSource, string> funcDebug = null)
			where TSource : class
			where TTarget : class
		{
			try
			{
				var (name1, db1) = target.GetTableNameAndContext();
				Console.Write($"Migration [{name1}] — ");
				if (!(source?.Count() > 0))
				{
					Console.WriteLine("the source is empty.");
					return 0;
				}				
				var count1 = 0;
				var c1 = source.Count();
				var i1 = bufferCount;
				foreach (var item1 in source)
				{
					target.Add(newItem(item1));
					if (--i1 < 1)
					{
						count1 += db1.SaveChanges();
						i1 = bufferCount;
					}
					if (funcDebug != null)
					{
						Console.WriteLine($"{name1}: {c1--} {funcDebug(item1)}  ");
						//SuppConsole.CursorRestopePos();
					}
				}
				count1 += db1.SaveChanges();
				db1.SerialSequenceSetMax($"{name1}");
				Console.WriteLine(
					SuppLangEn.GetDeclineEn(
						"{0} {1} added.",
						target.Count(),
						"entity",
						"entites"));
				//Console.CursorVisible = true;
				return count1;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"error {ex.Message}.");
				return -1;
			}
		}


		public static int MigrationIfNotAny<TSource, TTarget>(
			this DbSet<TTarget> target,
			IEnumerable<TSource> source,
			Func<TSource, TTarget> newItem,
			int bufferCount = 500)
			where TSource : class
			where TTarget : class
		{
			if (target.Any())
			{
				var (name1, _) = target.GetTableNameAndContext();
				Console.Write($"Migration [{name1}] — ");
				Console.WriteLine(SuppLangEn.GetDeclineEn(
					"already contains {0} {1}.",
					target.Count(),
					"entity",
					"entites"));
				return 0;
			}
			return target.Migration(source, newItem, bufferCount);
		}


		//public static int Migration<T>(
		//	this DbSet<T> dbSet,
		//	IEnumerable<object> items,
		//	string fieldsDefinitions)
		//	where T : class
		//{
		//	var (name1, context1) = dbSet.GetTableNameAndContext();
		//	var helper1 = new MigrationTableHelper(name1, fieldsDefinitions);
		//	foreach (var item1 in items)
		//		helper1.AddInsert(item1);
		//	var count1 = context1.ExecuteSqlRawIfPresent(helper1.Sql.ToString());
		//	_ = context1.SerialSequenceSetMax(name1);
		//	return count1;
		//}


		public static int ExecuteSqlRawIfPresent(
			this DbContext context,
			string sql)
		{
			return (string.IsNullOrEmpty(sql))
				? 0 : context.Database.ExecuteSqlRaw(sql);
		}


		public static int SerialSequenceSetMax(
			this DbContext context,
			string table)
		{
			return context.Database.ExecuteSqlRaw(
				SuppSql.GetPsqlSerialSequence(table, "Id"));
		}


		public static int CreateFunction_DateUpdate(
			this DbContext context)
		{
			var sql1 = @"
CREATE OR REPLACE FUNCTION func_dateupdate()
RETURNS TRIGGER AS $$
BEGIN
   NEW.""DateUpdate"" = LOCALTIMESTAMP; 
   RETURN NEW;
END;
$$ language 'plpgsql';";
			return context.Database.ExecuteSqlRaw(sql1);
		}


		public static int CreateTrigger_DateUpdate(
			this DbContext context,
			string table)
		{
			var sql1 = @$"
CREATE OR REPLACE TRIGGER trigger_{table}_dateupdate
BEFORE UPDATE ON public.""{table}""
FOR EACH ROW EXECUTE PROCEDURE func_dateupdate();
";
			return context.Database.ExecuteSqlRaw(sql1);
		}

	}

}
