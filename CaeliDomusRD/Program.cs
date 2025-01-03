using System.Device.Gpio;

namespace CaeliDomusRD
{
    class Program
    {
        private static void Main(string [] args)
        {
            CaeliDomusRD.GpioReader.GetTemperatureFromThermistor();

        }

    }
}


