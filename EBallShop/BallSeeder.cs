using EBallShop.Models;

namespace EBallShop
{
    public class BallSeeder : IBallSeeder
    {
        private readonly BallDbContext _dbContext;
        public BallSeeder(BallDbContext dbContext) 
        {
            _dbContext = dbContext;
        } 
        public void Seed()
        {
            if(_dbContext.Database.CanConnect())
            {
                if(!_dbContext.Balls.Any())
                {
                    var balls = GetBalls();
                    _dbContext.Balls.AddRange(balls);
                    _dbContext.SaveChanges();
                }
            }
        }
        private IEnumerable<Ball> GetBalls()
        {
            var football = new Category()
            {
                Name = "Football"
            };
            var basketball = new Category()
            {
                Name = "Basketball"
            };
            var balls = new List<Ball>()
            {
                new Ball()
                {
                    Name = "Jabulani",
                    Size = "5",
                    Description = "abc",

                    Category = football
                },
                new Ball()
                {
                    Name = "Tellstar",
                    Size = "5",
                    Description = "abc",

                    Category = football
                },
                new Ball()
                {
                    Name = "Brazuca",
                    Size = "5",
                    Description = "abc",

                    Category = football
                },

                new Ball()
                {
                    Name = "Wilson",
                    Size = "7",
                    Description = "abc",

                    Category = basketball
                }
            };
        return balls;
        }
    }
}
