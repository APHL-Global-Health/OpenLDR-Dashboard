using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace OpenLDR.Dashboard.API.Models
{
    [Serializable]
    [XmlRoot(ElementName = "Transmission")]
    public class Transmission
    {
		#region Properties 
		public string Test { get; set; }

		public string System { get; set; }

		public string TestingLab { get; set; }

		public int Date { get; set; }

		public int Month { get; set; }

		public int Year { get; set; }

		public int Received { get; set; }

		public int Registered { get; set; }

		public int Tested { get; set; }

		public int Authorised { get; set; }

		public int Rejected { get; set; }

		public int Tested_Workload { get; set; }

		public int Authorised_Workload { get; set; }
		#endregion

		#region Constructor
		public Transmission()
		{
		}

		public Transmission(string Test, string System, string TestingLab, int Date, int Month, int Year, int Received, int Registered, int Tested, int Authorised, int Rejected, int Tested_Workload, int Authorised_Workload)
			: this()
		{
			this.Test = Test;
			this.System = System;
			this.TestingLab = TestingLab;
			this.Date = Date;
			this.Month = Month;
			this.Year = Year;
			this.Received = Received;
			this.Registered = Registered;
			this.Tested = Tested;
			this.Authorised = Authorised;
			this.Rejected = Rejected;

			this.Tested_Workload = Tested_Workload;
			this.Authorised_Workload = Authorised_Workload;
		}
		#endregion

		#region Methods
		#region All
		public static List<Transmission> All(List<string> tests, int year, int month, string connectionString, bool excludeTests = false)
		{
			var list = new List<Transmission>();

			var connection = new SqlConnection(connectionString);
			connection.Open();

			var _month = month;
			var _year = year;
			if (_month + 1 > 12)
			{
				_month = 0;
				_year += 1;
			}

			var query = "";
			query += "SELECT  ";
			query += "	(CASE WHEN @ExcludeTests = 1 THEN 'Other' ELSE [LIMSPanelCode] END) as \"Test\" ";
			query += "	 ";
			//We use Prefix of RequestID to identify the system data is coming from
			query += "	,(CASE WHEN LEFT([RequestID],6) = 'TZDISA'    THEN 'Disa*Lab'  ";
			query += "		  WHEN LEFT([RequestID],6) = 'TZNACP'    THEN 'EVLIMS'   ";
			query += "		  WHEN LEFT([RequestID],9) = 'TZLABMATE' THEN 'LabMate'  ";
			query += "		  WHEN LEFT([RequestID],6) = 'TZSKYL'    THEN 'SkyLIMS'  ";
			query += "		  WHEN LEFT([RequestID],5) = 'TZSTS'     THEN 'eSRS'  ";
			query += "		  ELSE 'Unknown' END) as \"System\"   ";
			query += " ";
			//We use part of RequestID to identify the testing lab, this would be easier if
			//every system used a master facility list
			query += "	,(CASE WHEN RIGHT(LEFT([RequestID],9),3) IN ('TZA','TZC','TZH')									THEN 'Mnazi Mmoja'  ";
			query += "		   WHEN RIGHT(LEFT([RequestID],9),3) IN ('TDS','TEQ')										THEN 'NHL-QATC'  ";
			query += "		   WHEN RIGHT(LEFT([RequestID],9),3) IN ('TMS','TMC','TMH')									THEN 'Kilimanjaro Christian Medical Centre'  ";
			query += "		   WHEN RIGHT(LEFT([RequestID],9),3) IN ('TBA','TBC','TBG')									THEN 'Bugando Medical Centre'  ";
			query += "		   WHEN RIGHT(LEFT([RequestID],9),3) IN ('TMA','TMB','TMG')									THEN 'Mbeya Zonal Referral Hospital'  ";
			query += "		   WHEN LEFT([RequestID],10) = 'TZNACP-IRI' OR [RequestID] = 'iringarrh'					THEN 'Iringa Referral Hospital'  ";
			query += "		   WHEN LEFT([RequestID],10) = 'TZNACP-MTR' OR [RequestID] = 'ligularrh'					THEN 'Ligula Hospital'  ";
			query += "		   WHEN LEFT([RequestID],10) = 'TZNACP-DOD' OR [RequestID] = 'dodoma'						THEN 'Dodoma Regional Referral Hospital'  ";
			query += "		   WHEN LEFT([RequestID],10) = 'TZNACP-SNG'													THEN 'Singida Region Referral'  ";
			query += "		   WHEN LEFT([RequestID],10) = 'TZNACP-TBR'													THEN 'Kitete Regional Referral Hospital'  ";
			query += "		   WHEN LEFT([RequestID],10) = 'TZNACP-STF'													THEN 'St. Francis Designated District Hospital'  ";
			query += "		   WHEN LEFT([RequestID],10) IN ('TZNACP-STE','TZNACP-STS')									THEN 'Iringa Dream Molecular Laboratory'   ";
			query += "		   WHEN LEFT([RequestID],10) IN ('TZNACP-MTM', 'TZNACP-MMT') OR [RequestID] = 'mtmerurrh'	THEN 'Mt. Meru'  ";
			query += "		   WHEN LEFT([RequestID],10) IN ('TZNACP-MOR', 'TZNACP-MRO') OR [RequestID] = 'morogoro'	THEN 'Morogoro Regional Referral Hospital'  ";
			query += "		   WHEN LEFT([RequestID],9)  = 'TZLABMATE'													THEN 'MDH - Temeke'  ";
			query += "		   WHEN LEFT([RequestID],11) = 'TZSKYLNDAND'												THEN 'Ndanda Regional Referral Hospital'  ";
			query += "		   ELSE 'Unknown : '+[RequestID] END) as \"TestingLab\"   ";
			//Check if date is between RegisteredDateTime, AnalysisDateTime or AuthorisedDateTime
			//If it is, return the date
			query += "   ,(CASE WHEN [RegisteredDateTime] >= @StartDate AND [RegisteredDateTime] < @EndDate THEN DATEPART(day,[RegisteredDateTime])  ";
			query += "         WHEN [AnalysisDateTime]   >= @StartDate AND [AnalysisDateTime]   < @EndDate THEN DATEPART(day,[AnalysisDateTime])  ";
			query += "         WHEN [AuthorisedDateTime] >= @StartDate AND [AuthorisedDateTime] < @EndDate THEN DATEPART(day,[AuthorisedDateTime])  ";
			query += "         ELSE 0 END) as \"Date\"  ";
			//Check if month is between RegisteredDateTime, AnalysisDateTime or AuthorisedDateTime
			//If it is, return the month
			query += "   ,(CASE WHEN [RegisteredDateTime] >= @StartDate AND [RegisteredDateTime] < @EndDate THEN DATEPART(month,[RegisteredDateTime])  ";
			query += "         WHEN [AnalysisDateTime]   >= @StartDate AND [AnalysisDateTime]   < @EndDate THEN DATEPART(month,[AnalysisDateTime])  ";
			query += "         WHEN [AuthorisedDateTime] >= @StartDate AND [AuthorisedDateTime] < @EndDate THEN DATEPART(month,[AuthorisedDateTime])  ";
			query += "         ELSE 0 END) as \"Month\"   ";
			//Check if year is between RegisteredDateTime, AnalysisDateTime or AuthorisedDateTime
			//If it is, return the year
			query += "   ,(CASE WHEN [RegisteredDateTime] >= @StartDate AND [RegisteredDateTime] < @EndDate THEN DATEPART(year,[RegisteredDateTime])  ";
			query += "         WHEN [AnalysisDateTime]   >= @StartDate AND [AnalysisDateTime]   < @EndDate THEN DATEPART(year,[AnalysisDateTime])  ";
			query += "         WHEN [AuthorisedDateTime] >= @StartDate AND [AuthorisedDateTime] < @EndDate THEN DATEPART(year,[AuthorisedDateTime])  ";
			query += "         ELSE 0 END) as \"Year\" ";
			//Get the counts for Received, Registered, Tested, Authorised and Rejected data for that date
			query += "	,SUM(CASE WHEN [ReceivedDateTime]   >= @StartDate AND [ReceivedDateTime]   < @EndDate THEN 1 ELSE 0 END) as \"Received\"  ";
			query += "	,SUM(CASE WHEN [RegisteredDateTime] >= @StartDate AND [RegisteredDateTime] < @EndDate THEN 1 ELSE 0 END) as \"Registered\"  ";
			query += "	,SUM(CASE WHEN [AnalysisDateTime]   >= @StartDate AND [AnalysisDateTime]   < @EndDate THEN 1 ELSE 0 END) as \"Tested\"  ";
			query += "	,SUM(CASE WHEN [AuthorisedDateTime] >= @StartDate AND [AuthorisedDateTime] < @EndDate THEN 1 ELSE 0 END) as \"Authorised\"  ";
			query += "	,SUM(CASE WHEN [LIMSRejectionCode] IS NOT NULL AND [LIMSRejectionCode] <> '' THEN 1 ELSE 0 END) as \"Rejected\" 	 ";
			query += " ";
			//Get the counts for Tested, Authorised data for that date if Registered with same period
			query += "	,SUM(CASE WHEN ([AnalysisDateTime]   >= @StartDate AND [AnalysisDateTime]   < @EndDate) AND  ";
			query += "	   ([RegisteredDateTime] >= @StartDate AND [RegisteredDateTime] < @EndDate)  ";
			query += "	   THEN 1 ELSE 0 END) as \"Tested_Workload\"  ";
			query += "    ";
			query += "	,SUM(CASE WHEN ([AnalysisDateTime]   >= @StartDate AND [AnalysisDateTime]   < @EndDate) AND  ";
			query += "	   ([RegisteredDateTime] >= @StartDate AND [RegisteredDateTime] < @EndDate) AND  ";
			query += "	   ([AuthorisedDateTime] >= @StartDate AND [AuthorisedDateTime] < @EndDate)  ";
			query += "	   THEN 1 ELSE 0 END) as \"Authorised_Workload\"   ";
			query += " ";
			query += "FROM [OpenLDRData].[dbo].[Requests]  ";
			query += "WHERE   ";
			//Get all data with in the date ranges
			query += "	( ";
			query += "		([RegisteredDateTime] >= @StartDate AND [RegisteredDateTime] < @EndDate) OR  ";
			query += "		([AnalysisDateTime]   >= @StartDate AND [AnalysisDateTime]   < @EndDate) OR  ";
			query += "		([AuthorisedDateTime] >= @StartDate AND [AuthorisedDateTime] < @EndDate)  ";
			query += "	 ) ";
			//Remove gibberish
			query += "	AND (CASE WHEN [RegisteredDateTime] >= @StartDate AND [RegisteredDateTime] < @EndDate THEN DATEPART(day,[RegisteredDateTime])  ";
			query += "         WHEN [AnalysisDateTime]   >= @StartDate AND [AnalysisDateTime]   < @EndDate THEN DATEPART(day,[AnalysisDateTime])  ";
			query += "         WHEN [AuthorisedDateTime] >= @StartDate AND [AuthorisedDateTime] < @EndDate THEN DATEPART(day,[AuthorisedDateTime])  ";
			query += "         ELSE 0 END) <> 0  ";
			query += "	AND (CASE WHEN [RegisteredDateTime] >= @StartDate AND [RegisteredDateTime] < @EndDate THEN DATEPART(month,[RegisteredDateTime])  ";
			query += "         WHEN [AnalysisDateTime]   >= @StartDate AND [AnalysisDateTime]   < @EndDate THEN DATEPART(month,[AnalysisDateTime])  ";
			query += "         WHEN [AuthorisedDateTime] >= @StartDate AND [AuthorisedDateTime] < @EndDate THEN DATEPART(month,[AuthorisedDateTime])  ";
			query += "         ELSE 0 END) <> 0  ";
			query += "	AND (CASE WHEN [RegisteredDateTime] >= @StartDate AND [RegisteredDateTime] < @EndDate THEN DATEPART(year,[RegisteredDateTime])  ";
			query += "         WHEN [AnalysisDateTime]   >= @StartDate AND [AnalysisDateTime]   < @EndDate THEN DATEPART(year,[AnalysisDateTime])  ";
			query += "         WHEN [AuthorisedDateTime] >= @StartDate AND [AuthorisedDateTime] < @EndDate THEN DATEPART(year,[AuthorisedDateTime])  ";
			query += "         ELSE 0 END) <> 0 	 ";
			//Ignore some data that may cause duplicates especially if dealing with multiple databases/sources
			query += "	AND LEFT([RequestID],5) <> 'TZSTS'  ";
			query += "	AND LEFT([RequestID],5) <> 'TZHUB'  ";
			query += "	AND LEFT([RequestID],5) <> 'TZMNH'  ";
			query += "	AND LEFT([RequestID],7) <> 'TZTILLE'  ";
			//Filter by specified tests or fetch everything else
			query += "	AND (CASE WHEN @ExcludeTests = 0 AND [LIMSPanelCode] IN ('HIVVL' ,'HIVPC' ,'EID') THEN 1 ";
			query += "			  WHEN @ExcludeTests = 1 AND [LIMSPanelCode] NOT IN ('HIVVL' ,'HIVPC' ,'EID') THEN 1 ";
			query += "			  ELSE 0 END) = 1 ";
			query += "GROUP BY   ";
			//Group by System, TestingLab, Tests and Dates
			query += "   (CASE WHEN [RegisteredDateTime] >= @StartDate AND [RegisteredDateTime] < @EndDate THEN DATEPART(year,[RegisteredDateTime])  ";
			query += "         WHEN [AnalysisDateTime]   >= @StartDate AND [AnalysisDateTime]   < @EndDate THEN DATEPART(year,[AnalysisDateTime])  ";
			query += "         WHEN [AuthorisedDateTime] >= @StartDate AND [AuthorisedDateTime] < @EndDate THEN DATEPART(year,[AuthorisedDateTime])  ";
			query += "         ELSE 0 END)  ";
			query += "   ,(CASE WHEN [RegisteredDateTime] >= @StartDate AND [RegisteredDateTime] < @EndDate THEN DATEPART(month,[RegisteredDateTime])  ";
			query += "         WHEN [AnalysisDateTime]    >= @StartDate AND [AnalysisDateTime]   < @EndDate THEN DATEPART(month,[AnalysisDateTime])  ";
			query += "         WHEN [AuthorisedDateTime]  >= @StartDate AND [AuthorisedDateTime] < @EndDate THEN DATEPART(month,[AuthorisedDateTime])  ";
			query += "         ELSE 0 END)  ";
			query += "   ,(CASE WHEN [RegisteredDateTime] >= @StartDate AND [RegisteredDateTime] < @EndDate THEN DATEPART(day,[RegisteredDateTime])  ";
			query += "         WHEN [AnalysisDateTime]    >= @StartDate AND [AnalysisDateTime]   < @EndDate THEN DATEPART(day,[AnalysisDateTime])  ";
			query += "         WHEN [AuthorisedDateTime]  >= @StartDate AND [AuthorisedDateTime] < @EndDate THEN DATEPART(day,[AuthorisedDateTime])  ";
			query += "         ELSE 0 END)  ";
			query += "	,(CASE WHEN LEFT([RequestID],6) = 'TZDISA' THEN 'Disa*Lab'  ";
			query += "      WHEN LEFT([RequestID],6) = 'TZNACP' THEN 'EVLIMS'   ";
			query += "	  WHEN LEFT([RequestID],9) = 'TZLABMATE' THEN 'LabMate'  ";
			query += "	  WHEN LEFT([RequestID],6) = 'TZSKYL' THEN 'SkyLIMS'  ";
			query += "	  WHEN LEFT([RequestID],5) = 'TZSTS' THEN 'eSRS'  ";
			query += "      ELSE 'Unknown' END),  ";
			query += "	  (CASE WHEN RIGHT(LEFT([RequestID],9),3) IN ('TZA','TZC','TZH')								THEN 'Mnazi Mmoja'  ";
			query += "		   WHEN RIGHT(LEFT([RequestID],9),3) IN ('TDS','TEQ')										THEN 'NHL-QATC'  ";
			query += "		   WHEN RIGHT(LEFT([RequestID],9),3) IN ('TMS','TMC','TMH')									THEN 'Kilimanjaro Christian Medical Centre'  ";
			query += "		   WHEN RIGHT(LEFT([RequestID],9),3) IN ('TBA','TBC','TBG')									THEN 'Bugando Medical Centre'  ";
			query += "		   WHEN RIGHT(LEFT([RequestID],9),3) IN ('TMA','TMB','TMG')									THEN 'Mbeya Zonal Referral Hospital'  ";
			query += "		   WHEN LEFT([RequestID],10) = 'TZNACP-IRI' OR [RequestID] = 'iringarrh'					THEN 'Iringa Referral Hospital'  ";
			query += "		   WHEN LEFT([RequestID],10) = 'TZNACP-MTR' OR [RequestID] = 'ligularrh'					THEN 'Ligula Hospital'  ";
			query += "		   WHEN LEFT([RequestID],10) = 'TZNACP-DOD' OR [RequestID] = 'dodoma'						THEN 'Dodoma Regional Referral Hospital'  ";
			query += "		   WHEN LEFT([RequestID],10) = 'TZNACP-SNG'													THEN 'Singida Region Referral'  ";
			query += "		   WHEN LEFT([RequestID],10) = 'TZNACP-TBR'													THEN 'Kitete Regional Referral Hospital'  ";
			query += "		   WHEN LEFT([RequestID],10) = 'TZNACP-STF'													THEN 'St. Francis Designated District Hospital'  ";
			query += "		   WHEN LEFT([RequestID],10) IN ('TZNACP-STE','TZNACP-STS')									THEN 'Iringa Dream Molecular Laboratory'   ";
			query += "		   WHEN LEFT([RequestID],10) IN ('TZNACP-MTM', 'TZNACP-MMT') OR [RequestID] = 'mtmerurrh'	THEN 'Mt. Meru'  ";
			query += "		   WHEN LEFT([RequestID],10) IN ('TZNACP-MOR', 'TZNACP-MRO') OR [RequestID] = 'morogoro'	THEN 'Morogoro Regional Referral Hospital'  ";
			query += "		   WHEN LEFT([RequestID],9)  = 'TZLABMATE'													THEN 'MDH - Temeke'  ";
			query += "		   WHEN LEFT([RequestID],11) = 'TZSKYLNDAND'												THEN 'Ndanda Regional Referral Hospital'  ";
			query += "		   ELSE 'Unknown : '+[RequestID] END) ";
			query += "	,(CASE WHEN @ExcludeTests = 1 THEN 'Other' ELSE [LIMSPanelCode] END) ";
			query += "ORDER BY  ";
			//Order by System, TestingLab, Tests and Dates
			query += "	(CASE WHEN @ExcludeTests = 1 THEN 'Other' ELSE [LIMSPanelCode] END) ";
			query += "   ,(CASE WHEN [RegisteredDateTime] >= @StartDate AND [RegisteredDateTime] < @EndDate THEN DATEPART(year,[RegisteredDateTime])  ";
			query += "         WHEN [AnalysisDateTime]    >= @StartDate AND [AnalysisDateTime]   < @EndDate THEN DATEPART(year,[AnalysisDateTime])  ";
			query += "         WHEN [AuthorisedDateTime]  >= @StartDate AND [AuthorisedDateTime] < @EndDate THEN DATEPART(year,[AuthorisedDateTime])  ";
			query += "         ELSE 0 END)  ";
			query += "   ,(CASE WHEN [RegisteredDateTime] >= @StartDate AND [RegisteredDateTime] < @EndDate THEN DATEPART(month,[RegisteredDateTime])  ";
			query += "         WHEN [AnalysisDateTime]    >= @StartDate AND [AnalysisDateTime]   < @EndDate THEN DATEPART(month,[AnalysisDateTime])  ";
			query += "         WHEN [AuthorisedDateTime]  >= @StartDate AND [AuthorisedDateTime] < @EndDate THEN DATEPART(month,[AuthorisedDateTime])  ";
			query += "         ELSE 0 END)  ";
			query += "   ,(CASE WHEN [RegisteredDateTime] >= @StartDate AND [RegisteredDateTime] < @EndDate THEN DATEPART(day,[RegisteredDateTime])  ";
			query += "         WHEN [AnalysisDateTime]    >= @StartDate AND [AnalysisDateTime]   < @EndDate THEN DATEPART(day,[AnalysisDateTime])  ";
			query += "         WHEN [AuthorisedDateTime]  >= @StartDate AND [AuthorisedDateTime] < @EndDate THEN DATEPART(day,[AuthorisedDateTime])  ";
			query += "         ELSE 0 END)  ";
			query += "	,(CASE WHEN LEFT([RequestID],6) = 'TZDISA' THEN 'Disa*Lab'  ";
			query += "      WHEN LEFT([RequestID],6) = 'TZNACP' THEN 'EVLIMS'   ";
			query += "	  WHEN LEFT([RequestID],9) = 'TZLABMATE' THEN 'LabMate'  ";
			query += "	  WHEN LEFT([RequestID],6) = 'TZSKYL' THEN 'SkyLIMS'  ";
			query += "	  WHEN LEFT([RequestID],5) = 'TZSTS' THEN 'eSRS'  ";
			query += "      ELSE 'Unknown' END),  ";
			query += "	  (CASE WHEN RIGHT(LEFT([RequestID],9),3) IN ('TZA','TZC','TZH')								THEN 'Mnazi Mmoja'  ";
			query += "		   WHEN RIGHT(LEFT([RequestID],9),3) IN ('TDS','TEQ')										THEN 'NHL-QATC'  ";
			query += "		   WHEN RIGHT(LEFT([RequestID],9),3) IN ('TMS','TMC','TMH')									THEN 'Kilimanjaro Christian Medical Centre'  ";
			query += "		   WHEN RIGHT(LEFT([RequestID],9),3) IN ('TBA','TBC','TBG')									THEN 'Bugando Medical Centre'  ";
			query += "		   WHEN RIGHT(LEFT([RequestID],9),3) IN ('TMA','TMB','TMG')									THEN 'Mbeya Zonal Referral Hospital'  ";
			query += "		   WHEN LEFT([RequestID],10) = 'TZNACP-IRI' OR [RequestID] = 'iringarrh'					THEN 'Iringa Referral Hospital'  ";
			query += "		   WHEN LEFT([RequestID],10) = 'TZNACP-MTR' OR [RequestID] = 'ligularrh'					THEN 'Ligula Hospital'  ";
			query += "		   WHEN LEFT([RequestID],10) = 'TZNACP-DOD' OR [RequestID] = 'dodoma'						THEN 'Dodoma Regional Referral Hospital'  ";
			query += "		   WHEN LEFT([RequestID],10) = 'TZNACP-SNG'													THEN 'Singida Region Referral'  ";
			query += "		   WHEN LEFT([RequestID],10) = 'TZNACP-TBR'													THEN 'Kitete Regional Referral Hospital'  ";
			query += "		   WHEN LEFT([RequestID],10) = 'TZNACP-STF'													THEN 'St. Francis Designated District Hospital'  ";
			query += "		   WHEN LEFT([RequestID],10) IN ('TZNACP-STE','TZNACP-STS')									THEN 'Iringa Dream Molecular Laboratory'   ";
			query += "		   WHEN LEFT([RequestID],10) IN ('TZNACP-MTM', 'TZNACP-MMT') OR [RequestID] = 'mtmerurrh'	THEN 'Mt. Meru'  ";
			query += "		   WHEN LEFT([RequestID],10) IN ('TZNACP-MOR', 'TZNACP-MRO') OR [RequestID] = 'morogoro'	THEN 'Morogoro Regional Referral Hospital'  ";
			query += "		   WHEN LEFT([RequestID],9)  = 'TZLABMATE'													THEN 'MDH - Temeke'  ";
			query += "		   WHEN LEFT([RequestID],11) = 'TZSKYLNDAND'												THEN 'Ndanda Regional Referral Hospital'  ";
			query += "		   ELSE 'Unknown : '+[RequestID] END) ";

			var dt = new DateTime(year,month,1);
			var nd = dt.AddMonths(1);

			SqlCommand cmd = new SqlCommand(query, connection) { CommandTimeout = 0 };
			cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = dt;
			cmd.Parameters.Add("@EndDate", SqlDbType.Date).Value = nd;
			cmd.Parameters.Add("@ExcludeTests", SqlDbType.Bit).Value = excludeTests ? 1 : 0;
			SqlDataReader dataReader = cmd.ExecuteReader();

			while (dataReader.Read())
			{
				var Test = dataReader["Test"].ToString();
				var System = dataReader["System"].ToString();
				var TestingLab = dataReader["TestingLab"].ToString();
				var Date = dataReader.ToInt("Date");
				var Month = dataReader.ToInt("Month");
				var Year = dataReader.ToInt("Year");
				var Received = dataReader.ToInt("Received");
				var Registered = dataReader.ToInt("Registered");
				var Tested = dataReader.ToInt("Tested");
				var Authorised = dataReader.ToInt("Authorised");
				var Rejected = dataReader.ToInt("Rejected");

				var Tested_Workload = dataReader.ToInt("Tested_Workload");
				var Authorised_Workload = dataReader.ToInt("Authorised_Workload");

				list.Add(new Transmission(Test, System, TestingLab, Date, Month, Year, Received, Registered, Tested, Authorised, Rejected, Tested_Workload, Authorised_Workload));
			}

			dataReader.Close();
			connection.Close();

			return list;
		}
		#endregion
		#endregion
	}
}
