#region Using
using System;
using System.Threading.Tasks;
#endregion

namespace OpenLDR.Dashboard.API.Models
{
    [Serializable]
    public class Response
    {
        #region Properties
        public string Status { get; set; }

        public string Data { get; set; }
        #endregion

        #region Constructor
        public Response()
        {

        }

        public Response(string status, string data)
        {
            this.Status = status;
            this.Data = data;
        }
        #endregion

        #region ToReturnType
        public async Task<string> ToReturnType(string returntype, string rootName = "")
        {
            string value = null;
            if (returntype.Trim().ToLower().Equals("json")) value = this.ToJSON();
            else if (returntype.Trim().ToLower().Equals("xml")) value = this.ToXML(rootName);

            return await Task.FromResult<string>(value);
        }
        #endregion
    }
}
