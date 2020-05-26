﻿#region Using
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
    [Route("api/openldr/general")]
    [ApiController]
    public class GeneralController : ControllerBase
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
                
                var apiConfiguration = Configuration.GetSection("Api");
                if (apiConfiguration != null) return apiConfiguration.GetSection("ReturnTypes").Get<string[]>();
                else return null;
            }
        }

        public string ApiKey
        {
            get
            {
                var apiConfiguration = Configuration.GetSection("Api");
                if (apiConfiguration != null) return apiConfiguration.GetValue<string>("Key");
                else return null;
            }
        }

        public double ApiVersion
        {
            get
            {
                var apiConfiguration = Configuration.GetSection("Api");
                if (apiConfiguration != null) return apiConfiguration.GetValue<double>("Version");
                else return 1;
            }
        }
        #endregion

        #region Constructor
        public GeneralController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        #region Routes
        [Route("~/api/openldr/general")]
        public ActionResult<string> Index()
        {
            return Core.OutputText("General API version " + ApiVersion).Result;
        }

        //Example = https://[Domain]:[Port]/api/general/e98389ca62d99875ba7a4e0f2929960b/v1/json/viralload
        [HttpGet("{apikey}/v{version}/{returntype}/viralload")]
        public async Task<string> GetViralLoad(string apikey, double version, string returntype)
        {
            try
            {
                if (version.Equals("v"+ApiVersion))
                {
                    if (!string.IsNullOrEmpty(returntype) && ValidReturnTypes != null && ValidReturnTypes.Contains(returntype.ToLower()))
                    {
                        if (!string.IsNullOrEmpty(apikey) && !string.IsNullOrEmpty(ApiKey) && apikey.Length == 32 && ApiKey == apikey)
                        {
                            return await Task.FromResult<string>("Get viralload data");
                        }
                        else return await Core.ToReturnType(new Response("Failed", "Invalid apikey"), returntype);
                    }
                    else return await Core.OutputText("Supported return types are json and xml only.");
                }
                else return await Core.OutputText("Version not yet implemented");
            }
            catch (Exception ex) { return await Core.ToReturnType(new Response("Failed", ex.Message), "json"); }
        }

        //Example = https://[Domain]:[Port]/api/general/e98389ca62d99875ba7a4e0f2929960b/v1/json/eid
        [HttpGet("{apikey}/v{version}/{returntype}/eid")]
        public async Task<string> GetEID(string apikey, double version, string returntype)
        {
            try
            {
                if (version.Equals(ApiVersion))
                {
                    if (!string.IsNullOrEmpty(returntype) && ValidReturnTypes != null && ValidReturnTypes.Contains(returntype.ToLower()))
                    {
                        if (!string.IsNullOrEmpty(apikey) && !string.IsNullOrEmpty(ApiKey) && apikey.Length == 32 && ApiKey == apikey)
                        {
                            return await Task.FromResult<string>("Get eid data");
                        }
                        else return await Core.ToReturnType(new Response("Failed", "Invalid apikey"), returntype);
                    }
                    else return await Core.OutputText("Supported return types are json and xml only.");
                }
                else return await Core.OutputText("Version not yet implemented");
            }
            catch (Exception ex) { return await Core.ToReturnType(new Response("Failed", ex.Message), "json"); }
        }

        //Example = https://[Domain]:[Port]/api/general/e98389ca62d99875ba7a4e0f2929960b/v1/json/transmission/2020/1
        [HttpGet("{apikey}/v{version}/{returntype}/transmission/{year}/{month}")]
        public async Task<string> GetTransmission(string apikey, double version, string returntype, int year, int month)
        {
            try
            {
                if (version.Equals(ApiVersion))
                {
                    if (!string.IsNullOrEmpty(returntype) && ValidReturnTypes != null && ValidReturnTypes.Contains(returntype.ToLower()))
                    {
                        if (!string.IsNullOrEmpty(apikey) && !string.IsNullOrEmpty(ApiKey) && apikey.Length == 32 && ApiKey == apikey)
                        {
                            if (year > 2010)
                            {
                                if (month > 0 && month < 13)
                                {
                                    if (!string.IsNullOrEmpty(ConnectionString))
                                    {
                                        var tests = new List<string>() { "HIVVL", "HIVPC", "EID" };

                                        var otherTests = Transmission.All(tests, year, month, ConnectionString, true);
                                        var OpenLDRTests = Transmission.All(tests, year, month, ConnectionString, false);

                                        var list = new List<Transmission>();
                                        list.AddRange(otherTests);
                                        list.AddRange(OpenLDRTests);

                                        return await list.ToReturnType(returntype);
                                    }
                                    else return await Core.ToReturnType(new Response("Failed", "Month can only be 1 to 12"), returntype);
                                }
                                else return await Core.ToReturnType(new Response("Failed", "Month can only be 1 to 12"), returntype);
                            }
                            else return await Core.ToReturnType(new Response("Failed", "Year is lower than the minimum acceptable value"), returntype);
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