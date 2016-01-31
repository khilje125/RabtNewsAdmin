using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAppAdmin.MODEL
{
    public class ModelFeedMultimedia
    {
        public long MultiMediaId { get; set; }
        public long TwitterFeedDetailId { get; set; }
        public int MultiMediaType { get; set; }
        public string MultiMediaURL { get; set; }
        public string MultiMediaExtension { get; set; }
    }
}
