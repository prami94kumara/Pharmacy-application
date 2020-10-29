using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;

namespace My_Pharma_DB.Domain
{
    /// <summary>
    /// All the properties from tbl_medicine is being declared in the domain class 
    /// </summary>
    public class Medicine
    {
        /// <summary>
        /// Medicine ID
        /// </summary>
        public long MedicineID { get; set; }

        /// <summary>
        /// Medicine Name
        /// </summary>
        [Required]
        public string MedicineName { get; set; }

        /// <summary>
        /// Description of medicine
        /// </summary>
        public string MedicineDescription { get; set; }

        /// <summary>
        /// Qty of the medicine 
        /// </summary>
        [Range(0, 10000)]
        public int MediQty { get; set; }

        /// <summary>
        /// Expiry date of the medicie
        /// </summary>
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public string MediExpiryDate { get; set; }

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

        /// <summary>
        /// Medicine Category
        /// u need coloumn to store category ID into database
        /// </summary>
        public long MedicineCategoryID { get; set; }


        /// <summary>
        /// Gets the medicine category names and ids as a list
        /// </summary>
        public List<SelectListItem> MedCategoryList { get; set; }

        /// <summary>
        /// Medicine images path is stored here
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// To show as a button
        /// </summary>
        public HttpPostedFileBase ImageFile { get; set; }
    }
}
