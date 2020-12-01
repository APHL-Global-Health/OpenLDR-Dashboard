using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;

namespace OpenLDR.Dashboard.API.Models
{
	[Serializable]
	[XmlRoot(ElementName = "VLClientBasedList")]
	public class VLClientBasedList
	{
		#region Properties 
		public string Province { get; set; }

		public string District { get; set; }

		public string Facility { get; set; }

		public int Clients { get; set; }


		public int SuppressedC { get; set; }

		public int UnsuppressedC { get; set; }

		public int UndetectableC { get; set; }

		public int lt01m { get; set; }
		public int _1to4m { get; set; }
		public int _5to9m { get; set; }
		public int _10to14m { get; set; }
		public int _15to19m { get; set; }
		public int _20to24m { get; set; }
		public int _25to29m { get; set; }
		public int _30to34m { get; set; }
		public int _35to39m { get; set; }
		public int _40to44m { get; set; }
		public int _45to49m { get; set; }
		public int _50plusm { get; set; }
		public int AgeMissingm { get; set; }
		public int lt01f { get; set; }
		public int _1to4f { get; set; }
		public int _5to9f { get; set; }
		public int _10to14f { get; set; }
		public int _15to19f { get; set; }
		public int _20to24f { get; set; }
		public int _25to29f { get; set; }
		public int _30to34f { get; set; }
		public int _35to39f { get; set; }
		public int _40to44f { get; set; }
		public int _45to49f { get; set; }
		public int _50plusf { get; set; }
		public int AgeMissingf { get; set; }
		public int lt01i { get; set; }
		public int _1to4i { get; set; }
		public int _5to9i { get; set; }
		public int _10to14i { get; set; }
		public int _15to19i { get; set; }
		public int _20to24i { get; set; }
		public int _25to29i { get; set; }
		public int _30to34i { get; set; }
		public int _35to39i { get; set; }
		public int _40to44i { get; set; }
		public int _45to49i { get; set; }
		public int _50plusi { get; set; }
		public int AgeMissingi { get; set; }

		public int lt01u { get; set; }
		public int _1to4u { get; set; }
		public int _5to9u { get; set; }
		public int _10to14u { get; set; }
		public int _15to19u { get; set; }
		public int _20to24u { get; set; }
		public int _25to29u { get; set; }
		public int _30to34u { get; set; }
		public int _35to39u { get; set; }
		public int _40to44u { get; set; }
		public int _45to49u { get; set; }
		public int _50plusu { get; set; }
		public int AgeMissingu { get; set; }

		public int lt01x { get; set; }
		public int _1to4x { get; set; }
		public int _5to9x { get; set; }
		public int _10to14x { get; set; }
		public int _15to19x { get; set; }
		public int _20to24x { get; set; }
		public int _25to29x { get; set; }
		public int _30to34x { get; set; }
		public int _35to39x { get; set; }
		public int _40to44x { get; set; }
		public int _45to49x { get; set; }
		public int _50plusx { get; set; }
		public int AgeMissingx { get; set; }



#endregion

#region Constructor
public VLClientBasedList()
		{
		}

