using Microsoft.EntityFrameworkCore;
using Openlatino.Admin.Infrastucture.DataContexts;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Internationalization;
using OpenLatino.Core.Domain.MapperStyle;
using OpenLatino.MapServer.Domain.Map.Filters;
using OpenLatino.MapServer.Domain.Map.Filters.Enums;
using OpenLatino.MapServer.Domain.Map.Filters.Helpers;
using OpenLatino.MapServer.Infrastucture.SQL.DataSource;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace OpenLatino.Admin.Infrastructure.DataContexts
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            
            #region Creando trigger
            //try
            //{
            //    modelBuilder.HasDbFunction()
            //    context.Database.ExecuteSqlCommand($@"
            //        CREATE TRIGGER UniqueLayerNameByProvider ON LayerTranslations 
            //        AFTER INSERT, UPDATE 
            //        AS
            //        BEGIN
	           //         DECLARE @count int
	           //         DECLARE @provider int	
	           //         DECLARE @name nvarchar(MAX)
	           //         DECLARE @language int 
	           //         DECLARE @layer int 
	                    
	           //         SELECT TOP 1 @provider=l.ProviderInfoId,@name=i.Name,@language=i.LanguageID,@layer=l.ID 
	           //         FROM inserted AS i	
	           //         JOIN Layers as l ON i.EntityID=l.ID	
	                    
	           //         SELECT @count=COUNT(*)
	           //         FROM LayerTranslations lt 
	           //         JOIN Layers AS l ON l.ID = lt.EntityID	
	           //         WHERE l.ProviderInfoId=@provider and lt.LanguageID=@language and @name=lt.Name and @layer<>l.ID	
	                    
	           //         IF @count>0
	           //         BEGIN
		          //          RAISERROR ('No pueden haber dos layers de un mismo proveedor con un mismo nombre para un mismo lenguaje', 16, 1);
		          //          ROLLBACK TRANSACTION;
		          //          RETURN 
	           //         END
            //        END");
            //}
            //catch (Exception)
            //{

            //    //throw;
            //}
            #endregion
            #region Configurando los lenguajes
            var spanish = new Language() { ID = 3082, LanguageName = "Espa�ol", Default = true };
            var english = new Language() { ID = 1033, LanguageName = "English" };
            modelBuilder.Entity<Language>().HasData(
                spanish,
                english,
                new Language() { ID = 1031, LanguageName = "Alem�n" }
            );
            #endregion           
            #region Configurando los proveedores y sus traducciones
            #region SqlRegion for providers and providers_translations            
            var provTransLatinoSpanish = new ProviderTranslations()
            {
                EntityId = 1,
                Name = "Latino DB",
                Description = "Descripci�n de latino",
                LanguageId = 3082,
            };
            var provTransLatinoEnglish = new ProviderTranslations()
            {
                Description = "Latino's description",
                Name = "Latino name",
                LanguageId = 1033,
                EntityId = 1,
            };

            var latinoProvider = new ProviderInfo()
            {
                Id = 1,
                Table = "points",
                GeoField = "ogr_geometry",
                BoundingBoxField = "BBOX_latino",
                ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\GeoCuba.mdf;Integrated Security=True",
                PkField = "osm_id",
                Type = typeof(ProviderSQL).AssemblyQualifiedName,
            };

            var provTransLatinoSpanish1 = new ProviderTranslations()
            {
                EntityId = 2,
                Name = "Latino1 DB",
                Description = "Descripci�n de latino1",
                LanguageId = 3082,
            };

            var latinoProvider1 = new ProviderInfo()
            {
                Id = 2,
                Table = "roads",
                GeoField = "ogr_geometry",
                BoundingBoxField = "BBOX_latino1",
                ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\GeoCuba.mdf;Integrated Security=True",
                PkField = "osm_id",
                Type = typeof(ProviderSQL).AssemblyQualifiedName
            };

            var provTransLatinoSpanish2 = new ProviderTranslations()
            {
                EntityId = 3,
                Name = "Latino2 DB",
                Description = "Descripci�n de latino2",
                LanguageId = 3082,
            };

            var latinoProvider2 = new ProviderInfo()
            {
                Id = 3,
                Table = "buildings",
                GeoField = "ogr_geometry",
                BoundingBoxField = "BBOX_latino1",
                ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\GeoCuba.mdf;Integrated Security=True",
                PkField = "osm_id",
                Type = typeof(ProviderSQL).AssemblyQualifiedName,
            };

            modelBuilder.Entity<ProviderInfo>().HasData(
                latinoProvider,
                latinoProvider1,
                latinoProvider2

            //old!!!!
            //,new ProviderInfo()
            //{
            //    Table = "points",
            //    GeoField = "ogr_geometry",
            //    BoundingBoxField = "BBOX_latino",
            //    ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\GeoCuba.mdf;Integrated Security=True",
            //    PkField = "osm_id",
            //    Type = typeof(ProviderSQL).AssemblyQualifiedName,
            //    ProviderTranslations = new List<ProviderTranslations>()
            //    {
            //        new ProviderTranslations()
            //        {
            //            Name = "Latino DB",
            //            Description = "Descripci�n de latino",
            //            Language = spanish
            //        },
            //        new ProviderTranslations()
            //        {
            //            Description = "Latino's description",
            //            Name = "Latino name",
            //            Language = english
            //        }
            //    }
            //}
            //,new ProviderInfo()
            //{
            //    Table = "roads",
            //    GeoField = "ogr_geometry",
            //    BoundingBoxField = "BBOX_latino1",
            //    ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\GeoCuba.mdf;Integrated Security=True",
            //    PkField = "osm_id",
            //    Type = typeof(ProviderSQL).AssemblyQualifiedName,
            //    ProviderTranslations = new List<ProviderTranslations>()
            //    {
            //        new ProviderTranslations()
            //        {
            //            Name = "Latino1 DB",
            //            Description = "Descripci�n de latino1",
            //            Language = spanish
            //        }
            //    }
            //}
            //,new ProviderInfo()
            //{
            //    Table = "buildings",
            //    GeoField = "ogr_geometry",
            //    BoundingBoxField = "BBOX_latino1",
            //    ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\GeoCuba.mdf;Integrated Security=True",
            //    PkField = "osm_id",
            //    Type = typeof(ProviderSQL).AssemblyQualifiedName,
            //    ProviderTranslations = new List<ProviderTranslations>()
            //    {
            //        new ProviderTranslations()
            //        {
            //            Name = "Latino2 DB",
            //            Description = "Descripci�n de latino1",
            //            Language = spanish
            //        }
            //    }
            //}

            );

            modelBuilder.Entity<ProviderTranslations>().HasData(
                provTransLatinoEnglish,
                provTransLatinoSpanish,
                provTransLatinoSpanish1,
                provTransLatinoSpanish2
                );
            #endregion
            #endregion
            #region Configurando las capas, sus traducciones y sus estilos

           VectorStyle buildingstyle = null,
                roadsstyle = null,
                pointsstyle = null,
                schoolStyle = null,
                hospitalStyle = null,
                universityStyle = null,
                buildingThematicStyle = null;

            

            BuildStyles();
            buildingstyle = VectorStyles["buildings_style"];
            roadsstyle = VectorStyles["roads_style"];
            pointsstyle = VectorStyles["points_style"];
            schoolStyle = VectorStyles["school_style"];
            hospitalStyle = VectorStyles["hospital_style"];
            universityStyle = VectorStyles["university_style"];
            buildingThematicStyle = VectorStyles["buildingThematic_style"];

            modelBuilder.Entity<LayerStyle>().HasData(
                new LayerStyle() {LayerId = 1, VectorStyleId = 1 },
                new LayerStyle() { LayerId = 2, VectorStyleId = 2 },
                new LayerStyle() { LayerId = 3, VectorStyleId = 3 },
                new LayerStyle() { LayerId = 4, VectorStyleId = 7 }
                );

            modelBuilder.Entity<VectorStyle>().HasData(buildingstyle, roadsstyle, pointsstyle, hospitalStyle, schoolStyle, universityStyle, buildingThematicStyle);

            Layer buildings = null, points = null, roads = null, buildingThematicLayer = null;

            var road_spanish_transl = new LayerTranslation()
            {
                Name = "calles",
                Description = "Esta capa representa a todas las capas de la ciudad",
                LanguageId = 3082,
                EntityId = 1,
            };

            roads = new Layer()
            {
                Id = 3,
                Order = 2,
                ProviderInfoId = 2,
            };

            var building_spanish_transl = new LayerTranslation()
            {
                Name = "Edificios",
                Description = "En esta capa est�n representados todos los edificios, casas e industrias.",
                LanguageId = 3082,
                EntityId = 2,
            };

            var building_engl_transl = new LayerTranslation()
            {
                Name = "Buildings",
                Description = "This lay contains all buildings",
                LanguageId = 1033,
                EntityId = 2,
            };

            buildings = new Layer()
            {
                Id = 1,
                Order = 3,
                ProviderInfoId = 3,
            };

            var points_english_trans = new LayerTranslation
            {
                Name = "Hotel layer",
                Description = "All hotels",
                LanguageId = 1033,
                EntityId = 3,
            };

            points = new Layer()
            {
                Id = 2,
                Order = 0,
                ProviderInfoId = 1,
            };

            #region Thematics Layers
            var thematic_spanish_trans = new LayerTranslation()
            {
                EntityId = 4,
                LanguageId = 3082,
                Name = "Building Thematic",
                Description = "Esta capa representa a todas las capas de la ciudad",
            };

            buildingThematicLayer = new Layer
            {
                Id = 4,
                Order = 4,
                ProviderInfoId = 3,
            };
            #endregion

            modelBuilder.Entity<LayerTranslation>().HasData(points_english_trans, building_engl_transl, building_spanish_transl, road_spanish_transl, thematic_spanish_trans);
            modelBuilder.Entity<Layer>().HasData(points, buildings, roads, buildingThematicLayer);
            #endregion
            #region Filters

            Core.Domain.Entities.TematicQuery defaultFilter = null,
                schoolFilter = null,
                hospitalFilter = null,
                primaryFilter = null,
                Calzada_ZapataFilter = null,
                universityFilter = null;

            BuildFilters();

            modelBuilder.Entity<Core.Domain.Entities.TematicQuery>().HasData(
            defaultFilter = new Core.Domain.Entities.TematicQuery
            {
                Id = 1,
                Name = "default",
                Function = Filters["default"]
            },
            schoolFilter = new Core.Domain.Entities.TematicQuery
            {
                Id = 2,
                Name = "school",
                Function = Filters["school"]
            },
            hospitalFilter = new Core.Domain.Entities.TematicQuery
            {
                Id = 3,
                Name = "hospital",
                Function = Filters["hospital"]
            },
            universityFilter = new Core.Domain.Entities.TematicQuery
            {
                Id = 4,
                Name = "university",
                Function = Filters["university"]
            },
            primaryFilter = new Core.Domain.Entities.TematicQuery
            {
                Id = 5,
                Name = "primary street",
                Function = Filters["primary"]
            },
            Calzada_ZapataFilter = new Core.Domain.Entities.TematicQuery
            {
                Id = 6,
                Name = "Calzada Zapata",
                Function = Filters["Calzada_Zapata"]
            }
            );
            #endregion
            #region Configurations

            modelBuilder.Entity<TematicLayer>().HasData(
                new TematicLayer
                {
                    Id = 1,
                    Name = "tematicPrimaryStreet"
                }
            );
            modelBuilder.Entity<TematicLayer>().HasData(
                new TematicLayer
                {
                    Id = 2,
                    Name = "tematicCalzadaZapata"
                }
            );
            modelBuilder.Entity<TematicLayer>().HasData(
                new TematicLayer
                {
                    Id = 3,
                    Name = "tematicHospital"
                }
            );

            modelBuilder.Entity<StyleConfig>().HasData(
                new StyleConfig
                {
                    LayerId = 3,
                    TematicTypeId = 5,
                    StyleId = 1,
                    TematicLayerId = 1
                },
                new StyleConfig
                {
                    LayerId = 3,
                    TematicTypeId = 6,
                    StyleId = 1,
                    TematicLayerId = 2
                },
                new StyleConfig
                {
                    LayerId = 1,
                    TematicTypeId = 3,
                    StyleId = 5,
                    TematicLayerId = 3
                }
            );
            #endregion
            #region Configurando la informaci�n alfanum�rica y sus traducciones

            AlfaInfo roadsInfo, buildingsInfo, pointsInfo, buildingThematicInfo;

            var alfInftransl1 = new AlfaInfoTranslation()
            {
                Name = "Roads Info",
                Description = "contain roads names, max speed and other infos",
                LanguageId = 1033,
                EntityId = 1,
            };

            var alfInftransl2 = new AlfaInfoTranslation()
            {
                Name = "Informaci�n de las construcciones",
                Description = "La informaci�n que contiene de las edificaciones es: tipo de edificaci�n (oficina, hotel, ...) y nombre",
                LanguageId = 3082,
                EntityId = 2,
            };

            var alfInftransl3 = new AlfaInfoTranslation()
            {
                Name = "points info",
                Description = "timestamp, name and type of points",
                LanguageId = 1033,
                EntityId = 3,
            };

            var alfInftransl4 = new AlfaInfoTranslation()
            {
                Name = "Informaci�n de las construcciones",
                Description = "La informaci�n que contiene de las edificaciones es: tipo de edificaci�n (oficina, hotel, ...) y nombre",
                LanguageId = 3082,
                EntityId = 4,
            };

            modelBuilder.Entity<AlfaInfoTranslation>().HasData(alfInftransl1, alfInftransl2, alfInftransl3, alfInftransl4);

            modelBuilder.Entity<AlfaInfo>().HasData(
                roadsInfo = new AlfaInfo()
                {
                    Id = 1,  //hay que especificarlo para que funcione
                    Columns = "type,oneway,bridge,maxspeed,name,ref",
                    ConnectionString = "Data Source=localhost;Initial Catalog=SpatialDemo;Integrated Security=True",
                    PkField = "osm_id",
                    Table = "roads",
                    LayerId = 3,
                },
                buildingsInfo = new AlfaInfo()
                {
                    Id = 2,
                    Columns = "name,type",
                    ConnectionString = "Data Source=localhost;Initial Catalog=SpatialDemo;Integrated Security=True",
                    LayerId = 1,
                    PkField = "osm_id",
                    Table = "buildings",
                },
                pointsInfo = new AlfaInfo()
                {
                    Id = 3,
                    Columns = "timestamp,name,type",
                    ConnectionString = "Data Source=localhost;Initial Catalog=SpatialDemo;Integrated Security=True",
                    LayerId = 2,
                    Table = "points",
                    PkField = "osm_id",
                },
                buildingThematicInfo = new AlfaInfo()
                {
                    Id = 4,
                    Columns = "name,type",
                    ConnectionString = "Data Source=localhost;Initial Catalog=SpatialDemo;Integrated Security=True",
                    LayerId = 4,
                    PkField = "osm_id",
                    Table = "buildings",
                }
            );
            #endregion
                #region Services & Functions
            Service wmsService = null, sopService = null;
            modelBuilder.Entity<Service>().HasData(
                wmsService = new Service
                {
                    Id = 1,
                    Name = "WMS",
                },
                sopService = new Service
                {
                    Id = 2,
                    Name = "SOP",
                }
                );

            ServiceFunction getMap = null, getTematicMap = null, getFeatureInfo = null, getCapabilities = null, getGraphicLegend = null,
                describeLayers = null, spatialQuery = null, advanceQuery = null, spatialQuery2 = null;
            modelBuilder.Entity<ServiceFunction>().HasData(
                getMap = new ServiceFunction
                {
                    Id = 1,
                    ServiceId = 1,
                    Name = "GetMap"
                },
                getFeatureInfo = new ServiceFunction
                {
                    Id = 2,
                    ServiceId = 1,
                    Name = "GetFeatureInfo"
                },
                getCapabilities = new ServiceFunction
                {
                    Id = 3,
                    ServiceId = 1,
                    Name = "GetCapabilities"
                },
                getGraphicLegend = new ServiceFunction
                {
                    Id = 4,
                    ServiceId = 1,
                    Name = "GetGraphicLegend"
                },
                describeLayers = new ServiceFunction
                {
                    Id = 5,
                    ServiceId = 1,
                    Name = "DescribeLayers"
                },
                spatialQuery = new ServiceFunction
                {
                    Id = 6,
                    ServiceId = 1,
                    Name = "SpatialQuery"
                },
                advanceQuery = new ServiceFunction
                {
                    Id = 7,
                    ServiceId = 2,
                    Name = "AdvancedQuery"
                },
                spatialQuery2 = new ServiceFunction
                {
                    Id = 8,
                    ServiceId = 2,
                    Name = "SpatialQuery"
                },
                getTematicMap = new ServiceFunction
                {
                    Id = 9,
                    ServiceId = 1,
                    Name = "GetTematicMap"
                }
                );
            #endregion

            #region Users
            Client frankie;
            modelBuilder.Entity<Client>().HasData(
                frankie = new Client
                {
                    Id = "1",
                    Name = "fmujica"
                });
            #endregion

            #region WorkSpace & WorkspaceFUntions
            WorkSpace commonWorkSpace = null;
            modelBuilder.Entity<WorkSpace>().HasData(
                commonWorkSpace = new WorkSpace
                {
                    Id = 1,
                    Name = "common",
                }
            );
            modelBuilder.Entity<ClientWorkSpaces>().HasData(
                new ClientWorkSpaces
                {
                    ClientId = "1",
                    WorkSpaceId = 1,
                }
            );
            modelBuilder.Entity<WorkspaceFunction>().HasData(
                new WorkspaceFunction
                {
                    WorkSpaceId = 1,
                    FunctionId = 1
                }
            );
            modelBuilder.Entity<WorkspaceFunction>().HasData(
                new WorkspaceFunction
                {
                    WorkSpaceId = 1,
                    FunctionId = 9
                }
            );
            modelBuilder.Entity<WorkspaceFunction>().HasData(
                new WorkspaceFunction
                {
                    WorkSpaceId = 1,
                    FunctionId = 2
                }
            );
            modelBuilder.Entity<WorkspaceFunction>().HasData(
                new WorkspaceFunction
                {
                    WorkSpaceId = 1,
                    FunctionId = 3
                }
            );
            modelBuilder.Entity<WorkspaceFunction>().HasData(
                new WorkspaceFunction
                {
                    WorkSpaceId = 1,
                    FunctionId = 4
                }
            );
            modelBuilder.Entity<WorkspaceFunction>().HasData(
               new WorkspaceFunction
               {
                   WorkSpaceId = 1,
                   FunctionId = 5
               }
           );
            modelBuilder.Entity<WorkspaceFunction>().HasData(
                new WorkspaceFunction
                {
                    WorkSpaceId = 1,
                    FunctionId = 6
                }
            );
            modelBuilder.Entity<WorkspaceFunction>().HasData(
                new WorkspaceFunction
                {
                    WorkSpaceId = 1,
                    FunctionId = 7
                }
            );
            modelBuilder.Entity<WorkspaceFunction>().HasData(
                new WorkspaceFunction
                {
                    WorkSpaceId = 1,
                    FunctionId = 8
                }
            );
            #endregion
        }
        private static void BuildFilters()
        {
            Filters = new Dictionary<string, byte[]>();

            #region buildingFilter

            var school = new Clause
            {
                Source = InfoSource.Geometry,
                Name = "type",
                Operator = ComparisonOperator.Contains,
                Value = "school"
            };

            var primary = new Clause
            {
                Source = InfoSource.Geometry,
                Name = "type",
                Operator = ComparisonOperator.Contains,
                Value = "primary"
            };

            var Calzada_Zapata = new Clause
            {
                Source = InfoSource.Geometry,
                Name = "name",
                Operator = ComparisonOperator.Contains,
                Value = "Calzada Zapata"
            };

            var hospital = new Clause
            {
                Source = InfoSource.Geometry,
                Name = "type",
                Operator = ComparisonOperator.Contains,
                Value = "hospital"
            };

            var university = new Clause
            {
                Source = InfoSource.Geometry,
                Name = "type",
                Operator = ComparisonOperator.Contains,
                Value = "university"

            };

            var defaultFilterExpression = new GeometryFilter();
            var schoolFilterExpression = new GeometryFilter(school);
            var hospitalFilterExpression = new GeometryFilter(hospital);
            var universityFilterExpression = new GeometryFilter(university);
            var primaryFilterExpression = new GeometryFilter(primary);
            var Calzada_ZapataFilterExpression = new GeometryFilter(Calzada_Zapata);

            var defaultFilter = FilterSerializer.FilterExpressionToByteArray(defaultFilterExpression);
            var schoolFilter = FilterSerializer.FilterExpressionToByteArray(schoolFilterExpression);
            var hospitalFilter = FilterSerializer.FilterExpressionToByteArray(hospitalFilterExpression);
            var attractionFilter = FilterSerializer.FilterExpressionToByteArray(universityFilterExpression);
            var primaryFilter = FilterSerializer.FilterExpressionToByteArray(primaryFilterExpression);
            var Calzada_ZapataFilter = FilterSerializer.FilterExpressionToByteArray(Calzada_ZapataFilterExpression);

            Filters["default"] = defaultFilter;
            Filters["school"] = schoolFilter;
            Filters["hospital"] = hospitalFilter;
            Filters["university"] = attractionFilter;
            Filters["primary"] = primaryFilter;
            Filters["Calzada_Zapata"] = Calzada_ZapataFilter;

            #endregion
        }

        private static void BuildStyles()
        {            
            VectorStyles = new Dictionary<string, VectorStyle>();

            #region VectorStyles
            //cambiar el path y generar la base de nuevo
            var path = @"../OpenLatino.Admin.Server/wwwroot/Images/map-pin.png";
            var file = File.OpenRead(path);
            var reader = new BinaryReader(file);
            var countBytes = Convert.ToInt32(file.Length);
            var bytes = reader.ReadBytes(countBytes);

            var buildingStyle = new SysDrawingVectorStyle
            {                
                Name = "buildings_style",
                EnableOutLine = true,
                FillStyle = Brushes.LightYellow,
                LineStyle = new Pen(Color.Red, 2),
                OutLineStyle = Pens.Black,
                PointBrush = Brushes.Brown,
                PointSize = 2,
                Image = null,
                ImageRotacion = 0,
                ImageScale = 0
            };

            var roadsstyle = new SysDrawingVectorStyle()
            {
                Name = "roads_style",
                EnableOutLine = false,
                FillStyle = Brushes.Crimson,
                LineStyle = Pens.Black,
                OutLineStyle = Pens.Black,
                PointBrush = Brushes.Brown,
                PointSize = 2,
                Image = null,
                ImageRotacion = 0,
                ImageScale = 0
            };

            var pointsstyle = new SysDrawingVectorStyle()
            {
                Name = "points_style",
                EnableOutLine = false,
                FillStyle = Brushes.Crimson,
                LineStyle = Pens.Red,
                OutLineStyle = Pens.Black,
                PointBrush = Brushes.Brown,
                PointSize = 10,
                ImageRotacion = 0,
                ImageScale = 0,
                Image = Image.FromStream(new MemoryStream(bytes))
            };

            var schoolStyle = new SysDrawingVectorStyle()
            {
                Name = "school_style",
                EnableOutLine = true,
                FillStyle = Brushes.LawnGreen,
                LineStyle = new Pen(Color.DarkOliveGreen, 2),
                OutLineStyle = Pens.Black,
                PointBrush = Brushes.Brown,
                PointSize = 2,
                Image = null,
                ImageRotacion = 0,
                ImageScale = 0
            };

            var hospitalStyle = new SysDrawingVectorStyle()
            {
                Name = "hospital_style",
                EnableOutLine = true,
                FillStyle = Brushes.DarkMagenta,
                LineStyle = new Pen(Color.CornflowerBlue, 2),
                OutLineStyle = Pens.Black,
                PointBrush = Brushes.Brown,
                PointSize = 2,
                Image = null,
                ImageRotacion = 0,
                ImageScale = 0
            };

            var universityStyle = new SysDrawingVectorStyle()
            {
                Name = "university_style",
                EnableOutLine = true,
                FillStyle = Brushes.DarkRed,
                LineStyle = new Pen(Color.DarkBlue, 2),
                OutLineStyle = Pens.Black,
                PointBrush = Brushes.Brown,
                PointSize = 2,
                Image = null,
                ImageRotacion = 0,
                ImageScale = 0
            };

            var buildingThematicStyle = new SysDrawingVectorStyle()
            {
                Name = "buildingThematic_style",
                EnableOutLine = true,
                FillStyle = Brushes.Tomato,
                LineStyle = new Pen(Color.Red, 2),
                OutLineStyle = Pens.Black,
                PointBrush = Brushes.Brown,
                PointSize = 2,
                Image = null,
                ImageRotacion = 0,
                ImageScale = 0
            };

            #endregion

            VectorStyles["buildings_style"] = MapperStyle.ToVectorStyle(buildingStyle);
            VectorStyles["buildings_style"].Id = 1;
            VectorStyles["roads_style"] = MapperStyle.ToVectorStyle(roadsstyle);
            VectorStyles["roads_style"].Id = 3;
            VectorStyles["points_style"] = MapperStyle.ToVectorStyle(pointsstyle);
            VectorStyles["points_style"].Id = 2;
            VectorStyles["school_style"] = MapperStyle.ToVectorStyle(schoolStyle);
            VectorStyles["school_style"].Id = 4;
            VectorStyles["hospital_style"] = MapperStyle.ToVectorStyle(hospitalStyle);
            VectorStyles["hospital_style"].Id = 5;
            VectorStyles["university_style"] = MapperStyle.ToVectorStyle(universityStyle);
            VectorStyles["university_style"].Id = 6;
            VectorStyles["buildingThematic_style"] = MapperStyle.ToVectorStyle(buildingThematicStyle);
            VectorStyles["buildingThematic_style"].Id = 7;
        }         
        private static Dictionary<string, VectorStyle> VectorStyles { get; set; }
        private static Dictionary<string, byte[]> Filters { get; set; }
    }
}
