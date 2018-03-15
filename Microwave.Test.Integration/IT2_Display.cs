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
    public class IT2_Display
    {
        private Display _uut_display;

        private CookController _uut_cooker;
        private UserInterface _uut_ui;

        private ITimer _timer;
        
        private IPowerTube _powerTube;

        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;

        private IDoor _door;
        private ILight _light;

        private IOutput _output;

        [SetUp]
        public void Setup()
        {
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();
            _door = Substitute.For<IDoor>();
            _light = Substitute.For<ILight>();

            _timer = Substitute.For<ITimer>();
            _powerTube = Substitute.For<IPowerTube>();

            _output = Substitute.For<IOutput>();

            _uut_display = new Display(_output);

            _uut_cooker = new CookController(_timer, _uut_display, _powerTube);

            _uut_ui = new UserInterface(
                _powerButton, _timeButton, _startCancelButton,
                _door,
                _uut_display,
                _light,
                _uut_cooker);

            _uut_cooker.UI = _uut_ui;
        }

        [Test]
        public void OnPowerPressed_StateIsReady_DisplayShowPowerIsCalled()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("50 W")));
        }

        [Test]
        public void OnTimePressed_StateIsSetPower_DisplayShowTime()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("01:00")));
        }

        [Test]
        public void OnStartCancelPressed_StateIsSetPower_DisplayClear()
        {
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("Display cleared")));
        }

        [Test]
        public void OnTimerTick_SetAndTickTime_DisplayShowTimeRemaining()
        {
            _timer.TimeRemaining.Returns(115);
            _timer.TimerTick += Raise.EventWith(this, EventArgs.Empty);
            _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("01:55")));
        }
    }

}
