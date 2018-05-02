using barefeetmodels_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace barefeetmodels_api.Controllers
{
    [RoutePrefix("api/Blobs")]
    public class BlobsController : ApiController
    {
        private Data.BareFeetModelsDBDataContext db = new Data.BareFeetModelsDBDataContext();

        // Interface in place so you can resolve with IoC container of your choice
        private readonly IBlobService _service = new BlobService();

        /// <summary>
        /// Uploads one or more blob files.
        /// </summary>
        /// <returns></returns>
        /// public async Task<IHttpActionResult> PostBlobUpload()
        [HttpPost, Route("Add")]
        [ResponseType(typeof(List<BlobUploadModel>))]
        public async Task<IHttpActionResult> PostBlobUpload()
        {
            try
            {
                //// This endpoint only supports multipart form data
                if (!Request.Content.IsMimeMultipartContent("form-data"))
                {
                    return StatusCode(HttpStatusCode.UnsupportedMediaType);
                }

                //// Call service to perform upload, then check result to return as content
                var result = await _service.UploadBlobs(Request.Content);
                if (result != null && result.Count > 0)
                {
                    // Save to database
                    try
                    {
                        foreach (var uploadedVideo in result)
                        {
                            Data.MstVideo newVideo = new Data.MstVideo()
                            {
                                Title = uploadedVideo.Title,
                                Description = uploadedVideo.Description,
                                DateUploaded = DateTime.Now,
                                ModelId = Convert.ToInt32(uploadedVideo.Model),
                                FileName = uploadedVideo.FileName,
                                FileUrl = uploadedVideo.FileUrl,
                                FileSizeInKb = uploadedVideo.FileSizeInKb,
                                FileSizeInBytes = uploadedVideo.FileSizeInBytes
                            };

                            db.MstVideos.InsertOnSubmit(newVideo);
                        }

                        db.SubmitChanges();

                        return Ok(result);
                    }
                    catch 
                    {
                        return BadRequest();
                    }
                }

                // Otherwise
                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Downloads a blob file.
        /// </summary>
        /// <param name="blobId">The ID of the blob.</param>
        /// <returns></returns>
        [HttpGet, Route("Download/{id}")]
        public async Task<HttpResponseMessage> GetBlobDownload(string id)
        {
            // IMPORTANT: This must return HttpResponseMessage instead of IHttpActionResult

            Int32 blobId = Convert.ToInt32(id);

            try
            {
                var result = await _service.DownloadBlob(blobId);
                if (result == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                // Reset the stream position; otherwise, download will not work
                result.BlobStream.Position = 0;

                // Create response message with blob stream as its content
                var message = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StreamContent(result.BlobStream)
                };

                // Set content headers
                message.Content.Headers.ContentLength = result.BlobLength;
                message.Content.Headers.ContentType = new MediaTypeHeaderValue(result.BlobContentType);
                message.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = HttpUtility.UrlDecode(result.BlobFileName),
                    Size = result.BlobLength
                };

                return message;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(ex.Message)
                };
            }
        }
    }
}
