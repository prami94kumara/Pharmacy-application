using My_Pharma_COMMON.Service;
using My_Pharma_DB.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace My_Pharma_MVC.Controllers
{
    public class HomeController : Controller
    {
        //To access the methods from DB layer the bridge layer SERVICE should be connected here while referencing
        private readonly IMedicineService _medServ = new MedicineService();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //Show all the medicine details in a list view
        [HttpGet]
        public ActionResult MedicineList()
        {
            var listFound = _medServ.GetAllMedicineDetails(true);
            return View(listFound);
        }

        //Give input to the view to save those details in the table 
        //why r u calling a model to this method
        //R u passing any data ?
        [HttpGet]
        public ActionResult CreateMedicine()
        {
            Medicine model = new Medicine();
            model.MedCategoryList = new SelectList(_medServ.GetMedicineCategoryList(), "MedCategoryID", "MedCategoryName").ToList();
            return View(model);
        }

        //Create or save medicines to the DB table
        [HttpPost]
        public ActionResult CreateMedicine(Medicine med)
        {
            string tmpImgPathName = "";
            if (ModelState.IsValid)
            {
                tmpImgPathName = UploadImage(med); //Save image path

                med.ImagePath = tmpImgPathName;
                //method to save the medicine details
                var recSaved = _medServ.SaveMedicine(med); // correct this also same like that
                ModelState.Clear(); //clear all the fields in the page that is given input records
                Medicine model = new Medicine(); // to fill the combobox again this code needs to be given
                model.MedCategoryList = new SelectList(_medServ.GetMedicineCategoryList(), "MedCategoryID", "MedCategoryName").ToList();
                return RedirectToAction("MedicineList");
            }
            return null;
        }

        //Upload an image for the first time
        public string UploadImage(Medicine med)
        {
            string fileName = "";
            string extension = "";

            ////Add Image file path
            fileName = Path.GetFileNameWithoutExtension(med.ImageFile.FileName);
            extension = Path.GetExtension(med.ImageFile.FileName);
            fileName = fileName + extension;
            med.ImagePath = "~/Images/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Images/"), fileName); // thie file path give your image save address exact place address ..something like C:// where your project located address
            med.ImageFile.SaveAs(fileName);
            return med.ImagePath; // haha herer u need pass path which not locate domain address addded 
            //but for database its need location path wherer image save inside project folder
            //so just need pass >>>> ~/Images ....
            //becasue after u upload your project to a server when your website run in live server it will expect from database  "~/Images" this path only 
        }

        //Get medicine data of a particular med item ID for editing
        [HttpGet]
        public ActionResult EditMedicine(long medID)
        {
            ////Have to use view data because cant return the model here becasue have to return the data being gathered too
            //ViewData["Medicine"] = new SelectList(_medServ.GetMedicineCategoryList(), "MedCategoryID", "MedCategoryName");

            //Have to get the ID also again inside the get details method for passing it 
            //in the next method for the medicine object
            Medicine recFound;
            recFound = _medServ.GetParticularMedicine(medID);

            //setup dropdown list
            recFound.MedCategoryList = new SelectList(_medServ.GetMedicineCategoryList(), "MedCategoryID", "MedCategoryName").ToList();

            return View(recFound); //Loads the retrieved details to the view 
        }

        //Save the edit fields to the database table
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMedicine(Medicine med)
        {
            //You need check first your pramateres have values or not t
            //then values which come from client are corrrect or not  after that only u can pass to service layer
            //all  post methods should have this
            string tmpImgPathName = "";
            if (ModelState.IsValid)
            {
                if (med.ImageFile != null)
                {
                    tmpImgPathName = UploadImage(med); // this one giving image path right ?
                    med.ImagePath = tmpImgPathName; // then u can store path here , why are taking that to repositiory
                }

                string message = _medServ.EditMedicine(med);

                if (message == null)
                {
                    ModelState.Clear(); //clear all the fields in the page that is given input records
                    ViewBag.showmessage = "Edited successfully";
                }
                else
                {
                    //this viewbag hold your datbase error message !!
                    ViewBag.errormessage = message;
                }
                //do u understand what i did ?
            }

            //to get the new updated image path to the new medicine object
            med = _medServ.GetParticularMedicineWithCategory(med.MedicineID);

            //setup dropdown list 
            med.MedCategoryList = new SelectList(_medServ.GetMedicineCategoryList(), "MedCategoryID", "MedCategoryName").ToList();

            // you are overriding same method name so both methods communicate with same view 
            //so this enough . it will show you error summary 

            return View(med);
            //return RedirectToAction("MedicineList");
        }

        //Get medicine data of a particular med item ID for deleting
        [HttpGet]
        public ActionResult DeleteMedicine(long medID)
        {
            var recFound = _medServ.GetParticularMedicine(medID);
            return View(recFound);
        }

        //Delete records of the medicine
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMedicine(Medicine med)
        {
            var deleted = _medServ.DeleteMedicine(med);
            return View();
        }

        //Show all the deleted medicine items
        [HttpGet]
        public ActionResult ViewDeletedMedicines()
        {
            var listFound = _medServ.GetAllMedicineDetails(false);
            return View(listFound);
        }

        //Show medicines in a list based on their Medicine category
        [HttpGet]
        public ActionResult ShowMedicineInCategories(long medCatID)
        {
            var listFound = _medServ.GetAllMedicineListCategoryBased(medCatID);
            return View(listFound);
        }
    }
}