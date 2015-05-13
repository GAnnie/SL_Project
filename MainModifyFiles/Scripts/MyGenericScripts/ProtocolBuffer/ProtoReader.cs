using System;
//using System.Xml;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

/// <summary>
/// This type supports the Fluorine infrastructure and is not intended to be used directly from your code.
/// </summary>
using com.nucleus.h1.logic.core.modules.proto;

public class ProtoReader : BinaryReader
{
	static ProtoReader()
	{
	}

	/// <summary>
	/// Initializes a new instance of the AMFReader class based on the supplied stream and using UTF8Encoding.
	/// </summary>
	/// <param name="stream"></param>
	public ProtoReader(Stream stream) : base(stream)
	{
		Reset();
	}
    /// <summary>
    /// Resets object references.
    /// </summary>
	public void Reset()
	{
	}
	
    /// <summary>
    /// Deserializes object graphs from Action Message Format (AMF).
    /// </summary>
    /// <returns>The Object deserialized from the AMF stream.</returns>
	public object ReadData( byte[] data)
	{
		return ProtobufUtilsNet.parseTypedMessageFrom( data );
	}
	/// <summary>
    /// Reads a 2-byte unsigned integer from the current AMF stream using network byte order encoding and advances the position of the stream by two bytes.
	/// </summary>
    /// <returns>The 2-byte unsigned integer.</returns>
    
    public override ushort ReadUInt16()
	{
		//Read the next 2 bytes, shift and add.
		byte[] bytes = ReadBytes(2);
		return (ushort)(((bytes[0] & 0xff) << 8) | (bytes[1] & 0xff));
	}
    /// <summary>
    /// Reads a 2-byte signed integer from the current AMF stream using network byte order encoding and advances the position of the stream by two bytes.
    /// </summary>
    /// <returns>The 2-byte signed integer.</returns>
    public override short ReadInt16()
	{
		//Read the next 2 bytes, shift and add.
		byte[] bytes = ReadBytes(2);
		return (short)((bytes[0] << 8) | bytes[1]);
	}
    /// <summary>
    /// Reads an UTF-8 encoded String from the current AMF stream.
    /// </summary>
    /// <returns>The String value.</returns>
	public override string ReadString()
	{
		//Get the length of the string (first 2 bytes).
		int length = ReadUInt16();
		return ReadUTF(length);
	}
    /// <summary>
    /// Reads a Boolean value from the current AMF stream using network byte order encoding and advances the position of the stream by one byte.
    /// </summary>
    /// <returns>The Boolean value.</returns>
    public override bool ReadBoolean()
	{
		return base.ReadBoolean();
	}
    /// <summary>
    /// Reads a 4-byte signed integer from the current AMF stream using network byte order encoding and advances the position of the stream by four bytes.
    /// </summary>
    /// <returns>The 4-byte signed integer.</returns>
	public override int ReadInt32()
	{
		// Read the next 4 bytes, shift and add
		byte[] bytes = ReadBytes(4);
		int value = (int)((bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3]);
        return value;
	}
    /// <summary>
    /// Reads a 3-byte signed integer from the current AMF stream using network byte order encoding and advances the position of the stream by three bytes.
    /// </summary>
    /// <returns>The 3-byte signed integer.</returns>
    public int ReadUInt24()
    {
        byte[] bytes = this.ReadBytes(3);
        int value = bytes[0] << 16 | bytes[1] << 8 | bytes[2];
        return value;
    }
    /// <summary>
    /// Reads an 8-byte IEEE-754 double precision floating point number from the current AMF stream using network byte order encoding and advances the position of the stream by eight bytes.
    /// </summary>
    /// <returns>The 8-byte double precision floating point number.</returns>
	public override double ReadDouble()
	{			
		byte[] bytes = ReadBytes(8);
		byte[] reverse = new byte[8];
		//Grab the bytes in reverse order 
		for(int i = 7, j = 0 ; i >= 0 ; i--, j++)
		{
			reverse[j] = bytes[i];
		}
		double value = BitConverter.ToDouble(reverse, 0);
		return value;
	}
    /// <summary>
    /// Reads a single-precision floating point number from the current AMF stream using network byte order encoding and advances the position of the stream by eight bytes.
    /// </summary>
    /// <returns>The single-precision floating point number.</returns>
	public float ReadFloat()
	{			
		byte[] bytes = this.ReadBytes(4);
		byte[] invertedBytes = new byte[4];
		//Grab the bytes in reverse order from the backwards index
		for(int i = 3, j = 0 ; i >= 0 ; i--, j++)
		{
			invertedBytes[j] = bytes[i];
		}
		float value = BitConverter.ToSingle(invertedBytes, 0);
		return value;
	}

    /// <summary>
    /// Reads an UTF-8 encoded String.
    /// </summary>
    /// <param name="length">Byte-length header.</param>
    /// <returns>The String value.</returns>
	public string ReadUTF(int length)
	{
		if( length == 0 )
            return string.Empty;
		UTF8Encoding utf8 = new UTF8Encoding(false, true);
		byte[] encodedBytes = this.ReadBytes(length);
        string decodedString = utf8.GetString(encodedBytes, 0, encodedBytes.Length);
        return decodedString;
	}
	/// <summary>
    /// Reads an UTF-8 encoded AMF0 Long String type.
	/// </summary>
    /// <returns>The String value.</returns>
	public string ReadLongString()
	{
		int length = this.ReadInt32();
		return this.ReadUTF(length);
	}

    /// <summary>
    /// Reads an XML Document Type.
    /// The XML document type is always encoded as a long UTF-8 string.
    /// </summary>
    /// <returns>The XmlDocument.</returns>
//	public XmlDocument ReadXmlDocument()
//	{
//		string text = this.ReadLongString();
//		XmlDocument document = new XmlDocument();
//        if( text != null && text != string.Empty)
//		    document.LoadXml(text);
//		return document;
//	}
}
