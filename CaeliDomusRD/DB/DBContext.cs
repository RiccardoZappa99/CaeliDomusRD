using FreeSql;
using FreeSql.Extensions.EntityUtil;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.ComponentModel;

//uso IFreeSql che è l'oggetto di primo livello dell'ORMcui si hanno tutti gli
//accessi alle relative funzioni di CRUD ed estensioni (nuget)
namespace DBContext
{
    internal class DB : IDisposable
    {
        private IFreeSql _db;
        private string _config;
        public IFreeSql Db { get => _db; set => _db = value; }
        public string Config { get => _config; set => _config = value; }

        public DB()
        {
            var configuration = new ConfigurationBuilder()
                        .SetBasePath("C:\\Users\\AryRicky\\Documents\\GitHub\\CaeliDomusRD\\CaeliDomusRD")
                        .AddJsonFile("appsettings.json")
                        .Build(); 
            Config = configuration.GetConnectionString("CaeliDomusRD");
            Db = new FreeSql.FreeSqlBuilder()
          .UseConnectionString(FreeSql.DataType.Sqlite, Config)
          .UseAutoSyncStructure(true) //automatically synchronize the entity structure to the database
          .Build();
        }

        public void Dispose()
        {
            Db.Dispose();   //si puo valutare di ampliare con argomento di controllo bool e logiche piuu approfondite
        }
    }
}