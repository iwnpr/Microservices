using CommandService.Models;

namespace CommandService.Data
{
    public interface ICommandRepository
    {
        bool SaveChange();

        IEnumerable<Platform> GetAllPlatforms();

        void CreatePlatform(Platform plat);

        bool PlatformExist(int platformId);

        IEnumerable<Command> GetCommandsForPlatoform(int platformId);

        Command GetCommand(int platformId, int commandId);

        void CreateCommand(int platoformId, Command command);




    }
}