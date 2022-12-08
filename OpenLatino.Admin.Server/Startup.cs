using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Openlatino.Admin.Infrastucture.DataContexts;
using OpenLatino.Admin.Application.Services;
using OpenLatino.Admin.Server.Controllers;
using OpenLatino.Core.Domain.Interfaces;
using OpenLatino.Core.Domain.Services;
using OpenLatino.MapServer.Domain.Entities.Functions;
using OpenLatino.MapServer.Domain.Entities.Functions.WMS;
using OpenLatino.MapServer.Domain.Entities.Protocol;
using OpenLatino.MapServer.Domain.Entities.Providers;
using OpenLatino.MapServer.Domain.Entities.Querys;
using OpenLatino.MapServer.Domain.Map.Render;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OpenLatino.Core.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Admin.Application.ServiceInterface;
using Security;
using Microsoft.AspNetCore.Authorization;
using OpenLatino.MapServer.Domain.Entities.Response;
using System.Threading.Tasks;
using System.Security.Claims;

namespace OpenLatino.Admin.Server
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();            
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder => {
                builder.WithOrigins("http://localhost:4200").AllowCredentials().AllowAnyMethod().AllowAnyHeader();
            }));

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var conn = Configuration.GetConnectionString("AdminDbConnection");

            services.AddDbContext<AdminDb>(options => {
                options.UseLazyLoadingProxies();
                options.UseSqlServer(conn);
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddWebApiConventions();
            services.AddScoped<IUnitOfWork, AdminDb>();
            services.AddScoped<IClientService, ClientService>();

            //---------Controllers Services--------------
            services.AddScoped<IAlfaInfoHelper, AlfaInfoService>();
            services.AddScoped<ILayerHelper, LayerService>();
            services.AddScoped<IProviderHelper, ProviderService>();
            services.AddScoped<IStyleHelper, StyleService>();
            services.AddScoped<ILanguageHelper, LanguageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITematicLayerHelper, TematicLayerService>();

            //-------------Configure Identity----------------
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AdminDb>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options => options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier);

            var connectionConf = Configuration.GetSection("ConnectionStrings");
            services.Configure<DbConnectionOpt>(connectionConf);

            //--------------Configure Auth-----------------
            var authConfSection = Configuration.GetSection("JWT");
            services.Configure<ConfigurationModel>(authConfSection);
            services.AddTransient<ITokenService, TokenService>();
            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options=>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret_key"]))
                }
            );

            services.AddAuthorization(opt=>
            {
                opt.AddPolicy("CanAccessToClient", policy =>
                {
                    policy.Requirements.Add(new CanAccessToClient());
                });
                opt.AddPolicy("IsClientOf", policy =>
                {
                    policy.Requirements.Add(new IsClientOf());
                });
                opt.AddPolicy("CanAccessToWorkspace", policy =>
                {
                    policy.Requirements.Add(new CanAccessToWorkspace());
                });
                opt.AddPolicy("CanEditWorkspace", policy =>
                {
                    policy.Requirements.Add(new CanEditWorkspace());
                });
                opt.AddPolicy("CanDeleteWorkspace", policy =>
                {
                    policy.Requirements.Add(new CanDeleteWorkspace());
                });
                opt.AddPolicy("CanEditUser", policy =>
                {
                    policy.Requirements.Add(new CanEditUser());
                });
                opt.AddPolicy("ClientCanAccessWorkspace", policy =>
                {
                    policy.Requirements.Add(new ClientCanAccessWorkspace());
                });
            });
            services.AddTransient<IAuthorizationHandler, CanAccessToCLientHandler>();
            services.AddTransient<IAuthorizationHandler, IsClientOfHandler>();
            services.AddTransient<IAuthorizationHandler, CanAccessToWorkspaceHandler>();
            services.AddTransient<IAuthorizationHandler, CarEditWorkspaceHandler>();
            services.AddTransient<IAuthorizationHandler, CanDeleteWorkspaceHandler>();
            services.AddTransient<IAuthorizationHandler, CanEditUserHandler>();
            services.AddTransient<IAuthorizationHandler, ClientCanAccessWorkspaceHandler>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseIdentity();

            app.UseCors("ApiCorsPolicy");


            app.UseMvc(routes =>
            {
                routes.MapRoute
                (
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute
                (
                    name: "IndexApi",//IndexApi
                    template: "api",
                    defaults: new { controller = nameof(ImsController).Replace("Controller", ""), action = nameof(ImsController.Index) }
                );

                routes.MapRoute
                (
                    name: "ApiExecuteMethods",
                    template: "api/{controller}/{action}"
                );

                //routes.MapRoute
                //(
                //    name: "DefaultApi",
                //    template: "api/{controller}/{id}",
                //    defaults: new { id = UrlParameter.Optional }
                //);

                SeedUsers(app.ApplicationServices);
            });            
        }

        private async void SeedUsers(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var rolManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            var admin = new ApplicationUser() { UserName = "adminopenlatino@gmail.com", Email = "adminopenlatino@gmail.com", EmailConfirmed = true};
            var rols = new[] { new IdentityRole("Admin"), new IdentityRole("RegularUser") };
            
            if ((await userManager.FindByEmailAsync(admin.Email)) == null)
                await userManager.CreateAsync(admin, "Admin_1234");
            foreach (var item in rols)
            {
                if ((await rolManager.FindByNameAsync(item.Name)) == null)
                    await rolManager.CreateAsync(item);
            }
            if (!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                var admin_fromDB = await userManager.FindByEmailAsync(admin.Email);
                await userManager.AddToRoleAsync(admin_fromDB, "Admin");
            }
        }
    }

    public class UnityConfig
    {
        #region Unity Container
        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            var newContainer = new UnityContainer();
            RegisterTypes(newContainer);
            return newContainer;

            //return container.value;
        }
        #endregion

        private static void Load<T>(IUnityContainer container)
        {
            List<Type> list = new List<Type>();

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
                        {
                        }
                        System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom(directory + Path.DirectorySeparatorChar + file.Name);
                        foreach (Type type in asm.GetTypes())
                        {
                            if (type.GetInterfaces().Contains(typeof(T)) && !type.IsAbstract && !type.IsInterface)
                                container.RegisterType(type.GetInterfaces()[0], type, type.Name);
                        }
                    }
                    catch
                    { }
                }
            }
            catch
            { }
        }

        private static void LoadInstance<T>(IUnityContainer container)
        {
            List<Type> list = new List<Type>();

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
                        { }
                        System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom(directory + Path.DirectorySeparatorChar + file.Name);
                        foreach (Type type in asm.GetTypes())
                        {
                            if (type == typeof(T))
                            { }
                        }
                    }
                    catch
                    { }
                }
            }
            catch
            { }
        }


        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            //esto es para que Unity sepa devolverme un IEnumerable de algo, ejemplo para que me pueda devolver un IEnumerable<IProtocol>
            container.RegisterType(typeof(IEnumerable<>), new InjectionFactory((IUnityContainer _container, Type type, string name) => _container.ResolveAll(type.GetGenericArguments().Single())));
            #if XML_CONFIG
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            container.LoadConfiguration();
            #endif
            //#if CONVENTION_CONFIG
            //LoadProviders(container);

            //LoadConverters(container);

            //LoadProtocols(container);

            //LoadFactories(container);

            Load<IProviderService>(container);

            Load<IProtocol>(container);

            Load<IQuery>(container);

            Load<IFunction>(container);

            Load<IWMSFunction>(container);

            Load<ILegendResponse>(container);

            //#endif

            //Registrando los singleton de las clases que se van a utilizar
            //ShapeEnclousureSingleton enclosureshape = ShapeEnclousureSingleton.Instance;
            //ShapeFenceBuilderSingleton fenceBuildershape = ShapeFenceBuilderSingleton.Instance;
            //SetOperationSingleton setOperationshape = SetOperationSingleton.Instance;




            container
                .RegisterType<IUnitOfWork, AdminDb>(new ContainerControlledLifetimeManager());
            container
                //.RegisterType<IGeometryFactory, GeometryFactory>(new ContainerControlledLifetimeManager())
                //.RegisterType<IQueryConverter, CapabilitiesQueryConverter>("CapabilitiesQueryConverter", new ContainerControlledLifetimeManager())
                //.RegisterType<IQueryConverter, FeatureInfoConverter>("FeatureInfoConverter", new ContainerControlledLifetimeManager(), new InjectionProperty("GeometryFactory"))
                //.RegisterType<IQueryConverter, MapQueryConverter>("MapQueryConverter", new ContainerControlledLifetimeManager(), new InjectionProperty("GeometryFactory"))
                //.RegisterType<IQueryConverter, SpatialConverter>("SpatialConverter", new ContainerControlledLifetimeManager(), new InjectionProperty("GeometryFactory"))
                //.RegisterType<IProtocol, WMSProtocol>("WMSProtocol", new ContainerControlledLifetimeManager(), new InjectionProperty("WMSfunctions"))
                .RegisterType<ILayerRender, LayerRender>(new ContainerControlledLifetimeManager())
                .RegisterType<IRender, TileRender>(new ContainerControlledLifetimeManager(), new InjectionProperty("LayerRender"));
        }
    }
}
