using EBallShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace EBallShop.Controllers
{
    [Route("api/balls")]
    public class BallController : ControllerBase
    {
        private readonly BallDbContext _dbContext;
        public BallController(BallDbContext dbContext) 
        { 
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Ball>> GetAll()
        {
            var balls = _dbContext
                .Balls
                .ToList();

            return Ok(balls);
        }

        [HttpGet("{id}")]
        public ActionResult<Ball> Get([FromRoute]int id)
        {
            var ball = _dbContext
                .Balls
                .FirstOrDefault(b => b.Id == id);

            if(ball == null)
            {
                return NotFound();
            }

            return Ok(ball);
        }

        //[HttpDelete("{id}")]
        //public ActionResult<Ball> Delete([FromRoute]int id)
        //{
        //    var ball = _dbContext
        //        .Balls
        //        .FirstOrDefault(b => b.Id == id);

        //    if(ball == null)
        //    {
        //        return NotFound();
        //    }


        //}

        //[HttpPost]
        //public ActionResult AddNewBall([FromBody]AddBall ab)
        //{

        //}
    }
}
