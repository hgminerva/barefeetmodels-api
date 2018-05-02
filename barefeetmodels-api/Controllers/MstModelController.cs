using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using barefeetmodels_api.Models;

namespace barefeetmodels_api.Controllers
{
    [RoutePrefix("api/MstModel")] 
    public class MstModelController : ApiController
    {
        private Data.BareFeetModelsDBDataContext db = new Data.BareFeetModelsDBDataContext();

        // list
        [HttpGet, Route("List")]
        public List<MstModel> GetMstModel()
        {
            var models = from d in db.MstModels
                         select new MstModel
                         {
                            Id = d.Id,
                            FullName = d.FullName,
                            Description = d.Description
                         };
            return models.ToList();
        }

        // detail
        [HttpGet, Route("Detail/{id}")]
        public MstModel GetMstModelDetail(string id)
        {
            var model = from d in db.MstModels
                        where d.Id == Convert.ToInt32(id)
                        select new MstModel
                        {
                             Id = d.Id,
                             FullName = d.FullName,
                             Description = d.Description
                        };

            return model.FirstOrDefault();
        }

        // add
        [HttpPost, Route("Add")]
        public Int32 PostMstModel(MstModel model)
        {
            try
            {
                Data.MstModel newModel = new Data.MstModel()
                {
                    FullName = model.FullName,
                    Description = model.Description
                };

                db.MstModels.InsertOnSubmit(newModel);
                db.SubmitChanges();

                return newModel.Id;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        // save
        [HttpPut, Route("Save")]
        public HttpResponseMessage SaveMstModel(MstModel model)
        {
            try
            {
                var editModels = from d in db.MstModels where d.Id == Convert.ToInt32(model.Id) select d;
                if (editModels.Any())
                {
                    var editModel = editModels.FirstOrDefault();

                    editModel.FullName = model.FullName;
                    editModel.Description = model.Description;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // delete
        [HttpDelete, Route("Delete/{id}")]
        public HttpResponseMessage DeleteMstModel(string id)
        {
            try
            {
                var deleteModels = from d in db.MstModels where d.Id == Convert.ToInt32(id) select d;
                if (deleteModels.Any())
                {
                    db.MstModels.DeleteOnSubmit(deleteModels.FirstOrDefault());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

    }
}
