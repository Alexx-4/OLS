using System;
using System.Net.Http;
using System.Threading.Tasks;
using OpenLatino.Admin.Application.Services;
using Openlatino.Admin.Infrastucture.DataContexts;
using OpenLatino.Core.Domain.Interfaces;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Entities.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using OpenLatino.Core.Domain.Models;

namespace OpenLatino.Admin.Server.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ImsController : System.Web.Http.ApiController //ControllerBase
    {
        private AdminDb _adminDb;
        private IUnitOfWork _unitOfWork;
        private IOptions<DbConnectionOpt> _dbConnection;
        public ImsController(AdminDb db, IUnitOfWork uow, IOptions<DbConnectionOpt> options)
        {
            _unitOfWork = uow;
            _adminDb = db;
            _dbConnection = options;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Index()
        {
            var IMSService = new IMSService(UnityConfig.GetConfiguredContainer(), _adminDb, _unitOfWork, _dbConnection);
            return await IMSService.GetImsResponse(Request, Request);
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Index([FromBody] SpatialGeometriesRequests[] geos)
        {
            if (geos == null)
                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest);

            var request = (ImsRequest)Request;
            request.Body = geos;
            try
            {
                var IMSService = new IMSService(UnityConfig.GetConfiguredContainer(), _adminDb, _unitOfWork, _dbConnection);
                return await IMSService.GetImsResponse(request, Request);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }           
        }
    }
}
