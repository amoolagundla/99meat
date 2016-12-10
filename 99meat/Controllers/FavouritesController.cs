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
    [Authorize]
    public class FavouritesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Favourites
        public IQueryable<Favourite> GetFavourites()
        {
            return db.Favourites;
        }

        // GET: api/Favourites/5
        [ResponseType(typeof(Favourite))]
        public async Task<IHttpActionResult> GetFavourite(int id)
        {
            Favourite favourite = await db.Favourites.FindAsync(id);
            if (favourite == null)
            {
                return NotFound();
            }

            return Ok(favourite);
        }

        // PUT: api/Favourites/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFavourite(int id, Favourite favourite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != favourite.Id)
            {
                return BadRequest();
            }

            db.Entry(favourite).State = System.Data.Entity.EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavouriteExists(id))
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

        // POST: api/Favourites
        [ResponseType(typeof(Favourite))]
        public async Task<IHttpActionResult> PostFavourite(Favourite favourite)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Favourites.Add(favourite);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = favourite.Id }, favourite);
        }

        // DELETE: api/Favourites/5
        [ResponseType(typeof(Favourite))]
        public async Task<IHttpActionResult> DeleteFavourite(int id)
        {
            Favourite favourite = await db.Favourites.FindAsync(id);
            if (favourite == null)
            {
                return NotFound();
            }

            db.Favourites.Remove(favourite);
            await db.SaveChangesAsync();

            return Ok(favourite);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FavouriteExists(int id)
        {
            return db.Favourites.Count(e => e.Id == id) > 0;
        }
    }
}