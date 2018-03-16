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

using System.Threading;


namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT7_CookController_Timer
    {
        private ITimer _uut_timer;

        private IPowerTube _uut_powerTube;
        private IButton _uut_powerButton;
        private IButton _uut_timeButton;
        private IButton _uut_startCancelButton;
        private ILight _uut_light;
        private IDoor _uut_door;
        private IDisplay _uut_display;
        private CookController _uut_cooker;
        private IUserInterface _uut_ui;

        private IOutput _output;

        [SetUp]
        public void Setup()
        {

            _output = Substitute.For<IOutput>();

            _uut_timer = new MicrowaveOvenClasses.Boundary.Timer();

            _uut_powerTube = new PowerTube(_output);

            _uut_powerButton = new Button();
            _uut_timeButton = new Button();
            _uut_startCancelButton = new Button();

            _uut_light = new Light(_output);

            _uut_door = new Door();

            _uut_display = new Display(_output);

            _uut_cooker = new CookController(_uut_timer, _uut_display, _uut_powerTube);

            _uut_ui = new UserInterface(
                _uut_powerButton, _uut_timeButton, _uut_startCancelButton,
                _uut_door,
                _uut_display,
                _uut_light,
                _uut_cooker);

            _uut_cooker.UI = _uut_ui;
        }

        [Test]
        public void Start_TimerTick_ShortEnough()
        {
            ManualResetEvent pause = new ManualResetEvent(false);

            _uut_powerButton.Press();
            _uut_timeButton.Press();
            _uut_startCancelButton.Press();

            _uut_timer.TimerTick += (sender, args) => pause.Set();

            pause.WaitOne();

            _output.Received().OutputLine(Arg.Is<string>(str => str.Contains("PowerTube turned off")));

            
        }
    }
}
