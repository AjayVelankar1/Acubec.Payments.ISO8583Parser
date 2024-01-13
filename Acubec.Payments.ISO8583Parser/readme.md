# Project Title
ISO 8583 Parser for .Net Core

## Description
this pis a open source .net core library for, mission-critical enterprise software, based on International Organization for Standardization transaction card originated messages standard (ISO-8583). 
This library is similar  of the popular jPOS library in JAVA.

## Usage
To Use this library you need to create a class SchemaConfiguration, which provides definitions of ISO8583.
this  is a serializable class. the same JSON file can be found in the test project.


### Example
 
 To Use this library add AddISO8583Parser reference as 
 services.AddISO8583Parser();
 
 Add in code

 var parser = new ISO8583MessageParser(_serviceProvider);
 var messageBytes = Array<byte>.Empty; // Get the message bytes from the network
 parser.Parse(configuration, messageBytes, _serviceProvider);

### Interfaces

**IMTIParser** - this default interface assume your MTI is 4 bit long and is in the first 4 bytes of the message.If your specification is different you can implement your own parser.

**IEncoderFormator** - This interface is used to encode and decode the message. There are three default implementation is register. 
DataEncoding.ASCII
DataEncoding.Binary
DataEncoding.EBCDIC


**IIsoField , ICustomFiledFactory** - This interface is used to define the field. There are two default implementation is register.
Fixed
Variable

if your DataField or field header mapping is different you can implement your own ICustomFiledFactory.

##TAGS

ISO8583 Parser


##  Repository URL 

Details on how to contribute to this project.

Git is used for source code management. The repository can be found at
https://github.com/AjayVelankar1/Acubec.Payments.ISO8583Parser

## Copyright
This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.
