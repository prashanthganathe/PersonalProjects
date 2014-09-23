using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Windows8Theme.Common
{
    public class AppConfig
    {
        public static string KnownFolderRootDir = "Carl";
        public static string KnownFolderDownload = "Download";
        public static string KnownFolderUpload = "Upload";


        public async Task<StorageFolder> GetFileReference(string libraryFolder,string foldername)
        {
            StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
            IReadOnlyList<StorageFolder> fol = await picturesFolder.GetFoldersAsync();
            foreach (StorageFolder folder in fol)
            {
                if (folder.Name == foldername)
                {
                   
                    return folder;
                }
            }
            return null;
        }



    }
}