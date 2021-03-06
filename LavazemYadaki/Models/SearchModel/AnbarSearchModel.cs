using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace LavazemYadaki.Models {
public class AnbarSearchModel : Utility.Paging {
[Display(Name = "AnbarIdFrom", Description = "AnbarIdFrom_Description", ResourceType = typeof(Resources.Anbar))]
public int AnbarIdFrom { get; set; }
[Display(Name = "AnbarIdTo", Description = "AnbarIdTo_Description", ResourceType = typeof(Resources.Anbar))]
public int AnbarIdTo { get; set; }
[Display(Name = "TitleAnbar", Description = "TitleAnbar_Description", ResourceType = typeof(Resources.Anbar))]
public string TitleAnbar { get; set; }
}
}
