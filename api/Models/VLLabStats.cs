using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace OpenLDR.Dashboard.API.Models
{
	[Serializable]
	[XmlRoot(ElementName = "VLLabStats")]
	public class VLLabStats
	{
		#region Properties 
		public string Laboratory { get; set; }

		public double CollectedDate { get; set; }

		public double ReceivedDate { get; set; }

		public int VLTestRequests { get; set; }

		public int ProcessedNotReviewed { get; set; }

		public int ReviewedWithInvalids { get; set; }

		public int ValidTests { get; set; }

		public int ValidTestsTotal { get; set; }

		public double LabThroughput1 { get; set; }

		public double LabThroughput2 { get; set; }

		public double Referrals { get; set; }

		public double DisaLink { get; set; }
		#endregion
		public double Suppressed { get; set; }

		public double LabTestedWithin3days { get; set; }

		public double LabTestedWithin7days { get; set; }

		public double LabTestedWithin14days { get; set; }
		#region Constructor
		public VLLabStats()
		{
		}

		public VLLabStats(string Laboratory, double CollectedDate, double ReceivedDate, int VLTestRequests, int ProcessedNotReviewed, int ReviewedWithInvalids, int ValidTests, int ValidTestsTotal, double LabThroughput1, double LabThroughput2, double Referrals, double DisaLink, double Suppressed, double LabTestedWithin3days, double LabTestedWithin7days, double LabTestedWithin14days)
			: this()
		{
			this.Laboratory = Laboratory;
			this.CollectedDate = CollectedDate;
			this.ReceivedDate = ReceivedDate;
			this.VLTestRequests = VLTestRequests;
			this.ProcessedNotReviewed = ProcessedNotReviewed;
			this.ReviewedWithInvalids = ReviewedWithInvalids;
			this.ValidTests = ValidTests;
			this.ValidTestsTotal = ValidTestsTotal;
			this.LabThroughput1 = LabThroughput1;
			this.LabThroughput2 = LabThroughput2;
			this.Referrals = Referrals;
			this.DisaLink = DisaLink;
			this.Suppressed = Suppressed;
			this.LabTestedWithin3days = LabTestedWithin3days;
			this.LabTestedWithin7days = LabTestedWithin7days;
			this.LabTestedWithin14days = LabTestedWithin14days;
			
			
		}
		#endregion

		#region Methods
		#region All
		public static List <VLLabStats> All(IConfigurationSection configuration, string connectionString, DateTime? stdate, DateTime? edate)
		{

			var list = new List<VLLabStats>();
			var query = Core.GetQueryScript(configuration, "general_getHIVVLLabStats");
			if (!string.IsNullOrEmpty(query))
			{

				var connection = new SqlConnection(connectionString);
				connection.Open();


				SqlCommand cmd = new SqlCommand(query, connection) { CommandTimeout = 0 };
				
				
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
					var Laboratory = dataReader["Laboratory"].ToString();
					var CollectedDate = dataReader.ToDouble("CollectedDate");
					var ReceivedDate = dataReader.ToDouble("ReceivedDate");
					var VLTestRequests = dataReader.ToInt("VLTestRequests");
					var ProcessedNotReviewed = dataReader.ToInt("ProcessedNotReviewed");
					var ReviewedWithInvalids = dataReader.ToInt("ReviewedWithInvalids");
					var ValidTests = dataReader.ToInt("ValidTests");
					var ValidTestsTotal = dataReader.ToInt("ValidTestsTotal");
					var LabThroughput1 = dataReader.ToDouble("LabThroughput1");
					var LabThroughput2 = dataReader.ToDouble("LabThroughput2");
					var Referrals = dataReader.ToDouble("Referrals");
					var DisaLink = dataReader.ToDouble("DisaLink");
					var Suppressed = dataReader.ToDouble("Suppressed");
					var LabTestedWithin3days = dataReader.ToDouble("LabTestedWithin3days");
					var LabTestedWithin7days = dataReader.ToDouble("LabTestedWithin7days");
					var LabTestedWithin14days = dataReader.ToDouble("LabTestedWithin14days");
					
					list.Add(new VLLabStats(Laboratory,CollectedDate, ReceivedDate,  VLTestRequests, ProcessedNotReviewed, ReviewedWithInvalids, ValidTests, ValidTestsTotal,LabThroughput1,LabThroughput2,Referrals,DisaLink,Suppressed,LabTestedWithin3days,LabTestedWithin7days,LabTestedWithin14days));
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
