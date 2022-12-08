using OpenLatino.Core.Domain;
using OpenLatino.MapServer.Domain.Entities;
using OpenLatino.MapServer.Domain.Map.Primitives;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OpenLatino.Core.Domain.Entities;


namespace OpenLatino.MapServer.Domain.Map.Render
{
    public interface IRender
    {
        Task<IMapImage> TematicRenderCaheDisableAsync(IEnumerable<Element> elems, List<string> layersRequested, List<string> tematicLayersRequested, List<string> stylesRequested, Tuple<Point, Point> bbox, string crs, bool transparent, string format, string bg, int[] size, WorkSpace workSpace = null, string mongoCs = "");
        Task<IMapImage> RenderCaheDisableAsync(IEnumerable<Element> elems, List<string> layersRequested, List<string> stylesRequested, Tuple<Point, Point> bbox, string crs, bool transparent, string format, string bg, int[] size, WorkSpace workSpace = null, string mongoCs="");
        Task<IMapImage> RenderUsingCacheAsync(List<string> layersRequested, List<string> stylesRequested, Tuple<Point, Point> bbox, string crs, bool transparent, string format, string bg, int[] size, WorkSpace workSpace = null, string mongoCs= "mongodb://localhost:27017");
        Tuple<Point, Point> MaxBoundingBox { get; set; }
        IEnumerable<Layer> LayerList { get; set; }
        IEnumerable<Layer> _tematicLayersRequested { get; set; }
        IEnumerable<TematicLayer> tematicLayerList { get; set; }
        IEnumerable<VectorStyle> Styles { get; set; }
        void UpdateLayerList(List<string> tematicLayersRequested);
    }
}
