using barefeetmodels_api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace barefeetmodels_api.Controllers
{
    public class BlobStorageUploadProvider : MultipartFileStreamProvider
    {
        public List<BlobUploadModel> Uploads { get; set; }

        public BlobStorageUploadProvider()
            : base(Path.GetTempPath())
        {
            Uploads = new List<BlobUploadModel>();
        }

        public override Task ExecutePostProcessingAsync()
        {
            // NOTE: FileData is a property of MultipartFileStreamProvider and is a list of multipart
            // files that have been uploaded and saved to disk in the Path.GetTempPath() location.
            int counter = 1;
            string title = "";
            string model = "";
            string description = "";

            foreach (var fileData in FileData)
            {
                

                if (fileData.Headers.ContentDisposition.Name.Replace("\"", "") == "File")
                {
                    // Sometimes the filename has a leading and trailing double-quote character
                    // when uploaded, so we trim it; otherwise, we get an illegal character exception
                    var fileName = Path.GetFileName(fileData.Headers.ContentDisposition.FileName.Trim('"'));

                

                    // Retrieve reference to a blob
                    var blobContainer = BlobHelper.GetBlobContainer();
                    var blob = blobContainer.GetBlockBlobReference(fileName);

                    // Set the blob content type
                    blob.Properties.ContentType = fileData.Headers.ContentType.MediaType;

                    // Upload file into blob storage, basically copying it from local disk into Azure
                    using (var fs = File.OpenRead(fileData.LocalFileName))
                    {
                        blob.UploadFromStream(fs);
                    }

                    // Delete local file from disk
                    File.Delete(fileData.LocalFileName);

                    // Create blob upload model with properties from blob info
                    var blobUpload = new BlobUploadModel {
                        FileName = blob.Name,
                        FileUrl = blob.Uri.AbsoluteUri,
                        FileSizeInBytes = blob.Properties.Length,
                        Title = title,
                        Model = model,
                        Description = description
                    };

                    // Add uploaded blob to the list
                    Uploads.Add(blobUpload);
                }
                else
                {
                    switch(counter) {
                        case 1:
                            title = fileData.Headers.ContentDisposition.Name.Replace("\"", "");
                            break;
                        case 2:
                            model = fileData.Headers.ContentDisposition.Name.Replace("\"", "");
                            break;
                        case 3:
                            description = fileData.Headers.ContentDisposition.Name.Replace("\"", "");
                            break;
                    }
                }
                counter++;
            }

            return base.ExecutePostProcessingAsync();
        }
    }
}