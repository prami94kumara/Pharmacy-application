using My_Pharma_DB.Domain;
using My_Pharma_DB.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.IO;
using System.Data.Entity;

namespace My_Pharma_DB.Repository
{
    /// <summary>
    /// Referencing the inheritance between the repository medicine class and interface
    /// </summary>
  public class MedicineRepository : IMedicineRepository
    {
        //Get all the medicine details from the table and show as list using the domain class properties
        public List<Medicine> GetAllMedicineDetails(bool isActive)
        {
            //create an DB object to get all the records to the view 
            using (my_pharmaEntities db = new my_pharmaEntities())
            {
                //Get all data which are currently in active and not deleted,
                //using similar to a query and assign to a variable
                var medDataToList = db.tbl_medicine.Where(x => x.is_active== isActive).ToList();

                //Assigning domain model to link the data been retrieved from the DB to the properties in domain 
                //to show in the webpage while creating a new medicine object and assign the data gathered
                var medDomain = medDataToList.Select(d => new Medicine
                {
                    MedicineID = d.med_id,
                    MedicineName = d.med_name,
                    MedicineDescription = d.med_description,
                    MediQty = (d.med_qty == null) ? 0 : Convert.ToInt32(d.med_qty), //if qty is empty then save as 0 or save the number as an interger
                    MediExpiryDate = d.med_expiry_date,
                    ImagePath = d.image_path //get the image path
                });

                //Return the list with data retrieved from DB table tbl_medicine
                return medDomain.ToList();
            }
        }

        //Create a new Medicine record and save
        //successfully added returns null else error message
        public string SaveMedicine(Medicine med)
        {
            //creating an DB object
            using (my_pharmaEntities db = new my_pharmaEntities())
            {
                //create a table object to assign all the values from the class of Domain (stored in object Medicine med) 
                //to the DB table columns names
                tbl_medicine tbl_med_Object = new tbl_medicine()
                {
                    med_name = med.MedicineName,
                    med_description = med.MedicineDescription,
                    med_qty = med.MediQty,
                    med_expiry_date = med.MediExpiryDate,
                    med_cat_id = med.MedicineCategoryID, //to save the medicine category from the dropdown list
                    image_path = med.ImagePath, //the path the file is being saved to
                    is_active = true
                };
                    //Assign value to the table in the Database
                    db.tbl_medicine.Add(tbl_med_Object);
                try
                {
                    //Execute the changes to the DB
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                   return ex.Message;
                }
                return null;
            }
        }

        //Get medicine details of a particular item using its ID from object
        public Medicine GetParticularMedicine(long medID)
        {
            //create a DB object
            using (my_pharmaEntities db = new my_pharmaEntities())
            {
                //Get medicine data of particular ID and checks whether item is currently in active
                //using similar to a query and assign to a variable
                var medData = db.tbl_medicine.Where(x => x.is_active == true && x.med_id == medID);

                // //Assigning domain model to link the data been retrieved from the DB to the properties in domain 
                //to show in the webpage while creating a new medicine object and assign the data gathered
                var medDomain = medData.Select(d => new Medicine
                {
                    MedicineID = d.med_id,
                    MedicineName = d.med_name,
                    MedicineDescription = d.med_description,
                    MediQty = d.med_qty.Value,
                    MediExpiryDate = d.med_expiry_date, 
                    MedicineCategoryID = d.med_cat_id, //get medicine category ID been saved in the table
                    ImagePath = d.image_path //get the image path of the medicine grom the table 
                });

                //Return the data of the particular medicine ID
                return medDomain.FirstOrDefault();
            }
        }

        //Get medicine details of a particular item using its ID from object
        public Medicine GetParticularMedicineWithCategory(long medID)
        {
            //create a DB object
            using (my_pharmaEntities db = new my_pharmaEntities())
            {
                //Get medicine data of particular ID and checks whether item is currently in active
                //using similar to a query and assign to a variable
                var medData = db.tbl_medicine.Where(x => x.is_active == true && x.med_id == medID);

                // //Assigning domain model to link the data been retrieved from the DB to the properties in domain 
                //to show in the webpage while creating a new medicine object and assign the data gathered
                var medDomain = medData.Select(d => new Medicine
                {
                    MedicineID = d.med_id,
                    MedicineName = d.med_name,
                    MedicineDescription = d.med_description,
                    MediQty = d.med_qty.Value,
                    MediExpiryDate = d.med_expiry_date,
                    MedicineCategoryID = d.med_cat_id, //get medicine category ID been saved in the table
                    ImagePath = d.image_path //get the image path from the table
                });


                Medicine model = new Medicine(); // to fill the combobox again this code needs to be given
                model.MedCategoryList = new SelectList(GetMedicineCategoryList(), "MedCategoryID", "MedCategoryName").ToList();
                //return model;

                //Return the data of the particular medicine ID
                return medDomain.FirstOrDefault();
            }
        }

        //Edit details of the medicine and save to the table
        public string EditMedicine(Medicine med)
        {
            //create a DB object
            using (my_pharmaEntities db = new my_pharmaEntities())
            {
                //Get the records from the database of the medicine ID choosen 
                var medData = db.tbl_medicine.FirstOrDefault(x => x.med_id == med.MedicineID);

                if (medData == null)
                {
                    return "eecord not found.";
                }

                else
                {
                    //Gets the image path already in the database before editing the record
                    string tmpImgPath = medData.image_path;

                    medData.med_name = med.MedicineName;
                    medData.med_description = med.MedicineDescription;
                    medData.med_qty = med.MediQty;
                    medData.med_expiry_date = med.MediExpiryDate;
                    medData.med_cat_id = med.MedicineCategoryID; //saving the new edited category for the medicine
                    medData.image_path = (med.ImagePath == null) ? tmpImgPath : med.ImagePath; //Savng the new path of the image to the table
                }
                try
                {
                    db.SaveChanges();
                    return null;
                }
                catch (Exception ex)
                {
                   return ex.Message;
                }
            }
        }

