using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Models.UsersManagement;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        public readonly IConfiguration dbUsersConfig;

        public UsersController(IConfiguration config)
        {
            dbUsersConfig = config;
        }

        [HttpGet("lstUsers")]
        public async Task<ActionResult<IEnumerable<TbUser>>> GetTbUsers()
        {
            using (DbusersContext dbUsers = new DbusersContext(dbUsersConfig))
            {
                if (dbUsers.TbUsers == null)
                {
                    return NotFound();
                }
                try
                {
                    List<TbUser> lstUsers = await dbUsers.TbUsers.ToListAsync();

                    return dbUsers.TbUsers.ToList();
                }
                catch (Exception ex)
                {
                    throw (new Exception("Error: " + ex.Message));
                }
                
            }
        }

        [IgnoreAntiforgeryToken]
        [HttpPost("new")]
        public async Task<ActionResult<TbUser>> NewTbUsers(TbUser user)
        {
            DateOnly now = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            using (DbusersContext dbUsers = new DbusersContext(dbUsersConfig))
            {
                if (dbUsers.TbUsers == null)
                {
                    return Problem("Entity set 'DbusersContext.TbUsers'  is null.");
                }
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, "No content");
                }
                if (TbUserExists(user.UserId))
                {
                    return Conflict();
                }
                else if (user.DateOfBirth.AddYears(18) >= now)
                {
                    return StatusCode(StatusCodes.Status412PreconditionFailed, "User must be over eighteen years of age to register");
                }
                else
                {
                    try
                    {
                        user.RetirementDate = user.DateOfBirth.AddYears(62);
                        dbUsers.TbUsers.Add(user);
                        await dbUsers.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw (new Exception(ex.ToString()));
                    }
                }

                return user;
            }
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdUser(TbUser userUpdt)
        {
            TbUser user = new TbUser();
            DateOnly now = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No content");
            }

            using (DbusersContext dbUsers = new DbusersContext(dbUsersConfig))
            {
                if (userUpdt == null)
                {
                    return StatusCode(StatusCodes.Status204NoContent, "No content");
                }
                else if (userUpdt.DateOfBirth.AddYears(18) >= now)
                {
                    return StatusCode(StatusCodes.Status412PreconditionFailed, "User must be over eighteen years of age to register. Update denied");
                }

                user = dbUsers.TbUsers.Where(u => u.UserId == userUpdt.UserId).FirstOrDefault();
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status304NotModified, "User not found");
                }
                else
                {
                    userUpdt.UserId = user.UserId;
                    userUpdt.RetirementDate = user.DateOfBirth.AddYears(62);

                    dbUsers.Entry(user).State = EntityState.Modified;
                }

                try
                {
                    await dbUsers.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return StatusCode(StatusCodes.Status200OK, "Updated");
            }
        }

        [HttpDelete()]
        public async Task<IActionResult> DeleteUser(int UserID)
        {
            TbUser user = new TbUser();

            if (UserID < 1)
            {
                return StatusCode(StatusCodes.Status304NotModified, "Id Error");
            }
            using (DbusersContext dbUsers = new DbusersContext(dbUsersConfig))
            {
                user = await dbUsers.TbUsers.Where(u => u.UserId == UserID).FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound();
                }

                try
                {
                    dbUsers.Remove(user);
                    dbUsers.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw (new Exception(ex.ToString()));
                }

                return StatusCode(StatusCodes.Status200OK, "Removed");
            }
        }

        private bool TbUserExists(int userId)
        {
            using (DbusersContext dbUsers = new DbusersContext(dbUsersConfig))
            {
                return (dbUsers.TbUsers?.Any(m => m.UserId == userId)).GetValueOrDefault();
            }
        }
    }
}
