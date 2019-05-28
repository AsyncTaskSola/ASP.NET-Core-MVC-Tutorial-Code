using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.ViewModels
{
    public class ManageClaimsViewModel
    {
        public string userId { get; set; }
        public string CliamId { get; set; }
        public List<string> AllClims { get; set; }
    }
}
