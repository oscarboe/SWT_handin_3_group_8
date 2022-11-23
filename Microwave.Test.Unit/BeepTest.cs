using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microwave.Classes.Boundary;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using IOutput = Microwave.Classes.Interfaces.IOutput;

namespace Microwave.Test.Unit
{
    internal class BeepTest
    {
        private Beep uut;
        private IOutput output;

        [SetUp]
        public void Setup()
        {
            output = Substitute.For<IOutput>();
            uut = new Beep(output);
        }

        [Test]
        public void CommandCalled()
        {
            var returned = uut.PlayBeep();
            Assert.That(returned, Is.EqualTo(1));
        }

        [Test]
        public void BeepSoundAndConsole()
        {
            uut.PlayBeep();
            output.Received().OutputLine(Arg.Is<string>(str => str.Contains("Beep sound has been fired. Now waiting 3 seconds.")));
        }
    }
}
