using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace LavazemYadaki.Models {
[MetadataType(typeof(AnbarMetadata))]
public partial class Anbar {
public override string ToString()
{
//TO DO: Replace with a meaningful code
return base.ToString();
}
}
public partial class AnbarMetadata
{
[Display(Name = "AnbarId", Description = "AnbarId_Description", ResourceType = typeof(Resources.Anbar))]
public int AnbarId { get; set; }
[Display(Name = "TitleAnbar", Description = "TitleAnbar_Description", ResourceType = typeof(Resources.Anbar))]
public string TitleAnbar { get; set; }
}
}
