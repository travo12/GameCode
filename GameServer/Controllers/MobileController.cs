using Planc.Dal;
using Planc.Dal.GameModels.LeeGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Planc.Controllers
{   
    [Authorize]
    public class MobileController : ApiController
    {
        // GET: api/Mobile
        public string Get()
        {
            var game = Planc.Dal.Game.NewGame<Lee>();
            GameConstants.Dal.SaveGame(game);
            return game.Id;
        }

        // GET: api/Mobile/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Mobile
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Mobile/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Mobile/5
        public void Delete(int id)
        {
        }
        public string NewGame()
        {
            var game = Planc.Dal.Game.NewGame<Lee>();
            GameConstants.Dal.SaveGame(game);
            return game.Id;
            
        }
    }
}
