using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Planc.Dal;
using Planc.Dal.GameModels.LeeGame;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
namespace Planc.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        
        // GET: Index
        public ActionResult Index()
        {
            ViewBag.GameNames = GameConstants.GameNames;
            return View();
        }

        /// <summary>
        ///     Function to create a new Lee game
        /// </summary>
        /// <returns></returns>
        public ActionResult NewLee()
        {
            var game = Planc.Dal.Game.NewGame<Lee>();
            GameConstants.Dal.SaveGame(game);
            return RedirectToActionPermanent("Lee", new {game.Id});
        }

        
        /// <summary>
        ///     Function to retrieve game and show Lee Page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Lee(string id)
        {
            var game = GameConstants.Dal.LoadGame<Lee>(id);
            var playerId = User.Identity.GetUserId();

            ViewBag.PlayerID = playerId;
            return PartialView("Lee", (Lee) game);
        }

        public ActionResult LeeOb(string id)
        {
            //throw new NotImplementedException("Shea to implement");

            var game = GameConstants.Dal.LoadGame<Lee>(id);
            if (game == null)
            {
                //return error on load
                throw new NotImplementedException("Game object does not currently exist (via Observer).");
            }

            ViewBag.PlayerID = GameConstants.ObserverId;
            return View("LeeOb", (Lee) game);
        }
    }
}