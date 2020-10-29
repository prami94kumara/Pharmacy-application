using My_Pharma_DB.Domain;
using My_Pharma_DB.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace My_Pharma_COMMON.Service
{
    /// <summary>
    /// Referencing the inheritance between the service medicine class and interface
    /// </summary>
    public class MedicineService : IMedicineService
    {
        //Creating an object of the DB repository to access all the methods from the DB layer to the service layer
        private readonly IMedicineRepository _medRep = new MedicineRepository();

        //Get all the medicine details from the table and show as list using the domain class properties
        public List<Medicine> GetAllMedicineDetails(bool isActive)
        {
            return _medRep.GetAllMedicineDetails(isActive);
        }

        //Create a new Medicine record and save
        public string SaveMedicine(Medicine med)
        {
            return _medRep.SaveMedicine(med);
        }

        //Get medicine details of a particular item using its ID from object
        public Medicine GetParticularMedicine(long medID)
        {
            return _medRep.GetParticularMedicine(medID);
        }

        //Edit details of the medicine and save to the table
        public string EditMedicine(Medicine med)
        {
            return _medRep.EditMedicine(med);
        }

        //Delete the medicine details from the table
        //but here instead of removing the line we change the status to inactive
        public string DeleteMedicine(Medicine med)
        {
            return _medRep.DeleteMedicine(med);
        }

        ////Show all the deleted medicine details in a list
        //public Medicine ViewDeletedMedicines()
        //{
        //    return _medRep.ViewDeletedMedicines();
        //}

        ////Get the medicine category to a list
        //public IEnumerable<MedicineCategory> MedicineCategoryList()
        //{
        //    return _medRep.MedicineCategoryList();
        //}

        //Get the medicine category to a list using a query format
        //public IEnumerable<MedicineCategory> GetMedicineCategoryList()
        //{
        //    return _medRep.GetMedicineCategoryList();
        //}

        public List<MedicineCategory> GetMedicineCategoryList()
        {
            return _medRep.GetMedicineCategoryList();
        }


        //Get medicine details of a particular item using its ID from object
        public Medicine GetParticularMedicineWithCategory(long medID)
        {
            return _medRep.GetParticularMedicineWithCategory(medID);
        }

        //Get all the medicine details from the table based on the medicine category
        public List<Medicine> GetAllMedicineListCategoryBased(long medCatID)
        {
            return _medRep.GetAllMedicineListCategoryBased(medCatID);
        }

        //Get the FilePath of the saved image for the Medicine ID 
        public Medicine GetMedicineImage(long medID, Medicine med)
        {
            return _medRep.GetMedicineImage(medID, med);
        }
    }
}