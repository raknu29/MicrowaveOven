using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class CookControllerAndDisplayTest
    {
        private CookController iut_cooker;
        private UserInterface iut_ui;

        private ITimer timer;
        private IDisplay display;
        private IPowerTube powerTube;

        private IButton powerButton;
        private IButton timeButton;
        private IButton startCancelButton;

        private IDoor door;
        private ILight light;

        [SetUp]
        public void Setup()
        {
            powerButton = Substitute.For<IButton>();
            timeButton = Substitute.For<IButton>();
            startCancelButton = Substitute.For<IButton>();
            door = Substitute.For<IDoor>();
            light = Substitute.For<ILight>();
            display = Substitute.For<IDisplay>();

            timer = Substitute.For<ITimer>();
            powerTube = Substitute.For<IPowerTube>();

            iut_ui = new UserInterface(
                powerButton, timeButton, startCancelButton,
                door,
                display,
                light,
                iut_cooker);

            iut_cooker = new CookController(timer, display, powerTube, iut_ui);
        }

        [Test]
        public void Cooking_TimerExpired_UICalled()
        {
            iut_cooker.StartCooking(50, 60);

            timer.Expired += Raise.EventWith(this, EventArgs.Empty);

            iut_ui.Received().CookingIsDone();
        }
    }
}
