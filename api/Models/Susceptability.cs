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
		public int ResultCount { get; set; }
		public int TotalCount { get; set; }
		public double Ratio { get; set; }
		#endregion

		#region Constructor
		public Susceptability()
		{
		}

		public Susceptability(string Organism, string Drug, string Class, int ResultCount, int TotalCount, double Ratio)
			: this()
		{
			this.Organism = Organism;
			this.Drug = Drug;
			this.Class = Class;
			this.ResultCount = ResultCount;
			this.TotalCount = TotalCount;
			this.Ratio = Ratio;
		}
		#endregion

		#region Methods
		#region All
		public static List<Susceptability> All(string startDate, string endDate, string connectionString, 
			string surveillenceCode, string classIn, 
			string organismNotIn, string drugsNotIn,
            string[] zone, string[] region, string[] facility)
		{
			var list = new List<Susceptability>();

            var tsql = "WITH Resistant(RequestID, OBRSetID, OBXSetID, OBXSubID, ORGANISM, Drug, ResistantDrugs, IsRow) AS ( ";
            tsql += "	SELECT  ";
            tsql += "		A.RequestID ";
            tsql += "		, A.OBRSetID ";
            tsql += "		, OBXSetID ";
            tsql += "		, OBXSubID ";
            tsql += "		, ORGANISM ";
            tsql += "		, CAST(LEFT(ResistantDrugs, CHARINDEX('~',ResistantDrugs+'~')-1) AS VARCHAR(250)) ";
            tsql += "		, CAST(STUFF(ResistantDrugs, 1, CHARINDEX('~',ResistantDrugs+'~'), '') AS VARCHAR(250)) ";
            tsql += "		, 1  ";
            tsql += "	FROM [OpenLDRData].[dbo].[Monitoring] A ";
            tsql += "	JOIN [OpenLDRData].[dbo].[Requests] B ";
            tsql += "	ON A.RequestID = B.RequestID  ";
            tsql += "	AND A.OBRSetID = B.OBRSetID ";
            //tsql += "	JOIN [OpenLDRData].[dbo].[Facilities] F  ";
            //tsql += "	ON B.RequestingFacilityCode COLLATE DATABASE_DEFAULT = 'DISA' + F.FacilityCode ";
            tsql += "	WHERE SurveillanceCode LIKE '{2}' ";
            tsql += "	AND DATEADD(dd,0, DATEDIFF(dd,0, B.LIMSDateTimeStamp)) BETWEEN '{0}' AND '{1}' ";
            //tsql += "	AND({6} = 0 OR F." + CountryConfigSettings.HierarchyRoles[1].Column1 + " IN({7})) ";
            //tsql += "	AND({8} = 0 OR F." + CountryConfigSettings.HierarchyRoles[2].Column1 + " IN({9})) ";
            //tsql += "	AND({10} = 0 OR F." + CountryConfigSettings.HierarchyRoles[3].Column1 + " IN({11})) ";
            tsql += "	UNION ALL ";
            tsql += "	SELECT  ";
            tsql += "		  RequestID ";
            tsql += "		, OBRSetID ";
            tsql += "		, OBXSetID ";
            tsql += "		, OBXSubID ";
            tsql += "		, ORGANISM ";
            tsql += "		, CAST(LEFT(ResistantDrugs, CHARINDEX('~',ResistantDrugs+'~')-1) AS VARCHAR(250)) ";
            tsql += "		, CAST(STUFF(ResistantDrugs, 1, CHARINDEX('~',ResistantDrugs+'~'), '') AS VARCHAR(250)) ";
            tsql += "		, 0 ";
            tsql += "	FROM [Resistant] ";
            tsql += "	WHERE ResistantDrugs > '' ";
            tsql += "), ";

            tsql += "[Intermediate](RequestID, OBRSetID, OBXSetID, OBXSubID, ORGANISM, Drug, IntermediateDrugs, IsRow) AS ( ";
            tsql += "	SELECT  ";
            tsql += "		  A.RequestID ";
            tsql += "		, A.OBRSetID ";
            tsql += "		, OBXSetID ";
            tsql += "		, OBXSubID ";
            tsql += "		, ORGANISM ";
            tsql += "		, CAST(LEFT(IntermediateDrugs, CHARINDEX('~',IntermediateDrugs+'~')-1) AS VARCHAR(250)) ";
            tsql += "		, CAST(STUFF(IntermediateDrugs, 1, CHARINDEX('~',IntermediateDrugs+'~'), '') AS VARCHAR(250)) ";
            tsql += "		, 1 ";
            tsql += "	FROM [OpenLDRData].[dbo].[Monitoring] A ";
            tsql += "	JOIN [OpenLDRData].[dbo].[Requests] B ";
            tsql += "	ON A.RequestID = B.RequestID  ";
            tsql += "	AND A.OBRSetID = B.OBRSetID ";
            //tsql += "	JOIN [OpenLDRData].[dbo].[Facilities] F  ";
            //tsql += "	ON B.RequestingFacilityCode COLLATE DATABASE_DEFAULT = 'DISA' + F.FacilityCode ";
            tsql += "	WHERE SurveillanceCode LIKE '{2}' ";
            tsql += "	AND DATEADD(dd,0, DATEDIFF(dd,0, B.LIMSDateTimeStamp)) BETWEEN '{0}' AND '{1}' ";
            //tsql += "	AND({6} = 0 OR F." + CountryConfigSettings.HierarchyRoles[1].Column1 + " IN({7})) ";
            //tsql += "	AND({8} = 0 OR F." + CountryConfigSettings.HierarchyRoles[2].Column1 + " IN({9})) ";
            //tsql += "	AND({10} = 0 OR F." + CountryConfigSettings.HierarchyRoles[3].Column1 + " IN({11})) ";
            tsql += "	UNION ALL ";
            tsql += "	SELECT  ";
            tsql += "		RequestID ";
            tsql += "		, OBRSetID ";
            tsql += "		, OBXSetID ";
            tsql += "		, OBXSubID ";
            tsql += "		, ORGANISM ";
            tsql += "		, CAST(LEFT(IntermediateDrugs, CHARINDEX('~',IntermediateDrugs+'~')-1) AS VARCHAR(250)) ";
            tsql += "		, CAST(STUFF(IntermediateDrugs, 1, CHARINDEX('~',IntermediateDrugs+'~'), '') AS VARCHAR(250)) ";
            tsql += "		, 0 ";
            tsql += "	FROM [Intermediate] ";
            tsql += "	WHERE IntermediateDrugs > '' ";
            tsql += "), ";

            tsql += "[Sensitive](RequestID, OBRSetID, OBXSetID, OBXSubID, ORGANISM, Drug, SensitiveDrugs, IsRow) AS ( ";
            tsql += "	SELECT  ";
            tsql += "		  A.RequestID ";
            tsql += "		, A.OBRSetID ";
            tsql += "		, OBXSetID ";
            tsql += "		, OBXSubID ";
            tsql += "		, ORGANISM ";
            tsql += "		, CAST(LEFT(SensitiveDrugs, CHARINDEX('~',SensitiveDrugs+'~')-1) AS VARCHAR(250)) ";
            tsql += "		, CAST(STUFF(SensitiveDrugs, 1, CHARINDEX('~',SensitiveDrugs+'~'), '') AS VARCHAR(250)) ";
            tsql += "		, 1 ";
            tsql += "	FROM [OpenLDRData].[dbo].[Monitoring] A ";
            tsql += "	JOIN [OpenLDRData].[dbo].[Requests] B ";
            tsql += "	ON A.RequestID = B.RequestID  ";
            tsql += "	AND A.OBRSetID = B.OBRSetID ";
            //tsql += "	JOIN [OpenLDRData].[dbo].[Facilities] F  ";
            //tsql += "	ON B.RequestingFacilityCode COLLATE DATABASE_DEFAULT = 'DISA' + F.FacilityCode ";
            tsql += "	WHERE SurveillanceCode LIKE '{2}' ";
            tsql += "	AND DATEADD(dd,0, DATEDIFF(dd,0, B.LIMSDateTimeStamp)) BETWEEN '{0}' AND '{1}' ";
            //tsql += "	AND({6} = 0 OR F." + CountryConfigSettings.HierarchyRoles[1].Column1 + " IN({7})) ";
            //tsql += "	AND({8} = 0 OR F." + CountryConfigSettings.HierarchyRoles[2].Column1 + " IN({9})) ";
            //tsql += "	AND({10} = 0 OR F." + CountryConfigSettings.HierarchyRoles[3].Column1 + " IN({11})) ";
            tsql += "	UNION ALL ";
            tsql += "	SELECT  ";
            tsql += "		  RequestID ";
            tsql += "		, OBRSetID ";
            tsql += "		, OBXSetID ";
            tsql += "		, OBXSubID ";
            tsql += "		, ORGANISM ";
            tsql += "		, CAST(LEFT(SensitiveDrugs, CHARINDEX('~',SensitiveDrugs+'~')-1) AS VARCHAR(250)) ";
            tsql += "		, CAST(STUFF(SensitiveDrugs, 1, CHARINDEX('~',SensitiveDrugs+'~'), '') AS VARCHAR(250)) ";
            tsql += "		, 0 ";
            tsql += "	FROM [Sensitive] ";
            tsql += "	WHERE SensitiveDrugs > '' ";
            tsql += "), ";

            tsql += "[ClassInList](Class, ClassParam) ";
            tsql += "AS ";
            tsql += "( ";
            tsql += "	SELECT  ";
            tsql += "		CAST(LEFT('{3}', CHARINDEX('~','{3}'+'~')-1) AS VARCHAR(250)) ";
            tsql += "		,CAST(STUFF('{3}', 1, CHARINDEX('~','{3}'+'~'), '') AS VARCHAR(250)) ";
            tsql += "	UNION ALL ";
            tsql += "	SELECT  ";
            tsql += "		CAST(LEFT(ClassParam, CHARINDEX('~',ClassParam+'~')-1) AS VARCHAR(250)) ";
            tsql += "		, CAST(STUFF(ClassParam, 1, CHARINDEX('~',ClassParam+'~'), '') AS VARCHAR(250)) ";
            tsql += "	FROM [ClassInList] ";
            tsql += "	WHERE ClassParam > '' ";
            tsql += "), ";

            tsql += "[OrganismNotInList](ORGANISM, ORGANISMParam) AS ( ";
            tsql += "	SELECT  ";
            tsql += "		CAST(LEFT('{4}', CHARINDEX('~','{4}'+'~')-1) AS VARCHAR(250)) ";
            tsql += "		, CAST(STUFF('{4}', 1, CHARINDEX('~','{4}'+'~'), '') AS VARCHAR(250)) ";
            tsql += "	UNION ALL ";
            tsql += "	SELECT  ";
            tsql += "		CAST(LEFT(ORGANISMParam, CHARINDEX('~',ORGANISMParam+'~')-1) AS VARCHAR(250)) ";
            tsql += "		, CAST(STUFF(ORGANISMParam, 1, CHARINDEX('~',ORGANISMParam+'~'), '') AS VARCHAR(250)) ";
            tsql += "	FROM [OrganismNotInList] ";
            tsql += "	WHERE ORGANISMParam > '' ";
            tsql += "), ";

            tsql += "[DrugNotInList](Drug, DrugParam) ";
            tsql += "AS ";
            tsql += "( ";
            tsql += "	SELECT  ";
            tsql += "		CAST(LEFT('{5}', CHARINDEX('~','{5}'+'~')-1) AS VARCHAR(250)) ";
            tsql += "		, CAST(STUFF('{5}', 1, CHARINDEX('~','{5}'+'~'), '') AS VARCHAR(250)) ";
            tsql += "	UNION ALL ";
            tsql += "	SELECT  ";
            tsql += "		CAST(LEFT(DrugParam, CHARINDEX('~',DrugParam+'~')-1) AS VARCHAR(250)) ";
            tsql += "		, CAST(STUFF(DrugParam, 1, CHARINDEX('~',DrugParam+'~'), '') AS VARCHAR(250)) ";
            tsql += "	FROM [DrugNotInList] ";
            tsql += "	WHERE DrugParam > '' ";
            tsql += "), ";

            tsql += "Totals(ORGANISM, Drug, ResultCount) AS ( ";
            tsql += "	SELECT  ";
            tsql += "		ORGANISM ";
            tsql += "		, Drug ";
            tsql += "		, COUNT(*) AS ResultCount  ";
            tsql += "	FROM ( SELECT  ";
            tsql += "				RequestID ";
            tsql += "				, OBRSetID ";
            tsql += "				, OBXSetID ";
            tsql += "				, OBXSubID ";
            tsql += "				, ORGANISM ";
            tsql += "				, Drug ";
            tsql += "				, 'Resistant' AS Class ";
            tsql += "				, IsRow  ";
            tsql += "			FROM Resistant  ";
            tsql += "			WHERE NULLIF(Drug, '') IS NOT NULL  ";
            tsql += "				AND ORGANISM NOT IN (SELECT ORGANISM FROM OrganismNotInList)  ";
            tsql += "				AND Drug NOT IN (SELECT Drug FROM DrugNotInList)  ";
            tsql += "			UNION ALL  ";
            tsql += "			SELECT  ";
            tsql += "				RequestID ";
            tsql += "				, OBRSetID ";
            tsql += "				, OBXSetID ";
            tsql += "				, OBXSubID ";
            tsql += "				, ORGANISM ";
            tsql += "				, Drug ";
            tsql += "				, 'Intermediate' AS Class ";
            tsql += "				, IsRow  ";
            tsql += "			FROM [Intermediate]  ";
            tsql += "			WHERE NULLIF(Drug, '') IS NOT NULL  ";
            tsql += "				AND ORGANISM NOT IN (SELECT ORGANISM FROM OrganismNotInList)  ";
            tsql += "				AND Drug NOT IN (SELECT Drug FROM DrugNotInList)  ";
            tsql += "			UNION ALL  ";
            tsql += "			SELECT  ";
            tsql += "				RequestID ";
            tsql += "				, OBRSetID ";
            tsql += "				, OBXSetID ";
            tsql += "				, OBXSubID ";
            tsql += "				, ORGANISM ";
            tsql += "				, Drug ";
            tsql += "				, 'Sensitive' AS Class ";
            tsql += "				, IsRow  ";
            tsql += "			FROM [Sensitive]  ";
            tsql += "			WHERE NULLIF(Drug, '') IS NOT NULL  ";
            tsql += "			AND ORGANISM NOT IN (SELECT ORGANISM FROM OrganismNotInList)  ";
            tsql += "			AND Drug NOT IN (SELECT Drug FROM DrugNotInList) ";
            tsql += "		) A ";
            tsql += "	GROUP BY ORGANISM, Drug ";
            tsql += "),  ";

            tsql += "Results(ORGANISM, Drug, Class, ResultCount) AS (  ";
            tsql += "	SELECT  ";
            tsql += "		ORGANISM ";
            tsql += "		, Drug ";
            tsql += "		, Class ";
            tsql += "		, COUNT(*) AS ResultCount  ";
            tsql += "	FROM ( SELECT  ";
            tsql += "				RequestID ";
            tsql += "				, OBRSetID ";
            tsql += "				, OBXSetID ";
            tsql += "				, OBXSubID ";
            tsql += "				, ORGANISM ";
            tsql += "				, Drug ";
            tsql += "				, 'Resistant' AS Class ";
            tsql += "				, IsRow  ";
            tsql += "			FROM Resistant ";
            tsql += "			WHERE NULLIF(Drug, '') IS NOT NULL  ";
            tsql += "				AND ORGANISM NOT IN (SELECT ORGANISM FROM OrganismNotInList)  ";
            tsql += "				AND Drug NOT IN (SELECT Drug FROM DrugNotInList)  ";
            tsql += "			UNION ALL  ";
            tsql += "			SELECT  ";
            tsql += "				RequestID ";
            tsql += "				, OBRSetID ";
            tsql += "				, OBXSetID ";
            tsql += "				, OBXSubID ";
            tsql += "				, ORGANISM ";
            tsql += "				, Drug ";
            tsql += "				, 'Intermediate' AS Class ";
            tsql += "				, IsRow  ";
            tsql += "			FROM [Intermediate]  ";
            tsql += "			WHERE NULLIF(Drug, '') IS NOT NULL  ";
            tsql += "				AND ORGANISM NOT IN (SELECT ORGANISM FROM OrganismNotInList)  ";
            tsql += "				AND Drug NOT IN (SELECT Drug FROM DrugNotInList)  ";
            tsql += "			UNION ALL  ";
            tsql += "			SELECT  ";
            tsql += "				RequestID ";
            tsql += "				, OBRSetID ";
            tsql += "				, OBXSetID ";
            tsql += "				, OBXSubID ";
            tsql += "				, ORGANISM ";
            tsql += "				, Drug ";
            tsql += "				, 'Sensitive' AS Class ";
            tsql += "				, IsRow  ";
            tsql += "			FROM [Sensitive]  ";
            tsql += "			WHERE NULLIF(Drug, '') IS NOT NULL  ";
            tsql += "			AND ORGANISM NOT IN (SELECT ORGANISM FROM OrganismNotInList)  ";
            tsql += "			AND Drug NOT IN (SELECT Drug FROM DrugNotInList) ";
            tsql += "		) A ";
            tsql += "	GROUP BY ORGANISM, Drug, Class ";
            tsql += ") ";

            tsql += "SELECT  ";
            tsql += "	A.ORGANISM  as [Organism] ";
            tsql += "	, A.Drug ";
            tsql += "	, A.Class ";
            tsql += "	, B.ResultCount AS TotalCount ";
            tsql += "	, CAST(A.ResultCount AS Decimal(30,6))/CAST(B.ResultCount AS DECIMAL(30,6)) AS Ratio  ";
            tsql += "FROM Results A  ";
            tsql += "LEFT OUTER JOIN Totals B  ";
            tsql += "ON A.ORGANISM = B.ORGANISM  ";
            tsql += "AND A.Drug = B.Drug  ";
            tsql += "WHERE A.Class IN (SELECT Class FROM ClassInList) ";

            tsql = string.Format(tsql,
					startDate,
					endDate,
					surveillenceCode,
					classIn,
					organismNotIn,
                    drugsNotIn
					//,commonParams.Zones.Count(),
					//commonParams.Zones.ToSqlJoin("'"),
					//commonParams.Regions.Count(),
					//commonParams.Regions.ToSqlJoin("'"),
					//commonParams.Facilities.Count(),
					//commonParams.Facilities.ToSqlJoin("'")
                    );

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
					Ratio = double.Parse(dataReader["Ratio"].ToString())
				});
			}
			connection.Close();

			return list;
		}
		#endregion
		#endregion
	}
}
