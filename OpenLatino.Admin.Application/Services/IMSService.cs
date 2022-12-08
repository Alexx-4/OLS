using Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Protocol;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Requests;
using OpenLatino.MapServer.Domain.Entities.Response;
using System.Net.Http;
using System.IO;
using OpenLatino.MapServer.Domain.Map.Render;
using System.Net;
using OpenLatino.MapServer.Infrastructure.SQL.DataSource;
using Openlatino.Admin.Infrastucture.DataContexts;
using OpenLatino.Core.Domain.Interfaces;
using System.Threading.Tasks;
using OpenLatino.Core.Domain.Models;
using Microsoft.Extensions.Options;

namespace OpenLatino.Admin.Application.Services
{
    public class IMSService
    {
        public IEnumerable<IProtocol> Protocols { get; set; }
        public IEnumerable<ILegendResponse> Legend { get; set; }
        public List<IProviderService> Providers { get; set; }
        public WorkSpace WorkSpace { get; set; }
        public IEnumerable<Layer> Layers { get; set; }
        private IUnityContainer container { get; set; }
        private AdminDb context;
        private WorkspaceService workspaceService;
        private WorkspaceFunctionService WorkspaceFunctionServices;
        private LayerService layerService;
        private TematicLayerService tematiclayerService;
        private string _connectionString;
        private bool isTematicRequest;

        public IMSService(IUnityContainer container, AdminDb adminDb, IUnitOfWork uow, IOptions<DbConnectionOpt> options)
        {
            _connectionString = options.Value.AdminDbConnection;
            this.context = adminDb;
            this.workspaceService = new WorkspaceService(uow);
            this.WorkspaceFunctionServices = new WorkspaceFunctionService(uow);
            this.layerService = new LayerService(uow);
            this.tematiclayerService = new TematicLayerService(uow);
            this.container = container;
            this.isTematicRequest = false;
        }       

        public async Task<IResponse> Process(IRequest request)
        {
             return await Protocols.FirstOrDefault(p => p.CanResolve(request))?.Resolve(request, Providers, Layers, WorkSpace,Legend);
        }

        private void FillProviders(IEnumerable<ProviderInfo> providers_info, IUnityContainer container)
        {
            List<IProviderService> res = new List<IProviderService>();
            foreach (var item in providers_info)
            {
                Type type = Type.GetType(item.Type);
                IProviderService instance = (IProviderService)container.Resolve(type);
                //IProviderService instance = Providers.FirstOrDefault(sp => sp.GetType().ToString() == item.Type);
                instance.ID = item.Id;
                instance.BoundingBoxField = item.BoundingBoxField;
                instance.ConnectionString = item.ConnectionString;
                instance.GeoField = item.GeoField;
                instance.PkField = item.PkField;
                instance.Table = item.Table;
                instance.Layers = item.Layers;

                res.Add(instance);
            }
            Providers.AddRange(res);
        }

        //NEW----------------------------
        public async Task<HttpResponseMessage> GetImsResponse(ImsRequest request, HttpRequestMessage requestMessage)
        {
            #region Comprobaciones
            WorkSpace workspace = null;
            requestMessage.Headers.TryGetValues("Workspace", out IEnumerable<string> workspaces);

            //IEnumerable<string> workspaces = new string[] { "2003" }; //Para testing

            if (workspaces == null || workspaces.Count() < 1)
                workspace = workspaceService.GetWorkSpace(1); //si no pidio ninguno asignarle el common
            else
            {
                //El workspace debe ser un Id del tipo numérico
                if (!int.TryParse(workspaces.First(), out int workspaceId))
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);

                //Comprabando que el cliente tiene acceso al workspace solicitado
                workspace = workspaceService.GetWorkSpace(workspaceId, false);
                if (workspace == null)
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);

            }

            if(request.HasParameter("REQUEST") && request["REQUEST"] == "GetGraphicLegend")
            {
                if(request.HasParameter("FORMAT") && (request["FORMAT"].Contains("text/") || request["FORMAT"].Contains("image/")))
                {
                    
                }
                else
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var clientId = TokenService.GetClientId(TokenService.GetRequestToken(requestMessage.Headers));
            //var clientId = "08a5acb663e44349b7d53b794e431244"; //Para testing

            if (!workspace.CLientWorkSpaces.Any(cw => cw.ClientId == clientId))
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);

            //Comprobando que las capas solicitadas pertenecen al workspace y que las capas son validas
            var layersRequested = request.HasParameter("LAYERS") ? request["LAYERS"].Split(',').ToList() : new List<string>();
            
