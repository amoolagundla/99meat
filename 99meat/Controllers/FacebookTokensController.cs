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
    [AllowAnonymous]
    public class FacebookTokensController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/FacebookTokens
        public IQueryable<FacebookTokens> GetfacebookTokens()
        {
            return db.facebookTokens;
        }

        // GET: api/FacebookTokens/5
        [ResponseType(typeof(FacebookTokens))]
        public async Task<IHttpActionResult> GetFacebookTokens(int id)
        {
            FacebookTokens facebookTokens = await db.facebookTokens.FindAsync(id);
            if (facebookTokens == null)
            {
                return NotFound();
            }

            return Ok(facebookTokens);
        }

        // PUT: api/FacebookTokens/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFacebookTokens(int id, FacebookTokens facebookTokens)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != facebookTokens.ID)
            {
                return BadRequest();
            }

            db.Entry(facebookTokens).State = System.Data.Entity.EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacebookTokensExists(id))
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

        // POST: api/FacebookTokens
        [ResponseType(typeof(FacebookTokens))]
        public async Task<IHttpActionResult> PostFacebookTokens(FacebookTokens facebookTokens)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.facebookTokens.Add(facebookTokens);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = facebookTokens.ID }, facebookTokens);
        }

        // DELETE: api/FacebookTokens/5
        [ResponseType(typeof(FacebookTokens))]
        public async Task<IHttpActionResult> DeleteFacebookTokens(int id)
        {
            FacebookTokens facebookTokens = await db.facebookTokens.FindAsync(id);
            if (facebookTokens == null)
            {
                return NotFound();
            }

            db.facebookTokens.Remove(facebookTokens);
            await db.SaveChangesAsync();

            return Ok(facebookTokens);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FacebookTokensExists(int id)
        {
            return db.facebookTokens.Count(e => e.ID == id) > 0;
        }
    }
}