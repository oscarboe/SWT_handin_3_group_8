﻿using System;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Unit
{
    [TestFixture]
    public class UserInterfaceTest
    {
        private UserInterface uut;

        private IButton powerButton;
        private IButton timeButton;
        private IButton startCancelButton;

        private IDoor door;

        private IDisplay display;
        private ILight light;

        private ICookController cooker;

        [SetUp]
        public void Setup()
        {
            uut = CreateUut(700);
        }

      

        public UserInterface CreateUut(int maxPower)
        {
            powerButton = Substitute.For<IButton>();
            timeButton = Substitute.For<IButton>();
            startCancelButton = Substitute.For<IButton>();
            door = Substitute.For<IDoor>();
            light = Substitute.For<ILight>();
            display = Substitute.For<IDisplay>();
            cooker = Substitute.For<ICookController>();

            uut = new UserInterface(
                powerButton, timeButton, startCancelButton,
                door,
                display,
                light,
                cooker, maxPower);
            return uut;
        }

        [Test]
        public void Ready_DoorOpen_LightOn()
        {
            // This test that uut has subscribed to door opened, and works correctly
            // simulating the event through NSubstitute
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            light.Received().TurnOn();
        }

        [Test]
        public void DoorOpen_DoorClose_LightOff()
        {
            // This test that uut has subscribed to door opened and closed, and works correctly
            // simulating the event through NSubstitute
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);
            light.Received().TurnOff();
        }

        [Test]
        public void Ready_DoorOpenClose_Ready_PowerIs50()
        {
            // This test that uut has subscribed to power button, and works correctly
            // simulating the events through NSubstitute
            door.Opened += Raise.EventWith(this, EventArgs.Empty);
            door.Closed += Raise.EventWith(this, EventArgs.Empty);

            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            display.Received(1).ShowPower(Arg.Is<int>(50));
        }

        [Test]
        public void Ready_2PowerButton_PowerIs100()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            display.Received(1).ShowPower(Arg.Is<int>(100));
        }

        [Test]
        public void Ready_14PowerButton_PowerIs700()
        {
            for (int i = 1; i <= 14; i++)
            {
                powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            }
            display.Received(1).ShowPower(Arg.Is<int>(700));
        }

        [Test]
        public void Ready_15PowerButton_PowerIs50Again()
        {
            for (int i = 1; i <= 15; i++)
            {
                powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            }
            // And then once more
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            display.Received(2).ShowPower(50);
        }

        [Test]
        public void SetPower_CancelButton_DisplayCleared()
        {
            // Also checks if TimeButton is subscribed
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            display.Received(1).Clear();
        }

        [Test]
        public void SetPower_DoorOpened_DisplayCleared()
        {
            // Also checks if TimeButton is subscribed
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            door.Opened += Raise.EventWith(this, EventArgs.Empty);

            display.Received(1).Clear();
        }

        [Test]
        public void SetPower_DoorOpened_LightOn()
        {
            // Also checks if TimeButton is subscribed
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            door.Opened += Raise.EventWith(this, EventArgs.Empty);

            light.Received(1).TurnOn();
        }

        [Test]
        public void SetPower_TimeButton_TimeIs1()
        {
            // Also checks if TimeButton is subscribed
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            display.Received(1).ShowTime(Arg.Is<int>(1), Arg.Is<int>(0));
        }

        [Test]
        public void SetPower_2TimeButton_TimeIs2()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            display.Received(1).ShowTime(Arg.Is<int>(2), Arg.Is<int>(0));
        }

        [Test]
        public void SetTime_StartButton_CookerIsCalled()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime
            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            cooker.Received(1).StartCooking(50, 60);
        }

        [Test]
        public void SetTime_DoorOpened_DisplayCleared()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime
            door.Opened += Raise.EventWith(this, EventArgs.Empty);

            display.Received().Clear();
        }

        [Test]
        public void SetTime_DoorOpened_LightOn()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime
            door.Opened += Raise.EventWith(this, EventArgs.Empty);

            light.Received().TurnOn();
        }

        [Test]
        public void Ready_PowerAndTime_CookerIsCalledCorrectly()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            // Should call with correct values
            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            cooker.Received(1).StartCooking(100, 120);
        }

        [Test]
        public void Ready_FullPower_CookerIsCalledCorrectly()
        {
            for (int i = 50; i <= 700; i += 50)
            {
                powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            }

            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime

            // Should call with correct values
            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            cooker.Received(1).StartCooking(700, 60);

        }


        [Test]
        public void SetTime_StartButton_LightIsCalled()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime
            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now cooking

            light.Received(1).TurnOn();
        }

        [Test]
        public void Cooking_CookingIsDone_LightOff()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime
            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in cooking

            uut.CookingIsDone();
            light.Received(1).TurnOff();
        }

        [Test]
        public void Cooking_CookingIsDone_ClearDisplay()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime
            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in cooking

            // Cooking is done
            uut.CookingIsDone();
            display.Received(1).Clear();
        }

        [Test]
        public void Cooking_DoorIsOpened_CookerCalled()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime
            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in cooking

            // Open door
            door.Opened += Raise.EventWith(this, EventArgs.Empty);

            cooker.Received(1).Stop();
        }

        [Test]
        public void Cooking_DoorIsOpened_DisplayCleared()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime
            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in cooking

            // Open door
            door.Opened += Raise.EventWith(this, EventArgs.Empty);

            display.Received(1).Clear();
        }

        [Test]
        public void Cooking_CancelButton_CookerCalled()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime
            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in cooking

            // Open door
            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            cooker.Received(1).Stop();
        }

        [Test]
        public void Cooking_CancelButton_LightCalled()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime
            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in cooking

            // Open door
            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            light.Received(1).TurnOff();
        }

        [Test]
        public void ChangeTimeWhileCooking_TimeButton_CookerIsCalled()
        {
            powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetPower
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime
            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now adds time
            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            //
            cooker.Received(1).ChangeTimeWhileCooking();

        }

        [TestCase(1000000)]
        [TestCase(1000)]
        [TestCase(2000)]
        [TestCase(2000)]
        [TestCase(50)]
        [TestCase(700)]

        public void Ready_FullPowerWithHigherMaxPower_CookerIsCalledCorrectly(int maxPowerInTest)
        {
            uut = CreateUut(maxPowerInTest);

            for (int i = 50; i <= maxPowerInTest ; i += 50)
            {
                powerButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            }

            timeButton.Pressed += Raise.EventWith(this, EventArgs.Empty);
            // Now in SetTime

            // Should call with correct values
            startCancelButton.Pressed += Raise.EventWith(this, EventArgs.Empty);

            cooker.Received(1).StartCooking(maxPowerInTest, 60);

        }

        [TestCase(1000001)]
        [TestCase(49)]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(-500)]
        
        public void Ready_FullPowerWithHigherMaxPower_Exceptionthrown(int maxPowerInTest)
        {
            Assert.That(() => new UserInterface(powerButton, timeButton, startCancelButton, door, display, light, cooker, maxPowerInTest), 
                Throws.TypeOf<System.ArgumentOutOfRangeException>());
        }

    }

}
