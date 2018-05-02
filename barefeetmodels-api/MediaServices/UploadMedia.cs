using System.Configuration;
using System.Threading;
using System.IO;
using Microsoft.WindowsAzure.MediaServices.Client;

namespace barefeetmodels_api.MediaServices
{
    public class UploadMedia
    {
        static public IAsset CreateAssetAndUploadSingleFile(AssetCreationOptions assetCreationOptions, string singleFilePath)
        {
            if (!File.Exists(singleFilePath))
            {
                //Console.WriteLine("File does not exist.");
                return null;
            }

            var assetName = Path.GetFileNameWithoutExtension(singleFilePath);
            //IAsset inputAsset = _context.Assets.Create(assetName, assetCreationOptions);
            //IAsset inputAsset = new IAsset(); //_context.Assets.Create(assetName, assetCreationOptions);

            //var assetFile = inputAsset.AssetFiles.Create(Path.GetFileName(singleFilePath));

            //Console.WriteLine("Upload {0}", assetFile.Name);

            //assetFile.Upload(singleFilePath);
            //Console.WriteLine("Done uploading {0}", assetFile.Name);

            //return inputAsset;
            return null;
        }
    }
}