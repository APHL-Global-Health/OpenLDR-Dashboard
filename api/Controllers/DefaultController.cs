#region Using
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
#endregion

namespace OpenLDR.Dashboard.API.Controllers
{
    [Route("default")]
    [EnableCors("AllowOrigin")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        #region Properties
        public IConfiguration Configuration { get; }

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
        public DefaultController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        #region Routes
        [Route("~/")]
        public ActionResult<string> Index()
        {
            return Core.OutputText("OpenLDR Dashboard API version " + ApiVersion).Result;
        }
        #endregion
    }
}