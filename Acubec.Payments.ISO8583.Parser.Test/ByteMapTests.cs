using Acubec.Payments.ISO8583.Parser.Implementation;
using NUnit.Framework.Legacy;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace Acubec.Payments.ISO8583.Parser.Test
{
    [TestFixture]
    public class ByteMapTests
    {
       
        [Test]
        public void ByteMap_DefaultConstructor_InitializesBitMap()
        {
            var map = new ByteMap();
            Assert.That(map.BitMap.Length, Is.EqualTo(8));
        }

        [Test]
        public void ByteMap_SetBitMap_UpdatesBitMap()
        {
            var map = new ByteMap();
            var newMap = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            map.SetBitMap(newMap);
            Assert.That(map.BitMap.ToArray(), Is.EquivalentTo(newMap));
        }

        [Test]
        public void ByteMaps_Constructor_InitializesFields()
        {
            var maps = new ByteMaps("1100", 2);
            Assert.That(maps.ByteMapLength, Is.EqualTo(2));
        }

        [Test]
        public void ByteMaps_ResetBitMap_ResetsAll()
        {
            var maps = new ByteMaps("1100", 2);
            maps.resetBitMap();
            Assert.That(maps.getBitMap(0).Length, Is.EqualTo(8));
        }

        [Test]
        public void ByteMaps_SetValue_AddsField()
        {
            var maps = new ByteMaps("1100", 2);
            var field = new DummyIsoField { Value = "abc" };
            maps.SetValue(2, field);
            // No exception means success
        }

        [Test]
        public void ByteMaps_SetBitMap_UpdatesBitMap()
        {
            var maps = new ByteMaps("1100", 2);
            var newMap = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            maps.SetBitMap(0, newMap);
            Assert.That(maps.getBitMap(0).ToArray(), Is.EquivalentTo(newMap));
        }
    }

    // Minimal dummy IIsoField for testing
    class DummyIsoField : Acubec.Payments.ISO8583.Parser.Implementation.IIsoField
    {
        public bool IsSet { get; set; } = true;
        public int Length => 3;
        public string Name => "Dummy";
        public Acubec.Payments.ISO8583.Parser.Types.IsoFieldType Type => Acubec.Payments.ISO8583.Parser.Types.IsoFieldType.Fixed;
        public string Value { get; set; }
        public int MessageIndex => 1;
        public bool Mask { get; set; }

        public ReadOnlySpan<byte> ValueSpans => Span<byte>.Empty;

        public ReadOnlySpan<byte> GetValueBytes() => new byte[] { 1, 2, 3 };
        public int SetValueBytes(ReadOnlySpan<byte> dataByte, int offset) => 0;
        public string LogDump() => "log";
        public override string ToString() => Value;
    }
}
