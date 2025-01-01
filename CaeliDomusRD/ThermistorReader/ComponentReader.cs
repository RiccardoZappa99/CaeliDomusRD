using System;
using System.Device.Gpio;

//            Vout= (R1/(R(T)+R1)*Vin
//            R(T)=((Vin/Vout)-1)*R1
//            1/T = 1/TO + (1/β) ⋅ ln (R/RO)
namespace ComponentReader
{
    internal class ComponentReader : GpioController //per scalabilità ogni metodo misura componenti diversi
    {
        #region Thermistor field&property
        private const int _pinNumber = 26;
        private const short _balanceResistance = 10000;
        private const short _thermisotrValue = 10000;
        private float _voltageOutput;
        private const float _voltageInput = 3.3f;

        public float VoltageOutput { get => _voltageOutput; set => _voltageOutput = value; }
        
        #endregion
        public static float GetTemperatureFromThermistor()
        {

            //fare for di campionamento output vout e calcolare le formule
            return 3.3f;

        }

    }
}