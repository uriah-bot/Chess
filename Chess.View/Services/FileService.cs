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

            // gets the hidden Windows AppData folder
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            // makes the path for the files that's specific for each user
            string safeSalt = user.PasswordSalt.Replace("/", "_").Replace("+", "-").Replace("=", "");
            string userDirectory = Path.Combine(appDataPath, "Chess.View", "Assets", "Users", safeSalt);

            if (!Directory.Exists(userDirectory))
            {
                return Enumerable.Empty<RadioChannelEntity>();
            }

            // take files and create them into radioChannelEntities
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

            // gets the hidden Windows AppData folder
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // makes the path for the files that's specific for each user
            string safeSalt = user.PasswordSalt.Replace("/", "_").Replace("+", "-").Replace("=", "");
            string userDirectory = Path.Combine(appDataPath, "Chess.View", "Assets" , "Users", safeSalt);

            // ensures the folder actually exists on the hard drive
            Directory.CreateDirectory(userDirectory);

            // gets just the file name (e.g., "avatar.png")
            string fileName = Path.GetFileName(sourceFilePath);

            // creates the final destination path
            string destinationPath = Path.Combine(userDirectory, fileName);

            // copies the file into the user's path (overwrite: true just in case they upload the exact same file name again)
            File.Copy(sourceFilePath, destinationPath, overwrite: true);

            return destinationPath;
        }

        public string SelectFile(string filterName, string[] fileTypes)
        {
            // gets formatting types from an array of strings
            var formattedTypes = fileTypes.Select(ext => ext.StartsWith(".") ? ext : "." + ext);

            string extensionString = string.Join(";", formattedTypes.Select(ext => "*" + ext));

            // displays when selecting a file
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