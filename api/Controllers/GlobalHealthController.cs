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

        //Example = https://[Domain]:[Port]/api/globalhealth/e98389ca62d99875ba7a4e0f2929960b/v1/json/susceptibility
        [HttpGet("{apikey}/v{version}/{returntype}/susceptibility")]
        public async Task<string> GetSusceptibility(string apikey, double version, string returntype)
        {
            try
            {
                if (version.Equals(ApiVersion))
                {
                    if (!string.IsNullOrEmpty(returntype) && ValidReturnTypes != null && ValidReturnTypes.Contains(returntype.ToLower()))
                    {
                        if (!string.IsNullOrEmpty(apikey) && !string.IsNullOrEmpty(ApiKey) && apikey.Length == 32 && ApiKey == apikey)
                        {
                            return await Task.FromResult<string>("Get susceptibility data");
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