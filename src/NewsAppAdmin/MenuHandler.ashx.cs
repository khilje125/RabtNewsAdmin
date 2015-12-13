using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NewsAppAdmin.MODEL;
using NewsAppAdmin.BLL;
using System.Web.Script.Serialization;

namespace NewsAppAdmin
{
    /// <summary>
    /// Summary description for MenuHandler
    /// </summary>
    public class MenuHandler : IHttpHandler
    {
        BLLAdminPages objBLLAdminPages = new BLLAdminPages(); 
        public void ProcessRequest(HttpContext context)
        {
            List<ModelLeftMenu> LeftMenu = new List<ModelLeftMenu>();
            LeftMenu = objBLLAdminPages.LeftMenu();
            JavaScriptSerializer js = new JavaScriptSerializer();
            context.Response.Write(js.Serialize(LeftMenu));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}