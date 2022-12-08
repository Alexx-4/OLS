using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using RBush;
using System.Collections.Generic;
using OpenLatino.MapServer.Domain.Entities.Auxiliars;

namespace OpenLatino.MapServer.Domain.Entities.FileData
{
    /// <summary>
    /// Basic strucuture for operationns in a file provider 
    /// </summary>
    public interface IFileDataStructure<T> where T : IFeature
    {
        /// <summary>
        /// Add one item to data structure
        /// </summary>
        void Add(T item);
        /// <summary>
        /// Remove one specif item to datastrcuture
        /// </summary>
        /// <param name="item"></param>
        void Remove(T item);

        /// <summary>
        /// Remove all item for data stucture
        /// </summary>
        void Clear();

        /// <summary>
        /// Insert a list of elements in the structure
        /// </summary>
        /// <param name="items"></param>
        void Load(List<T> items);

        /// <summary>
        /// Return the element that intercept the bounding box
        /// </summary>
        /// <param name="boundingBox"></param>
        /// <returns></returns>
        List<T> Search(in Envelope boundingBox);


        /// <summary>
        /// Return the number of Ifeature inside the rtree
        /// </summary>
        /// <returns></returns>
        int Count();

        //Operaciones WMS

        /// <summary>
        /// Return a list of geometries full contained in the specific boundingbox
        /// </summary>
        /// <param name="boundingBox"></param>
        /// <returns></returns>
        List<GeometryWithFeatures> RangeQuery(Envelope boundingBox);

        List<GeometryWithFeatures> GetFeatureInfo(double x, double y, int k);

        List<GeometryWithFeatures> GetFeatureInfo(Envelope envelope);


        //Operaciones Espaciales

        IEnumerable<Geometry> Interception(List<Point> polygon);
        IEnumerable<Geometry> Overlap(List<Point> polygon);
        IEnumerable<Geometry> Interiors(List<Point> polygon);
        IEnumerable<Geometry> Out(List<Point> polygon);
        // Operaciones espaciales con circunferencias
        //TODO
        List<Geometry> KnearestNeighboord(double x, double y, int k);



        Envelope BBox { get; set; }

    }
}