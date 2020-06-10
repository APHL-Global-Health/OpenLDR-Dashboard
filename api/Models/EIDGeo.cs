using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace OpenLDR.Dashboard.API.Models
{
    [Serializable]
    [XmlRoot(ElementName = "EIDGeo")]
    public class EIDGeo
    {
		#region Properties 
		public string Province { get; set; }

		public string District { get; set; }

		public string Facility { get; set; }

		public int Tests { get; set; }

		public int Positive { get; set; }

		public int Negative { get; set; }

		public double PositivityRate { get; set; }

		public double AverageTAT { get; set; }

		#endregion

		#region Constructor
		public EIDGeo()
		{
		}

		public EIDGeo(string Province, string District, string Facility, int Tests, int Positive, int Negative, double PositivityRate, double AverageTAT)
			: this()
		{
			this.Province = Province;
			this.District = District;
			this.Facility = Facility;
			this.Tests = Tests;
			this.Positive = Positive;
			this.Negative = Negative;
			this.PositivityRate = PositivityRate;
			this.AverageTAT = AverageTAT;
			
		}
		#endregion

		#region Methods
		#region All
		public static EIDGeo All(IConfigurationSection configuration, string connectionString, string province, string district, string facility, DateTime stdate, DateTime edate)
		{
			EIDGeo obj = null;

			var query = Core.GetQueryScript(configuration, "zambia_getEIDGeo");
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
					var Positive = dataReader.ToInt("Positive");
					var Negative = dataReader.ToInt("Negative");
					var PositivityRate = dataReader.ToInt("PositivityRate");
					var AverageTAT = dataReader.ToInt("AverageTAT");
					

					obj = new EIDGeo(Province, District, Facility, Tests, Positive, Negative, PositivityRate, AverageTAT);
				

				dataReader.Close();
				connection.Close();
			}

			return obj;
		}
		#endregion
		#endregion
	}
}
