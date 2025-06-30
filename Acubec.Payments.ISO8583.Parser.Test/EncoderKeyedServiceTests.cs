using System;
using Acubec.Payments.ISO8583.Parser.Implementation;
using Acubec.Payments.ISO8583.Parser.Interfaces;
using Acubec.Payments.ISO8583.Parser.Types;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Acubec.Payments.ISO8583.Parser.Test
{
    [TestFixture]
    public class EncoderKeyedServiceTests: GlobalTestSetup
    {
       
        [Test]
        public void CanResolveAsciiEncoderByKey()
        {
            var encoder = _provider.GetKeyedService<IEncoder>(DataEncoding.ASCII.ToString());
            Assert.That(encoder, Is.Not.Null);
            Assert.That(encoder, Is.TypeOf<AsciiEncoder>());
        }

        [Test]
        public void CanResolveHexEncoderByKey()
        {
            var encoder = _provider.GetKeyedService<IEncoder>(DataEncoding.HEX.ToString());
            Assert.That(encoder, Is.Not.Null);
            Assert.That(encoder, Is.TypeOf<HexEncoder>());
        }

        [Test]
        public void CanUseEncoderToEncodeAndDecode()
        {
            var encoder = _provider.GetKeyedService<IEncoder>(DataEncoding.ASCII.ToString());
            var bytes = encoder.Decode("TEST".AsSpan());
            var str = encoder.Encode(bytes);
            Assert.That(str.ToString(), Is.EqualTo("TEST"));
        }
    }
}
