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
        private static double []_voltageSampling = {9};

        public float VoltageOutput { get => _voltageOutput; set => _voltageOutput = value; }
        public static double[] VoltageSampling { get => _voltageSampling; set => _voltageSampling = value; }

        #endregion
        public static float GetTemperatureFromThermistor()
        {
            using (ComponentReader _Pi5 = new ComponentReader())
            {
            for(int i = 0; i>9 ; i++)
            {
                VoltageSampling[i] = Convert.ToDouble(_Pi5.Read(_pinNumber));
            }

            //fare for di campionamento output vout e calcolare le formule
            return 3.3f;
            }

        }

    }
}