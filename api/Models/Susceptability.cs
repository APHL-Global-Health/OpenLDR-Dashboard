using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace OpenLDR.Dashboard.API
{
	[Serializable]
	[XmlRoot(ElementName = "Susceptibility")]
	public class Susceptability
    {
		#region Properties 
		public string Organism { get; set; }
		public string Drug { get; set; }
		public string Class { get; set; }
		public int TotalCount { get; set; }
		public int ResultCount { get; set; }
		#endregion

		#region Constructor
		public Susceptability()
		{
		}

		public Susceptability(string Organism, string Drug, string Class, int TotalCount, int ResultCount)
			: this()
		{
			this.Organism = Organism;
			this.Drug = Drug;
			this.Class = Class;
			this.TotalCount = TotalCount;
			this.ResultCount = ResultCount;
		}
		#endregion

		#region Methods
		#region All
		public static List<Susceptability> All(IConfigurationSection configuration, string startDate, string endDate, string connectionString, 
			string surveillenceCode, string classIn, 
			string organismNotIn, string drugsNotIn,
            string[] zone, string[] region, string[] facility)
		{
			var list = new List<Susceptability>();

            var tsql = Core.GetQueryScript(configuration, "globalhealth_GetSusceptability");
			if (!string.IsNullOrEmpty(tsql))
			{
				tsql = string.Format(tsql, startDate, endDate, surveillenceCode, classIn, organismNotIn, drugsNotIn);

				var connection = new SqlConnection(connectionString);
				connection.Open();

				var cmd = new SqlCommand(tsql, connection) { CommandTimeout = 0 };
				var dataReader = cmd.ExecuteReader();
				while (dataReader.Read())
				{
					list.Add(new Susceptability()
					{
						Organism = dataReader["Organism"].ToString(),
						Drug = dataReader["Drug"].ToString(),
						Class = dataReader["Class"].ToString(),
						TotalCount = int.Parse(dataReader["TotalCount"].ToString()),
						//Ratio = double.Parse(dataReader["Ratio"].ToString())
					});
				}
				connection.Close();
			}

			return list;
		}
		#endregion
		#endregion
	}
}
