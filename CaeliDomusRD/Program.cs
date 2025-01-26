
namespace CaeliDomusRD
{
    class Program
    {
        private static void Main(string [] args)
        {
            //TO DO :
            //oggetti job per definire la lettura tenendo aperto il software
            //oppure cronjob linux per stabilirne l'esecuzione con YAML https://kubernetes.io/docs/concepts/workloads/controllers/cron-jobs/
            CaeliDomusRD.Bme680Reader.Bme680Controller();
        }

    }
}


