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
    
    public partial class FactorForoosh
    {
        public int FactorForooshId { get; set; }
        public int KalaId { get; set; }
        public int CountKala { get; set; }
        public int Fi { get; set; }
    
        public virtual Kala Kala { get; set; }
    }
}
