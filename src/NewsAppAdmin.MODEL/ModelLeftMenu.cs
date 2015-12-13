using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppAdmin.MODEL
{
    public class ModelLeftMenu
    {
        public int Id { get; set; }
        public string MenuText { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }
        public string URL  { get; set; }
        public List<ModelLeftMenu> List { get; set; }

    }
}
