//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LavazemYadaki.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Person
    {
        public Person()
        {
            this.FactorAnbar = new HashSet<FactorAnbar>();
        }
    
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
    
        public virtual ICollection<FactorAnbar> FactorAnbar { get; set; }
    }
}
