//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Brobet.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BetObject
    {
        public int id { get; set; }
        public Nullable<int> fromBetRequestId { get; set; }
        public Nullable<int> toBetRequestId { get; set; }
        public Nullable<int> fromBetId { get; set; }
        public Nullable<int> toBetId { get; set; }
        public int betTypeId { get; set; }
        public string value { get; set; }
        public string status { get; set; }
    
        public virtual Bet FromBet { get; set; }
        public virtual Bet ToBet { get; set; }
        public virtual BetType BetType { get; set; }
        public virtual BetRequest FromBetRequest { get; set; }
        public virtual BetRequest ToBetRequest { get; set; }
    }
}
