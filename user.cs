//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace equipmentMangement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            this.reservations = new HashSet<reservations>();
        }
    
        public int idUser { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }

        public string FullName
        {
            get
            {
                return Name + " " + Surname;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<reservations> reservations { get; set; }

        public int NumberOfReservations
        {
            get 
            {
                return reservations.Count();
            }
        }
    }
}
