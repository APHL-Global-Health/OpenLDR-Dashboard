using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace OpenLDR.Dashboard.API.Models
{
	[Serializable]
	[XmlRoot(ElementName = "EIDTrend")]
	public class EIDTrend
	{
		#region Properties 
		public string MonthID { get; set; }

		public int Tests { get; set; }

		public int Positive { get; set; }

		public int Negative { get; set; }

		public int Within14 { get; set; }

		

		#endregion

		#region Constructor
		public EIDTrend()
		{
		}

		public EIDTrend(string MonthID, int Tests, int Positive, int Negative, int Within14)
			: this()
		{
			this.MonthID = MonthID;
			this.Tests = Tests;
			this.Positive = Positive;
			this.Negative = Negative;
			this.Within14 = Within14;
		}
		#endregion

		#region Methods
		#region All
		public static List <EIDTrend> All(IConfigurationSection configuration, string connectionString, string province, string district, string facility)
		{

			var list = new List<EIDTrend>();
			var query = Core.GetQueryScript(configuration, "general_getHIVPCTrends");
			if (!string.IsNullOrEmpty(query))
			{

				var connection = new SqlConnection(connectionString);
				connection.Open();


				SqlCommand cmd = new SqlCommand(query, connection) { CommandTimeout = 0 };
				if (province == null)
					cmd.Parameters.Add("@Province", SqlDbType.VarChar).Value = DBNull.Value; 
				else cmd.Parameters.Add("@Province", SqlDbType.VarChar).Value = province;
				if (district == null)
					cmd.Parameters.Add("@District", SqlDbType.VarChar).Value = DBNull.Value;
				else cmd.Parameters.Add("@District", SqlDbType.VarChar).Value = district;
				if(facility == null)
					cmd.Parameters.Add("@Facility", SqlDbType.VarChar).Value = DBNull.Value;
				else cmd.Parameters.Add("@Facility", SqlDbType.VarChar).Value = facility;
				
				SqlDataReader dataReader = cmd.ExecuteReader();

				while (dataReader.Read())
				{
					var MonthID = dataReader["MonthID"].ToString();
					
					var Tests = dataReader.ToInt("Tests");
					var Positive= dataReader.ToInt("Positive");
					
					var Negative = dataReader.ToInt("Negative");
					
					
					var Within14 = dataReader.ToInt("Within14");
					
					
					


					list.Add(new EIDTrend(MonthID,Tests, Positive,  Negative, Within14));
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
