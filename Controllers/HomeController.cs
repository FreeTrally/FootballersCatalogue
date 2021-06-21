using FootballersCatalogue.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FootballersCatalogue.Controllers
{
    public class HomeController : Controller
    {
        private CatalogueContext db;
        private IHubContext<IndexHub> hub;

        public HomeController(CatalogueContext context, IHubContext<IndexHub> hubContext)
        {
            db = context;
            hub = hubContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(db.Players.ToList());
        }

        [HttpGet]
        [ActionName("Error")]
        public IActionResult HandleError(int id)
        {
            return View(new ErrorViewModel() { RequestId = id.ToString() });
        }

        [HttpGet]
        [ActionName("Register")]
        public IActionResult AddNewPlayer()
        {
            ViewData["Teams"] = db.Teams.ToList();
            return View();
        }

        [HttpPost]
        [ActionName("Register")]
        public async Task<IActionResult> AddNewPlayer(Player player)
        {
            SetPlayerTeamRegister(player);
            db.Players.Add(player);
            db.SaveChanges();
            await hub.Clients.All.SendAsync("RefreshTable", db.Players.ToList());
            return RedirectToAction("Index");
        }      

        [HttpGet]
        public IActionResult Delete(int? id)
        {
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
            if (id != null)
            {
                var player = await db.Players.FirstOrDefaultAsync(p => p.Id == id);
                if (player != null)
                {
                    return View(new EditViewModel(player, db.Teams.ToList()));
                }
                    
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Player player)
        {
            SetPlayerTeamEdit(player);
            db.Players.Update(player);
            await db.SaveChangesAsync();
            await hub.Clients.All.SendAsync("RefreshTable", db.Players.ToList());
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetTeamsDatalist()
        {
            return PartialView(db.Teams.ToList());
        }

        private void SetPlayerTeamRegister(Player player)
        {
            if (!db.Teams.Where(team => team.Name.Equals(player.TeamName)).Any())
            {
                db.Teams.Add(new Team(player.TeamName));
                db.SaveChanges();
            }

            var playerTeam = db.Teams.Where(team => team.Name.Equals(player.TeamName)).FirstOrDefault();
            player.TeamId = playerTeam.TeamId;
            player.Team = playerTeam;
        }

        private void SetPlayerTeamEdit(Player player)
        {
            var playerTeam = db.Teams.Where(team => team.TeamId.Equals(player.TeamId)).FirstOrDefault();

            if (player.TeamName != null)
            {
                if (!db.Teams.Where(team => team.Name.Equals(player.TeamName)).Any())
                {
                    db.Teams.Add(new Team(player.TeamName));
                    db.SaveChanges();
                }
                playerTeam = db.Teams.Where(team => team.Name.Equals(player.TeamName)).FirstOrDefault();
                player.TeamId = playerTeam.TeamId;
            }
            else
                player.TeamName = playerTeam.Name;

            player.Team = playerTeam;
        }
    }
}
