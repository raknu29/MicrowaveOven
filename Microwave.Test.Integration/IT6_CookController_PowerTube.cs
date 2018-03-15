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
    class IT6_CookController_PowerTube
    {
        private ILight _uut_light;

        private IDoor _uut_door;
        private IDisplay _uut_display;
        private CookController _uut_cooker;
        private UserInterface _uut_ui;

        private ITimer _timer;

        private IPowerTube _uut_powerTube;

        private IButton _uut_powerButton;
        private IButton _uut_timeButton;
        private IButton _uut_startCancelButton;

        private IOutput _output;
        [SetUp]
        public void Setup()
        {
            _output = Substitute.For<IOutput>();

            _uut_door = new Door();
            _uut_powerButton = new Button();
            _uut_timeButton = new Button();
            _uut_startCancelButton = new Button();
            _uut_light = new Light(_output);

            _uut_display = new Display(_output);
            _timer = Substitute.For<ITimer>();
            _uut_powerTube = new PowerTube(_output);

            _uut_cooker = new CookController(_timer, _uut_display, _uut_powerTube);
            _uut_ui = new UserInterface(_uut_powerButton, _uut_timeButton, _uut_startCancelButton, _uut_door, _uut_display, _uut_light, _uut_cooker);

            _uut_cooker.UI = _uut_ui;
        }

        [Test]
        public void Start_StateIsSetTime_PowerTubeTurnsOn()
        {
            _uut_powerButton.Press();
            _uut_timeButton.Press();
            _uut_startCancelButton.Press();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("PowerTube works with 50")));
        }

        [Test]
        public void Stop_StateIsCooking_PowerTubeTurnsOff()
        {
            _uut_powerButton.Press();
            _uut_timeButton.Press();
            _uut_startCancelButton.Press();
            _uut_door.Open();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("PowerTube turned off")));
        }
    }
}
