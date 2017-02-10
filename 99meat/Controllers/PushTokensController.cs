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
    public class PushTokensController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PushTokens
        public async Task<IQueryable<PushTokens>> GetPushTokens()
        {
            //var user = User.Identity.Name;
            //var pushToken = new SendNotification();
            //var token = await db.PushTokens.Where(x => x.Email == user).FirstOrDefaultAsync();
            //await pushToken.SendPushNotification("Order Status", "Yum Yum your food is cooked and redy to puck up", token.token);
            return db.PushTokens;
        }

        // GET: api/Addresses/5
        [ResponseType(typeof(string))]
        [Route("api/PushTokens/SavePushTokens")]
        [HttpPost]
        public async Task<IHttpActionResult> SavePushTokens(FacebookTokens facebookTokens)
        {
            var user = User.Identity.Name;
            PushTokens pushTokens = await db.PushTokens.Where(x => x.Email.ToString().Equals(user)).OrderBy(x => x.ID).FirstOrDefaultAsync();

            if (pushTokens == null)
            {
                db.PushTokens.Add(new PushTokens()
                {
                    Email = user,
                    token = facebookTokens.token
                });
            }
            else
            {
                pushTokens.token = facebookTokens.token;
                db.Entry(pushTokens).State = System.Data.Entity.EntityState.Modified;
            }

            try
            {
                await db.SaveChangesAsync();
                //var pushToken = new SendNotification();
                //var token = await  db.PushTokens.Where(x => x.Email == user).FirstOrDefaultAsync();
                //await pushToken.SendPushNotification("Order Status", "Yum Yum,your food is cooked and redy to pick up", token.token);
            }
            catch (DbUpdateConcurrencyException)
            {
               
                    return NotFound();
               
            }

            return StatusCode(HttpStatusCode.NoContent);
        }



        // GET: api/PushTokens/5
        [ResponseType(typeof(PushTokens))]
        public async Task<IHttpActionResult> GetPushTokens(int id)
        {
            PushTokens pushTokens = await db.PushTokens.FindAsync(id);
            if (pushTokens == null)
            {
                return NotFound();
            }

            return Ok(pushTokens);
        }

        // PUT: api/PushTokens/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPushTokens(int id, PushTokens pushTokens)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != pushTokens.ID)
            {
                return BadRequest();
            }

            db.Entry(pushTokens).State = System.Data.Entity.EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PushTokensExists(id))
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

        // POST: api/PushTokens
        [ResponseType(typeof(PushTokens))]
        public async Task<IHttpActionResult> PostPushTokens(PushTokens pushTokens)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PushTokens.Add(pushTokens);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = pushTokens.ID }, pushTokens);
        }

        // DELETE: api/PushTokens/5
        [ResponseType(typeof(PushTokens))]
        public async Task<IHttpActionResult> DeletePushTokens(int id)
        {
            PushTokens pushTokens = await db.PushTokens.FindAsync(id);
            if (pushTokens == null)
            {
                return NotFound();
            }

            db.PushTokens.Remove(pushTokens);
            await db.SaveChangesAsync();

            return Ok(pushTokens);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PushTokensExists(int id)
        {
            return db.PushTokens.Count(e => e.ID == id) > 0;
        }
    }
}