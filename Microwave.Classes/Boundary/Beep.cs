using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microwave.Classes.Interfaces;

namespace Microwave.Classes.Boundary
{
    public class Beep : IBeep
    {
        private IOutput myBeepOutput;

        public Beep(IOutput output)
        {
            myBeepOutput = output;
        }


        public int PlayBeep()
        {
            Console.Beep();
            myBeepOutput.OutputLine("Beep sound has been fired. Now waiting 3 seconds.");
            Thread.Sleep(3000);

            Console.Beep();
            myBeepOutput.OutputLine("Beep sound has been fired two times. Now waiting 3 seconds.");
            Thread.Sleep(3000);

            myBeepOutput.OutputLine("Beep sound has been fired three times.");
            Console.Beep();

            return 1;
        }
    }
}