        //Delete the medicine details from the table
        //but here instead of removing the line we change the status to inactive
        public string DeleteMedicine(Medicine med)
        {
            //create DB object
            using (my_pharmaEntities db = new my_pharmaEntities())
            {
                //get the data of that particular medicine ID 
                var medData = db.tbl_medicine.FirstOrDefault(x => x.med_id == med.MedicineID);

                if (medData != null)
                {
                    medData.is_active = false;
                }

                try
                {
                    db.SaveChanges();
                    return null;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        //Get the medicine category to a list
        public List<MedicineCategory> MedicineCategoryList()
        {
            //create a DB object
            using (my_pharmaEntities db = new my_pharmaEntities())
            {
                //Assign the query to a temporary variable
                var getCategoryListData = db.tbl_med_category.Where(x => x.is_active == true).ToList();

                //assign the Domain model to the values retrieved from table
                var medCatDomain = getCategoryListData.Select(d => new MedicineCategory
                {
                    MedCategoryID = d.med_cat_id,
                    MedCategoryName = d.med_cat_name
                });
                
                //return the list of medical categories
                return medCatDomain.ToList();
            }
        }

        /*
        //Get the medicine category to a list using a query format
        // get list here 
        //public IEnumerable<MedicineCategory> GetMedicineCategoryList()
        //{
        //    //first create a db object to access the table 
        //    using (my_pharmaEntities db = new my_pharmaEntities())
        //    {
        //        //get all data from the table as a list
        //        var medicineDataToList = db.tbl_med_category.Where(d => d.is_active == true).ToList();

        //        //Assign to the domain model
        //        var medCatDomain = medicineDataToList.Select(x => new MedicineCategory
        //        {
        //            MedCategoryID = x.med_cat_id,
        //            MedCategoryName = x.med_cat_name
        //        });
        //        return medCatDomain.ToList();
        //    }
        //}
        */

        //Get the medicine category to a list using a query format
        // get list here 
        public List<MedicineCategory> GetMedicineCategoryList()
        {
            //first create a db object to access the table 
            using (my_pharmaEntities db = new my_pharmaEntities())
            {
                //get all data from the table as a list
                var medicineDataToList = db.tbl_med_category.Where(d => d.is_active == true).ToList();

                //Assign to the domain model
                var medCatDomain = medicineDataToList.Select(x => new MedicineCategory
                {
                    MedCategoryID = x.med_cat_id,
                    MedCategoryName = x.med_cat_name
                });
                return medCatDomain.ToList();
            }
        }

        /*
         what did i say to u 
         just get category list normally 
         why r u using sql queriies
         just get list in normal way and pass list to controller 


        is that hard to understand coding ? to be honest tell me answer ...do u understand logic whats happenging  here ?
        in industry be honest no one going to sit next to u and teach u . in all companies career requirments first basic thing mention  candidate should learn stuffs by their self and fast.....?????
        learning by yourself skill its very less in my point of view ...because i gave link right ? if u follow that its within 5 mins u can finish this one !!
        im again again telling no use in memorizing works ... never will help u any time in your life .
         
         */

        //Get all the medicine details from the table based on the medicine category
        public List<Medicine> GetAllMedicineListCategoryBased(long medCatID)
        {
            //create an DB object to get all the records to the view 
            using (my_pharmaEntities db = new my_pharmaEntities())
            {
                //Get all data which are currently in active
                var medDataToList = db.tbl_medicine.Where(x => x.is_active == true && x.med_cat_id == medCatID).ToList();

                //Assigning domain model to link the data been retrieved from the DB to the properties in domain 
                //to show in the webpage while creating a new medicine object and assign the data gathered
                var medDomain = medDataToList.Select(d => new Medicine
                {
                    MedicineID = d.med_id,
                    MedicineName = d.med_name,
                    MedicineDescription = d.med_description,
                    MediQty = (d.med_qty == null) ? 0 : Convert.ToInt32(d.med_qty), //if qty is empty then save as 0 or save the number as an interger
                    MediExpiryDate = d.med_expiry_date
                });

                //Return the list with data retrieved from DB table tbl_medicine
                return medDomain.ToList();
            }
        }


        //Get the FilePath of the saved image for the Medicine ID 
        public Medicine GetMedicineImage(long medID, Medicine med)
        {
            //create db object
            using (my_pharmaEntities db = new my_pharmaEntities())
            {
                //Get all the data from table using the medicine ID
                var meddata = db.tbl_medicine.Where(x => x.med_id == medID);

                //Get the image path from the table using the medicine ID
                var medImgData = meddata.Select(d => new Medicine
                {
                    ImagePath = d.image_path
                });
                db.Entry(med).State = EntityState.Modified;
                db.SaveChanges();
                return medImgData.FirstOrDefault();
            }

        }

        //public void SaveImagePath(Medicine med)
        //{
        //    using (my_pharmaEntities db = new my_pharmaEntities())
        //    {
        //        db.Entry(med).State = EntityState.Modified;

        //        if (db.SaveChanges()>0)
        //        {
                    
        //        }

        //    }
        //}
    }
}
