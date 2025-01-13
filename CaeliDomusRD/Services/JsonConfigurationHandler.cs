using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace CaeliDomusRD.Services
{
    internal class JsonConfigurationHandler
    {
        private IConfiguration _jsonConfiguration;

        public IConfiguration JsonConfiguration { get => _jsonConfiguration; set => _jsonConfiguration = value; }

        public JsonConfigurationHandler() 
        {
            JsonConfiguration = new ConfigurationBuilder()
                .SetBasePath("C:\\Users\\AryRicky\\Documents\\GitHub\\CaeliDomusRD\\CaeliDomusRD")  //fare in modo che sia portabile il software. escogitare una directory dinamica
                .AddJsonFile("appsettings.json")
                .Build();
        }

    }
}
