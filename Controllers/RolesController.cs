using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VotingApp.Data;

namespace VotingApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(IdentityRole role)
        {
            var id = new Random();
            role.Id = id.Next(1000,10000).ToString();
            await _roleManager.CreateAsync(new IdentityRole {Id = role.Id,Name = role.Name });
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult EditRole(string id)
        {
            var role = _roleManager.FindByIdAsync(id);
            return View(role);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(IdentityRole role)
        {
            await _roleManager.UpdateAsync(role);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = _db.Roles.Where(x => x.Id == id).FirstOrDefault();
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }
            
            return RedirectToAction("Index");
        }
    }
}
