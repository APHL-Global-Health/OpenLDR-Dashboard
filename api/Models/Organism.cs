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
		public static List<string> All(string connectionString)
		{
			var list = new List<string>();

			var connection = new SqlConnection(connectionString);
			connection.Open();

			var query = "SELECT DISTINCT [ORGANISM] ";
			query += "FROM [OpenLDRData].[dbo].[Monitoring] ";
			query += "WHERE [ORGANISM] <> '' ";
			query += " ORDER BY [ORGANISM] ";

			SqlCommand cmd = new SqlCommand(query, connection) { CommandTimeout = 0 };
			SqlDataReader dataReader = cmd.ExecuteReader();

			while (dataReader.Read())
			{
				var organism = dataReader["ORGANISM"].ToString();
				list.Add(organism);
			}

			dataReader.Close();
			connection.Close();

			return list;
		}
		#endregion
		#endregion
	}
}
