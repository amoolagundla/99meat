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
using System.Data.SqlClient;

namespace _99meat.Controllers
{
    [Authorize]   
    public class AddressesController : ApiController
    {
         
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Addresses
        public IQueryable<Address> GetAddresses()
        {
            return db.Addresses;
        }

        // GET: api/Addresses/5
        [ResponseType(typeof(Address))]
        public async Task<IHttpActionResult> GetAddress(int id)
        {
            Address address = await db.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }

        // GET: api/Addresses/5
        [ResponseType(typeof(Address))]
        [Route("api/Addresses/GetByUserName/{userName}")]
        public IQueryable<Address> GetByUserName(string userName)
        {
            IQueryable<Address>  address =  db.Addresses;

            if (address == null)
            {
                return null;
            }

            return (IQueryable<Address>)address.ToList().FindAll(x => x.UserName.ToLower().ToString().Equals(userName));
        }


        // GET: api/Addresses/5
        [ResponseType(typeof(string))]
        [Route("api/Addresses/GetToAndFroAddress/{Is}")]
        public  async Task<IHttpActionResult> GetToAndFroAddress(int Id)
        {
            var userInfo = await db.Database.SqlQuery<string>("GetToAndFroAddress @Id", new SqlParameter("@Id", Id)).FirstOrDefaultAsync();

            if (userInfo == null)
            {
                return InternalServerError();
            }

            return  Ok("http://maps.google.com/maps?daddr=" + userInfo + "&saddr=");
        }




        // GET: api/Addresses/5
        [HttpPost]
        [AllowAnonymous]
        [Route("api/Addresses/Login")]
        public IHttpActionResult Login(string redirect_uri,string state,string token)
        {
            try
            {
               

            }
            catch(Exception ex)
            {
                return Unauthorized();
            }
            return Ok();
        }


        // PUT: api/Addresses/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAddress(int id, Address address)
        {
            if (string.IsNullOrEmpty(address.UserName.ToString()))
                address.UserName = User.Identity.Name;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != address.Id)
            {
                return BadRequest();
            }

            db.Entry(address).State = System.Data.Entity.EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                if (!AddressExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
        }

        // POST: api/Addresses
        [ResponseType(typeof(Address))]
        public async Task<IHttpActionResult> PostAddress(Address address)
        {
            if (string.IsNullOrEmpty(address.UserName.ToString()))
                address.UserName = User.Identity.Name;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Addresses.Add(address);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = address.Id }, address);
        }

        // DELETE: api/Addresses/5
        [ResponseType(typeof(Address))]
        public  IHttpActionResult DeleteAddress(int id)
        {
            var i = db.Database.SqlQuery<int>(
    "exec DeleteAddresses @Id",
    new SqlParameter("Id", id)).ToList<int>();
           
            if (i == null)
            {
                return NotFound();
            }
          
            return Ok(i);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AddressExists(int id)
        {
            return db.Addresses.Count(e => e.Id == id) > 0;
        }
    }
}