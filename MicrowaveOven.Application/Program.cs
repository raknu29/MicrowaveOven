using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using MicrowaveOvenClasses.Boundary;

namespace MicrowaveOven.Application
{
    class Program
    {

        static void Main(string[] args)
        {
            // Setup all the objects, 
            var door = new Door();
            var powerButton = new Button();
            var timeButton = new Button();
            var startCancelButton = new Button();
            var output = new Output();
            var display = new Display(output);
            var light = new Light(output);
            var powertube = new PowerTube(output);
            var timer = new MicrowaveOvenClasses.Boundary.Timer();

            var cookController = new CookController(timer, display, powertube);
            var userInterface = new UserInterface(powerButton, timeButton, startCancelButton, door, display, light, cookController);
            cookController.UI = userInterface;

            // Simulate user activities - Main Scenario
            // 1.The user opens the door
            System.Console.WriteLine("User: Open Door");
            door.Open();
            // 2.The light goes on inside the oven
            // 3.The user places the dish in the oven
            // 4.The user closes the door
            System.Console.WriteLine("User: Close Door");
            door.Close();
            // 5.The light goes off inside the oven
            // 6.The user presses the Power button one or more times, to select the desired
            //    microwave power. The display shows the currently selected power from 50 to 700
            //    increment with 50 each press - press until "rollover" 
            System.Console.WriteLine("User: Press Powerbutton 15 times");
            for(int index = 0; index<14; index++)
            {
                powerButton.Press();
            }

            // 7.The user presses the Time button one or more times to select the desired cooking
            // time.The display shows the currently selected time as minutes:seconds, starting
            //     with 01:00.Each press increases the selected time with one minute.
            System.Console.WriteLine("User: Press time button");
            timeButton.Press();
            timeButton.Press();
            timeButton.Press();
            // 8. The user presses the Start-Cancel button.
            System.Console.WriteLine("User: Press startCancel Button");
            startCancelButton.Press();
            // 9. The light goes on inside the oven
            // 10. The powertube starts working at the desired powerlevel
            // 11. The display shows and updates the remaing time every second as
            //     minutes:seconds.
            // 12. When the time has expired, the power tube is turned off
            // 13. The light inside the oven goes off
            // 14. The display is blanked
            // 15. The user opens the door
            //System.Console.WriteLine("Tast enter når applikationen skal køre videre");
            //System.Console.ReadLine();
            //System.Console.WriteLine("User: Open Door");
            //door.Open();
            // 16. The light goes on inside the oven
            // 17. The user removes the food
            // 18. The user closes the door
            //System.Console.WriteLine("User: Close door");
            //door.Close();
            // 19. The light inside the oven goes off.

            // Wait while the classes, including the timer, do their job
            //System.Console.WriteLine("Tast enter når applikationen skal afsluttes");
            System.Console.ReadLine();
        }
    }
}
