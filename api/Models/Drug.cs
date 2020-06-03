using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace OpenLDR.Dashboard.API
{
    [Serializable]
	[XmlRoot(ElementName = "Drug")]
	public class Drug
    {
		#region Methods
		#region All
		public static List<string> All(IConfigurationSection configuration, string connectionString)
		{
			var list = new List<string>();

			var query = Core.GetQueryScript(configuration, "globalhealth_GetDrugs");
			if (!string.IsNullOrEmpty(query))
			{
				var connection = new SqlConnection(connectionString);
				connection.Open();


				SqlCommand cmd = new SqlCommand(query, connection) { CommandTimeout = 0 };
				SqlDataReader dataReader = cmd.ExecuteReader();

				while (dataReader.Read())
				{
					var drug = dataReader["Drug"].ToString();
					list.Add(drug);
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
