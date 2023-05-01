using Emulator._6502;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emulator.Tests
{
    [TestClass()]
    public class CpuDeviceTests6502
    {
        private Cpu6502 _cpu;
        private Ram6502 _ram;

        [TestInitialize()]
        public void Init()
        {
            _cpu = new Cpu6502();
            _ram = new Ram6502();

            _cpu.RegisterDevice(_ram);
        }

        [TestCleanup()]
        public void Cleanup()
        {
            _cpu.Reset();

            _ram.Clear();
        }

        [DataTestMethod]
        [TestCategory("Device")]
        [DataRow((ushort)0xAADB, (byte)0x01, DisplayName = "T1")]
        [DataRow((ushort)0xBA09, (byte)0x10, DisplayName = "T2")]
        [DataRow((ushort)0xFE52, (byte)0x11, DisplayName = "T3")]
        [DataRow((ushort)0xED23, (byte)0x00, DisplayName = "T4")]
        public void WriteByteReadByteTest(ushort addr, byte data)
        {
            _cpu.WriteByte(addr, data);
            var l = _cpu.ReadByte(addr);

            Assert.AreEqual(data, l, "Byte Wrong");
        }

        [DataTestMethod]
        [TestCategory("Device")]
        [DataRow((ushort)0xAADB, (ushort)0x0AC1, DisplayName = "T1")]
        [DataRow((ushort)0xBA09, (ushort)0x1FE0, DisplayName = "T2")]
        [DataRow((ushort)0xFE52, (ushort)0xCD11, DisplayName = "T3")]
        [DataRow((ushort)0xED23, (ushort)0xEE00, DisplayName = "T4")]
        public void WriteWordReadWordTest(ushort addr, ushort data)
        {

            _cpu.WriteWord(addr, data);
            var l = _cpu.ReadWord(addr);

            Assert.AreEqual(data, l);
        }
    }
}