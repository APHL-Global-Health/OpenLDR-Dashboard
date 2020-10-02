using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace OpenLDR.Dashboard.API.Models
{
    [Serializable]
    [XmlRoot(ElementName = "EIDGeo")]
    public class EIDGeo
    {
		#region Properties 
		public string Province { get; set; }

		public string District { get; set; }

		public string Facility { get; set; }

		public int Tests { get; set; }

		public int Positive { get; set; }

		public int Negative { get; set; }

		public double PositivityRate { get; set; }

		public double AverageTAT { get; set; }

		public double TATWithin14 { get; set; }

		public int Male { get; set; }

		public int Female { get; set; }

		public int IndeterminateGender { get; set; }

		public int UnknownGender { get; set; }

		public int MissingGender { get; set; }

		public int At_Birth { get; set; }

		public int _1_to_6wks { get; set; }

		public int _7wks_to_6Ms { get; set; }

		public int _7_to_9Ms { get; set; }

		public int _10_to_12Ms { get; set; }

		public int _13_to_18Ms { get; set; }

		public int _19_to_24Ms { get; set; }

		public int Over_2Y { get; set; }

		public int AgeMissing { get; set; }

		public int At_Birth_m { get; set; }

		public int _1_to_6wks_m { get; set; }

		public int _7wks_to_6Ms_m { get; set; }

		public int _7_to_9Ms_m { get; set; }

		public int _10_to_12Ms_m { get; set; }

		public int _13_to_18Ms_m { get; set; }

		public int _19_to_24Ms_m { get; set; }

		public int Over_2Y_m { get; set; }

		public int AgeMissingm { get; set; }

		public int At_Birth_f { get; set; }

		public int _1_to_6wks_f { get; set; }

		public int _7wks_to_6Ms_f { get; set; }

		public int _7_to_9Ms_f { get; set; }

		public int _10_to_12Ms_f { get; set; }

		public int _13_to_18Ms_f { get; set; }

		public int _19_to_24Ms_f { get; set; }

		public int Over_2Y_f { get; set; }

		public int AgeMissingf { get; set; }

		public int At_Birth_i { get; set; }

		public int _1_to_6wks_i { get; set; }

		public int _7wks_to_6Ms_i { get; set; }

		public int _7_to_9Ms_i { get; set; }

		public int _10_to_12Ms_i { get; set; }

		public int _13_to_18Ms_i { get; set; }

		public int _19_to_24Ms_i { get; set; }

		public int Over_2Y_i { get; set; }

		public int AgeMissingi { get; set; }

		public int At_Birth_u { get; set; }

		public int _1_to_6wks_u { get; set; }

		public int _7wks_to_6Ms_u { get; set; }

		public int _7_to_9Ms_u { get; set; }

		public int _10_to_12Ms_u { get; set; }

		public int _13_to_18Ms_u { get; set; }

		public int _19_to_24Ms_u { get; set; }

		public int Over_2Y_u { get; set; }

		public int AgeMissingu { get; set; }

		public int At_Birth_x { get; set; }

		public int _1_to_6wks_x { get; set; }

		public int _7wks_to_6Ms_x { get; set; }

		public int _7_to_9Ms_x { get; set; }

		public int _10_to_12Ms_x { get; set; }

		public int _13_to_18Ms_x { get; set; }

		public int _19_to_24Ms_x { get; set; }

		public int Over_2Y_x { get; set; }

		public int AgeMissingx { get; set; }

		#endregion

		#region Constructor
		public EIDGeo()
		{
		}

		public EIDGeo(string Province, string District, string Facility, int Tests, int Positive, int Negative, double PositivityRate, double AverageTAT, double TATWithin14, int Male, int Female, int IndeterminateGender,int UnknownGender,int MissingGender, int At_Birth, int _1_to_6wks, int _7wks_to_6Ms, int _7_to_9Ms, int _10_to_12Ms, int _13_to_18Ms, int _19_to_24Ms, int Over_2Y, int AgeMissing, int At_Birth_m, int _1_to_6wks_m, int _7wks_to_6Ms_m, int _7_to_9Ms_m, int _10_to_12Ms_m, int _13_to_18Ms_m, int _19_to_24Ms_m, int Over_2Y_m, int AgeMissingm, int At_Birth_f, int _1_to_6wks_f, int _7wks_to_6Ms_f, int _7_to_9Ms_f, int _10_to_12Ms_f, int _13_to_18Ms_f, int _19_to_24Ms_f, int Over_2Y_f, int AgeMissingf, int At_Birth_i, int _1_to_6wks_i, int _7wks_to_6Ms_i, int _7_to_9Ms_i, int _10_to_12Ms_i, int _13_to_18Ms_i, int _19_to_24Ms_i, int Over_2Y_i, int AgeMissingi, int At_Birth_u, int _1_to_6wks_u, int _7wks_to_6Ms_u, int _7_to_9Ms_u, int _10_to_12Ms_u, int _13_to_18Ms_u, int _19_to_24Ms_u, int Over_2Y_u, int AgeMissingu, int At_Birth_x, int _1_to_6wks_x, int _7wks_to_6Ms_x, int _7_to_9Ms_x, int _10_to_12Ms_x, int _13_to_18Ms_x, int _19_to_24Ms_x, int Over_2Y_x, int AgeMissingx)
			: this()
		{
			this.Province = Province;
			this.District = District;
			this.Facility = Facility;
			this.Tests = Tests;
			this.Positive = Positive;
			this.Negative = Negative;
			this.PositivityRate = PositivityRate;
			this.AverageTAT = AverageTAT;
			this.TATWithin14 = TATWithin14;
			this.Male = Male;
			this.Female = Female;
			this.IndeterminateGender = IndeterminateGender;
			this.UnknownGender = UnknownGender;
			this.MissingGender = MissingGender;
			this.At_Birth = At_Birth;
			this._1_to_6wks = _1_to_6wks;
			this._7wks_to_6Ms = _7wks_to_6Ms;
			this._7_to_9Ms = _7_to_9Ms;
			this._10_to_12Ms = _10_to_12Ms;
			this._13_to_18Ms = _13_to_18Ms;
			this._19_to_24Ms = _19_to_24Ms;
			this.Over_2Y = Over_2Y;
			this.AgeMissing = AgeMissing;
			this.At_Birth_m = At_Birth_m;
			this._1_to_6wks_m = _1_to_6wks_m;
			this._7wks_to_6Ms_m = _7wks_to_6Ms_m;
			this._7_to_9Ms_m = _7_to_9Ms_m;
			this._10_to_12Ms_m = _10_to_12Ms_m;
			this._13_to_18Ms_m = _13_to_18Ms_m;
			this._19_to_24Ms_m = _19_to_24Ms_m;
			this.Over_2Y_m = Over_2Y_m;
			this.AgeMissingm = AgeMissingm;
			this.At_Birth_f = At_Birth_f;
			this._1_to_6wks_f = _1_to_6wks_f;
			this._7wks_to_6Ms_f = _7wks_to_6Ms_f;
			this._7_to_9Ms_f = _7_to_9Ms_f;
			this._10_to_12Ms_f = _10_to_12Ms_f;
			this._13_to_18Ms_f = _13_to_18Ms_f;
			this._19_to_24Ms_f = _19_to_24Ms_f;
			this.Over_2Y_f = Over_2Y_f;
			this.AgeMissingf = AgeMissingf;
			this.At_Birth_i = At_Birth_i;
			this._1_to_6wks_i = _1_to_6wks_i;
			this._7wks_to_6Ms_i = _7wks_to_6Ms_i;
			this._7_to_9Ms_i = _7_to_9Ms_i;
			this._10_to_12Ms_i = _10_to_12Ms_i;
			this._13_to_18Ms_i = _13_to_18Ms_i;
			this._19_to_24Ms_i = _19_to_24Ms_i;
			this.Over_2Y_i = Over_2Y_i;
			this.AgeMissingi = AgeMissingi;
			this.At_Birth_u = At_Birth_u;
			this._1_to_6wks_u = _1_to_6wks_u;
			this._7wks_to_6Ms_u = _7wks_to_6Ms_u;
			this._7_to_9Ms_u = _7_to_9Ms_u;
			this._10_to_12Ms_u = _10_to_12Ms_u;
			this._13_to_18Ms_u = _13_to_18Ms_u;
			this._19_to_24Ms_u = _19_to_24Ms_u;
			this.Over_2Y_u = Over_2Y_u;
			this.AgeMissingu = AgeMissingu;
			this.At_Birth_x = At_Birth_x;
			this._1_to_6wks_x = _1_to_6wks_x;
			this._7wks_to_6Ms_x = _7wks_to_6Ms_x;
			this._7_to_9Ms_x = _7_to_9Ms_x;
			this._10_to_12Ms_x = _10_to_12Ms_x;
			this._13_to_18Ms_x = _13_to_18Ms_x;
			this._19_to_24Ms_x = _19_to_24Ms_x;
			this.Over_2Y_x = Over_2Y_x;
			this.AgeMissingx = AgeMissingx;

		}
		#endregion

		#region Methods
		#region All
		public static EIDGeo All(IConfigurationSection configuration, string connectionString, string province, string district, string facility, DateTime? stdate, DateTime? edate)
		{
			EIDGeo obj = null;
			var query = Core.GetQueryScript(configuration, "general_getEIDByGeography");
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
				if (facility == null)
					cmd.Parameters.Add("@Facility", SqlDbType.VarChar).Value = DBNull.Value;
				else cmd.Parameters.Add("@Facility", SqlDbType.VarChar).Value = facility;
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
					var Tests = dataReader.ToInt("Tests");
					var Positive = dataReader.ToInt("Positive");
					var Negative = dataReader.ToInt("Negative");
					var PositivityRate = dataReader.ToDouble("PositivityRate");
					var AverageTAT = dataReader.ToDouble("AverageTAT");
					var TATWithin14 = dataReader.ToDouble("TATWithin14");
					var Male = dataReader.ToInt("Male");
					var Female = dataReader.ToInt("Female");
					var IndeterminateGender = dataReader.ToInt("IndeterminateGender");
					var UnknownGender = dataReader.ToInt("UnknownGender");
					var MissingGender = dataReader.ToInt("MissingGender");
					var At_Birth = dataReader.ToInt("At_Birth");
					var _1_to_6wks = dataReader.ToInt("_1_to_6wks");
					var _7wks_to_6Ms = dataReader.ToInt("_7wks_to_6Ms");
					var _7_to_9Ms = dataReader.ToInt("_7_to_9Ms");
					var _10_to_12Ms = dataReader.ToInt("_10_to_12Ms");
					var _13_to_18Ms = dataReader.ToInt("_13_to_18Ms");
					var _19_to_24Ms = dataReader.ToInt("_19_to_24Ms");
					var Over_2Y = dataReader.ToInt("Over_2Y");
					var AgeMissing = dataReader.ToInt("AgeMissing");
					var At_Birth_m = dataReader.ToInt("At_Birth_m");
					var _1_to_6wks_m = dataReader.ToInt("_1_to_6wks_m");
					var _7wks_to_6Ms_m = dataReader.ToInt("_7wks_to_6Ms_m");
					var _7_to_9Ms_m = dataReader.ToInt("_7_to_9Ms_m");
					var _10_to_12Ms_m = dataReader.ToInt("_10_to_12Ms_m");
					var _13_to_18Ms_m = dataReader.ToInt("_13_to_18Ms_m");
					var _19_to_24Ms_m = dataReader.ToInt("_19_to_24Ms_m");
					var Over_2Y_m = dataReader.ToInt("Over_2Y_m");
					var AgeMissingm = dataReader.ToInt("AgeMissingm");
					var At_Birth_f = dataReader.ToInt("At_Birth_f");
					var _1_to_6wks_f = dataReader.ToInt("_1_to_6wks_f");
					var _7wks_to_6Ms_f = dataReader.ToInt("_7wks_to_6Ms_f");
					var _7_to_9Ms_f = dataReader.ToInt("_7_to_9Ms_f");
					var _10_to_12Ms_f = dataReader.ToInt("_10_to_12Ms_f");
					var _13_to_18Ms_f = dataReader.ToInt("_13_to_18Ms_f");
					var _19_to_24Ms_f = dataReader.ToInt("_19_to_24Ms_f");
					var Over_2Y_f = dataReader.ToInt("Over_2Y_f");
					var AgeMissingf = dataReader.ToInt("AgeMissingf");
					var At_Birth_i = dataReader.ToInt("At_Birth_i");
					var _1_to_6wks_i = dataReader.ToInt("_1_to_6wks_i");
					var _7wks_to_6Ms_i = dataReader.ToInt("_7wks_to_6Ms_i");
					var _7_to_9Ms_i = dataReader.ToInt("_7_to_9Ms_i");
					var _10_to_12Ms_i = dataReader.ToInt("_10_to_12Ms_i");
					var _13_to_18Ms_i = dataReader.ToInt("_13_to_18Ms_i");
					var _19_to_24Ms_i = dataReader.ToInt("_19_to_24Ms_i");
					var Over_2Y_i = dataReader.ToInt("Over_2Y_i");
					var AgeMissingi = dataReader.ToInt("AgeMissingi");
					var At_Birth_u = dataReader.ToInt("At_Birth_u");
					var _1_to_6wks_u = dataReader.ToInt("_1_to_6wks_u");
					var _7wks_to_6Ms_u = dataReader.ToInt("_7wks_to_6Ms_u");
					var _7_to_9Ms_u = dataReader.ToInt("_7_to_9Ms_u");
					var _10_to_12Ms_u = dataReader.ToInt("_10_to_12Ms_u");
					var _13_to_18Ms_u = dataReader.ToInt("_13_to_18Ms_u");
					var _19_to_24Ms_u = dataReader.ToInt("_19_to_24Ms_u");
					var Over_2Y_u = dataReader.ToInt("Over_2Y_u");
					var AgeMissingu = dataReader.ToInt("AgeMissingu");
					var At_Birth_x = dataReader.ToInt("At_Birth_x");
					var _1_to_6wks_x = dataReader.ToInt("_1_to_6wks_x");
					var _7wks_to_6Ms_x = dataReader.ToInt("_7wks_to_6Ms_x");
					var _7_to_9Ms_x = dataReader.ToInt("_7_to_9Ms_x");
					var _10_to_12Ms_x = dataReader.ToInt("_10_to_12Ms_x");
					var _13_to_18Ms_x = dataReader.ToInt("_13_to_18Ms_x");
					var _19_to_24Ms_x = dataReader.ToInt("_19_to_24Ms_x");
					var Over_2Y_x = dataReader.ToInt("Over_2Y_x");
					var AgeMissingx = dataReader.ToInt("AgeMissingx");



					obj = new EIDGeo(Province, District, Facility, Tests, Positive, Negative, PositivityRate, AverageTAT, TATWithin14, Male, Female, IndeterminateGender, UnknownGender, MissingGender,At_Birth, _1_to_6wks,_7wks_to_6Ms,_7_to_9Ms,_10_to_12Ms,_13_to_18Ms,_19_to_24Ms,Over_2Y,AgeMissing,At_Birth_m,_1_to_6wks_m,_7wks_to_6Ms_m,_7_to_9Ms_m,_10_to_12Ms_m,_13_to_18Ms_m,_19_to_24Ms_m,Over_2Y_m,AgeMissingm,At_Birth_f,_1_to_6wks_f,_7wks_to_6Ms_f,_7_to_9Ms_f,_10_to_12Ms_f,_13_to_18Ms_f,_19_to_24Ms_f,Over_2Y_f,AgeMissingf,At_Birth_i,_1_to_6wks_i,_7wks_to_6Ms_i,_7_to_9Ms_i,_10_to_12Ms_i,_13_to_18Ms_i,_19_to_24Ms_i,Over_2Y_i,AgeMissingi,At_Birth_u,_1_to_6wks_u,_7wks_to_6Ms_u,_7_to_9Ms_u,_10_to_12Ms_u,_13_to_18Ms_u,_19_to_24Ms_u,Over_2Y_u,AgeMissingu,At_Birth_x,_1_to_6wks_x,_7wks_to_6Ms_x,_7_to_9Ms_x,_10_to_12Ms_x,_13_to_18Ms_x,_19_to_24Ms_x,Over_2Y_x,AgeMissingx);

				}
				dataReader.Close();
				connection.Close();
			}

			return obj;
		}
		#endregion
		#endregion
	}
}
