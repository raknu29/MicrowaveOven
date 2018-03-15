using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT3_UserInterface_Door
    {
        private CookController _uut_cooker;
        private UserInterface _uut_ui;
        private IDoor _uut_door;
        private IDisplay _uut_display;

        private IOutput _output;
        private ITimer _timer;
        private IPowerTube _powerTube;

        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;

        private ILight _light;

        [SetUp]
        public void Setup()
        {
            // Stubs
            _timer = Substitute.For<ITimer>();
            _powerTube = Substitute.For<IPowerTube>();
            _output = Substitute.For<IOutput>();

            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();

            _light = Substitute.For<ILight>();

            // Included
            _uut_door = new Door();
            _uut_cooker = new CookController(_timer, _uut_display, _powerTube);
            _uut_display = Substitute.For<IDisplay>();

            _uut_ui = new UserInterface(
                _powerButton, _timeButton, _startCancelButton,
                _uut_door,
                _uut_display,
                _light,
                _uut_cooker);

            _uut_cooker.UI = _uut_ui;
        }

        [Test]
        public void Open_CookerIsCooking_CookerStopsCooking()
        {
            // "Press" powerbutton --> raise event --> set state to SETPOWER for the UserInterface uut
            // Is this the correct way to drive the uut? We are using the mocks to drive right now!?
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            _uut_door.Open();
            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("Display cleared")));
        }
    }
}
