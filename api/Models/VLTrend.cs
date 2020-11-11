using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace OpenLDR.Dashboard.API.Models
{
	[Serializable]
	[XmlRoot(ElementName = "VLTrend")]
	public class VLTrend
	{
		#region Properties 
		public string MonthID { get; set; }

		public int Tests { get; set; }

		public int Suppressed { get; set; }

		public int Undetectable { get; set; }

		public int Within14 { get; set; }

		public int Pregnant { get; set; }

		public int BreastFeeding { get; set; }

		public int Baseline { get; set; }

		#endregion

		#region Constructor
		public VLTrend()
		{
		}

		public VLTrend(string MonthID, int Tests, int Suppressed, int Undetectable, int Within14, int Pregnant, int BreastFeeding, int Baseline)
			: this()
		{
			this.MonthID = MonthID;
			this.Tests = Tests;
			this.Suppressed = Suppressed;
			this.Undetectable = Undetectable;
			
			
			this.Within14 = Within14;
			this.Pregnant = Pregnant;
			this.BreastFeeding = BreastFeeding;
			this.Baseline = Baseline;
			
			
		}
		#endregion

		#region Methods
		#region All
		public static List <VLTrend> All(IConfigurationSection configuration, string connectionString, string province, string district, string facility)
		{

			var list = new List<VLTrend>();
			var query = Core.GetQueryScript(configuration, "general_getHIVVLTrends");
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
					var Suppressed= dataReader.ToInt("Suppressed");
					
					var Undetectable = dataReader.ToInt("Undetectable");
					
					
					var Within14 = dataReader.ToInt("Within14");
					var Pregnant = dataReader.ToInt("Pregnant");
					var BreastFeeding = dataReader.ToInt("BreastFeeding");
					var Baseline = dataReader.ToInt("Baseline");
					
					


					list.Add(new VLTrend(MonthID,Tests, Suppressed,  Undetectable, Within14, Pregnant, BreastFeeding, Baseline));
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
