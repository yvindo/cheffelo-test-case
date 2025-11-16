using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Work.ApiModels;
using Work.Database;
using Work.Implementation;

namespace Work.Controllers
{
    [ApiController]
    public class UserController(UserRepository repository, IMapper mapper) : ControllerBase
    {
        private readonly UserRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public IActionResult Get(Guid id)
        {
            var user = _repository.Read(id);
            if (user == null)
            {
                return NotFound($"User with user id {id} not found.");
            }

            return Ok(user);
        }

        public IActionResult Post(UserModelDto user)
        {
            try
            {
                _repository.Create(_mapper.Map<User>(user));
                return Ok();
            }
            catch (ArgumentException)
            {
                return BadRequest($"User with id {user.Id}");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        
        public IActionResult Put(UserModelDto user)
        {
            try
            {
                _repository.Update(_mapper.Map<User>(user));
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

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