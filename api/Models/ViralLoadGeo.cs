using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace OpenLDR.Dashboard.API.Models
{
    [Serializable]
    [XmlRoot(ElementName = "ViralLoadGeo")]
    public class ViralLoadGeo
    {
		#region Properties 
		public string Province { get; set; }

		public string District { get; set; }

		public string Facility { get; set; }

		public int Tests { get; set; }

		public int Suppressed { get; set; }

		public int Unsuppressed { get; set; }

		public double SuppressionRate { get; set; }

		public double AverageTAT { get; set; }


		#endregion

		#region Constructor
		public ViralLoadGeo()
		{
		}

		public ViralLoadGeo(string Province, string District, string Facility, int Tests, int Suppressed, int Unsuppressed, double SuppressionRate, double AverageTAT)
			: this()
		{
			this.Province = Province;
			this.District = District;
			this.Facility = Facility;
			this.Tests = Tests;
			this.Suppressed = Suppressed;
			this.Unsuppressed = Suppressed;
			this.SuppressionRate = SuppressionRate;
			this.AverageTAT = AverageTAT;
			
		}
		#endregion

		#region Methods
		#region All
		public static ViralLoadGeo All(IConfigurationSection configuration, string connectionString, string province, string district, string facility, DateTime stdate, DateTime edate)
		{

			ViralLoadGeo obj = null;
			var query = Core.GetQueryScript(configuration, "zambia_getHIVVLByGeography");
			if (!string.IsNullOrEmpty(query))
			{

				var connection = new SqlConnection(connectionString);
				connection.Open();


				SqlCommand cmd = new SqlCommand(query, connection) { CommandTimeout = 0 };
				cmd.Parameters.Add("@Province", SqlDbType.VarChar).Value = province;
				cmd.Parameters.Add("@District", SqlDbType.VarChar).Value = district;
				cmd.Parameters.Add("@Facility", SqlDbType.VarChar).Value = facility;
				cmd.Parameters.Add("@StDate", SqlDbType.DateTime).Value = stdate;
				cmd.Parameters.Add("@EDate", SqlDbType.DateTime).Value = edate;
				SqlDataReader dataReader = cmd.ExecuteReader();

				
					var Province = dataReader["Province"].ToString();
					var District = dataReader["District"].ToString();
					var Facility = dataReader["Facility"].ToString();
					var Tests = dataReader.ToInt("Tests");
					var Suppressed = dataReader.ToInt("Suppresed");
					var Unsuppressed = dataReader.ToInt("Unsuppressed");
					var SuppressionRate = dataReader.ToDouble("SuppressionRate");
					var AverageTAT = dataReader.ToDouble("AverageTAT");
				

					obj = new ViralLoadGeo(Province, District, Facility, Tests, Suppressed, Unsuppressed, SuppressionRate, AverageTAT);
				

				dataReader.Close();
				connection.Close();
			}

			return obj;
		}
		#endregion
		#endregion
	}
}
