using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace OpenLDR.Dashboard.API.Models
{
    [Serializable]
    [XmlRoot(ElementName = "OrgLevel")]
    public class OrgLevel
    {
		#region Properties 
		public string Province { get; set; }
		public string District { get; set; }
		public string Facility { get; set; }
		#endregion

		#region Constructor
		public OrgLevel()
		{
		}

		public OrgLevel(string Province, string District, string Facility)
			: this()
		{
			this.Province = Province;
			this.District = District;
			this.Facility = Facility;
			
		}
		#endregion

		#region Methods
		#region All
		public static List<OrgLevel> All(IConfigurationSection configuration, string connectionString)
		{
			var list = new List<OrgLevel>();

			var query = Core.GetQueryScript(configuration, "general_GetOrgLevel");
			if (!string.IsNullOrEmpty(query))
			{

				var connection = new SqlConnection(connectionString);
				connection.Open();

				SqlCommand cmd = new SqlCommand(query, connection) { CommandTimeout = 0 };
				SqlDataReader dataReader = cmd.ExecuteReader();

				while (dataReader.Read())
				{
					var Province = dataReader["Province"].ToString();
					var District = dataReader["District"].ToString();
					var Facility = dataReader["Facility"].ToString();
					
					list.Add(new OrgLevel(Province, District, Facility));
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
