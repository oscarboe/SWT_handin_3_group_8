using Microwave.Classes.Boundary;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NSubstitute.Core.Arguments;
using NUnit.Framework;

namespace Microwave.Test.Unit
{
    [TestFixture]
    public class PowerTubeTest
    {
        private PowerTube uut;
        private IOutput output;

        [SetUp]
        public void Setup()
        {
            output = Substitute.For<IOutput>();
            uut = new PowerTube(output, 700);
        }


        [TestCase(1000001)]
        [TestCase(49)]
        [TestCase(1)]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-50)]
        public void Constructor_OutofRange_ThrowsException(int maxPower)
        {
            Assert.That(() => new PowerTube(output, maxPower), Throws.TypeOf<System.ArgumentOutOfRangeException>());
        }



        [TestCase(50)]
        [TestCase(700)]
        [TestCase(1000)]
        [TestCase(1000000)]
        public void Constructor_InRange_DoesNotThrowException(int maxPower)
        {
            Assert.That(() => new PowerTube(output, maxPower), Throws.Nothing);
        }


        

        

        [TestCase(1,700)]
        [TestCase(50,700)]
        [TestCase(100,700)]
        [TestCase(699, 700)]
        [TestCase(700, 700)]
        [TestCase(1000, 1000)]
        [TestCase(1000000, 1000000)]
        [TestCase(50, 50)]

        public void TurnOn_WasOffCorrectPower_CorrectOutput(int power, int maxPower)
        {
            uut = new PowerTube(output, maxPower);
            uut.TurnOn(power);
            output.Received().OutputLine(Arg.Is<string>(str => str.Contains($"{power}")));
        }

        [TestCase(-5,700)]
        [TestCase(-1,700)]
        [TestCase(0,700)]
        [TestCase(701,700)]
        [TestCase(750,700)]
        [TestCase(1000001, 1000000)]
        [TestCase(700,500)]
        [TestCase(500, 100)]
        public void TurnOn_WasOffOutOfRangePower_ThrowsException(int power, int maxPower)

        {
            uut = new PowerTube(output, maxPower);
            Assert.Throws<System.ArgumentOutOfRangeException>(() => uut.TurnOn(power));
        }

        [Test]
        public void TurnOff_WasOn_CorrectOutput()
        {
            uut.TurnOn(50);
            uut.TurnOff();
            output.Received().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        [Test]
        public void TurnOff_WasOff_NoOutput()
        {
            uut.TurnOff();
            output.DidNotReceive().OutputLine(Arg.Any<string>());
        }

        [Test]
        public void TurnOn_WasOn_ThrowsException()
        {
            uut.TurnOn(50);
            Assert.Throws<System.ApplicationException>(() => uut.TurnOn(60));
        }
    }
}