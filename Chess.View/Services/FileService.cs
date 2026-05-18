using Chess.Model;
using Microsoft.Win32;
using System.IO;

namespace Chess.Service
{
    public class FileService : IFileService
    {
        public IEnumerable<RadioChannelEntity> GetUserRadioFiles(UserEntity user)
        {
            if (user == null) return Enumerable.Empty<RadioChannelEntity>();

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string safeSalt = user.PasswordSalt.Replace("/", "_").Replace("+", "-").Replace("=", "");
            string userDirectory = Path.Combine(appDataPath, "Chess.View", "Assets", "Users", safeSalt);

            if (!Directory.Exists(userDirectory))
            {
                return Enumerable.Empty<RadioChannelEntity>();
            }

            var paths = Directory.GetFiles(userDirectory);

            List<RadioChannelEntity> channels = new List<RadioChannelEntity>(); 
            foreach (var path in paths)
            {
                channels.Add(new RadioChannelEntity
                {
                    ChannelPath = path,
                    ChannelName = Path.GetFileNameWithoutExtension(path)
                });
            }

            return channels;
        }

        public string SaveRadioFileForUser<RadioChannelEntity>(string sourceFilePath, UserEntity user)
        {
            if (user == null) return null;

            // Gets the hidden Windows AppData folder
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            string safeSalt = user.PasswordSalt.Replace("/", "_").Replace("+", "-").Replace("=", "");
            // Gets a unique folder just for this user (the hash is one-time and doesn't change)
            string userDirectory = Path.Combine(appDataPath, "Chess.View", "Assets" , "Users", safeSalt);

            // Ensures the folder actually exists on the hard drive
            Directory.CreateDirectory(userDirectory);

            // Gets just the file name (e.g., "avatar.png")
            string fileName = Path.GetFileName(sourceFilePath);

            // Creates the final destination path
            string destinationPath = Path.Combine(userDirectory, fileName);

            // Copies the file into safe zone! (overwrite: true just in case they upload the exact same file name again)
            File.Copy(sourceFilePath, destinationPath, overwrite: true);

            return destinationPath;
        }

        public string SelectFile(string filterName, string[] fileTypes)
        {
            var formattedTypes = fileTypes.Select(ext => ext.StartsWith(".") ? ext : "." + ext);

            string extensionString = string.Join(";", formattedTypes.Select(ext => "*" + ext));

            string finalFilter = $"{filterName}|{extensionString}";

            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = finalFilter,
                Title = "Select a File"
            };

            bool? result = dialog.ShowDialog();

            return result == true ? dialog.FileName : null;
        }
    }
}