using Newtonsoft.Json;
using OpenLDR.Dashboard.API.Utils;
using Optimizer.Logging;
using Optimizer.Models;
using System;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Optimizer
{
    public static class Core
    {
        public static ILog Logger;

        #region ToQueryString
        public static string ToQueryString(this NameValueCollection collection)
        {
            var array = (from key in collection.AllKeys
                         from value in collection.GetValues(key)
                         select string.Format("{0}={1}", key, value)).ToArray();
            return "?" + string.Join("&", array);
        }
        #endregion

        #region Beautify
        public static string Beautify(this string xml)
        {
            var doc = new XmlDocument();
            var sb = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                NewLineHandling = NewLineHandling.Replace
            };

            doc.LoadXml(xml);
            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                doc.Save(writer);
            }
            return sb.ToString();
        }
        #endregion

        #region ToJSON
        public static string ToJSON(this object Object)
        {
            return JsonConvert.SerializeObject(Object);
        }
        #endregion

        #region ToXML
        public static string ToXML(this object Object, string rootName = "")
        {
            using (StringWriter stream = new StringWriter())
            {
                var namepsaces = new XmlSerializerNamespaces();
                namepsaces.Add("", "");

                var xmlSerializer = new XmlSerializer(Object.GetType());
                if (!string.IsNullOrEmpty(rootName)) xmlSerializer = new XmlSerializer(Object.GetType(), new XmlRootAttribute(rootName));

                xmlSerializer.UnknownElement += (s, e) =>
                {
                    /*throw new Exception("Unknown element " + e.Element.Name + " found in "
                        + e.ObjectBeingDeserialized.ToString() + " in line "
                        + e.LineNumber + " at position " + e.LinePosition);*/
                };

                var settings = new System.Xml.XmlWriterSettings();
                settings.Encoding = Encoding.GetEncoding("ISO-8859-1");
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                settings.ConformanceLevel = System.Xml.ConformanceLevel.Document;

                using (var writer = System.Xml.XmlWriter.Create(stream, settings))
                {
                    using (var write = new XmlWriterEE(writer))
                    {
                        xmlSerializer.Serialize(write, Object, namepsaces);
                        return stream.ToString().Beautify();
                    }
                }
            }
        }
        #endregion

        #region ToReturnType
        public static async Task<string> ToReturnType(this object data, string returntype, string rootName = "")
        {
            string value = null;
            if (returntype.Trim().ToLower().Equals("json")) value = data.ToJSON();
            else if (returntype.Trim().ToLower().Equals("xml")) value = data.ToXML(rootName);

            return await Task.FromResult<string>(value);
        }
        #endregion

        #region OutputText
        public static async Task<string> OutputText(string data)
        {
            return await Task.FromResult<string>(data);
        }
        #endregion

        #region ToInt
        public static int ToInt(this SqlDataReader reader, string name)
        {
            int result = 0;
            var value = reader[name].ToString();
            if (!string.IsNullOrEmpty(value) && int.TryParse(value, out result))
            {

            }
            return result;
        }
        #endregion

        #region ToDouble
        public static double ToDouble(this SqlDataReader reader, string name)
        {
            double result = 0;
            var value = reader[name].ToString();
            if (!string.IsNullOrEmpty(value) && double.TryParse(value, out result))
            {

            }
            return result;
        }
        #endregion

        #region ToBoolean
        public static bool ToBoolean(this SqlDataReader reader, string name)
        {
            bool result = false;
            var value = reader[name].ToString();
            if (!string.IsNullOrEmpty(value) && bool.TryParse(value, out result))
            {

            }
            return result;
        }
        #endregion

        #region ToByte
        public static byte ToByte(this SqlDataReader reader, string name)
        {
            byte result = 0;
            var value = reader[name].ToString();
            if (!string.IsNullOrEmpty(value) && byte.TryParse(value, out result))
            {

            }
            return result;
        }
        #endregion

        #region ToDateTime
        public static string ToDateTime(this SqlDataReader dataReader, string key)
        {
            try
            {
                var ordinal = dataReader.GetOrdinal(key);
                if (ordinal != -1)
                {
                    var dt = dataReader.GetDateTime(ordinal);
                    if (dt != null) return dt.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            catch { }

            return null;
        }
        #endregion

        #region ToSqlDatetime
        public static object ToSqlDatetimeEx(this string source)
        {
            if (string.IsNullOrEmpty(source)) return DBNull.Value;

            DateTime dt;
            if (DateTime.TryParse(source, out dt))
            {
                if (dt.Day >= 1 && dt.Month >= 1 && dt.Year >= 1753) return dt;
                else return DBNull.Value;
            }
            else return DBNull.Value;// source;
        }
        #endregion

        #region ToStringOrNull
        public static string ToStringOrNull(this object value)
        {
            if (value == null) return null;

            try { return value.ToString().Trim(); }
            catch { }

            return null;
        }
        #endregion

        #region ToSqlJoin
        public static string ToSqlJoin(this Array list, string separator)
        {
            if (list.Length == 0) return "''";
            return String.Join(separator, list);
        }
        #endregion

        #region GetQueryScript
        public static string GetQueryScript(dynamic configuration, string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                try
                {
                    return ((Newtonsoft.Json.Linq.JArray)configuration)
                       .Where(p =>
                       {
                           var k = ((Newtonsoft.Json.Linq.JObject)p).GetValue("key", StringComparison.OrdinalIgnoreCase)?.ToString();
                           return k != null && k.ToLower().Equals(key.ToLower());
                       })
                       .Select(p => { return p.ToObject<QueriesHelper>().ToSql(); })
                       .FirstOrDefault();
                }
                catch(Exception ex)
                {
                    if(Logger != null) Logger.Error(string.Format("Error fetching query script : {0}", ex.Message));
                }
            }
            return null;
        }
        #endregion

        #region ToSqlValue
        public static object ToSqlValue(this string source)
        {
            if (string.IsNullOrEmpty(source)) return DBNull.Value;
            return source;
        }

        public static object ToSqlValue(this int source)
        {
            return source;
        }

        public static object ToSqlValue(this float source)
        {
            return source;
        }

        public static object ToSqlValue(this double source)
        {
            return source;
        }

        public static object ToSqlValue(this bool source)
        {
            return source;
        }
        #endregion
    }
}
