using NewsAppAdmin.DAL;
using NewsAppAdmin.MODEL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace NewsAppAdmin.BLL
{
    public class BLLEmail
    {
        #region "Forgot password Email"
        public double Forgot_Password(double UserId, string strNewPassword)
        {
            try
            {
                //Get the Email Template
                ModelEmailTemplate objModelEmailTemplate = GetEmailTemplates("ADMIN_FORGOT_PASSWORD");
                string StrEmailText = null;
                string strHeaderText = null;
                string strEmailSender = null;
                string strSMTP = null;
                string StrUsername = null;
                string StrPassword = null;
                string strAdminUserEmail = null;
                string strAdminUserFullName = null;
                string strAdminUserName = null;
                BLLAdminUser objDALAdminUser = new BLLAdminUser();

                DataTable dtUserDetails = objDALAdminUser.GetAdminUserDetailsByUserNo(UserId);

                //Get Mail Data Based on User prefered Language Type
                strHeaderText = objModelEmailTemplate.EmailEN;

                //Get Data Based on Language
                StrEmailText = objModelEmailTemplate.EmailTextEN;

                strAdminUserEmail = dtUserDetails.Rows[0]["AdminUserEmail"].ToString();
                strAdminUserFullName = dtUserDetails.Rows[0]["AdminUserFirstName"].ToString() + " " + dtUserDetails.Rows[0]["AdminUserLastName"].ToString();
                strAdminUserName = dtUserDetails.Rows[0]["AdminUserName"].ToString();

                //Get SMTP Username Password
                strSMTP = objModelEmailTemplate.EmailSMTP;
                StrUsername = objModelEmailTemplate.EmailUserName;
                StrPassword = objModelEmailTemplate.Emailpassword;
                strEmailSender = objModelEmailTemplate.EmailSender;

                Hashtable shs = new Hashtable();
                shs["[$body]"] = StrEmailText;
                string strHeaderBody = DALUtility.StringReplace(strHeaderText, shs);

                //Set the Replace text for the Email body with user Details Details
                Hashtable hsh = new Hashtable();
                hsh["[$AdminName]"] = strAdminUserFullName;
                hsh["[$password]"] = strNewPassword;
                hsh["[$username]"] = strAdminUserName;

                //Get the Email body by Replacing Text in Template
                string strEmailBody = DALUtility.StringReplace(strHeaderBody, hsh);

                //Send Email

                if ((BLLEmail.SendEmail(strAdminUserEmail, objModelEmailTemplate.EmailSender, objModelEmailTemplate.EmailSubjectEN, strEmailBody, objModelEmailTemplate.EmailSMTP, objModelEmailTemplate.EmailUserName, objModelEmailTemplate.Emailpassword) > 0))
                {
                    return 1;
                }

            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "Forgot_Password");
            }

            return 0;
        }
        #endregion

        public static ModelEmailTemplate GetEmailTemplates(string TemplateName)
        {
            DataTable dtEmails = default(DataTable);
            ModelEmailTemplate objModelEmailTemplate = new ModelEmailTemplate();

            SqlParameter[] param = new SqlParameter[1];
            param[0] = new SqlParameter("@EmailTemplate", TemplateName);
            dtEmails = DALCommon.GetDataUsingDataTable("[sp_Task_GetEmailTemplates]", param);

            if (dtEmails.Rows.Count > 0)
            {
                var _with1 = objModelEmailTemplate;
                _with1.EmailEN = dtEmails.Rows[0]["EmailEN"].ToString();
                _with1.EmailAR = dtEmails.Rows[0]["EmailAR"].ToString();
                _with1.EmailTextEN = dtEmails.Rows[0]["EmailTextEN"].ToString();
                _with1.EmailTextAR = dtEmails.Rows[0]["EmailTextAR"].ToString();
                _with1.EmailSubjectAR = dtEmails.Rows[0]["EmailSubjectAR"].ToString();
                _with1.EmailSubjectEN = dtEmails.Rows[0]["EmailSubjectEN"].ToString();
                _with1.EmailSMTP = dtEmails.Rows[0]["EmailSMTP"].ToString();
                _with1.EmailSender = dtEmails.Rows[0]["EmailSender"].ToString();
                _with1.EmailUserName = dtEmails.Rows[0]["EmailUserName"].ToString();
                _with1.Emailpassword = dtEmails.Rows[0]["Emailpassword"].ToString();
            }

            return objModelEmailTemplate;
        }

        public static int SendEmail(string ToEmail, string fromEmail, string Subject, string Body, string SMTP, string Username, string Password)
        {
            try
            {
                MailMessage mailMessage = new MailMessage(new MailAddress(fromEmail), new MailAddress(ToEmail));
                System.Net.NetworkCredential SmtpUser = new System.Net.NetworkCredential(Username, Password);
                dynamic emailHeader = default(StringBuilder);
                StringBuilder emailFooter = new StringBuilder();

                //message body and subject property
                mailMessage.Subject = Subject;
                mailMessage.Body = emailHeader.ToString() + Body + emailFooter.ToString();
                mailMessage.IsBodyHtml = true;

                //create smtp client
                SmtpClient smtpMail = new SmtpClient();

                //assign smtp properties
                smtpMail.Host = SMTP;
                smtpMail.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpMail.UseDefaultCredentials = false;
                smtpMail.Credentials = SmtpUser;

                //send mail
                smtpMail.Send(mailMessage);

                return 1;
            }
            catch (Exception ex)
            {
                DALUtility.ErrorLog(ex.Message, "SendEmail");
                return 0;
            }
        }
    }
}
