using Acubec.Payments.ISO8583.Parser.Types;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Acubec.Payments.ISO8583.Parser.Test
{
    [TestFixture]
    public class SchemaConfigurationTests : GlobalTestSetup
    {
        
        [Test]
        public void SchemaConfiguration_PropertySettersAndGetters_Work()
        {
            var config = new SchemaConfiguration
            {
                HeaderParserName = "TestParser",
                NetworkName = "TestNet",
                NetworkDescription = "Desc",
                SchemaEncoding = DataEncoding.ASCII,
                Fields = new List<Field>(),
                Messages = new List<Message>(),
                ByteMapLength = 2
            };
            Assert.That(config.HeaderParserName, Is.EqualTo("TestParser"));
            Assert.That(config.NetworkName, Is.EqualTo("TestNet"));
            Assert.That(config.NetworkDescription, Is.EqualTo("Desc"));
            Assert.That(config.SchemaEncoding, Is.EqualTo(DataEncoding.ASCII));
            Assert.That(config.Fields, Is.Not.Null);
            Assert.That(config.Messages, Is.Not.Null);
            Assert.That(config.ByteMapLength, Is.EqualTo(2));
        }

        [Test]
        public void Field_SizeInt_ParsesCorrectly()
        {
            var field = new Field { Size = "12" };
            Assert.That(field.SizeInt, Is.EqualTo(12));
        }

        [Test]
        public void Message_PropertySettersAndGetters_Work()
        {
            var msg = new Message
            {
                Name = "Msg1",
                Description = "desc",
                Alias = "alias",
                Type = "type",
                Identifier = new[] { "id1" },
                Fields = new[] { "f1" },
                IsAdviceMessage = true,
                IsRepeatMessage = false,
                IsResponseMessage = true,
                IsNetworkMessage = false
            };
            Assert.That(msg.Name, Is.EqualTo("Msg1"));
            Assert.That(msg.Description, Is.EqualTo("desc"));
            Assert.That(msg.Alias, Is.EqualTo("alias"));
            Assert.That(msg.Type, Is.EqualTo("type"));
            Assert.That(msg.Identifier[0], Is.EqualTo("id1"));
            Assert.That(msg.Fields[0], Is.EqualTo("f1"));
            Assert.That(msg.IsAdviceMessage, Is.True);
            Assert.That(msg.IsRepeatMessage, Is.False);
            Assert.That(msg.IsResponseMessage, Is.True);
            Assert.That(msg.IsNetworkMessage, Is.False);
        }
    }
}
