using My_Pharma_DB.Domain;
using My_Pharma_DB.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Pharma_COMMON.Service
{
   public interface IMedicineService
    {
        //Get all the medicine details from the table and show as list using the domain class properties
        List<Medicine> GetAllMedicineDetails(bool isActive);

        //Create a new Medicine record and save
        string SaveMedicine(Medicine med);

        //Get medicine details of a particular item using its ID from object
        Medicine GetParticularMedicine(long medID);

        //Edit details of the medicine and save to the table
        string EditMedicine(Medicine med);

        //Delete the medicine details from the table
        //but here instead of removing the line we change the status to inactive
        string DeleteMedicine(Medicine med);

        ////Show all the deleted medicine details in a list
        //Medicine ViewDeletedMedicines();

        ////Get the medicine category to a list
        //IEnumerable<MedicineCategory> MedicineCategoryList();

        //Get the medicine category to a list using a query format
        //IEnumerable<MedicineCategory> GetMedicineCategoryList();
        List<MedicineCategory> GetMedicineCategoryList();

        //Get medicine details of a particular item using its ID from object
        Medicine GetParticularMedicineWithCategory(long medID);

        //Get all the medicine details from the table based on the medicine category
        List<Medicine> GetAllMedicineListCategoryBased(long medCatID);

        //Get the FilePath of the saved image for the Medicine ID 
        Medicine GetMedicineImage(long medID, Medicine med);
        
    }
}
