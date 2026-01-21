using Ans.Net10.Common;
using System.Globalization;

namespace Ans.Net10.Psql
{

	public static class SuppSql
	{

		/* functions */


		public static string GetPsqlSerialSequence(
			string table,
			string key)
		{
			return
				$"select setval(pg_get_serial_sequence('public.\"{table}\"', '{key}'), " +
				$"(select max(\"{key}\") from public.\"{table}\"));";
		}


		// string


		public static string GetValue(
			string value)
		{
			if (string.IsNullOrEmpty(value))
				return "NULL";
			string s1 = value.Replace("'", "''");
			return $"N'{s1}'";
		}


		public static string GetValueAsString(
			object value)
		{
			return GetValue(value.ToString());
		}


		// int


		public static string GetValue(
			int value)
		{
			return value.ToString();
		}


		public static string GetValue(
			int? value)
		{
			if (value == null)
				return "NULL";
			return GetValue(value.Value);
		}


		public static string GetValueAsIntOrNULL(
			object value)
		{
			return GetValue(
				value?.ToString().ToInt());
		}


		public static string GetValueAsIntOr0(
			object value)
		{
			return GetValue(
				value?.ToString().ToInt(0));
		}


		// long


		public static string GetValue(
			long value)
		{
			return value.ToString();
		}


		public static string GetValue(
			long? value)
		{
			if (value == null)
				return "NULL";
			return GetValue(value.Value);
		}


		public static string GetValueAsLongOrNULL(
			object value)
		{
			return GetValue(
				value?.ToString().ToLong());
		}


		public static string GetValueAsLongOr0(
			object value)
		{
			return GetValue(
				value?.ToString().ToLong(0));
		}


		// float


		public static string GetValue(
			float value)
		{
			return value.ToString(
				CultureInfo.InvariantCulture);
		}


		public static string GetValue(
			float? value)
		{
			if (value == null)
				return "NULL";
			return GetValue(value.Value);
		}


		public static string GetValueAsFloatOrNULL(
			object value)
		{
			return GetValue(
				value?.ToString().ToFloat());
		}


		public static string GetValueAsFloatOr0(
			object value)
		{
			return GetValue(
				value?.ToString().ToFloat(0));
		}


		// double


		public static string GetValue(
			double value)
		{
			return value.ToString(
				CultureInfo.InvariantCulture);
		}


		public static string GetValue(
			double? value)
		{
			if (value == null)
				return "NULL";
			return GetValue(value.Value);
		}


		public static string GetValueAsDoubleOrNULL(
			object value)
		{
			return GetValue(
				value?.ToString().ToDouble());
		}


		public static string GetValueAsDoubleOr0(
			object value)
		{
			return GetValue(
				value?.ToString().ToDouble(0));
		}


		// decimal


		public static string GetValue(
			decimal value)
		{
			return value.ToString(
				CultureInfo.InvariantCulture);
		}


		public static string GetValue(
			decimal? value)
		{
			if (value == null)
				return "NULL";
			return GetValue(value.Value);
		}


		public static string GetValueAsDecimalOrNULL(
			object value)
		{
			return GetValue(
				value?.ToString().ToDecimal());
		}


		public static string GetValueAsDecimalOr0(
			object value)
		{
			return GetValue(value.ToString().ToDecimal(0));
		}


		// DateTime


		public static string GetValue(
			DateTime value)
		{
			string s1 = value.ToString("yyyy-MM-dd HH:mm:ss.000");
			return $"'{s1}'";
			//return $"CAST(N'{s1}' AS DateTime)";
		}


		public static string GetValue(
			DateTime? value)
		{
			if (value == null)
				return "NULL";
			return GetValue(value.Value);
		}


		public static string GetValueAsDateTimeOrNULL(
			object value)
		{
			if (value == null)
				return "NULL";
			var v1 = value.ToString().ToDateTime();
			return GetValue(v1);
		}


		// DateOnly


		public static string GetValue(
			DateOnly value)
		{
			string s1 = value.ToString("yyyy-MM-dd");
			return $"'{s1}'";
		}


		public static string GetValue(
			DateOnly? value)
		{
			if (value == null)
				return "NULL";
			return GetValue(value.Value);
		}


		public static string GetValueAsDateOnlyOrNULL(
			object value)
		{
			if (value == null)
				return "NULL";
			var v1 = value.ToString().ToDateOnly();
			return GetValue(v1);
		}


		// TimeOnly


		public static string GetValue(
			TimeOnly value)
		{
			string s1 = value.ToString("HH:mm:ss.000");
			return $"'{s1}'";
		}


		public static string GetValue(
			TimeOnly? value)
		{
			if (value == null)
				return "NULL";
			return GetValue(value.Value);
		}


		public static string GetValueAsTimeOnlyOrNULL(
			object value)
		{
			if (value == null)
				return "NULL";
			var v1 = value.ToString().ToTimeOnly();
			return GetValue(v1);
		}


		// bool


		public static string GetValue(
			bool value)
		{
			if (value)
				return "'t'";
			return "'f'";
		}


		public static string GetValueAsBool(
			object value)
		{
			return GetValue((bool)value);
		}

	}

}