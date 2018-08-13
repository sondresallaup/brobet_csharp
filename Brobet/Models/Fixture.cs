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
    
    public partial class Fixture
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Fixture()
        {
            this.BetRequests = new HashSet<BetRequest>();
            this.Bets = new HashSet<Bet>();
        }
    
        public int id { get; set; }
        public Nullable<int> apiId { get; set; }
        public Nullable<int> seasonId { get; set; }
        public Nullable<int> stageId { get; set; }
        public Nullable<int> roundId { get; set; }
        public Nullable<int> groupId { get; set; }
        public Nullable<int> venueId { get; set; }
        public Nullable<int> refereeId { get; set; }
        public int localTeamId { get; set; }
        public int visitorTeamId { get; set; }
        public Nullable<int> localTeamApiId { get; set; }
        public Nullable<int> visitorTeamApiId { get; set; }
        public string scores { get; set; }
        public string time { get; set; }
        public string standings { get; set; }
        public Nullable<System.DateTime> updatedAt { get; set; }
        public Nullable<System.DateTime> startingAt { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public string events { get; set; }
    
        public virtual Team LocalTeam { get; set; }
        public virtual Team VisitorTeam { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BetRequest> BetRequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bet> Bets { get; set; }
    }
}
