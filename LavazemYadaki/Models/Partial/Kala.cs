using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace LavazemYadaki.Models {
[MetadataType(typeof(KalaMetadata))]
public partial class Kala {
public override string ToString()
{
//TO DO: Replace with a meaningful code
return base.ToString();
}
}
public partial class KalaMetadata
{
[Display(Name = "KalaId", Description = "KalaId_Description", ResourceType = typeof(Resources.Kala))]
public int KalaId { get; set; }
[Display(Name = "TitleKala", Description = "TitleKala_Description", ResourceType = typeof(Resources.Kala))]
public string TitleKala { get; set; }
[Display(Name = "CodeKala", Description = "CodeKala_Description", ResourceType = typeof(Resources.Kala))]
public string CodeKala { get; set; }
[Display(Name = "OrderPoint", Description = "OrderPoint_Description", ResourceType = typeof(Resources.Kala))]
public int OrderPoint { get; set; }
[Display(Name = "AnbarId", Description = "AnbarId_Description", ResourceType = typeof(Resources.Kala))]
public virtual ICollection<Anbar> AnbarId { get; set; }
}
}