		public VLClientBasedList(string Province, string District, string Facility, int Clients, int SuppressedC, int UnsuppressedC,int UndetectableC, int lt01m, int _1to4m, int _5to9m, int _10to14m, int _15to19m, int _20to24m, int _25to29m, int _30to34m, int _35to39m, int _40to44m, int _45to49m, int _50plusm, int AgeMissingm, int lt01f, int _1to4f, int _5to9f, int _10to14f, int _15to19f, int _20to24f, int _25to29f, int _30to34f, int _35to39f, int _40to44f, int _45to49f, int _50plusf, int AgeMissingf, int lt01i, int _1to4i, int _5to9i, int _10to14i, int _15to19i, int _20to24i, int _25to29i, int _30to34i, int _35to39i, int _40to44i, int _45to49i, int _50plusi, int AgeMissingi, int lt01u, int _1to4u, int _5to9u, int _10to14u, int _15to19u, int _20to24u, int _25to29u, int _30to34u, int _35to39u, int _40to44u, int _45to49u, int _50plusu, int AgeMissingu, int lt01x, int _1to4x, int _5to9x, int _10to14x, int _15to19x, int _20to24x, int _25to29x, int _30to34x, int _35to39x, int _40to44x, int _45to49x, int _50plusx, int AgeMissingx)
			: this()
		{
			this.Province = Province;
			this.District = District;
			this.Facility = Facility;
			this.Clients = Clients;
			this.SuppressedC = SuppressedC;
			this.UnsuppressedC = UnsuppressedC;
			this.UndetectableC = UndetectableC;
			this.lt01m = lt01m;
			this._1to4m = _1to4m;
			this._5to9m = _5to9m;
			this._10to14m = _10to14m;
			this._15to19m = _15to19m;
			this._20to24m = _20to24m;
			this._25to29m = _25to29m;
			this._30to34m = _30to34m;
			this._35to39m = _35to39m;
			this._40to44m = _40to44m;
			this._45to49m = _45to49m;
			this._50plusm = _50plusm;
			this.AgeMissingm = AgeMissingm;

			this.lt01f = lt01f;
			this._1to4f = _1to4f;
			this._5to9f = _5to9f;
			this._10to14f = _10to14f;
			this._15to19f = _15to19f;
			this._20to24f = _20to24f;
			this._25to29f = _25to29f;
			this._30to34f = _30to34f;
			this._35to39f = _35to39f;
			this._40to44f = _40to44f;
			this._45to49f = _45to49f;
			this._50plusf = _50plusf;
			this.AgeMissingf = AgeMissingf;

			this.lt01i = lt01i;
			this._1to4i = _1to4i;
			this._5to9i = _5to9i;
			this._10to14i = _10to14i;
			this._15to19i = _15to19i;
			this._20to24i = _20to24i;
			this._25to29i = _25to29i;
			this._30to34i = _30to34i;
			this._35to39i = _35to39i;
			this._40to44i = _40to44i;
			this._45to49i = _45to49i;
			this._50plusi = _50plusi;
			this.AgeMissingi = AgeMissingi;

			this.lt01u = lt01u;
			this._1to4u = _1to4u;
			this._5to9u = _5to9u;
			this._10to14u = _10to14u;
			this._15to19u = _15to19u;
			this._20to24u = _20to24u;
			this._25to29u = _25to29u;
			this._30to34u = _30to34u;
			this._35to39u = _35to39u;
			this._40to44u = _40to44u;
			this._45to49u = _45to49u;
			this._50plusu = _50plusu;
			this.AgeMissingu = AgeMissingu;

			this.lt01x = lt01x;
			this._1to4x = _1to4x;
			this._5to9x = _5to9x;
			this._10to14x = _10to14x;
			this._15to19x = _15to19x;
			this._20to24x = _20to24x;
			this._25to29x = _25to29x;
			this._30to34x = _30to34x;
			this._35to39x = _35to39x;
			this._40to44x = _40to44x;
			this._45to49x = _45to49x;
			this._50plusx = _50plusx;
			this.AgeMissingx = AgeMissingx;

		
		}
		#endregion

