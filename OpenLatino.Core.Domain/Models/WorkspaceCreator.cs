using OpenLatino.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenLatino.Core.Domain.Models
{
    public class WorkspaceCreator
    {
        public int id { get; set; }
        public string Name { get; set; }
        public List<Layer> Layers { get; set; }
        public List<string> LayersIds { get; set; }
        public List<ServiceFunction> ServiceFunctions { get; set; }
        public List<string> ServicesFunctionsIds { get; set; }
        public string clientId { get; set; }

        public WorkspaceCreator()
        {
            this.Layers = new List<Layer>();
            this.LayersIds = new List<string>();
            this.ServiceFunctions = new List<ServiceFunction>();
            this.ServicesFunctionsIds = new List<string>();
        }
    }
}
