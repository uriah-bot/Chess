using Chess.Model;

namespace Chess.Service
{
    public interface IFileService
    {
        string SelectFile(string filterName, string[] fileTypes);
        string SaveFileForUser(string sourceFilePath, UserEntity user);
        IEnumerable<RadioChannelEntity> GetUserFiles(UserEntity user);
    }

}
