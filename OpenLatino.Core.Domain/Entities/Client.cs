using OpenLatino.Core.Domain.Enums;
using System.Collections.Generic;


namespace OpenLatino.Core.Domain.Entities
{
    public class Client
    {
        public Client()
        {
            ClientWorkSpaces = new HashSet<ClientWorkSpaces>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; } //para cuando el admin del otro sitio quiere cambiar cosas como el allowedOrigin (COMO UNA PAGINA DE LOGIN!!!)
        public ApplicationType ApplicationType { get; set; }
        public bool Active { get; set; }
        public long ExpirationDate { get; set; }
        public string AllowedOrigin { get; set; }
        public string AccessKey { get; set; } //Token de acceso
        public string UpdateKey{ get; set; }
        //To interact with IdentityUser
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<ClientWorkSpaces> ClientWorkSpaces { get; set; }
    }
}