//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CPM.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Corporate_Customer_Tbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Corporate_Customer_Tbl()
        {
            this.Meeting_Minutes_Master_Tbl = new HashSet<Meeting_Minutes_Master_Tbl>();
        }
    
        public int CorporateCustomerID { get; set; }
        public string CorporateName { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Meeting_Minutes_Master_Tbl> Meeting_Minutes_Master_Tbl { get; set; }
    }
}