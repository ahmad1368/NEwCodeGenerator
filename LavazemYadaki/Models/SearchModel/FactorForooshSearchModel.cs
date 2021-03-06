using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace LavazemYadaki.Models {
public class FactorForooshSearchModel : Utility.Paging {
[Display(Name = "FactorForooshIdFrom", Description = "FactorForooshIdFrom_Description", ResourceType = typeof(Resources.FactorForoosh))]
public int FactorForooshIdFrom { get; set; }
[Display(Name = "FactorForooshIdTo", Description = "FactorForooshIdTo_Description", ResourceType = typeof(Resources.FactorForoosh))]
public int FactorForooshIdTo { get; set; }
[Display(Name = "KalaIdFrom", Description = "KalaIdFrom_Description", ResourceType = typeof(Resources.FactorForoosh))]
public int KalaIdFrom { get; set; }
[Display(Name = "KalaIdTo", Description = "KalaIdTo_Description", ResourceType = typeof(Resources.FactorForoosh))]
public int KalaIdTo { get; set; }
[Display(Name = "CountKalaFrom", Description = "CountKalaFrom_Description", ResourceType = typeof(Resources.FactorForoosh))]
public int CountKalaFrom { get; set; }
[Display(Name = "CountKalaTo", Description = "CountKalaTo_Description", ResourceType = typeof(Resources.FactorForoosh))]
public int CountKalaTo { get; set; }
[Display(Name = "FiFrom", Description = "FiFrom_Description", ResourceType = typeof(Resources.FactorForoosh))]
public int FiFrom { get; set; }
[Display(Name = "FiTo", Description = "FiTo_Description", ResourceType = typeof(Resources.FactorForoosh))]
public int FiTo { get; set; }
}
}
