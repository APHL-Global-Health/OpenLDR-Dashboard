#region Using
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OpenLDR.Dashboard.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion


namespace OpenLDR.Dashboard.API.Controllers
{
    [Route("api/openldr/globalhealth")]
    [ApiController]
    public class GlobalHealthController : ControllerBase
    {
        #region Properties
        public IConfiguration Configuration { get; }

        public string ConnectionString
        {
            get
            {
                var sqlConfiguration = Configuration.GetSection("Sql");
                if (sqlConfiguration != null) return sqlConfiguration.GetValue<string>("ConnectionString");
                else return null;
            }
        }

        public string[] ValidReturnTypes
        {
            get
            {

                var apiConfiguration = ApiConfiguration;
                if (apiConfiguration != null) return apiConfiguration.GetSection("ReturnTypes").Get<string[]>();
                else return null;
            }
        }

        public string ApiKey
        {
            get
            {
                var apiConfiguration = ApiConfiguration;
                if (apiConfiguration != null) return apiConfiguration.GetValue<string>("Key");
                else return null;
            }
        }

        public double ApiVersion
        {
            get
            {
                var apiConfiguration = ApiConfiguration;
                if (apiConfiguration != null) return apiConfiguration.GetValue<double>("Version");
                else return 1;
            }
        }

        public IConfigurationSection ApiConfiguration
        {
            get
            {
                return Configuration.GetSection("Api");
            }
        }
        #endregion

        #region Constructor
        public GlobalHealthController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        #region Routes
        [Route("~/api/openldr/globalhealth")]
        public ActionResult<string> Index()
        {
            return Core.OutputText("Global Health API version " + ApiVersion).Result;
        }

        //Example = https://[Domain]:[Port]/api/openldr/globalhealth/e98389ca62d99875ba7a4e0f2929960b/v1/json/organisms
        [HttpGet("{apikey}/v{version}/{returntype}/organisms")]
        public async Task<string> GetOrganisms(string apikey, double version, string returntype)
        {
            try
            {
                if (version.Equals(ApiVersion))
                {
                    if (!string.IsNullOrEmpty(returntype) && ValidReturnTypes != null && ValidReturnTypes.Contains(returntype.ToLower()))
                    {
                        if (!string.IsNullOrEmpty(apikey) && apikey.Length == 32 && ApiKey == apikey)
                        {
                            var list = Organism.All(ApiConfiguration, ConnectionString);
                            return await list.ToReturnType(returntype);
                        }
                        else return await Core.ToReturnType(new Response("Failed", "Invalid apikey"), returntype);
                    }
                    else return await Core.OutputText("Supported return types are json and xml only.");
                }
                else return await Core.OutputText("Version not yet implemented");
            }
            catch (Exception ex) { return await Core.ToReturnType(new Response("Failed", ex.Message), "json"); }
        }

        //Example = https://[Domain]:[Port]/api/openldr/globalhealth/e98389ca62d99875ba7a4e0f2929960b/v1/json/drugs
        [HttpGet("{apikey}/v{version}/{returntype}/drugs")]
        public async Task<string> GetDrugs(string apikey, double version, string returntype)
        {
            try
            {
                if (version.Equals(ApiVersion))
                {
                    if (!string.IsNullOrEmpty(returntype) && ValidReturnTypes != null && ValidReturnTypes.Contains(returntype.ToLower()))
                    {
                        if (!string.IsNullOrEmpty(apikey) && apikey.Length == 32 && ApiKey == apikey)
                        {
                            var list = Drug.All(ApiConfiguration, ConnectionString);
                            return await list.ToReturnType(returntype);
                        }
                        else return await Core.ToReturnType(new Response("Failed", "Invalid apikey"), returntype);
                    }
                    else return await Core.OutputText("Supported return types are json and xml only.");
                }
                else return await Core.OutputText("Version not yet implemented");
            }
            catch (Exception ex) { return await Core.ToReturnType(new Response("Failed", ex.Message), "json"); }
        }

