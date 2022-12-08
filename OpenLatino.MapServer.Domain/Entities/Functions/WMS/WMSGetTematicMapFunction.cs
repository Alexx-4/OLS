using System;
using System.Collections.Generic;
using System.Linq;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Querys.WMS;
using OpenLatino.MapServer.Domain.Entities.Requests;
using OpenLatino.MapServer.Domain.Entities.Response;
using OpenLatino.MapServer.Domain.Map.Primitives;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using OpenLatino.MapServer.Domain.Map.Render;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace OpenLatino.MapServer.Domain.Entities.Functions.WMS
{
    public class WMSGetTematicMapFunction : WMSMapFunction
    {
        public WMSGetTematicMapFunction()
        { }

        public WMSGetTematicMapFunction(IEnumerable<IWMSMapQuery> map_querys, IRender render) : base(map_querys, render)
        {
        }

        public override bool CanResolve(IRequest request)
        {
            return request.HasParameter("SERVICE") && request["SERVICE"] == _service &&
                request.HasParameter("REQUEST") && request["REQUEST"] == GetMap
                && request.HasParameter("TEMATICLAYERID") && request["TEMATICLAYERID"] != ""
                && request.HasParameter("VERSION") && request["VERSION"] == _version && request.HasParameter("LAYERS")
                && request.HasParameter("STYLES") && request.HasParameter("CRS")
                && request.HasParameter("BBOX") && request.HasParameter("WIDTH")
                && request.HasParameter("HEIGHT") && request.HasParameter("FORMAT");
        }

        public override async Task<IResponse> Process(IRequest request, IEnumerable<IProviderService> providers, IEnumerable<Layer> layers, WorkSpace workSpace, IEnumerable<ILegendResponse> Legend)
        {
            List<string> stylesRequested, layersRequested, tematicLayersRequested;
            int[] Size;
            string crs, bg, format;
            Tuple<Point, Point> bbox;
            bool transparent;
            ObtainRequest(request, out stylesRequested, out Size, out crs, out bbox, out bg, out transparent, out layersRequested, out format);
            tematicLayersRequested = request["TEMATICLAYERID"].Split(',').ToList();
            MapResponse response = new MapResponse();

            if (useCache)
            {
                //cambiar esto para llamar un nuevo metodo del render para los mapas tematicos
                var result = await Render.RenderUsingCacheAsync(layersRequested, stylesRequested, bbox, crs, transparent, format, bg, Size, workSpace, mongoCs);
                if (result != null)
                    response.Tile = result;
            }
            if (response.Tile == null || !useCache)
            {
                Render.UpdateLayerList(tematicLayersRequested);
                IEnumerable<Element> elems = await GetGeometry(providers, Render.LayerList, bbox, crs);
                response.Tile = await Render.TematicRenderCaheDisableAsync(elems, layersRequested, tematicLayersRequested, stylesRequested, bbox, crs, transparent, format, bg, Size, workSpace, mongoCs);
            }

            return response;
        }
        
    }
}
