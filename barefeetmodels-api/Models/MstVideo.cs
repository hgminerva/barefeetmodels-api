using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace barefeetmodels_api.Models
{
    public class MstVideo
    {
	    public Int32 Id { get; set; }
	    public String Title { get; set; }
	    public String Description { get; set; }
	    public String DateUploaded { get; set; }
	    public Int32 ModelId { get; set; }
        public String Model { get; set; }
	    public String FileName { get; set; }
	    public String FileUrl { get; set; }
	    public Decimal FileSizeInKb { get; set; }
        public Decimal FileSizeInBytes { get; set; }
        public HttpContent File { get; set; }
        public String FileGifUrl { get; set; }
    }
}