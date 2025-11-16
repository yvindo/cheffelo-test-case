using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Work.ApiModels;
using Work.Database;
using Work.Interfaces;

namespace Work.Controllers
{
    [ApiController]
    public class UserController(IRepository<User, Guid> repository, IMapper mapper) : ControllerBase
    {
        private readonly IRepository<User, Guid> _repository = repository;
        private readonly IMapper _mapper = mapper;

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
                return Ok();
            }
            catch (ArgumentException)
            {
                return BadRequest($"User with id {user.Id} already exists");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPut("UpdateUser")]
        public IActionResult Put(UserModelDto user)
        {
            try
            {
                _repository.Update(_mapper.Map<User>(user));
                return Ok();
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception)
            {
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