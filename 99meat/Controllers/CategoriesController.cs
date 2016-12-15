using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using _99meat.Models;

namespace _99meat.Controllers
{
    public class CategoriesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Categories
        public IHttpActionResult Getcategories()
        {
            var cat = db.categories.ToList<category>();
            var products = db.Products.ToList<Product>();

            foreach (var c in cat)
            {
                c.Items = new List<Product>();
                foreach (var p in products)
                {
                    if (p.category.Id.ToString().Equals(c.Id.ToString()))
                        c.Items.Add(p);
                }
            }
           

            return  Ok(cat);
        }

        // GET: api/Categories/5
        [ResponseType(typeof(category))]
        public async Task<IHttpActionResult> Getcategory(int id)
        {
            var cat = db.categories.ToList<category>();
            var products = db.Products.ToList<Product>();

            foreach (var c in cat)
            {
                c.Items = new List<Product>();
                foreach (var p in products)
                {
                    if (p.category.Id.ToString().Equals(c.Id.ToString()))
                        c.Items.Add(p);
                }
            }
            if (cat == null)
            {
                return NotFound();
            }

            return Ok(cat);
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Putcategory(int id, category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.Id)
            {
                return BadRequest();
            }

            db.Entry(category).State = System.Data.Entity.EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!categoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Categories
        [ResponseType(typeof(category))]
        public async Task<IHttpActionResult> Postcategory(category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.categories.Add(category);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = category.Id }, category);
        }

        // DELETE: api/Categories/5
        [ResponseType(typeof(category))]
        public async Task<IHttpActionResult> Deletecategory(int id)
        {
            category category = await db.categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            db.categories.Remove(category);
            await db.SaveChangesAsync();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool categoryExists(int id)
        {
            return db.categories.Count(e => e.Id == id) > 0;
        }
    }
}