using Auth.Web.Data;
using Auth.Web.Models;
using Auth.Web.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Web.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<User> userManager;

        public UserController(ApplicationDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }


        public IActionResult Index()
        {
            var users = db.Users.Where(x => !x.IsDeleted).
                Select(x => new UserViewModel() { 
                    Id = x.Id ,
                    CreatedAt = x.CreatedAt ,
                    Email = x.Email ,
                    PhoneNumber = x.PhoneNumber ,
                    UserName = x.UserName ,
                }).ToList();


            return View(users);
        }

        [HttpGet]
        public IActionResult Create() {
            // email , phone , password

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel input)
        {
            // email , phone , password
            if (ModelState.IsValid)
            {
                // add user to db
                var user = new User();
                user.CreatedAt = DateTime.Now;
                user.Email = input.Email;
                user.PhoneNumber = input.Phone;
                user.UserName = input.Email ;
                user.PasswordHash = input.PassWord ;
                
                //TempData["m"] = user.Email + " " + user.UserName + " " + user.PhoneNumber ;
               
                
                var x = await userManager.CreateAsync(user, input.PassWord);
                //if (x.Succeeded)
                //{
                //    TempData["m"] = "Succed";
                //}
                //else {
                //    TempData["m"] = x.ToString();
                //}
                // here i add as a original table
                // db.Users.Add(user);
                // db.SaveChanges();
                return RedirectToAction("Index");
               
            }

            return View(input);
        }

        public IActionResult Delete(string id) { 
        
            var user = db.Users.SingleOrDefault(x => x.Id == id && !x.IsDeleted);
            if (user == null) {
                return NotFound();
            }
            user.IsDeleted = true;
            db.Users.Update(user);
            db.SaveChanges();
            
            return RedirectToAction("Index");
        }
    }
}
