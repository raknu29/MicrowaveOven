using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using MicrowaveOvenClasses.Boundary;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    class IT5_UserInterface_Buttons
    {
        private Button _uut_powerButton;
        private Button _uut_timeButton;
        private Button _uut_startCancelButton;

        private Light _uut_light;
        private Door _uut_door;
        private Display _uut_display;
        private CookController _uut_cooker;
        private UserInterface _uut_ui;

        private ITimer _timer;

        private IPowerTube _powerTube;

        private IOutput _output;

        [SetUp]
        public void Setup()
        {
            _timer = Substitute.For<ITimer>();
            _powerTube = Substitute.For<IPowerTube>();

            _output = Substitute.For<IOutput>();

            _uut_powerButton = new Button();
            _uut_timeButton = new Button();
            _uut_startCancelButton = new Button();

            _uut_light = new Light(_output);

            _uut_door = new Door();

            _uut_display = new Display(_output);

            _uut_cooker = new CookController(_timer, _uut_display, _powerTube);

            _uut_ui = new UserInterface(
                _uut_powerButton, _uut_timeButton, _uut_startCancelButton,
                _uut_door,
                _uut_display,
                _uut_light,
                _uut_cooker);

            _uut_cooker.UI = _uut_ui;
        }

        [Test]
        public void Press_StateIsReadyPressPowerButton_DisplayShowPowerIsCalled()
        {
            _uut_powerButton.Press();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("50 W")));
        }

        [Test]
        public void Press_StateIsSetPowerPressTimeButton_DisplayShowTimeIsCalled()
        {
            _uut_powerButton.Press();
            _uut_timeButton.Press();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("01:00")));
        }

        [Test]
        public void Press_StateIsSetPowerPressStartCancelButton_DisplayClear()
        {
            _uut_powerButton.Press();
            _uut_startCancelButton.Press();
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Display cleared")));
        }

    }
}
