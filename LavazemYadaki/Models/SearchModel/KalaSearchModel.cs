using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace LavazemYadaki.Models {
public class KalaSearchModel : Utility.Paging {
[Display(Name = "KalaIdFrom", Description = "KalaIdFrom_Description", ResourceType = typeof(Resources.Kala))]
public int KalaIdFrom { get; set; }
[Display(Name = "KalaIdTo", Description = "KalaIdTo_Description", ResourceType = typeof(Resources.Kala))]
public int KalaIdTo { get; set; }
[Display(Name = "TitleKala", Description = "TitleKala_Description", ResourceType = typeof(Resources.Kala))]
public string TitleKala { get; set; }
[Display(Name = "CodeKala", Description = "CodeKala_Description", ResourceType = typeof(Resources.Kala))]
public string CodeKala { get; set; }
[Display(Name = "OrderPointFrom", Description = "OrderPointFrom_Description", ResourceType = typeof(Resources.Kala))]
public int OrderPointFrom { get; set; }
[Display(Name = "OrderPointTo", Description = "OrderPointTo_Description", ResourceType = typeof(Resources.Kala))]
public int OrderPointTo { get; set; }
[Display(Name = "AnbarIdFrom", Description = "AnbarIdFrom_Description", ResourceType = typeof(Resources.Kala))]
public int AnbarIdFrom { get; set; }
[Display(Name = "AnbarIdTo", Description = "AnbarIdTo_Description", ResourceType = typeof(Resources.Kala))]
public int AnbarIdTo { get; set; }
}
}
