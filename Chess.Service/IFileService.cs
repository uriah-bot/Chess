using Chess.Model;

namespace Chess.Service
{
    public interface IFileService
    {
        string SelectFile(string filterName, string[] fileTypes);
        string SaveFileForUser<T>(string sourceFilePath, UserEntity user) where T : DBEntity;
        IEnumerable<RadioChannelEntity> GetUserRadioFiles(UserEntity user);
    }
}
