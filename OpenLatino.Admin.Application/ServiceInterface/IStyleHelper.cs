using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenLatino.Admin.Application.ServiceInterface
{
    public interface IStyleHelper
    {
        CRUD_Service<VectorStyle> GetCRUD();
        IEnumerable<Layer> ListLayers();
        byte[] getImage(VectorStyle style);
    }
}
