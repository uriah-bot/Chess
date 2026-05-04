using Chess.Model;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.TextFormatting;

namespace Chess.Service
{
    public class FileService : IFileService
    {
        public IEnumerable<RadioChannelEntity> GetUserFiles(UserEntity user)
        {
            if (user == null) return Enumerable.Empty<RadioChannelEntity>();

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string userDirectory = Path.Combine(appDataPath, "Chess.View", "Assets", "Users", user.Id.ToString());

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
                    ChannelName = Path.GetFileName(path)
                });
            }

            return channels;
        }

        public string SaveFileForUser(string sourceFilePath, UserEntity user)
        {
            if (user == null) return null;

            // 1.Get the hidden Windows AppData folder(C: \Users\Name\AppData\Local)
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // 2. Build a unique folder just for this user: \Local\ChessApp\Users\JohnDoe\
            string userDirectory = Path.Combine(appDataPath, "Chess.View", "Assets" , "Users", user.Id.ToString());

            // Ensure the folder actually exists on the hard drive
            Directory.CreateDirectory(userDirectory);

            // 3. Get just the file name (e.g., "avatar.png")
            string fileName = Path.GetFileName(sourceFilePath);

            // 4. Create the final destination path
            string destinationPath = Path.Combine(userDirectory, fileName);

            // 5. Copy the file into our safe zone! (overwrite: true just in case they upload the exact same file name again)
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