//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TTV.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class user_permissions
    {
        public int up_id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> permission_id { get; set; }
        public string note { get; set; }
    
        public virtual permission permission { get; set; }
        public virtual User User { get; set; }
    }
}
