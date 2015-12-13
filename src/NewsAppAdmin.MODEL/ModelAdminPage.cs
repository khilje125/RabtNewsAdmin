using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppAdmin.MODEL
{
    public class ModelAdminPage
    {
        public double AdminPageID { get; set; }

        [Display(Name = "Admin page name title english")]
        [Required(ErrorMessage = "Please enter page title")]
        public string AdminPageNameEn { get; set; }

        [Display(Name = "Admin page name title arabic")]
        public string AdminPageNameAr { get; set; }

        [Display(Name = "Admin Page URL")]
        [Required(ErrorMessage = "Please enter page URL")]
        public string AdminPageURL { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Is show in menu")]
        public bool IsShowInMenu { get; set; }

        [Display(Name = "AdminPageAddedBy")]
        public double AdminPageAddedBy { get; set; }

        public DateTime AddedDate { get; set; }
       
    }
}
