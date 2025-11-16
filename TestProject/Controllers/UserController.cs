using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Work.ApiModels;
using Work.Database;
using Work.Interfaces;

namespace Work.Controllers
{
    [ApiController]
    public class UserController(IRepository<User, Guid> repository, IMapper mapper, ILogger<UserController> logger) : ControllerBase
    {
        private readonly IRepository<User, Guid> _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UserController> _logger = logger;

        [HttpGet("GetUser")]
        public IActionResult Get(Guid id)
        {
            var user = _repository.Read(id);
            if (user == null)
            {
                return NotFound($"User with user id {id} not found.");
            }

            return Ok(user);
        }

        [HttpPost("CreateUser")]
        public IActionResult Post(UserModelDto user)
        {
            try
            {
                _repository.Create(_mapper.Map<User>(user));
                _logger.LogInformation("User created with id {id}", user.Id);
                return Ok();
            }
            catch (ArgumentException)
            {
                _logger.LogInformation("User creation failed, user with id {id} already exists", user.Id);
                return BadRequest($"User with id {user.Id} already exists");
            }
            catch (Exception)
            {
                _logger.LogWarning("Unexpected error occured during user creation for user with id {id}", user.Id);
                return StatusCode(500);
            }
        }

        [HttpPut("UpdateUser")]
        public IActionResult Put(UserModelDto user)
        {
            try
            {
                _logger.LogInformation("User with id {id} updated", user.Id);
                _repository.Update(_mapper.Map<User>(user));
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                _logger.LogInformation("User update failed, user with id {id} does not exists", user.Id);
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
                _logger.LogWarning("Unexpected error occured during user update for user with id {id}", user.Id);
                return StatusCode(500);
            }
        }

        [HttpDelete("RemoveUser")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var user = _repository.Read(id);
                if (user == null)
                {
                    return NotFound($"User with user id {id} not found.");
                }
                _repository.Remove(user);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

    }
}