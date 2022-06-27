using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VotingApp.Data;
using VotingApp.Models;

namespace VotingApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<SelectListItem> PositionList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var positions = _db.Positions.ToList();
            foreach (var item in positions)
            {
                list.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }
            return list;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Positions()
        {
            var positions = _db.Positions.ToList();
            return View(positions);
        }
        
        public IActionResult CreatePosition()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePosition(Position position)
        {
            _db.Positions.Add(position);
            _db.SaveChanges();
            return RedirectToAction("Positions");
        }

        public IActionResult EditPosition(int id)
        {
            var position = _db.Positions.Find(id);
            return View(position);
        }

        [HttpPost]
        public IActionResult EditPosition(Position position)
        {
            _db.Positions.Update(position);
            _db.SaveChanges();
            return RedirectToAction("Positions");
        }

        public IActionResult DeletePosition(int id)
        {
            var position = _db.Positions.Find(id);
            return View(position);
        }

        [HttpPost]
        public IActionResult DeletePosition(Position position)
        {
            _db.Positions.Remove(position);
            _db.SaveChanges();
            return RedirectToAction("Positions");
        }

        public IActionResult Aspirants()
        {
            var aspirants = _db.Aspirants.ToList();
            return View(aspirants);
        }

        public IActionResult CreateAspirants()
        {
            ViewBag.PositionList = PositionList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateAspirants(Aspirants aspirants)
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnvironment.WebRootPath;

            string upload = webRootPath + @"\images\";
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(files[0].FileName);

            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
            {
                files[0].CopyTo(fileStream);
            }

            aspirants.Image = fileName + extension;

            _db.Aspirants.Add(aspirants);
            _db.SaveChanges();
            return RedirectToAction("Aspirants");
        }

        public IActionResult EditAsprants(int id)
        {
            ViewBag.PositionList = PositionList();
            var aspirant = _db.Aspirants.Find(id);
            return View(aspirant);
        }
        
        [HttpPost]
        public IActionResult EditAsprants(Aspirants aspirants)
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnvironment.WebRootPath;

            var objFromDb = _db.Aspirants.AsNoTracking().FirstOrDefault(u => u.Id == aspirants.Id);

            if (files.Count > 0)
            {
                string upload = webRootPath + "/images/";
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);

                var oldFile = Path.Combine(upload, objFromDb.Image);

                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }

                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                aspirants.Image = fileName + extension;
            }
            else
            {
                aspirants.Image = objFromDb.Image;
            }
            _db.Aspirants.Update(aspirants);
            _db.SaveChanges();
            return RedirectToAction("Aspirants");
        }
         
        public IActionResult AspirantsDetails(int id)
        {
            var details = _db.Aspirants.Find(id);
            return View(details);
        }

        public IActionResult DeleteAspirant(int id)
        {
            var details = _db.Aspirants.Find(id);
            return View(details);
        }

        [HttpPost]
        public IActionResult DeleteApirant(Aspirants aspirant)
        {
            _db.Aspirants.Remove(aspirant);
            _db.SaveChanges();
            return RedirectToAction("Aspirants");
        }
    }
}
