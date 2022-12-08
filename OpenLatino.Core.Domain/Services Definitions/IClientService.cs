using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenLatino.Core.Domain.Interfaces
{
    public interface IClientService
    {
        IEnumerable<Client> GetAll();

        Client FindClient(string clientId);

        ClientCredentialModel AddClient(ClientModel clientModel, string userId);
        Client UpdateAccessKey(string clientId, string newAccess);
        Client Update_UpdateKey(string clientId, string newUpdate);
        string getRandomUpdateKey();
    }
}
