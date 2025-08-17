using CommandService.Models;

namespace CommandService.Data
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext _context;

        public CommandRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreateCommand(int platformId, Command command)
        {
            if(command is null)
                throw new ArgumentNullException(nameof(command));

            command.PlatformId = platformId;

            _context.Commands.Add(command);

        }

        public void CreatePlatform(Platform plat)
        {
            if(plat is null)
                throw new ArgumentNullException(nameof(plat));
                
            _context.Platforms.Add(plat);
            SaveChange();

        }

        public bool ExternalPlatformExist(int externalPlatformId)
        {
            return _context.Platforms.Any(p => p.Id == externalPlatformId);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _context.Commands.SingleOrDefault(c => c.PlatformId == platformId && c.Id == commandId);
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.Commands
                .Where(c => c.Id == platformId)
                .OrderBy(c => c.Platform.Name);
        }

        public bool PlatformExist(int platformId)
        {
            return _context.Platforms.Any(p => p.Id == platformId);
        }

        public bool SaveChange()
        {
            return _context.SaveChanges() >= 0;
        }

    }
}