        //Example = https://[Domain]:[Port]/api/openldr/globalhealth/e98389ca62d99875ba7a4e0f2929960b/v1/json/turnaroundtime/2020-01-01/2020-02-01
        [HttpGet("{apikey}/v{version}/{returntype}/turnaroundtime/{startDate}/{endDate}")]
        public async Task<string> GetTurnAroundTime(string apikey, double version, string returntype, string startDate, string endDate)
        {
            try
            {
                if (version.Equals(ApiVersion))
                {
                    if (!string.IsNullOrEmpty(returntype) && ValidReturnTypes != null && ValidReturnTypes.Contains(returntype.ToLower()))
                    {
                        if (!string.IsNullOrEmpty(apikey) && apikey.Length == 32 && ApiKey == apikey)
                        {
                            return await Core.ToReturnType(new Response("Successful", "Turn Around Time"), returntype);
                        }
                        else return await Core.ToReturnType(new Response("Failed", "Invalid apikey"), returntype);
                    }
                    else return await Core.OutputText("Supported return types are json and xml only.");
                }
                else return await Core.OutputText("Version not yet implemented");
            }
            catch (Exception ex) { return await Core.ToReturnType(new Response("Failed", ex.Message), "json"); }
        }

        //Example = https://[Domain]:[Port]/api/openldr/globalhealth/e98389ca62d99875ba7a4e0f2929960b/v1/json/request_breakdown/2020-01-01/2020-02-01
        [HttpGet("{apikey}/v{version}/{returntype}/request_breakdown/{startDate}/{endDate}")]
        public async Task<string> GetRequestBreakdown(string apikey, double version, string returntype, string startDate, string endDate)
        {
            try
            {
                if (version.Equals(ApiVersion))
                {
                    if (!string.IsNullOrEmpty(returntype) && ValidReturnTypes != null && ValidReturnTypes.Contains(returntype.ToLower()))
                    {
                        if (!string.IsNullOrEmpty(apikey) && apikey.Length == 32 && ApiKey == apikey)
                        {
                            return await Core.ToReturnType(new Response("Successful", "Request Breakdown"), returntype);
                        }
                        else return await Core.ToReturnType(new Response("Failed", "Invalid apikey"), returntype);
                    }
                    else return await Core.OutputText("Supported return types are json and xml only.");
                }
                else return await Core.OutputText("Version not yet implemented");
            }
            catch (Exception ex) { return await Core.ToReturnType(new Response("Failed", ex.Message), "json"); }
        }


        //Example = https://[Domain]:[Port]/api/openldr/globalhealth/e98389ca62d99875ba7a4e0f2929960b/v1/json/susceptability/2020-01-01/2020-02-01?surveillenceCode=DST01&classIn=Resistant~Sensitive~Intermediate&organismNotIn=~@2x1 @2x2~@mix1 @mix2 @dbt3&drugnotIn=ancomycin&zone=eastern&zone=western
        [HttpGet("{apikey}/v{version}/{returntype}/susceptability/{startDate}/{endDate}")]
        public async Task<string> GetSusceptability(string apikey, double version, string returntype,string startDate, string endDate, 
            [FromQuery] string surveillenceCode = "DST01", [FromQuery] string classIn = "Resistant~Sensitive~Intermediate", [FromQuery] string organismNotIn = "", [FromQuery] string drugsNotIn = "",
            [FromQuery] string[] zone = null, [FromQuery] string[] region = null, [FromQuery] string[] facility = null)
        {
            try
            {
                if (version.Equals(ApiVersion))
                {
                    if (!string.IsNullOrEmpty(returntype) && ValidReturnTypes != null && ValidReturnTypes.Contains(returntype.ToLower()))
                    {
                        if (!string.IsNullOrEmpty(apikey) && apikey.Length == 32 && ApiKey == apikey)
                        {
                            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                            {
                                var list = Susceptability.All(ApiConfiguration, startDate, endDate, ConnectionString, surveillenceCode, 
                                    classIn, organismNotIn, drugsNotIn, zone, region, facility);
                                return await list.ToReturnType(returntype);
                            }
                            else return await Core.ToReturnType(new Response("Failed", "Invalid Start Date/End Date"), returntype);
                        }
                        else return await Core.ToReturnType(new Response("Failed", "Invalid apikey"), returntype);
                    }
                    else return await Core.OutputText("Supported return types are json and xml only.");
                }
                else return await Core.OutputText("Version not yet implemented");
            }
            catch (Exception ex) { return await Core.ToReturnType(new Response("Failed", ex.Message), "json"); }
        }
        #endregion
    }
}