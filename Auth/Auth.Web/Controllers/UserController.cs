using Auth.Web.Data;
using Auth.Web.Models;
using Auth.Web.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth.Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;
        private IMapper mapper;

        public UserController(ApplicationDbContext db, UserManager<User> userManager
                , RoleManager<IdentityRole> roleManager,IMapper mapper )
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }


        public IActionResult Index()
        {
            var users = db.Users.Where(x => !x.IsDeleted).ToList();
            var userVm = mapper.Map<List<User>, List<UserViewModel>>(users);
            return View(userVm);
        }

        [HttpGet]
        public IActionResult Create()
        {
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
                var user = mapper.Map<User>(input);
                //user.CreatedAt = DateTime.Now;
                //user.Email = input.Email;
                //user.PhoneNumber = input.PhoneNumber;
                //user.UserName = input.Email;
                //user.PasswordHash = input.PassWord;
                //user.UserType = input.UserType ;

                //TempData["m"] = user.Email + " " + user.UserName + " " + user.PhoneNumber ;


                await userManager.CreateAsync(user, input.PassWord);
                if (user.UserType == Enums.UserType.Admin)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
                if (user.UserType == Enums.UserType.Emloyee)
                {
                    await userManager.AddToRoleAsync(user, "Employee");
                }

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

        public IActionResult Delete(string id)
        {

            var user = db.Users.SingleOrDefault(x => x.Id == id && !x.IsDeleted);
            if (user == null)
            {
                return NotFound();
            }
            user.IsDeleted = true;
            db.Users.Update(user);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> InitRole()
        {
            if (!db.Roles.Any())
            {
                var roles = new List<string>();
                roles.Add("Admin");
                roles.Add("Employee");
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }



            return RedirectToAction("Index");
        }
    }
}
