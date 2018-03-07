using System;
using MicrowaveOvenClasses.Interfaces;

namespace MicrowaveOvenClasses.Controllers
{
    public class CookController : ICookController
    {
        private bool isCooking = false;

        private IDisplay myDisplay;
        private IPowerTube myPowerTube;
        private IUserInterface myUI;
        private ITimer myTimer;

        public CookController(
            ITimer timer,
            IDisplay display,
            IPowerTube powerTube,
            IUserInterface ui)
        {
            myTimer = timer;
            myDisplay = display;
            myPowerTube = powerTube;
            myUI = ui;

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
        }

        public void OnTimerExpired(object sender, EventArgs e)
        {
            if (isCooking)
            {
                myPowerTube.TurnOff();
                myUI.CookingIsDone();
                isCooking = false;
            }
        }

        public void OnTimerTick(object sender, EventArgs e)
        {
            int remaining = myTimer.TimeRemaining;
            myDisplay.ShowTime(remaining/60, remaining % 60);
        }
    }
}