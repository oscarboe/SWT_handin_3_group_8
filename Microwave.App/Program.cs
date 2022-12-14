using System;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;

namespace Microwave.App
{
    class Program
    {
        //Comment added to test webhook
        static void Main(string[] args)
        {
            int maxPowerInWatt = 1000;
            Button startCancelButton = new Button();
            Button powerButton = new Button();
            Button timeButton = new Button();

            Door door = new Door();

            Output output = new Output();

            Display display = new Display(output);

            PowerTube powerTube = new PowerTube(output, maxPowerInWatt);

            Light light = new Light(output);

            Beep beep = new Beep(output);

            Microwave.Classes.Boundary.Timer timer = new Timer();

            CookController cooker = new CookController(timer, display, powerTube, beep);

            UserInterface ui = new UserInterface(powerButton, timeButton, startCancelButton, door, display, light, cooker, maxPowerInWatt);

            // Finish the double association
            cooker.UI = ui;

            // Simulate a simple sequence
            
            //get the power to 1000 to showcase maxpower
            for (int i = 0; i < 20; i++)
            {
                powerButton.Press();

            }

            timeButton.Press();

            startCancelButton.Press();
            
            //Sleep 10 seconds before adding 30 sec

            System.Threading.Thread.Sleep(10000);
            
            //Time added 30 sec
            
            timeButton.Press();

            // The simple sequence should now run

            System.Console.WriteLine("When you press enter, the program will stop");
            // Wait for input

            System.Console.ReadLine();
            
        }
    }
}
