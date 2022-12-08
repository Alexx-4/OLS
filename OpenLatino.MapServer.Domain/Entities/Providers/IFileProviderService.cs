using System.Dynamic;
using OpenLatino.Core.Domain;
using OpenLatino.MapServer.Domain.Entities.FileData;

namespace OpenLatino.MapServer.Domain.Entities.Providers
{
    public interface IFileProviderService : IProviderService
    {

        IFileDataStructure<IFeature> DataStruct { get;}

        /// <summary>
        /// Initialice the provider service with the provider info 
        /// </summary>
        /// <param name="providerInfo"></param> 
        void InitProvider();
    }
}
