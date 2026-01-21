using Ans.Net10.Common;

namespace Ans.Net10.Psql
{

	public static class LibPsqlInfo
	{
		public static string GetName() => SuppApp.GetName();
		public static string GetVersion() => SuppApp.GetVersion();
		public static string GetDescription() => SuppApp.GetDescription();
	}

}
