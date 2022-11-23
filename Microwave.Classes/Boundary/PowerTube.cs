using System;
using Microwave.Classes.Interfaces;

namespace Microwave.Classes.Boundary
{
    public class PowerTube : IPowerTube
    {
        private IOutput myOutput;
        private int _maxPower;
        private bool IsOn = false;

        public PowerTube(IOutput output, int maxPower)
        {
            myOutput = output;

            if (maxPower < 50 || maxPower > 1000000)
                throw new ArgumentOutOfRangeException("PowerTube constructor", "Max power must be between 50 and 1000000 W");

            _maxPower = maxPower;
        }
        

        public void TurnOn(int power)
        {
            if (power < 1 || _maxPower < power)
            {
                throw new ArgumentOutOfRangeException("power", power, $"Must be between 1 and {_maxPower} (incl.)");
            }

            if (IsOn)
            {
                throw new ApplicationException("PowerTube.TurnOn: is already on");
            }

            myOutput.OutputLine($"PowerTube works with {power}");
            IsOn = true;
        }

        public void TurnOff()
        {
            if (IsOn)
            {
                myOutput.OutputLine($"PowerTube turned off");
            }

            IsOn = false;
        }
    }
}