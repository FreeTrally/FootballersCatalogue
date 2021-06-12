using FootballersCatalogue.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballersCatalogue.Controllers
{
    public class HomeController : Controller
    {
        CatalogueContext db;
        IHubContext<IndexHub> hub;

        public HomeController(CatalogueContext context, IHubContext<IndexHub> hubContext)
        {
            db = context;
            hub = hubContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Debug.WriteLine("Index");
            return View(db.Players.ToList());
        }

        [HttpGet]
        [ActionName("Register")]
        public IActionResult AddNewPlayer()
        {
            Debug.WriteLine("Register");
            return View(db.Teams.ToList());
        }

        [HttpPost]
        [ActionName("Register")]
        public async Task<IActionResult> AddNewPlayer(Player player)
        {
            Debug.WriteLine("Register post");

            if (!db.Teams.Where(team => team.Name.Equals(player.TeamName)).Any())
            {
                Debug.WriteLine("Created team");
                db.Teams.Add(new Team(player.TeamName));
                db.SaveChanges();
            }
                
            var playerTeam = db.Teams.Where(team => team.Name.Equals(player.TeamName)).FirstOrDefault();
            player.TeamId = playerTeam.TeamId;
            player.Team = playerTeam;
            db.Players.Add(player);

            db.SaveChanges();
            await hub.Clients.All.SendAsync("RefreshTable", db.Players.ToList());
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            Debug.WriteLine("Delete");
            Debug.WriteLine(id);

            ViewBag.PlayerId = id;
            return View(db.Players.FirstOrDefault(p => p.Id == id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                var player = await db.Players.FirstOrDefaultAsync(p => p.Id == id);
                if (player != null)
                {
                    db.Players.Remove(player);
                    await db.SaveChangesAsync();
                    await hub.Clients.All.SendAsync("RefreshTable", db.Players.ToList());
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            Debug.WriteLine("Edit");
            Debug.WriteLine(id);

            if (id != null)
            {
                ViewData["Teams"] = db.Teams.ToList();
                Debug.WriteLine(db.Teams.ToList().Count);
                var player = await db.Players.FirstOrDefaultAsync(p => p.Id == id);
                if (player != null)
                {
                    return View(player);
                }
                    
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Player player)
        {
            var playerTeam = db.Teams.Where(team => team.TeamId.Equals(player.TeamId)).FirstOrDefault();

            if (player.TeamName != null)
            {
                if (db.Teams.Where(team => team.Name.Equals(player.TeamName)).Any())
                {
                    playerTeam = db.Teams.Where(team => team.Name.Equals(player.TeamName)).FirstOrDefault();
                    player.TeamId = playerTeam.TeamId;
                }                  
                else
                {
                    db.Teams.Add(new Team(player.TeamName));
                    db.SaveChanges();
                    playerTeam = db.Teams.Where(team => team.Name.Equals(player.TeamName)).FirstOrDefault();
                    player.TeamId = playerTeam.TeamId;
                }             
            }
            else
                player.TeamName = playerTeam.Name;

            player.Team = playerTeam;

            db.Players.Update(player);
            await db.SaveChangesAsync();
            await hub.Clients.All.SendAsync("RefreshTable", db.Players.ToList());
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetTeamsDatalist()
        {
            Debug.WriteLine("GetTeamsDatalist");
            return PartialView(db.Teams.ToList());
        }

        [HttpGet]
        public IActionResult GetTeamName()
        {
            Debug.WriteLine("GetTeamName");
            return PartialView();
        }
    }
}
