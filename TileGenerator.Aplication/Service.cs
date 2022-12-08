using OpenLatino.Core.Domain;
using OpenLatino.MapServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Openlatino.Admin.Infrastucture.DataContexts;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Querys.WMS;
using Unity;

namespace TileGenerator.Aplication
{
    public class Service
    {
        //por que razon tiene esto aqui una referencia a una base de datos de Admin, eso no esta desacoplado
        //private AdminDb db = new AdminDb();
        //public IEnumerable<IProviderService> Providers { get; set; }
        //public IEnumerable<Layer> Layers { get; set; }
        //public IEnumerable<VectorStyle> Styles { get; set; }
        //public IEnumerable<IWMSMapQuery> Querys { get; set; }
        //public  Service(IUnityContainer container,List<string>layers)
        //{
        //    Layers = db.Layers.Where(layer => layers.Contains(layer.Id.ToString()));
        //    Styles = db.Styles;
        //    List<ProviderInfo> providers = db.Layers.Select(l => l.Provider).ToList();
        //    Providers = FillProviders(providers, container);
        //    Querys= UnityConfiguration.LoadQ(container);
        //}

        //private List<IProviderService> FillProviders(IEnumerable<ProviderInfo> providers_info, IUnityContainer container)
        //{
        //    List<IProviderService> res = new List<IProviderService>();
        //    foreach (var item in providers_info)
        //    {
        //        Type type = Type.GetType(item.Type);
        //        IProviderService instance = (IProviderService)container.Resolve(type);
        //        instance.ID = item.Id;
        //        instance.BoundingBoxField = item.BoundingBoxField;
        //        instance.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Victor\Desktop\OpenLatino\OpenLatino\OpenLatino.MapServer.Server\App_Data\GeoCuba.mdf;Integrated Security=True";
        //        instance.GeoField = item.GeoField;
        //        instance.PkField = item.PkField;
        //        instance.Table = item.Table;
        //        instance.Layers = item.Layers;

        //        res.Add(instance);
        //    }
        //    return res;
        //}

    }
}
