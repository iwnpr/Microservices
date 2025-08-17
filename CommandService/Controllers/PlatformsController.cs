using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepository commandRepository, IMapper mapper)
        {
            _commandRepository = commandRepository;
            _mapper = mapper;
        }
    
        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms from Command Service");

            var platforms = _commandRepository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpPost]
        public ActionResult TestInputConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");

            return Ok("Inbound test from Platforms Controller");
        }
    }
}