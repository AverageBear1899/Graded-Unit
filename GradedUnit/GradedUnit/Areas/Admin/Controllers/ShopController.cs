using GradedUnit.Models.Data;
using GradedUnit.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GradedUnit.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Categories
        public ActionResult Categories()
        {
            //declare a list of models
            List<CategoryVM> categoryVMList;

            using (Db db = new Db())
            {
                //init the list
                categoryVMList = db.Catagories.ToArray().OrderBy(x => x.Sorting).Select(x => new CategoryVM(x)).ToList();
            }
            //return view with list
            return View(categoryVMList);
        }
        // POST: Admin/Shop/AddNewCategory
        [HttpPost]
        public string AddNewCategory(string catName)
        {
            //declare id
            string id;

            using (Db db = new Db())
            {
                //check that category is unique
                if (db.Catagories.Any(x => x.Name == catName))
                
                    return "titletaken";

                //init dto
                CategoryDTO dto = new CategoryDTO();
                //add to dto
                dto.Name = catName;
                dto.Slug = catName.Replace(" ", "-").ToLower();
                dto.Sorting = 100;
                //save 
                db.Catagories.Add(dto);
                db.SaveChanges();
                //get id
                id = dto.Id.ToString();
            }
            //return id
            return id;
        }

        //POST : Admin/Shop/ReorderCategories
        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (Db db = new Db())
            {
                //set initial count
                int count = 1;
                //declare page dto
                CategoryDTO dto;
                //set sorting for each category
                foreach (var catId in id)
                {
                    dto = db.Catagories.Find(catId);
                    dto.Sorting = count;
                    db.SaveChanges();
                    count++;

                }
            }

        }

        //GET : Admin/Shop/DeleteCategory/id
        public ActionResult DeleteCategory(int id)
        {
            using (Db db = new Db())
            {
                //get the page
                CategoryDTO dto = db.Catagories.Find(id);

                //remove the categories
                db.Catagories.Remove(dto);

                //save
                db.SaveChanges();
            }
            //redirect
            return RedirectToAction("Categories");
        }

        [HttpPost]
        //GET : Admin/Shop/RenameCategory
        public string RenameCategory(string newCatName, int id)
        {
            using (Db db = new Db())
            {
                //check the cat name is unique
                if (db.Catagories.Any(x => x.Name == newCatName))
                    return "titletaken";
                //get dto
                CategoryDTO dto = db.Catagories.Find(id);
                //edit dto
                dto.Name = newCatName;
                dto.Slug = newCatName.Replace(" ", "-").ToLower();
                //save
                db.SaveChanges();

            }
            //return
            return "ok;";
        }

    }

}