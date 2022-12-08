using OpenLatino.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenLatino.Core.Domain.Services
{
    public interface ITokenService
    {
        string GetTokenFor(ClientCredentialModel client); 
    }
}