		#region Methods
		#region All
		public static List<VLClientBasedList> All(IConfigurationSection configuration, string connectionString, string province, string district, DateTime? stdate, DateTime? edate)
		{

			var list = new List<VLClientBasedList>();
			var query = Core.GetQueryScript(configuration, "general_getHIVVLCB_List");
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
				//if(facility == null)
				//	cmd.Parameters.Add("@Facility", SqlDbType.VarChar).Value = DBNull.Value;
				//else cmd.Parameters.Add("@Facility", SqlDbType.VarChar).Value = facility;
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
					
					var Clients = dataReader.ToInt("Clients");
					var SuppressedC = dataReader.ToInt("SuppressedC");
					var UnsuppressedC = dataReader.ToInt("UnsuppressedC");
					var UndetectableC = dataReader.ToInt("UndetectableC");
					var lt01m = dataReader.ToInt("lt01m");
					var _1to4m = dataReader.ToInt("1to4m");
					var _5to9m = dataReader.ToInt("5to9m");
					var _10to14m = dataReader.ToInt("10to14m");
					var _15to19m = dataReader.ToInt("15to19m");
					var _20to24m = dataReader.ToInt("20to24m");
					var _25to29m = dataReader.ToInt("25to29m");
					var _30to34m = dataReader.ToInt("30to34m");
					var _35to39m = dataReader.ToInt("35to39m");
					var _40to44m = dataReader.ToInt("40to44m");
					var _45to49m = dataReader.ToInt("45to49m");
					var _50plusm = dataReader.ToInt("50plusm");
					var AgeMissingm = dataReader.ToInt("AgeMissingm");

					var lt01f =  dataReader.ToInt("lt01f");
					var _1to4f = dataReader.ToInt("1to4f");
					var _5to9f = dataReader.ToInt("5to9f");
					var _10to14f = dataReader.ToInt("10to14f");
					var _15to19f = dataReader.ToInt("15to19f");
					var _20to24f = dataReader.ToInt("20to24f");
					var _25to29f = dataReader.ToInt("25to29f");
					var _30to34f = dataReader.ToInt("30to34f");
					var _35to39f = dataReader.ToInt("35to39f");
					var _40to44f = dataReader.ToInt("40to44f");
					var _45to49f = dataReader.ToInt("45to49f");
					var _50plusf = dataReader.ToInt("50plusf");
					var AgeMissingf = dataReader.ToInt("AgeMissingf");

					var lt01i = dataReader.ToInt("lt01i");
					var _1to4i = dataReader.ToInt("1to4i");
					var _5to9i = dataReader.ToInt("5to9i");
					var _10to14i = dataReader.ToInt("10to14i");
					var _15to19i = dataReader.ToInt("15to19i");
					var _20to24i = dataReader.ToInt("20to24i");
					var _25to29i = dataReader.ToInt("25to29i");
					var _30to34i = dataReader.ToInt("30to34i");
					var _35to39i = dataReader.ToInt("35to39i");
					var _40to44i = dataReader.ToInt("40to44i");
					var _45to49i = dataReader.ToInt("45to49i");
					var _50plusi = dataReader.ToInt("50plusi");
					var AgeMissingi = dataReader.ToInt("AgeMissingi");

					var lt01u = dataReader.ToInt("lt01u");
					var _1to4u = dataReader.ToInt("1to4u");
					var _5to9u = dataReader.ToInt("5to9u");
					var _10to14u = dataReader.ToInt("10to14u");
					var _15to19u = dataReader.ToInt("15to19u");
					var _20to24u = dataReader.ToInt("20to24u");
					var _25to29u = dataReader.ToInt("25to29u");
					var _30to34u = dataReader.ToInt("30to34u");
					var _35to39u = dataReader.ToInt("35to39u");
					var _40to44u = dataReader.ToInt("40to44u");
					var _45to49u = dataReader.ToInt("45to49u");
					var _50plusu = dataReader.ToInt("50plusu");
					var AgeMissingu = dataReader.ToInt("AgeMissingu");

					var lt01x = dataReader.ToInt("lt01x");
					var _1to4x = dataReader.ToInt("1to4x");
					var _5to9x = dataReader.ToInt("5to9x");
					var _10to14x = dataReader.ToInt("10to14x");
					var _15to19x = dataReader.ToInt("15to19x");
					var _20to24x = dataReader.ToInt("20to24x");
					var _25to29x = dataReader.ToInt("25to29x");
					var _30to34x = dataReader.ToInt("30to34x");
					var _35to39x = dataReader.ToInt("35to39x");
					var _40to44x = dataReader.ToInt("40to44x");
					var _45to49x = dataReader.ToInt("45to49x");
					var _50plusx = dataReader.ToInt("50plusx");
					var AgeMissingx = dataReader.ToInt("AgeMissingx");


					list.Add(new VLClientBasedList(Province, District, Facility, Clients, SuppressedC, UnsuppressedC, UndetectableC,lt01m,_1to4m,_5to9m,_10to14m,_15to19m,_20to24m,_25to29m,_30to34m,_35to39m,_40to44m,_45to49m,_50plusm,AgeMissingm,lt01f,_1to4f,_5to9f,_10to14f,_15to19f,_20to24f,_25to29f,_30to34f,_35to39f,_40to44f,_45to49f,_50plusf,AgeMissingf,lt01i,_1to4i,_5to9i,_10to14i,_15to19i,_20to24i,_25to29i,_30to34i,_35to39i,_40to44i,_45to49i,_50plusi,AgeMissingi,lt01u,_1to4u,_5to9u,_10to14u,_15to19u,_20to24u,_25to29u,_30to34u,_35to39u,_40to44u,_45to49u,_50plusu, AgeMissingu, lt01x, _1to4x, _5to9x, _10to14x, _15to19x, _20to24x, _25to29x, _30to34x, _35to39x, _40to44x, _45to49x, _50plusx, AgeMissingx));
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