            foreach (var layer in layersRequested)
            {
                if (!int.TryParse(layer, out int id))
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                if (layerService.GetById(id) == null)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            if (layersRequested.Any(layId => (layId != "" && !workspace.LayerWorkspaces.Any(lw => lw.LayerId.ToString() == layId))))
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            //Comprobando que las capas tematicas solicitadas pertenecen al workspace y que las capas tematicas son validas
            var tematicLayersRequested = request.HasParameter("TEMATICLAYERID") && request["TEMATICLAYERID"] != "" ? request["TEMATICLAYERID"].Split(',').ToList() : new List<string>();

            foreach (var tematicLayer in tematicLayersRequested)
            {
                isTematicRequest = true;
                if (!int.TryParse(tematicLayer, out int id))
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);

                if (tematiclayerService.GetById(id) == null)
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            //Comprobar que tiene acceso a la funcion solicitada
            var functionRequestedId = request.HasParameter("REQUEST") ? WorkspaceFunctionServices.GetServiceFunctionId(request["REQUEST"]) : -1;
            if (functionRequestedId == -1 || functionRequestedId == null)
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            if (!workspace.ServiceFunctions.Any(sf => sf.FunctionId == functionRequestedId))
                return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            //int tematicLayerId;
            //if (request.HasParameter("TEMATICLAYERID") && request["TEMATICLAYERID"] != "" && !int.TryParse(request["TEMATICLAYERID"],out tematicLayerId))
            //    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);

            //preparando el workspace para que sus estilos funcionen
            WorkSpace = workspaceService.FillWorkspaceStyles(workspace);

            #endregion

            try
            {
                Protocols = new List<IProtocol>(await Task.Run(()=> container.ResolveAll<IProtocol>()));
                Providers = new List<IProviderService>(await Task.Run(()=> container.ResolveAll<IProviderService>()));
                Legend = new List<ILegendResponse>(await Task.Run(() => container.ResolveAll<ILegendResponse>()));

                Layers = await InitConfig(layersRequested, tematicLayersRequested, container);

                //Adding a sql provider to access server database for getCapabilities request 
                Providers.Add(new DBContextProvider() { ConnectionString = _connectionString });
                List<ProviderInfo> providers_info = Layers.Select(layer => layer.ProviderInfo).ToList();
                await Task.Run(()=> FillProviders(providers_info, container));

                //Obtain Response
                var response = await Process(request);

                //Return Response
                if (response is IErrorResponse)
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);

                if (response != null)
                    if (response.HasImage())
                    {
                        var httpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                        httpResponse.Content = new StreamContent((Stream)response.GetImage());
                        httpResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                        return httpResponse;
                    }
                    else
                    {
                        var httpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
                        httpResponse.Content = (HttpContent)response.GetResponseContent();
                        httpResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(response.contentType);
                        return httpResponse;
                    }
        }
            catch (Exception e)
            {
                #if DEBUG
                var exception = new HttpResponseMessage(System.Net.HttpStatusCode.Conflict);
                exception.Content = new StringContent(e.Message);
                return exception;
                #else
                            throw new HttpResponseException(new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError));
                #endif

            }
#if DEBUG
    var httpRespose = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            httpRespose.Content = new StringContent("No se pudo obtener un protocolo que resolviera el pedido");
            return httpRespose;
#else
                return requestMessage.CreateResponse(System.Net.HttpStatusCode.OK, "No es posible responder a la solicitud");
                return Ok("No es posible responder a la solicitud");
#endif
        }

        private async Task<IEnumerable<Layer>> InitConfig(List<string> layersRequested, List<string> tematicLayersRequested, IUnityContainer container)
        {
            var tematicLayers = await Task.Run(() => context.getRopo<TematicLayer>()
                .GetAll(tematicLayer => tematicLayersRequested.Contains(tematicLayer.Id.ToString()), false, tematicLayer => tematicLayer.StyleConfiguration).ToList());
            List<Layer> layers = new List<Layer>();
            List<Layer> _tematicLayersRequested = new List<Layer>();
            if (!isTematicRequest)
                layers = await Task.Run(() => context.getRopo<Layer>()
                    .GetAll(layer => layersRequested.Contains(layer.Id.ToString()), false, layer => layer.StyleConfiguration, layer => layer.VectorStyles, layer => layer.AlfaInfoes, layer => layer.ProviderInfo.Layers, layer => layer.LayerTranslations).ToList());
            else
            {
                _tematicLayersRequested = await Task.Run(() => context.getRopo<Layer>()
                    .GetAll(layer => layersRequested.Contains(layer.Id.ToString()), false, layer => layer.StyleConfiguration, layer => layer.VectorStyles, layer => layer.AlfaInfoes, layer => layer.ProviderInfo.Layers, layer => layer.LayerTranslations).ToList());
                
                foreach (var item in tematicLayers)
                    layers = layers.Concat(item.StyleConfiguration.Select(sc => sc.Layer).ToList()).ToList();

                _tematicLayersRequested = _tematicLayersRequested.Where(l => !(layers.Contains(l))).ToList();
                layers = layers.Concat(_tematicLayersRequested).ToList();
            }

            var styleRepo = context.getRopo<VectorStyle>();

            //parche provisional para la leyenda!!!
            //foreach (var layer in layers)
            //{
            //    var first = layer.VectorStyles.FirstOrDefault();
            //    var defaultStyle = styleRepo.Find(first.VectorStyleId);
            //    layer.VectorStyles.FirstOrDefault().VectorStyle = defaultStyle;
            //}

            var styles = styleRepo.GetAll().ToList();
            IRender render= (container.Resolve<IRender>() as TileRender);

            //Task.WaitAll(Task.Run(() => render = container.Resolve<IRender>() as TileRender));
            if (render != null)
            {
                render.LayerList = layers;
                render.Styles = styles;
                render.tematicLayerList = tematicLayers;
                render._tematicLayersRequested = _tematicLayersRequested;
            }
            return layers;
        }

    }
}
