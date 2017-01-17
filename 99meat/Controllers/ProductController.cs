using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _99meat.Models;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;

namespace _99meat.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Product
        public async Task<ActionResult> Index()
        {
            var list = await db.Products.Include("category").ToListAsync();
            return View(list);
        }

        // GET: Product/Details/5
        public  ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var product =  db.Products.FirstOrDefault(c=>c.Id==id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            PopulateCategoryDropDownList();
            return View();
        }
        private void PopulateCategoryDropDownList(object selectedCategory = null)
        {
            var departmentsQuery = from d in db.categories
                                   orderby d.Name
                                   select d;
            ViewBag.category = new SelectList(db.categories, "Id", "Name", selectedCategory);
        }
        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProdcutName,ProductDesc,IsProductActive,Price,thumb,Offer,category")] Product product)
        {
            try
            {
                string catId = ModelState.Values.ToList()[6].Value.AttemptedValue.ToString();
                var cat = from d in db.categories
                          where d.Id.ToString().Equals(catId)
                                       select d;
                product.category = cat.FirstOrDefault();
               
                    db.Products.Add(product);
                     db.SaveChanges();
                    return RedirectToAction("Index");
                           
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            PopulateCategoryDropDownList(product.category);
            return View(product);
        }

        // GET: Product/Edit/5
        public  ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product =  db.Products.Include("category").ToList().Find(x=>x.Id==id);
            if (product == null)
            {
                return HttpNotFound();
            }
            PopulateCategoryDropDownList(product.category);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProdcutName,ProductDesc,IsProductActive,Price,thumb,Offer,category")] Product product)
        {
            try
            {

                string catId = ModelState.Values.ToList()[6].Value.AttemptedValue.ToString();
                var cat = from d in db.categories
                          where d.Id.ToString().Equals(catId)
                          select d;
                product.category = cat.FirstOrDefault();
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
