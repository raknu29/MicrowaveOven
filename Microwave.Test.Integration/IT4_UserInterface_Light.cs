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
    public class IT4_UserInterface_Light
    {
        private Light _uut_light;

        private Door _uut_door;
        private Display _uut_display;
        private CookController _uut_cooker;
        private UserInterface _uut_ui;

        private ITimer _timer;

        private IPowerTube _powerTube;

        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;

        private IOutput _output;
        [SetUp]
        public void Setup()
        {
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();

            _timer = Substitute.For<ITimer>();
            _powerTube = Substitute.For<IPowerTube>();

            _output = Substitute.For<IOutput>();

            _uut_light = new Light(_output);

            _uut_door = new Door();

            _uut_display = new Display(_output);

            _uut_cooker = new CookController(_timer, _uut_display, _powerTube);

            _uut_ui = new UserInterface(
                _powerButton, _timeButton, _startCancelButton,
                _uut_door,
                _uut_display,
                _uut_light,
                _uut_cooker);

            _uut_cooker.UI = _uut_ui;
        }

        [Test]
        public void TurnOn_StateIsReady_LightTurnOnIsCalled()
        {
            _uut_door.Open();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned on")));
        }

        [Test]
        public void TurnOff_StateIsDoorOben_LightTurnOffIsCalled()
        {
            _uut_door.Open();
            _uut_door.Close();

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Light is turned off")));
        }
    }
}
