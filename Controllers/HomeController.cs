using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VotingApp.Data;
using VotingApp.Models;
using VotingApp.Models.ViewModels;

namespace VotingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db,
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        

        public IActionResult Index()
        {
            return View();
        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> Votes(int currentPage, string username)
        {
            //Display the list of Aspirants according to their positions in a paginated list
            VotesViewModel vm = new();
            var query = _db.Positions.AsNoTracking().ToList();
            int pageSize = 1;
            var toSkip = (currentPage - 1) * pageSize;
            var totalRecord = query.Count();
            var numberOfPages = (int)Math.Ceiling((double)totalRecord / pageSize);
            vm.Position = query
                .Skip(toSkip)
                .Take(pageSize).FirstOrDefault();
            vm.Aspirants = _db.Aspirants.AsNoTracking().Where(x => x.PositionId == vm.Position.Id);
            vm.CurrentPage = currentPage;
            vm.PageSize = pageSize;
            vm.HasNextPage = currentPage < numberOfPages;
            vm.HasPreviousPage = currentPage > 1;
            vm.TotalItems = totalRecord;
            vm.TotalPages = numberOfPages;
            var user = await _userManager.FindByNameAsync(username);
            //If the user has already voted once they will not be allowed to vote again
            if (user.Voted == true)
            {
                return View("AlreadyVoted");
            }
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Votes(int? id, int currentPage, string username)
        {
            //Find the selected Aspirant by their id and increase the number of votes by 1
            var item = _db.Aspirants.Find(id);
            item.Votes += 1;
            _db.SaveChanges(); 
            var query = _db.Positions.AsNoTracking().ToList();
            var totalRecord = query.Count();
            var numberOfPages = (int)Math.Ceiling((double)totalRecord / 1);
            VotesViewModel vm = new VotesViewModel();
            vm.TotalItems = totalRecord;
            vm.TotalPages = numberOfPages;
            //Moves to the next page if there are any pages left, else signs the user out and indicates user has finished voting.
            if (currentPage <= vm.TotalPages)
            {
                return RedirectToAction("Votes", new { currentPage = currentPage, username = username});
            }
            else
            {
                var user = await _db.AppUsers.Where(x => x.UserName == username).FirstOrDefaultAsync();
                if (user != null)
                {
                    user.Voted = true;
                    _db.SaveChanges();
                    await _signInManager.SignOutAsync();
                }
                return View("Success");
            }
        }

        public IActionResult AlreadyVoted()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
