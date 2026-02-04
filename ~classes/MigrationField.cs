namespace Ans.Net10.Psql
{

	[Obsolete]
	public class MigrationField
	{

		/* ctor */


		/// <param name="definition">
		/// "name1[->name2[:type]]"
		/// </param>
		public MigrationField(
			string definition)
		{
			if (string.IsNullOrEmpty(definition))
				throw new ArgumentNullException(nameof(definition));
			var a1 = definition.Split(':');
			FuncToString = _getFunc((a1.Length > 1) ? a1[1] : null);
			var a2 = a1[0].Split("->");
			Name1 = a2[0];
			Name2 = (a2.Length > 1) ? a2[1] : Name1;
		}


		/* readonly properties */


		public string Name1 { get; }
		public string Name2 { get; }
		public Func<object, string> FuncToString { get; }


		/* privates */


		private static Func<object, string> _getFunc(
			string type)
		{
			if (string.IsNullOrEmpty(type))
				return x => SuppSql.GetValueAsString(x);
			return type switch
			{
				"int" => x => SuppSql.GetValueAsIntOr0(x),
				"int?" => x => SuppSql.GetValueAsIntOrNULL(x),
				"long" => x => SuppSql.GetValueAsLongOr0(x),
				"long?" => x => SuppSql.GetValueAsLongOrNULL(x),
				"double" => x => SuppSql.GetValueAsDoubleOr0(x),
				"double?" => x => SuppSql.GetValueAsDoubleOrNULL(x),
				"float" => x => SuppSql.GetValueAsFloatOr0(x),
				"float?" => x => SuppSql.GetValueAsFloatOrNULL(x),
				"decimal" => x => SuppSql.GetValueAsDecimalOr0(x),
				"decimal?" => x => SuppSql.GetValueAsDecimalOrNULL(x),
				"datetime" => x => SuppSql.GetValueAsDateTimeOrNULL(x),
				"dateonly" => x => SuppSql.GetValueAsDateOnlyOrNULL(x),
				"timeonly" => x => SuppSql.GetValueAsTimeOnlyOrNULL(x),
				"bool" => x => SuppSql.GetValueAsBool(x),
				_ => throw new ArgumentOutOfRangeException(type)
			};
		}

	}

}