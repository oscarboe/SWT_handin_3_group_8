﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microwave.Classes.Interfaces;
using Microwave.Classes.Boundary;


namespace Microwave.Classes.Controllers
{
    public class CookController : ICookController
    {
        // Since there is a 2-way association, this cannot be set until the UI object has been created
        // It also demonstrates property dependency injection
        public IUserInterface UI { set; private get; }

        private bool isCooking = false;


        private IDisplay myDisplay;
        private IPowerTube myPowerTube;
        private ITimer myTimer;
        private IBeep myBeep;

        public CookController(
            ITimer timer,
            IDisplay display,
            IPowerTube powerTube,
            IBeep Beep,
            IUserInterface ui) : this(timer, display, powerTube, Beep)
        {
            UI = ui;
        }

        public CookController(
            ITimer timer,
            IDisplay display,
            IPowerTube powerTube,
            IBeep Beep)
        {
            myTimer = timer;
            myDisplay = display;
            myPowerTube = powerTube;
            myBeep = Beep;

            

            timer.Expired += new EventHandler(OnTimerExpired);
            timer.TimerTick += new EventHandler(OnTimerTick);
        }

        public void StartCooking(int power, int time)
        {
            myPowerTube.TurnOn(power);
            myTimer.Start(time);
            isCooking = true;
        }

        public void Stop()
        {
            isCooking = false;
            myPowerTube.TurnOff();
            myTimer.Stop();
            
        }

        public void OnTimerExpired(object sender, EventArgs e)
        {
            if (isCooking)
            {
                isCooking = false;
                myPowerTube.TurnOff();
                UI.CookingIsDone();
                myBeep.PlayBeep();
            }
        }

        public void OnTimerTick(object sender, EventArgs e)
        {
            if (isCooking)
            {
                int remaining = myTimer.TimeRemaining;
                myDisplay.ShowTime(remaining / 60, remaining % 60);
            }
        }

        public void ChangeTimeWhileCooking()
        {
            if (isCooking)
            {
                int remaining = myTimer.TimeRemaining;
                remaining += 30;
                myTimer.Start(remaining);
                
            }
        }
    }
}