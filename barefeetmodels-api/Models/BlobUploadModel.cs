using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace barefeetmodels_api.Models
{
    public class BlobUploadModel
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public long FileSizeInBytes { get; set; }
        public long FileSizeInKb { get { return (long)Math.Ceiling((double)FileSizeInBytes / 1024); } }

        public string Title { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
    }
}