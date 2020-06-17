using Optimizer.Logging;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Optimizer.Models
{
    public class Transmission
    {
		#region Delete
		public static bool Delete(SqlConnection connection, string Test, string System, string TestingLab, int Date, int Month, int Year, out string error)
		{
			error = null;
			bool successful = false;
			try
			{
				var query = "DELETE FROM [OpenLDRRefs].[dbo].[Transmission] ";
					query += "WHERE [Test]=@Test AND [System]=@System AND [TestingLab]=@TestingLab AND [Date]=@Date AND [Month]=@Month AND [Year]=@Year  ";

				SqlCommand cmd = new SqlCommand(query, connection) { CommandTimeout = 0 };
				cmd.Parameters.Add("@Test", SqlDbType.NVarChar).Value = Test.ToSqlValue();
				cmd.Parameters.Add("@System", SqlDbType.NVarChar).Value = System.ToSqlValue();
				cmd.Parameters.Add("@TestingLab", SqlDbType.NVarChar).Value = TestingLab.ToSqlValue();
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
		public static bool Insert(SqlConnection connection, string Test, string System, string TestingLab, int Date, int Month, int Year, int Received, int Registered, int Tested, int Authorised, int Rejected, int Tested_Workload, int Authorised_Workload, out string error)
		{
			error = null;
			bool successful = false;
			try
			{
				var query = "INSERT INTO [OpenLDRRefs].[dbo].[Transmission] ([Test] ,[System] ,[TestingLab] ,[Date] ,[Month] ,[Year] ,[Received] ,[Registered] ,[Tested] ,[Authorised] ,[Rejected] ,[Tested_Workload] ,[Authorised_Workload]) ";
					query += "VALUES(@Test, @System, @TestingLab, @Date, @Month, @Year, @Received, @Registered, @Tested, @Authorised, @Rejected, @Tested_Workload, @Authorised_Workload)  ";

				SqlCommand cmd = new SqlCommand(query, connection) { CommandTimeout = 0 };
				cmd.Parameters.Add("@Test", SqlDbType.NVarChar).Value = Test.ToSqlValue();
				cmd.Parameters.Add("@System", SqlDbType.NVarChar).Value = System.ToSqlValue();
				cmd.Parameters.Add("@TestingLab", SqlDbType.NVarChar).Value = TestingLab.ToSqlValue();
				cmd.Parameters.Add("@Date", SqlDbType.Int).Value = Date.ToSqlValue();
				cmd.Parameters.Add("@Month", SqlDbType.Int).Value = Month.ToSqlValue();
				cmd.Parameters.Add("@Year", SqlDbType.Int).Value = Year.ToSqlValue();
				cmd.Parameters.Add("@Received", SqlDbType.Int).Value = Received.ToSqlValue();
				cmd.Parameters.Add("@Registered", SqlDbType.Int).Value = Registered.ToSqlValue();
				cmd.Parameters.Add("@Tested", SqlDbType.Int).Value = Tested.ToSqlValue();
				cmd.Parameters.Add("@Authorised", SqlDbType.Int).Value = Authorised.ToSqlValue();
				cmd.Parameters.Add("@Rejected", SqlDbType.Int).Value = Rejected.ToSqlValue();
				cmd.Parameters.Add("@Tested_Workload", SqlDbType.Int).Value = Tested_Workload.ToSqlValue();
				cmd.Parameters.Add("@Authorised_Workload", SqlDbType.Int).Value = Authorised_Workload.ToSqlValue();

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
		public static bool Sync(dynamic configuration, string startdate, string enddate, string connectionString, out string error, bool excludeTests = false)
        {
            error = null;
            var successful = false;

            var query = Core.GetQueryScript(configuration, "general_GetTransmission");
            if (!string.IsNullOrEmpty(query))
            {
				Core.Logger.Info(string.Format("Fetching transmission data from OpenLDR : {0}", excludeTests ? "Other tests" : "Specific tests"));

				var connection = new SqlConnection(connectionString);
				connection.Open();

				SqlCommand cmd = new SqlCommand(query, connection) { CommandTimeout = 0 };
				cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = startdate;
				cmd.Parameters.Add("@EndDate", SqlDbType.Date).Value = enddate;
				cmd.Parameters.Add("@ExcludeTests", SqlDbType.Bit).Value = excludeTests ? 1 : 0;
				SqlDataReader dataReader = cmd.ExecuteReader();

				var con = new SqlConnection(connectionString);
				con.Open();

				while (error == null && dataReader.Read())
				{
					var Test = dataReader["Test"].ToString();
					var System = dataReader["System"].ToString();
					var TestingLab = dataReader["TestingLab"].ToString();
					var Date = dataReader.ToInt("Date");
					var Month = dataReader.ToInt("Month");
					var Year = dataReader.ToInt("Year");
					var Received = dataReader.ToInt("Received");
					var Registered = dataReader.ToInt("Registered");
					var Tested = dataReader.ToInt("Tested");
					var Authorised = dataReader.ToInt("Authorised");
					var Rejected = dataReader.ToInt("Rejected");

					var Tested_Workload = dataReader.ToInt("Tested_Workload");
					var Authorised_Workload = dataReader.ToInt("Authorised_Workload");

					if (!TestingLab.ToLower().StartsWith("unknown"))
                    {
						if (Delete(con, Test, System, TestingLab, Date, Month, Year, out error))
							if (Insert(con, Test, System, TestingLab, Date, Month, Year, Received, Registered, Tested, Authorised, Rejected, Tested_Workload, Authorised_Workload, out error))
								Core.Logger.Info(string.Format("Synchronizing Test={0}, System={1}, TestingLab={2}, Date={3}, Month={4}, Year={5}, ExcludeTests={6}", Test, System, TestingLab, Date, Month, Year, excludeTests ? "Other tests" : "Specific tests"));
					}
					else Core.Logger.Info(string.Format("Skipped Test={0}, System={1}, TestingLab={2}, Date={3}, Month={4}, Year={5}, ExcludeTests={6}", Test, System, TestingLab, Date, Month, Year, excludeTests ? "Other tests" : "Specific tests"));
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
