using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IMapper _mapper;
        public CommandController(ICommandRepository commandRepository, IMapper mapper)
        {
            _commandRepository = commandRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");

            if (!_commandRepository.PlatformExist(platformId))
            {
                return NotFound();
            }

            var commands = _commandRepository.GetCommandsForPlatform(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommand")]
        public ActionResult<CommandReadDto> GetCommand(int platformId, int commandId)
        {
            Console.WriteLine($"--> Hit GetCommand: {platformId} / {commandId}");

            if (!_commandRepository.PlatformExist(platformId))
            {
                return NotFound();
            }

            var command = _commandRepository.GetCommand(platformId, commandId);

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(int platformId, CommandCreateDto commandCreateDto)
        {
            Console.WriteLine($"--> Hit CreateCommand: {platformId}");

            if (!_commandRepository.PlatformExist(platformId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandCreateDto);

            _commandRepository.CreateCommand(platformId, command);
            _commandRepository.SaveChange();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommand), new { platformId, commandId = commandReadDto.Id }, commandReadDto);
        }
        
    }
}