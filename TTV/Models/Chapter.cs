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
    using System.Web.Mvc;

    public partial class Chapter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Chapter()
        {
            this.Comments = new HashSet<Comment>();
        }
    
        public int chapter_id { get; set; }
        public Nullable<int> book_id { get; set; }
        public Nullable<int> chapter_number { get; set; }
        public string chapter_title { get; set; }
        [AllowHtml]
        public string chapter_content { get; set; }
        public Nullable<int> chapter_view { get; set; }
        public Nullable<System.DateTime> chapter_created_at { get; set; }
    
        public virtual Book Book { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}