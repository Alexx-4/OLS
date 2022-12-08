using System;
using Unity;
using System.Collections.Generic;
using System.Linq;
using OpenLatino.MapServer.Domain.Map.Render;
using OpenLatino.MapServer.Domain.Entities;
using System.IO;
using OpenLatino.MapServer.Domain.Entities.Functions.WMS;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Querys;
using OpenLatino.MapServer.Domain.Entities.Querys.WMS;
using Unity.Lifetime;
using Unity.Injection;

namespace TileGenerator.Aplication
{ 
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
     public class UnityConfiguration
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion
       
        public static void Load<T>(IUnityContainer container)
        {         
            try
            {
                Type typeprov = typeof(T);
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeprov);
                string directory = System.IO.Path.GetDirectoryName((new Uri(assembly.CodeBase)).LocalPath);

                directory = directory + System.IO.Path.DirectorySeparatorChar;
                DirectoryInfo di = new DirectoryInfo(directory);

                foreach (FileInfo file in di.GetFiles("*.dll"))
                {
                    try
                    {
                        if (file.Name.StartsWith("Open"))
                        {}
                        System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom(directory + Path.DirectorySeparatorChar + file.Name);
                        foreach (Type type in asm.GetTypes())
                        {
                            if (type.GetInterfaces().Contains(typeof(T)) && !type.IsAbstract && !type.IsInterface)
                            {
                                container.RegisterType(type.GetInterfaces()[0], type, type.Name);
                            }

                        }
                    }
                    catch
                    {}
                }
            }
            catch
            {}
        }
        public static List<IWMSMapQuery> LoadQ(IUnityContainer container)
        {

            List<IWMSMapQuery> mapquerys = new List<IWMSMapQuery>();
            try
            {
                Type typeprov = typeof(IWMSMapQuery);
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(typeprov);
                string directory = System.IO.Path.GetDirectoryName((new Uri(assembly.CodeBase)).LocalPath);

                directory = directory + System.IO.Path.DirectorySeparatorChar;
                DirectoryInfo di = new DirectoryInfo(directory);

                foreach (FileInfo file in di.GetFiles("*.dll"))
                {
                    try
                    {
                        if (file.Name.StartsWith("Open"))
                        {
                        }
                        System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom(directory + Path.DirectorySeparatorChar + file.Name);
                        foreach (Type type in asm.GetTypes())
                        {
                            if (type.GetInterfaces().Contains(typeof(IWMSMapQuery)) && !type.IsAbstract && !type.IsInterface)
                            {
                                container.RegisterType(type.GetInterfaces()[0], type, type.Name);
                                IWMSMapQuery instance = (IWMSMapQuery)container.Resolve(type);
                                mapquerys.Add(instance);
                            }

                        }
                    }
                    catch
                    {}
                }
            }
            catch
            {}
            return mapquerys;
        }

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            Load<IProviderService>(container);
            Load<IQuery>(container);
            Load<IWMSFunction>(container);
            //Load<IWMSMapQuery>(container);
            container
                .RegisterType<ILayerRender, LayerRender>(new ContainerControlledLifetimeManager())
                .RegisterType<IRender, TileRender>(new ContainerControlledLifetimeManager(), new InjectionProperty("LayerRender"));
        }

    }
}
