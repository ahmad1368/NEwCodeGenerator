using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace LavazemYadaki.Models
{
    [MetadataType(typeof(FactorForooshMetadata))]
    public partial class FactorForoosh
    {
        public FactorForoosh()
        {
            int i = 0;
        }
        public override string ToString()
        {
            //TO DO: Replace with a meaningful code
            return base.ToString();
        }
    }
    public partial class FactorForooshMetadata
    {
        [Display(Name = "FactorForooshId", Description = "FactorForooshId_Description", ResourceType = typeof(Resources.FactorForoosh))]
        public int FactorForooshId { get; set; }
        [Display(Name = "KalaId", Description = "KalaId_Description", ResourceType = typeof(Resources.FactorForoosh))]
        public virtual ICollection<Kala> KalaId { get; set; }
        [Display(Name = "CountKala", Description = "CountKala_Description", ResourceType = typeof(Resources.FactorForoosh))]
        public int CountKala { get; set; }
        [Display(Name = "Fi", Description = "Fi_Description", ResourceType = typeof(Resources.FactorForoosh))]
        public int Fi { get; set; }
    }
}
