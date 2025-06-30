using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Acubec.Payments.ISO8583.Parser.Implementation;
using Acubec.Payments.ISO8583.Parser.Types;

namespace Acubec.Payments.ISO8583.Parser.Test
{
    [TestFixture]
    public class IsoRequestTests : GlobalTestSetup
    {
        
        [Test]
        public void Constructor_InitializesProperties()
        {
            var req = new IsoRequest("1100", DataEncoding.ASCII, 2);
            Assert.That(req.MessageType, Is.EqualTo("1100"));
            Assert.That(req.ByteMapLength, Is.EqualTo(2));
            Assert.That(req.Fields, Is.Not.Null);
        }

        [Test]
        public void GetFieldList_ReturnsFieldsDictionary()
        {
            var req = new IsoRequest("1200", DataEncoding.ASCII, 2);
            var dict = req.GetFieldList();
            Assert.That(dict, Is.Not.Null);
            Assert.That(dict, Is.SameAs(req.Fields));
        }

        //[Test]
        //public void ToString_ReturnsBaseToString()
        //{
        //    var req = new IsoRequest("1300", DataEncoding.ASCII, 2);
        //    Assert.That(req.ToString(), Is.EqualTo(req.GetType().BaseType.ToString()));
        //}

        [Test]
        public void ToJson_ThrowsNotImplemented()
        {
            var req = new IsoRequest("1400", DataEncoding.ASCII, 2);
            Assert.Throws<NotImplementedException>(() => req.ToJson());
        }

        [Test]
        public void FromJson_ThrowsNotImplemented()
        {
            var req = new IsoRequest("1500", DataEncoding.ASCII, 2);
            Assert.Throws<NotImplementedException>(() => req.FromJson("{}"));
        }
    }
}
