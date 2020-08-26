using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace OpenLDR.Dashboard.API.Models
{
	[Serializable]
	[XmlRoot(ElementName = "ViralLoadList")]
	public class ViralLoadList
	{
		#region Properties 
		public string Province { get; set; }

		public string District { get; set; }

		public string Facility { get; set; }

		public string Gender { get; set; }

		public string AgeGroup { get; set; }

		public int Tests { get; set; }

		public int Suppressed { get; set; }

		public int Unsuppressed { get; set; }

		public int Undetectable { get; set; }
		
		public int TATWithin14 { get; set; }

		public int Pregnant { get; set; }

		public int BreastFeeding { get; set; }

		public int BaselineVL { get; set; }


#endregion

		#region Constructor
		public ViralLoadList()
		{
		}

		public ViralLoadList(string Province, string District, string Facility, string Gender, string AgeGroup, int Tests, int Suppressed, int Unsuppressed,int Undetectable, int TATWithin14,int Pregnant, int BreastFeeding, int BaselineVL)
			: this()
		{
			this.Province = Province;
			this.District = District;
			this.Facility = Facility;
			this.Gender = Gender;
			this.AgeGroup = AgeGroup;
			this.Tests = Tests;
			this.Suppressed = Suppressed;
			this.Unsuppressed = Unsuppressed;
			this.Undetectable = Undetectable;
			this.TATWithin14 = TATWithin14;
			this.Pregnant = Pregnant;
			this.BreastFeeding = BreastFeeding;
			this.BaselineVL = BaselineVL;
		}
		#endregion

		#region Methods
		#region All
		public static List<ViralLoadList> All(IConfigurationSection configuration, string connectionString, string province, string district, DateTime? stdate, DateTime? edate)
		{

			var list = new List<ViralLoadList>();
			var query = Core.GetQueryScript(configuration, "general_getHIVVLGeo_List");
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
				//if(facility == null)
				//	cmd.Parameters.Add("@Facility", SqlDbType.VarChar).Value = DBNull.Value;
				//else cmd.Parameters.Add("@Facility", SqlDbType.VarChar).Value = facility;
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
					var Gender = dataReader["Gender"].ToString();
					var AgeGroup = dataReader["AgeGroup"].ToString();
					var Tests = dataReader.ToInt("Tests");
					var Suppressed = dataReader.ToInt("Suppressed");
					var Unsuppressed = dataReader.ToInt("Unsuppressed");
					var Undetectable = dataReader.ToInt("Undetectable");
					var TATWithin14 = dataReader.ToInt("TATWithin14");
					var Pregnant = dataReader.ToInt("Pregnant");
					var BreastFeeding = dataReader.ToInt("BreastFeeding");
					var BaselineVL = dataReader.ToInt("BaselineVL");


					list.Add(new ViralLoadList(Province, District, Facility, Gender, AgeGroup, Tests, Suppressed, Unsuppressed, Undetectable, TATWithin14, Pregnant, BreastFeeding, BaselineVL));
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
