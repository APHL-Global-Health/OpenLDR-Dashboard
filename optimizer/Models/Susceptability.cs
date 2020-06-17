using Optimizer.Logging;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Optimizer.Models
{
    public class Susceptability
    {
		#region Delete
		public static bool Delete(SqlConnection connection, string Organism, string Drug, string Class, int Date, int Month, int Year, out string error)
		{
			error = null;
			bool successful = false;
			try
			{
				var query = "DELETE FROM [OpenLDRRefs].[dbo].[Susceptability] ";
				query += "WHERE [Organism]=@Organism AND [Drug]=@Drug AND [Class]=@Class AND [Date]=@Date AND [Month]=@Month AND [Year]=@Year  ";

				SqlCommand cmd = new SqlCommand(query, connection) { CommandTimeout = 0 };
				cmd.Parameters.Add("@Organism", SqlDbType.NVarChar).Value = Organism.ToSqlValue();
				cmd.Parameters.Add("@Drug", SqlDbType.NVarChar).Value = Drug.ToSqlValue();
				cmd.Parameters.Add("@Class", SqlDbType.NVarChar).Value = Class.ToSqlValue();
				cmd.Parameters.Add("@Date", SqlDbType.Int).Value = Date.ToSqlValue();
				cmd.Parameters.Add("@Month", SqlDbType.Int).Value = Month.ToSqlValue();
				cmd.Parameters.Add("@Year", SqlDbType.Int).Value = Year.ToSqlValue();

				cmd.ExecuteNonQuery();
				cmd.Dispose();
				successful = true;
			}
			catch (Exception ex)
			{
				error = ex.Message;
			}

			return successful;
		}
		#endregion

		#region Insert
		public static bool Insert(SqlConnection connection, string Organism, string Drug, string Class, int Date, int Month, int Year, int TotalCount, int ResultCount, out string error)
		{
			error = null;
			bool successful = false;
			try
			{
				var query = "INSERT INTO [OpenLDRRefs].[dbo].[Susceptability] ([Organism] ,[Drug] ,[Class] ,[Date] ,[Month] ,[Year] ,[TotalCount] ,[ResultCount]) ";
				query += "VALUES(@Organism, @Drug, @Class, @Date, @Month, @Year, @TotalCount, @ResultCount)  ";

				SqlCommand cmd = new SqlCommand(query, connection) { CommandTimeout = 0 };
				cmd.Parameters.Add("@Organism", SqlDbType.NVarChar).Value = Organism.ToSqlValue();
				cmd.Parameters.Add("@Drug", SqlDbType.NVarChar).Value = Drug.ToSqlValue();
				cmd.Parameters.Add("@Class", SqlDbType.NVarChar).Value = Class.ToSqlValue();
				cmd.Parameters.Add("@Date", SqlDbType.Int).Value = Date.ToSqlValue();
				cmd.Parameters.Add("@Month", SqlDbType.Int).Value = Month.ToSqlValue();
				cmd.Parameters.Add("@Year", SqlDbType.Int).Value = Year.ToSqlValue();
				cmd.Parameters.Add("@TotalCount", SqlDbType.Int).Value = TotalCount.ToSqlValue();
				cmd.Parameters.Add("@ResultCount", SqlDbType.Int).Value = ResultCount.ToSqlValue();

				cmd.ExecuteNonQuery();
				cmd.Dispose();
				successful = true;
			}
			catch (Exception ex)
			{
				error = ex.Message;
			}

			return successful;
		}
		#endregion

		#region Sync
		public static bool Sync(dynamic configuration, string startdate, string enddate, string connectionString, out string error, string classIn = "Resistant~Sensitive~Intermediate")
		{
			error = null;
			var successful = false;

			var query = Core.GetQueryScript(configuration, "globalhealth_GetSusceptability");
			if (!string.IsNullOrEmpty(query))
			{
				string surveillenceCode = "DST01";
				//string classIn = "Resistant~Sensitive~Intermediate";
				string organismNotIn = "";
				string drugsNotIn = "";
				//string[] zone = null;
				//string[] region = null;
				//string[] facility = null;

				query = string.Format(query, startdate, enddate, surveillenceCode, classIn, organismNotIn, drugsNotIn);


				Core.Logger.Info("Fetching susceptability data from OpenLDR");

				var connection = new SqlConnection(connectionString);
				connection.Open();

				SqlCommand cmd = new SqlCommand(query, connection) { CommandTimeout = 0 };
				SqlDataReader dataReader = cmd.ExecuteReader();

				var con = new SqlConnection(connectionString);
				con.Open();

				while (error == null && dataReader.Read())
				{
					var Organism = dataReader["Organism"].ToString();
					var Drug = dataReader["Drug"].ToString();
					var Class = dataReader["Class"].ToString();
					var Date = dataReader.ToInt("Date");
					var Month = dataReader.ToInt("Month");
					var Year = dataReader.ToInt("Year");
					var TotalCount = dataReader.ToInt("TotalCount");
					var ResultCount = dataReader.ToInt("ResultCount");

					if (Delete(con, Organism, Drug, Class, Date, Month, Year, out error))
						if (Insert(con, Organism, Drug, Class, Date, Month, Year, TotalCount, ResultCount, out error))
							Core.Logger.Info(string.Format("Synchronizing Organism={0}, Drug={1}, TestClassingLab={2}, Date={3}, Month={4}, Year={5}", Organism, Drug, Class, Date, Month, Year));
				}
				con.Close();

				dataReader.Close();
				connection.Close();

				successful = error == null;
			}
			return successful;
		}
		#endregion
	}
}
