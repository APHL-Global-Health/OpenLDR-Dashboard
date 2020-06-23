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

		public int Undetectable { get; set; }
		public double SuppressionRate { get; set; }

		public double AverageTAT { get; set; }

		public int lt01 { get; set; }

		public int _1to4 { get; set; }

		public int _5to9 { get; set; }

		public int _10to14 { get; set; }

		public int _15to19 { get; set; }

		public int _20to24 { get; set; }

		public int _25to29 { get; set; }

		public int _30to34 { get; set; }

		public int _35to39 { get; set; }

		public int _40to44 { get; set; }

		public int _45to49 { get; set; }

		public int _50plus { get; set; }


		#endregion

		#region Constructor
		public ViralLoadGeo()
		{
		}

		public ViralLoadGeo(string Province, string District, string Facility, int Tests, int Suppressed, int Unsuppressed,int Undetectable, double SuppressionRate, double AverageTAT, int lt01, int _1to4, int _5to9, int _10to14, int _15to19, int _20to24, int _25to29, int _30to34, int _35to39, int _40to44, int _45to49, int _50plus)
			: this()
		{
			this.Province = Province;
			this.District = District;
			this.Facility = Facility;
			this.Tests = Tests;
			this.Suppressed = Suppressed;
			this.Unsuppressed = Unsuppressed;
			this.Undetectable = Undetectable;
			this.SuppressionRate = SuppressionRate;
			this.AverageTAT = AverageTAT;
			this.lt01 = lt01;
			this._1to4 = _1to4;
			this._5to9 = _5to9;
			this._10to14 = _10to14;
			this._15to19 = _15to19;
			this._20to24 = _20to24;
			this._25to29 = _25to29;
			this._30to34 = _30to34;
			this._35to39 = _35to39;
			this._40to44 = _40to44;
			this._45to49 = _45to49;
			this._50plus = _50plus;
		}
		#endregion

		#region Methods
		#region All
		public static ViralLoadGeo All(IConfigurationSection configuration, string connectionString, string province, string district, string facility, DateTime? stdate, DateTime? edate)
		{

			ViralLoadGeo obj = null;
			var query = Core.GetQueryScript(configuration, "general_getHIVVLByGeography");
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
				if ((stdate == DateTime.MinValue) || (edate == DateTime.MinValue))
				{
					cmd.Parameters.Add("@StDate", SqlDbType.DateTime).Value = DBNull.Value;
					cmd.Parameters.Add("@EDate", SqlDbType.DateTime).Value = DBNull.Value;
				}
				else
				{
					cmd.Parameters.Add("@StDate", SqlDbType.DateTime).Value = stdate;
					cmd.Parameters.Add("@EDate", SqlDbType.DateTime).Value = edate;
				}
				SqlDataReader dataReader = cmd.ExecuteReader();

				while (dataReader.Read())
				{
					var Province = dataReader["Province"].ToString();
					var District = dataReader["District"].ToString();
					var Facility = dataReader["Facility"].ToString();
					var Tests = dataReader.ToInt("Tests");
					var Suppressed = dataReader.ToInt("Suppressed");
					var Unsuppressed = dataReader.ToInt("Unsuppressed");
					var Undetectable = dataReader.ToInt("Undetectable");
					var SuppressionRate = dataReader.ToDouble("SuppressionRate");
					var AverageTAT = dataReader.ToDouble("AverageTAT");
					var lt01 = dataReader.ToInt("lt01");
					var _1to4 = dataReader.ToInt("1to4");
					var _5to9 = dataReader.ToInt("5to9");
					var _10to14 = dataReader.ToInt("10to14");
					var _15to19= dataReader.ToInt("15to19");
					var _20to24 = dataReader.ToInt("20to24");
					var _25to29 = dataReader.ToInt("25to29");
					var _30to34 = dataReader.ToInt("30to34");
					var _35to39 = dataReader.ToInt("35to39");
					var _40to44 = dataReader.ToInt("40to44");
					var _45to49 = dataReader.ToInt("45to49");
					var _50plus = dataReader.ToInt("50plus");


					obj = new ViralLoadGeo(Province, District, Facility, Tests, Suppressed, Unsuppressed, Undetectable, SuppressionRate, AverageTAT,lt01,_1to4,_5to9,_10to14,_15to19,_20to24,_25to29,_30to34,_35to39,_40to44,_45to49,_50plus);
				}

				dataReader.Close();
				connection.Close();
			}

			return obj;
		}
		#endregion
		#endregion
	}
}
