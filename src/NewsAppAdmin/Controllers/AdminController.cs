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
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            try
            {
                if (Session["admin"] != null)
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

        // GET: /Login/
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
                        Session["admin"] = objAdminUser.UserName;
                        Session["Username"] = objAdminUser.UserFirstName + ' ' + objAdminUser.UserLastName;
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
                return View();
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
    }
}
