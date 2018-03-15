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
    public class IT1_CookController_UserInterface
    {
        private CookController _uut_cooker;
        private UserInterface _uut_ui;

        private ITimer _timer;
        private IDisplay _display;
        private IPowerTube _powerTube;

        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;

        private IDoor _door;
        private ILight _light;

        [SetUp]
        public void Setup()
        {
            _powerButton = Substitute.For<IButton>();
            _timeButton = Substitute.For<IButton>();
            _startCancelButton = Substitute.For<IButton>();
            _door = Substitute.For<IDoor>();
            _light = Substitute.For<ILight>();
            _display = Substitute.For<IDisplay>();

            _timer = Substitute.For<ITimer>();
            _powerTube = Substitute.For<IPowerTube>();

            _uut_cooker = new CookController(_timer, _display, _powerTube);

            _uut_ui = new UserInterface(
                _powerButton, _timeButton, _startCancelButton,
                _door,
                _display,
                _light,
                _uut_cooker);

            _uut_cooker.UI = _uut_ui;


        }

        [Test]
        public void OnStartCancelPressed_StateIsSetTime_CookControllerStartsCooking()
        {
            // "Press" powerbutton --> raise event --> set state to SETPOWER for the UserInterface uut
            // Is this the correct way to drive the uut? We are using the mocks to drive right now!?
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            // Is this OK? Or how else can we Assert that StartCooking is called on CookController
            _powerTube.Received(1).TurnOn(50);
        }

        [Test]
        public void OnStartCancelPressed_StateIsCooking_CookControllerStopsCooking()
        {
            // "Press" powerbutton --> raise event --> set state to SETPOWER for the UserInterface uut
            // Is this the correct way to drive the uut? We are using the mocks to drive right now!?
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            // Is this OK? Or how else can we Assert that Stop is called on CookController
            _powerTube.Received(1).TurnOff();
        }

        [Test]
        public void OnTimerExpired_StateIsCooking_UICookingIsDoneIsCalled()
        {
            // "Press" powerbutton --> raise event --> set state to SETPOWER for the UserInterface uut
            // Is this the correct way to drive the uut? We are using the mocks to drive right now!?
            _powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            _startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            _timer.Expired += Raise.EventWith(this, EventArgs.Empty);

            _light.Received(1).TurnOff();
        }

    }
}
