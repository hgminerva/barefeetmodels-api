using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using barefeetmodels_api.Models;

namespace barefeetmodels_api.Controllers
{
    [RoutePrefix("api/MstVideo")] 
    public class MstVideoController : ApiController
    {
        private Data.BareFeetModelsDBDataContext db = new Data.BareFeetModelsDBDataContext();

        // list
        [HttpGet, Route("List/{skip}")]
        public List<MstVideo> GetMstModel(String skip)
        {
            var videos = from d in db.MstVideos
                         orderby d.Id descending
                         select new MstVideo
                         {
                            Id = d.Id,
                            Title = d.Title,
                            Description = d.Description,
                            DateUploaded = d.DateUploaded.ToShortDateString(),
                            ModelId = d.ModelId,
                            Model = d.MstModel.FullName,
                            FileName = d.FileName,
                            FileUrl = d.FileUrl,
                            FileSizeInKb = d.FileSizeInKb,
                            FileSizeInBytes = d.FileSizeInBytes,
                            FileGifUrl = d.FileGifUrl
                         };
            return videos.Skip(Convert.ToInt32(skip)).Take(20).ToList();
        }

        // detail
        [HttpGet, Route("Detail/{id}")]
        public MstVideo GetMstModelDetail(string id)
        {
            var videos = from d in db.MstVideos where d.Id == Convert.ToInt32(id)
                         select new MstVideo
                         {
                             Id = d.Id,
                             Title = d.Title,
                             Description = d.Description,
                             DateUploaded = d.DateUploaded.ToShortDateString(),
                             ModelId = d.ModelId,
                             Model = d.MstModel.FullName,
                             FileName = d.FileName,
                             FileUrl = d.FileUrl,
                             FileSizeInKb = d.FileSizeInKb,
                             FileSizeInBytes = d.FileSizeInBytes,
                             FileGifUrl = d.FileGifUrl
                         };
            return videos.FirstOrDefault();
        }
        
        // save
        [HttpPut, Route("Save")]
        public HttpResponseMessage SaveMstVideo(MstVideo video)
        {
            try
            {
                var editVideos = from d in db.MstVideos where d.Id == Convert.ToInt32(video.Id) select d;
                if (editVideos.Any())
                {
                    var editVideo = editVideos.FirstOrDefault();

                    editVideo.Title = video.Title;
                    editVideo.ModelId = video.ModelId;
                    editVideo.Description = video.Description;
                    editVideo.FileGifUrl = video.FileGifUrl;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }

}
