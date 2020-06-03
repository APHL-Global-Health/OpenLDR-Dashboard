using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace OpenLDR.Dashboard.API.Models
{
    [Serializable]
    [XmlRoot(ElementName = "Transmission")]
    public class Transmission
    {
		#region Properties 
		public string Test { get; set; }

		public string System { get; set; }

		public string TestingLab { get; set; }

		public int Date { get; set; }

		public int Month { get; set; }

		public int Year { get; set; }

		public int Received { get; set; }

		public int Registered { get; set; }

		public int Tested { get; set; }

		public int Authorised { get; set; }

		public int Rejected { get; set; }

		public int Tested_Workload { get; set; }

		public int Authorised_Workload { get; set; }
		#endregion

		#region Constructor
		public Transmission()
		{
		}

		public Transmission(string Test, string System, string TestingLab, int Date, int Month, int Year, int Received, int Registered, int Tested, int Authorised, int Rejected, int Tested_Workload, int Authorised_Workload)
			: this()
		{
			this.Test = Test;
			this.System = System;
			this.TestingLab = TestingLab;
			this.Date = Date;
			this.Month = Month;
			this.Year = Year;
			this.Received = Received;
			this.Registered = Registered;
			this.Tested = Tested;
			this.Authorised = Authorised;
			this.Rejected = Rejected;

			this.Tested_Workload = Tested_Workload;
			this.Authorised_Workload = Authorised_Workload;
		}
		#endregion

		#region Methods
		#region All
		public static List<Transmission> All(IConfigurationSection configuration, List<string> tests, int year, int month, string connectionString, bool excludeTests = false)
		{
			var list = new List<Transmission>();

			var query = Core.GetQueryScript(configuration, "general_GetTransmission");
			if (!string.IsNullOrEmpty(query))
			{

				var connection = new SqlConnection(connectionString);
				connection.Open();

				var _month = month;
				var _year = year;
				if (_month + 1 > 12)
				{
					_month = 0;
					_year += 1;
				}

				var dt = new DateTime(year, month, 1);
				var nd = dt.AddMonths(1);

				SqlCommand cmd = new SqlCommand(query, connection) { CommandTimeout = 0 };
				cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = dt;
				cmd.Parameters.Add("@EndDate", SqlDbType.Date).Value = nd;
				cmd.Parameters.Add("@ExcludeTests", SqlDbType.Bit).Value = excludeTests ? 1 : 0;
				SqlDataReader dataReader = cmd.ExecuteReader();

				while (dataReader.Read())
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

					list.Add(new Transmission(Test, System, TestingLab, Date, Month, Year, Received, Registered, Tested, Authorised, Rejected, Tested_Workload, Authorised_Workload));
				}

				dataReader.Close();
				connection.Close();
			}

			return list;
		}
		#endregion
		#endregion
	}
}
