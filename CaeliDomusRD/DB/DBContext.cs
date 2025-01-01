using FreeSql;
using FreeSql.Extensions.EntityUtil;
using Microsoft.Extensions.Configuration;

//uso IFreeSql che è l'oggetto di primo livello dell'ORMcui si hanno tutti gli
//accessi alle relative funzioni di CRUD ed estensioni (nuget)
namespace DBContext
{
internal class DB
{
private IFreeSql _db;  
private IConfiguration _config;
public IFreeSql Db { get => _db; set => _db = value; }  
public IConfiguration Config { get => _config; set => _config = value; }

        public DB () 
{
    Db = new FreeSql.FreeSqlBuilder()
  .UseConnectionString(FreeSql.DataType.Sqlite, Config.GetConnectionString("CaeliDomusRD"))
  .UseAutoSyncStructure(true) //automatically synchronize the entity structure to the database
  .Build();
}

    }
}