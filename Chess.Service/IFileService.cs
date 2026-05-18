using Chess.Model;

namespace Chess.Service
{
    public interface IFileService
    {
        string SelectFile(string filterName, string[] fileTypes);
        string SaveRadioFileForUser<RadioChannelEntity>(string sourceFilePath, UserEntity user);
        IEnumerable<RadioChannelEntity> GetUserRadioFiles(UserEntity user);
    }
}
