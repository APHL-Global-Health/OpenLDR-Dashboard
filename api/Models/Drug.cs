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
		public static List<string> All(string connectionString)
		{
			var list = new List<string>();

			var connection = new SqlConnection(connectionString);
			connection.Open();

			var query = "SELECT DISTINCT CAST(LEFT([ResistantDrugs], CHARINDEX('~',[ResistantDrugs]+'~')-1) AS VARCHAR(250)) AS Drug ";
			query += "FROM [OpenLDRData].[dbo].[Monitoring] ";
			query += "WHERE [ResistantDrugs] > '' ";
			query += "ORDER BY [Drug] ";

			SqlCommand cmd = new SqlCommand(query, connection) { CommandTimeout = 0 };
			SqlDataReader dataReader = cmd.ExecuteReader();

			while (dataReader.Read())
			{
				var drug = dataReader["Drug"].ToString();
				list.Add(drug);
			}

			dataReader.Close();
			connection.Close();

			return list;
		}
		#endregion
		#endregion
	}
}
