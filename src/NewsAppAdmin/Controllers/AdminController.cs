using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsAppAdmin.MODEL;
using NewsAppAdmin.BLL;
using NewsAppAdmin.DAL;
using System.Data;

namespace NewsAppAdmin.Controllers
{
    
    public class AdminController : Controller
    {
        BLLAdminUser bllAdminUser = new BLLAdminUser();
        BLLFeed bllFeed = new BLLFeed();
        ModelFeed objModelFeed = new ModelFeed();
        ModelAdminUser objModelAdminUser = new ModelAdminUser();
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            try
            {
                if (Session[DALVariables.AdminUserNo] != null)
                {
                    return View();
                }
                return RedirectToAction("Login", "Admin");
            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "AdminController, Index");
            }
            return View();
        }

        // GET: /Login/

        public ActionResult Login()
        {
            return View();
        }

        // POST: /Login/
        [HttpPost]
        public ActionResult Login(string UserName, string UserPassword, string RememberMe)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ModelAdminUser objAdminUser = new ModelAdminUser();
                    DataTable userDetails = new DataTable();
                    userDetails = bllAdminUser.GetAdminUserDetailsByLogin(UserName, UserPassword);
                    if (userDetails.Rows.Count > 0)
                    {
                        objAdminUser.UserAccountNo = Convert.ToDouble(userDetails.Rows[0]["AdminUserNo"]);
                        objAdminUser.UserName = Convert.ToString(userDetails.Rows[0]["AdminUserName"]);
                        objAdminUser.UserFirstName = Convert.ToString(userDetails.Rows[0]["AdminUserFirstName"]);
                        objAdminUser.UserLastName = Convert.ToString(userDetails.Rows[0]["AdminUserLastName"]);
                        objAdminUser.UserEmail = Convert.ToString(userDetails.Rows[0]["AdminUserEmail"]);
                        objAdminUser.UserMobileNumber = Convert.ToDouble(userDetails.Rows[0]["AdminUserMobile"]);
                        
                        Session[DALVariables.AdminUserNo] = objAdminUser.UserAccountNo;
                        Session[DALVariables.UserEmail] = objAdminUser.UserEmail;
                        Session[DALVariables.UserFirstName] = objAdminUser.UserFirstName;
                        Session[DALVariables.UserLastName] = objAdminUser.UserLastName;
                        Session[DALVariables.UserName] = objAdminUser.UserName;
                        //Session[DALVariables.ProfileImage] = "";
                        return RedirectToAction("Index", "Admin");
                    }
                    ModelState.AddModelError("", "No User Found!,Re-check login details");
                    return View();

                }
                catch (Exception ex)
                {
                    DALUtility.ErrorLog(ex.Message, "AdminController, Login");
                }
            }
            else
            {
                ModelState.AddModelError("","Check error of form; Please correct to continue!");
            }
            return View();
        }

        // GET: /Logout/
        public ActionResult Logout()
        {
            try
            {
                System.Web.Security.FormsAuthentication.SignOut();
                Session.Abandon();
                return RedirectToAction("Login", "Admin");
            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "AdminController, Logout");
            }
            return View();
        }

        // GET: /SignUp/

        public ActionResult SignUp()
        {
            return View();
        }

        // GET: /Category/

        public ActionResult Category()
        {
            return View();
        }

        // GET: /SearchCategory/

        public ActionResult SearchCategory()
        {
            return View();
        }

        // GET: /AddRssLink/

        public ActionResult AddRssLink()
        {
            return View();
        }

        // POST: /AddRssLink/
        [HttpPost]
        public ActionResult AddRssLink(ModelFeed objModelFeed)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bllFeed = new BLLFeed();
                    decimal result = 0;
                    result = bllFeed.InsertFeedData(objModelFeed);
                    if (result > 0)
                    {
                        return View();
                    }
                    ModelState.AddModelError("", "No User Found!,Re-check login details");
                    return View();

                }
                catch (Exception ex)
                {
                    DALUtility.ErrorLog(ex.Message, "AdminController, Login");
                }
            }
            else
            {
                ModelState.AddModelError("", "Check error of form; Please correct to continue!");
            }
            return View();
        }

        // GET: /RssLinkList/

        public PartialViewResult RssLinkList(int page = 0)
        {
            try
            
            {
                List<ModelFeed> bllFeedList =  new List<ModelFeed>();
                bllFeedList = bllFeed.GetFeedDataList(page);
                return PartialView("_FeedList", bllFeedList);
                //return PartialView("_FeedDataList", bllFeedList);
                //return PartialView(customview("_FeedDataList", "Admin"), bllFeedList);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string customview(string view, string controller)
        {
            if (string.IsNullOrEmpty(controller))
                controller = Request.RequestContext.RouteData.Values["Controller"].ToString();
            return String.Format("~/Views/Shared/{0}/{1}.cshtml", controller, view);
        }

    }
}
