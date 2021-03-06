using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace LavazemYadaki.Models
{
    [MetadataType(typeof(FactorAnbarMetadata))]
    public partial class FactorAnbar
    {
        public override string ToString()
        {
            //TO DO: Replace with a meaningful code
            return base.ToString();
        }
    }
    public partial class FactorAnbarMetadata
    {
        [Display(Name = "FactorAnbarId", Description = "FactorAnbarId_Description", ResourceType = typeof(Resources.FactorAnbar))]
        public int FactorAnbarId { get; set; }
        [Display(Name = "KalaId", Description = "KalaId_Description", ResourceType = typeof(Resources.FactorAnbar))]
        public virtual ICollection<Kala> KalaId { get; set; }
        [Display(Name = "Bestankar", Description = "Bestankar_Description", ResourceType = typeof(Resources.FactorAnbar))]
        public int Bestankar { get; set; }
        [Display(Name = "Bedehkar", Description = "Bedehkar_Description", ResourceType = typeof(Resources.FactorAnbar))]
        public int Bedehkar { get; set; }
        [Display(Name = "Tarikh", Description = "Tarikh_Description", ResourceType = typeof(Resources.FactorAnbar))]
        public DateTime Tarikh { get; set; }
        [Display(Name = "PersonId", Description = "PersonId_Description", ResourceType = typeof(Resources.FactorAnbar))]
        public virtual ICollection<Person> PersonId { get; set; }
    }
}
