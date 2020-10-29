using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static My_Pharma_DB.Model.tbl_med_category;

namespace My_Pharma_DB.Domain
{ 
    /// <summary>
  /// All the properties from tbl_med_category is being declared in the domain class 
  /// </summary>
    public class MedicineCategory
    {
        /// <summary>
        /// Medicine Category ID
        /// </summary>
        public long MedCategoryID { get; set; }

        /// <summary>
        /// Medicine Category Name
        /// </summary>
        public string MedCategoryName { get; set; }

        /// <summary>
        /// Medicine Category Description
        /// </summary>
        public string MedCategoryDescription { get; set; }

        /// <summary>
        /// Record added by whom
        /// </summary>
        public string AddedBy { get; set; }

        /// <summary>
        /// Record added on when
        /// </summary>
        public DateTime AddedDate { get; set; }

        /// <summary>
        /// Record updated by whom
        /// </summary>
        public string UpdatedBy { get; set; }

        /// <summary>
        /// Record updated on when
        /// </summary>
        public DateTime UpdatedDate { get; set; }

        /// <summary>
        /// Whether the record is deleted or not
        /// </summary>
        public bool IsActive { get; set; }

        ///// <summary>
        ///// Not mapped
        ///// </summary>
        //public SelectList MedCategoryList { get; set; }

        ///// <summary>
        ///// List of medicine categories
        ///// </summary>
        // public SelectList MedCategoryList { get; set; } // u r into a category call r u trying to call list inside same class ???
        //now it will work as your select list 
        //when get error search why that error coming ...dont do something eles
        //selectlistitem will under MVC ,,,U r in library so u need install MVC package to use thing selectlistitem class
        //public List<SelectListItem> MedCategoryList { get; set; }
    }
}
