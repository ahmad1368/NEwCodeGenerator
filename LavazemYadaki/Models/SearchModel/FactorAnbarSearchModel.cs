using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace LavazemYadaki.Models
{
    public class FactorAnbarSearchModel : Utility.Paging
    {
        [Display(Name = "FactorAnbarIdFrom", Description = "FactorAnbarIdFrom_Description", ResourceType = typeof(Resources.FactorAnbar))]
        public int FactorAnbarIdFrom { get; set; }
        [Display(Name = "FactorAnbarIdTo", Description = "FactorAnbarIdTo_Description", ResourceType = typeof(Resources.FactorAnbar))]
        public int FactorAnbarIdTo { get; set; }
        [Display(Name = "KalaIdFrom", Description = "KalaIdFrom_Description", ResourceType = typeof(Resources.FactorAnbar))]
        public int KalaIdFrom { get; set; }
        [Display(Name = "KalaIdTo", Description = "KalaIdTo_Description", ResourceType = typeof(Resources.FactorAnbar))]
        public int KalaIdTo { get; set; }
        [Display(Name = "BestankarFrom", Description = "BestankarFrom_Description", ResourceType = typeof(Resources.FactorAnbar))]
        public int BestankarFrom { get; set; }
        [Display(Name = "BestankarTo", Description = "BestankarTo_Description", ResourceType = typeof(Resources.FactorAnbar))]
        public int BestankarTo { get; set; }
        [Display(Name = "BedehkarFrom", Description = "BedehkarFrom_Description", ResourceType = typeof(Resources.FactorAnbar))]
        public int BedehkarFrom { get; set; }
        [Display(Name = "BedehkarTo", Description = "BedehkarTo_Description", ResourceType = typeof(Resources.FactorAnbar))]
        public int BedehkarTo { get; set; }
        [Display(Name = "TarikhFrom", Description = "TarikhFrom_Description", ResourceType = typeof(Resources.FactorAnbar))]
        public DateTime? TarikhFrom { get; set; }
        [Display(Name = "TarikhTo", Description = "TarikhTo_Description", ResourceType = typeof(Resources.FactorAnbar))]
        public DateTime? TarikhTo { get; set; }
        [Display(Name = "PersonIdFrom", Description = "PersonIdFrom_Description", ResourceType = typeof(Resources.FactorAnbar))]
        public int PersonIdFrom { get; set; }
        [Display(Name = "PersonIdTo", Description = "PersonIdTo_Description", ResourceType = typeof(Resources.FactorAnbar))]
        public int PersonIdTo { get; set; }
    }
}
