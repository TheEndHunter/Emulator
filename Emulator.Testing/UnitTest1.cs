using Emulator._6502.Devices;

namespace Emulator.Testing
{
    [TestClass]
    public abstract class DeviceTests
    {
        [TestMethod]
        public abstract void ReadWriteByteTest(ref Device6502 device);

        [TestMethod]
        public abstract void ReadWriteWordTest(ref Device6502 device);
    }

    [TestClass]
    public class BusTests
    {
        private Bus6502 Bus;
        private Ram6502 _Ram;

        public BusTests()
        {
            Bus = new Bus6502();
            _Ram = new Ram6502();
            Bus.RegisterDevice(new AddressRange6502 { StartAddress = 0x0000, EndAddress = 0xFFFF }, _Ram);
        }
        ~BusTests()
        {
            Bus.UnRegisterDevice(_Ram);
        }


        [TestMethod]
        public void ReadWriteByteTest()
        {

        }
        [TestMethod]
        public void ReadWriteWordTest()
        {

        }
    }

    [TestClass]
    public class RegisterTests
    {
        [TestMethod]
        public void TestRegisterA()
        {

        }
        [TestMethod]
        public void TestRegisterX()
        {

        }
        [TestMethod]
        public void TestRegisterY()
        {

        }
        [TestMethod]
        public void TestStatusFlags()
        {

        }
    }

}

