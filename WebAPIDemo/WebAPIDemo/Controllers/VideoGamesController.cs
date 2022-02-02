using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoGamesController : ControllerBase
    {
        private static List<string> games = new() { "Watch Dogs", "GTA V", "Witcher", "God of War" };

        [HttpGet]
        public ActionResult GetVideoGames([FromQuery]int count)
        {
            try
            {
                if (!games.Any())
                    return NotFound();

                return Ok(games.Take(count));
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public ActionResult CreateVideoGame([FromBody]string videoGame)
        {
            try
            {
                games.Add(videoGame);
                return Created("",games);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{videoGame}")]
        public ActionResult DeleteVideoGame(string videoGame)
        {
            try
            {
                games.Remove(videoGame);
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("clear")]
        public ActionResult ClearVideoGames()
        {
            try
            {
                games.Clear();
                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
