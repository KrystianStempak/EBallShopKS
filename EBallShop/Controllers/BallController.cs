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
        public BallController(IBallService ballService)
        {
            _ballService = ballService;
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateBallDto dto, [FromRoute]int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUpdated = _ballService.Update(id, dto);
            if (!isUpdated)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete(("{id}"))]
        public ActionResult Delete([FromRoute] int id)
        {
            var isDeleted = _ballService.Delete(id);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult CreateBall([FromBody] CreateBallDto dto)
        {
            var id = _ballService.Create(dto);

            return Created($"/api/ball/{id}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<BallDto>> GetAll()
        {
            var ballsDtos = _ballService.GetAll();

            return Ok(ballsDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<BallDto> Get([FromRoute]int id)
        {
            var ball = _ballService.GetById(id);

            if(ball == null)
            {
                return NotFound();
            }


            return Ok(ball);
        }

        
    }
}
