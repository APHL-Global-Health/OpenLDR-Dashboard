using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace OpenLDR.Dashboard.API
{
    [Serializable]
	[XmlRoot(ElementName = "Organism")]
	public class Organism
    {
		#region Methods
		#region All
		public static List<string> All(IConfigurationSection configuration, string connectionString)
		{
			var list = new List<string>();

			var query = Core.GetQueryScript(configuration, "globalhealth_GetOrganisms");
			if (!string.IsNullOrEmpty(query))
			{
				var connection = new SqlConnection(connectionString);
				connection.Open();


				SqlCommand cmd = new SqlCommand(query, connection) { CommandTimeout = 0 };
				SqlDataReader dataReader = cmd.ExecuteReader();

				while (dataReader.Read())
				{
					var organism = dataReader["ORGANISM"].ToString();
					list.Add(organism);
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
