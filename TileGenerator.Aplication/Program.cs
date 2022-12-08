using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using OpenLatino.MapServer.Domain.Map.Primitives.Geometry;
using TileGenerator.Domain.Entities;

namespace TileGenerator.Aplication
{
    public class Program
    {
       
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to TileGenerator");
            Console.ReadLine();
            Console.WriteLine("Please enter the provider conection string");
            string cs = "";
            cs = Console.ReadLine();
            if (cs == "")
                cs = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Victor\\Desktop\\arquitectura\\OpenLatino\\OpenLatino.MapServer.Server\\App_Data\\GeoCuba.mdf;Integrated Security=True";
            Console.WriteLine("Please enter the Server MongoDB conection string");
            string csM = "";
            csM = Console.ReadLine();
            if (csM == "")
                csM = "mongodb://localhost:27017";
            Console.WriteLine("Please enter the layers separated by comma ");
            string layers = Console.ReadLine();
            string[] l = layers.Split(',');
            List<string> k = l.ToList();
            Console.WriteLine("Please enter the scale");
            double scale = double.Parse(Console.ReadLine());
            var bbox = new Tuple<Point, Point>(new Point(-85.0000, 19.0000), new Point(-75.0000, 24.0000));

            GenerateTiles tile = new GenerateTiles(cs, scale, k, csM, "mongo");
            Resolve(layers, tile);
            tile.GeneratingTiles(scale, bbox, k);
        }

        private static void Resolve(string layers, GenerateTiles tile)
        {
            List<string> layersRequested = layers.Split(',').ToList();

            IUnityContainer container = UnityConfiguration.GetConfiguredContainer();
            Service s = new Service(container, layersRequested);

            tile.LayersList = s.Layers.ToList();
            tile.StylesList = s.Styles.ToList();
            tile.providerList = s.Providers.ToList();
            tile.MapQuerys = s.Querys;
        }

      
      
    }
}
