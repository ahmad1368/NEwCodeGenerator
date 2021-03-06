using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace LavazemYadaki.Models {
public class PersonSearchModel : Utility.Paging {
[Display(Name = "PersonIdFrom", Description = "PersonIdFrom_Description", ResourceType = typeof(Resources.Person))]
public int PersonIdFrom { get; set; }
[Display(Name = "PersonIdTo", Description = "PersonIdTo_Description", ResourceType = typeof(Resources.Person))]
public int PersonIdTo { get; set; }
[Display(Name = "Name", Description = "Name_Description", ResourceType = typeof(Resources.Person))]
public string Name { get; set; }
[Display(Name = "Family", Description = "Family_Description", ResourceType = typeof(Resources.Person))]
public string Family { get; set; }
}
}
