using CaeliDomusRD.DbModelEntities;
using DBContext;
using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using System;
using System.Linq;
using System.Net.Sockets;
using Iot.Device.Bmxx80;
using System.Device.I2c;
using System.Linq.Expressions;
using UnitsNet;


//            Vout= (R1/(R(T)+R1)*Vin
//            R(T)=((Vin/Vout)-1)*R1
//            1/T = 1/TO + (1/β) ⋅ ln (R/RO)
//creo classe per scalabilità ogni metodo misura componenti diversi con
//le var. di instanza ssaranno genericamente dedicate alla lettura dal PI5
namespace CaeliDomusRD
{
    internal class Bme680Reader : Iot.Device.Bmxx80.Bme680
    {
        #region Thermistor field&property
        //private static int _pinNumber = 0;
        //private static double _voltageOutput = 0;
        //private static double _voltageInput = 0;
        //private static double []_voltageSampling = {};

        //public static double VoltageOutput { get => _voltageOutput; set => _voltageOutput = value; }
        //public static double VoltageInput { get => _voltageInput; set => _voltageInput = value; }
        //public static double[] VoltageSampling { get => _voltageSampling; set => _voltageSampling = value; }
        //public static int PinNumber { get => _pinNumber; set => _pinNumber = value; }

        private static readonly int _bmeBusID = 0;
        private static readonly int _deviceAddress = 0;

        public static int BmeBusID { get => _bmeBusID; }
        public static int DeviceAddress { get => _deviceAddress; }

        public Bme680Reader(I2cDevice i2cDevice) : base(i2cDevice)
        {
        }

        #endregion
        //metodo specializzato per lettura e manipolazione valori termisotori
        public static void Bme680Controller()
        {
            #region thermistor reader
            // PinNumber = 26;
            // VoltageInput = 3.3f;
            // const short _balanceResistance = 10000;
            // //const short _thermistorValue = 10000;
            // double _measeredThermistorResistanceValue = 0;
            // double _KelvinValue = 0;
            // double _CelsiusValue = 0;
            // const double _beta= 3974.0;
            // const double _typicalRoomTemperature = 298.15;

            // // PER USO IN AMBIENTE NATIVO PI5

            // //using (GpioReader _Pi5 = new GpioReader())
            // //{
            // //for(int i = 0; i>9 ; i++)
            // //{
            // //    VoltageSampling[i] = Convert.ToDouble(_Pi5.Read(PinNumber));
            // //    //campiono in piccoli frammenti di tempi diversi
            // //    Thread.Sleep(10); 
            // //}
            // //}

            // using (SshClient pi5 = new SshClient("192.168.1.61", "bombadil", "Bombadil25"))   // prendere dalla config
            // {
            //     pi5.Connect();
            //     for (int i = 0; i > 9; i++)
            //     {
            //         VoltageSampling[i] = Convert.ToDouble(pi5.RunCommand("gpioget 0 26").Result); //Convert.ToDouble(_Pi5.Read(PinNumber));
            //         //campiono in piccoli frammenti di tempi diversi
            //         Thread.Sleep(10);
            //     }
            //     pi5.Disconnect();
            // }


            // // calcolo la media degli output rilevati. somma dei campioni diviso il numero totali di essi.
            // VoltageOutput = VoltageSampling.Sum()/VoltageSampling.Count(); 

            // // calcolo valore resitivo termistore 
            // _measeredThermistorResistanceValue =((VoltageInput/VoltageOutput)-1)*_balanceResistance;

            // //applico la Steinhart-Hart per ricavare la temperatura sulla base del valore resitivo rilevato
            // _KelvinValue = (_beta * _typicalRoomTemperature) / 
            //(_beta + (_typicalRoomTemperature * Math.Log(_measeredThermistorResistanceValue / _typicalRoomTemperature)));

            //// converto in Celsius
            //_CelsiusValue = _KelvinValue - 273.15;
            #endregion

            //parametri di connesione 
            I2cConnectionSettings _pi5Bme680ProtocolSettings = new I2cConnectionSettings(BmeBusID, DeviceAddress);
            //protocollo i2c
            I2cDevice _pi5Bme680 = new I2cDevice(_pi5Bme680ProtocolSettings);
            //sensore 
            Bme680Reader _bme680TemperatureReader = new Bme680Reader(_pi5Bme680);

            float _tempValue = (float)_bme680TemperatureReader.Read().Temperature.DegreesCelsius;
            float _humidityValue = (float)_bme680TemperatureReader.Read().Humidity.Percent;
            float _pressureValue = (float)_bme680TemperatureReader.Read().Pressure.Atmospheres;
            DateTime _curentTime = DateTime.Now;
            //scrivo a db
            using (DB _RD = new DB())
            {
                //entità temperatura
                DbModelEntities.Temperature _temperature = new DbModelEntities.Temperature();
                _temperature.TemperatureValue = _tempValue;
                _temperature.ReadTime = _curentTime;
                //entità umidità
                DbModelEntities.Humidity _humidity = new DbModelEntities.Humidity();
                _humidity.HumidityValue = _humidityValue;
                _humidity.ReadTime = _curentTime;
                //entità pressione
                //DbModelEntities.Pressure _pressure = new DbModelEntities.Pressure();
                //_pressure.PressureValue = _pressureValue;
                //_pressure.ReadTime = _curentTime;

                try
                {
                    _RD.Db.Insert<DbModelEntities.Temperature>(_temperature).ExecuteAffrows();
                    _RD.Db.Insert<Humidity>().ExecuteAffrows();
                    //_RD.Db.Insert<Pressure>().ExecuteAffrows();    //creare tabella in db
                }
                catch (Exception e)
                {
                    //logiche di gestione log errori
                }

            }



        }

        public static void LightMeasurement()
        {
            //logiche di misurazione della luce  
        }
    }
}