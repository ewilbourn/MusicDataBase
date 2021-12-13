//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MusicWebsite.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Composer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Composer()
        {
            this.MusicPieces = new HashSet<MusicPiece>();
        }
    
        public int ComposerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> YearBorn { get; set; }
        public Nullable<int> YearDied { get; set; }
        public string CityBorn { get; set; }
        public string CityDied { get; set; }
        public string CountryBorn { get; set; }
        public string CountryDied { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MusicPiece> MusicPieces { get; set; }
    }
}
