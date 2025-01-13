using CaeliDomusRD.DbModelEntities;
using DBContext;
using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using System;
using System.Device.Gpio;
using System.Linq;
using System.Net.Sockets;

//            Vout= (R1/(R(T)+R1)*Vin
//            R(T)=((Vin/Vout)-1)*R1
//            1/T = 1/TO + (1/β) ⋅ ln (R/RO)
//creo classe per scalabilità ogni metodo misura componenti diversi con
//le var. di instanza ssaranno genericamente dedicate alla lettura dal PI5
namespace CaeliDomusRD
{
    internal class GpioReader : GpioController 
    {
        #region Thermistor field&property
        private static int _pinNumber = 0;
        private static double _voltageOutput = 0;
        private static double _voltageInput = 0;
        private static double []_voltageSampling = {};
        private IConfiguration _configuration;

        public static double VoltageOutput { get => _voltageOutput; set => _voltageOutput = value; }
        public static double VoltageInput { get => _voltageInput; set => _voltageInput = value; }
        public static double[] VoltageSampling { get => _voltageSampling; set => _voltageSampling = value; }
        public static int PinNumber { get => _pinNumber; set => _pinNumber = value; }


        #endregion
        //metodo specializzato per lettura e manipolazione valori termisotori
        public static void GetTemperatureFromThermistor()
        {
            PinNumber = 26;
            VoltageInput = 3.3f;
            const short _balanceResistance = 10000;
            //const short _thermistorValue = 10000;
            double _measeredThermistorResistanceValue = 0;
            double _KelvinValue = 0;
            double _CelsiusValue = 0;
            const double _beta= 3974.0;
            const double _typicalRoomTemperature = 298.15;

            // PER USO IN AMBIENTE NATIVO PI5

            //using (GpioReader _Pi5 = new GpioReader())
            //{
            //for(int i = 0; i>9 ; i++)
            //{
            //    VoltageSampling[i] = Convert.ToDouble(_Pi5.Read(PinNumber));
            //    //campiono in piccoli frammenti di tempi diversi
            //    Thread.Sleep(10); 
            //}
            //}

            using (SshClient pi5 = new SshClient("192.168.1.61", "bombadil", "Bombadil25"))   // prendere dalla config
            {
                pi5.Connect();
                for (int i = 0; i > 9; i++)
                {
                    VoltageSampling[i] = Convert.ToDouble(pi5.RunCommand("gpioget 0 26").Result); //Convert.ToDouble(_Pi5.Read(PinNumber));
                    //campiono in piccoli frammenti di tempi diversi
                    Thread.Sleep(10);
                }
                pi5.Disconnect();
            }


            // calcolo la media degli output rilevati. somma dei campioni diviso il numero totali di essi.
            VoltageOutput = VoltageSampling.Sum()/VoltageSampling.Count(); 

            // calcolo valore resitivo termistore 
            _measeredThermistorResistanceValue =((VoltageInput/VoltageOutput)-1)*_balanceResistance;

            //applico la Steinhart-Hart per ricavare la temperatura sulla base del valore resitivo rilevato
            _KelvinValue = (_beta * _typicalRoomTemperature) / 
           (_beta + (_typicalRoomTemperature * Math.Log(_measeredThermistorResistanceValue / _typicalRoomTemperature)));

           // converto in Celsius
           _CelsiusValue = _KelvinValue - 273.15;

            //scrivo a db

            using (DB _temperatureRD = new DB()) 
            { 
                Temperature temperature = new Temperature();

                temperature.TemperatureValue = 1;
                temperature.ReadTime = DateTime.Now;

                _temperatureRD.Db.Insert<Temperature>(temperature);
                
            }

            
        }

    }
}