using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace LavazemYadaki.Models {
[MetadataType(typeof(PersonMetadata))]
public partial class Person {
public override string ToString()
{
//TO DO: Replace with a meaningful code
return base.ToString();
}
}
public partial class PersonMetadata
{
[Display(Name = "PersonId", Description = "PersonId_Description", ResourceType = typeof(Resources.Person))]
public int PersonId { get; set; }
[Display(Name = "Name", Description = "Name_Description", ResourceType = typeof(Resources.Person))]
public string Name { get; set; }
[Display(Name = "Family", Description = "Family_Description", ResourceType = typeof(Resources.Person))]
public string Family { get; set; }
}
}
