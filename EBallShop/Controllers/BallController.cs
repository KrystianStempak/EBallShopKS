using AutoMapper;
using EBallShop.Models;
using EBallShop.ModelsDto;
using EBallShop.ModlesDto;
using EBallShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EBallShop.Controllers
{
    [Route("api/ball")]
    public class BallController : ControllerBase
    {
        private readonly IBallService _ballService;
        private readonly BallDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<BallController> _logger;


        public BallController(IBallService ballService, ILogger<BallController> logger)
        {
            _ballService = ballService;
            _logger = logger;
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateBallDto dto, [FromRoute]int id)
        {
            var ballOld = _ballService.GetById(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUpdated = _ballService.Update(id, dto);

            var ballNew = _ballService.GetById(id);

            _logger.LogInformation($"Updating ball with ID={id} | old name = {ballOld.Name} => new name = {ballNew.Name}, old size = {ballOld.Size} => new size = {ballNew.Size}, old description = {ballOld.Description} => new decription = {ballNew.Description}");

            if (!isUpdated)
            {
                _logger.LogError($"Ball with ID {id} not found for update.");
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete(("{id}"))]
        public ActionResult Delete([FromRoute] int id)
        {
            var ball = _ballService.GetById(id);

            _logger.LogInformation($"Deleting ball with ID = {id}, name = {ball.Name}, size = {ball.Size}, decryption = {ball.Description}");

            var isDeleted = _ballService.Delete(id);

            if (isDeleted)
            {
                return NoContent();
            }

            _logger.LogError($"Ball with ID {id} not found for deletion.");
            return NotFound();
        }

        [HttpPost]
        public ActionResult CreateBall([FromBody] CreateBallDto dto)
        {
            var id = _ballService.Create(dto);

            var ball = _ballService.GetById(id);

            _logger.LogInformation($"Created new ball with ID {id}, name = {ball.Name}, size = {ball.Size}, decription = {ball.Description}");

            return Created($"/api/ball/{id}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<BallDto>> GetAll()
        {
            _logger.LogInformation("Retrieving all balls.");
            var ballsDtos = _ballService.GetAll();

            return Ok(ballsDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<BallDto> Get([FromRoute]int id)
        {
            _logger.LogInformation($"Retrieving ball with ID = {id}");

            var ball = _ballService.GetById(id);

            if(ball == null)
            {
                _logger.LogError($"Ball with ID {id} not found.");
                return NotFound();
            }


            return Ok(ball);
        }

        
    }
}
