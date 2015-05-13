using UnityEngine;
using System.Collections;

using System;

/// <summary>
/// This type supports the Fluorine infrastructure and is not intended to be used directly from your code.
/// </summary>
class ProtoDataOutput// : IDataOutput
{
	private ProtoWriter _protoWriter;

	public ProtoDataOutput( ProtoWriter protoWriter )
	{
		_protoWriter = protoWriter;
	}

	#region IDataOutput Members

	/// <summary>
	/// Writes a Boolean value.
	/// </summary>
	/// <param name="value"></param>
	public void WriteBoolean(bool value)
	{
		_protoWriter.WriteBoolean(value);
	}
	/// <summary>
	/// Writes a byte.
	/// </summary>
	/// <param name="value"></param>
	public void WriteByte(byte value)
	{
		_protoWriter.WriteByte(value);
	}
	/// <summary>
	/// Writes a sequence of length bytes from the specified byte array, bytes, starting offset(zero-based index) bytes into the byte stream.
	/// </summary>
	/// <param name="bytes"></param>
	/// <param name="offset"></param>
	/// <param name="length"></param>
	public void WriteBytes(byte[] bytes, int offset, int length)
	{
		for(int i = offset; i < offset + length; i++)
			_protoWriter.WriteByte( bytes[i] );
	}
	/// <summary>
	/// Writes an IEEE 754 double-precision (64-bit) floating point number.
	/// </summary>
	/// <param name="value"></param>
	public void WriteDouble(double value)
	{
		_protoWriter.WriteDouble(value);
	}
	/// <summary>
	/// Writes an IEEE 754 single-precision (32-bit) floating point number.
	/// </summary>
	/// <param name="value"></param>
	public void WriteFloat(float value)
	{
		_protoWriter.WriteFloat(value);
	}
	/// <summary>
	/// Writes a 32-bit signed integer.
	/// </summary>
	/// <param name="value"></param>
	public void WriteInt(int value)
	{
		_protoWriter.WriteInt32(value);
	}
	/// <summary>
	/// Writes an object to the byte stream or byte array in AMF serialized format.
	/// </summary>
	/// <param name="value"></param>
	public void WriteObject(object value)
	{
    	_protoWriter.WriteData( value );
	}
	/// <summary>
	/// Writes a 16-bit integer.
	/// </summary>
	/// <param name="value"></param>
	public void WriteShort(short value)
	{
		_protoWriter.WriteShort(value);
	}
	/// <summary>
	/// Writes a 32-bit unsigned integer.
	/// </summary>
	/// <param name="value"></param>
	public void WriteUnsignedInt(uint value)
	{
		_protoWriter.WriteInt32((int)value);
	}
	/// <summary>
	/// Writes a UTF-8 string to the byte stream. 
	/// The length of the UTF-8 string in bytes is written first, as a 16-bit integer, followed by 
	/// the bytes representing the Charactors of the string.
	/// </summary>
	/// <param name="value"></param>
	public void WriteUTF(string value)
	{
		_protoWriter.WriteUTF(value);
	}
	/// <summary>
	/// Writes a UTF-8 string. Similar to writeUTF, but does not prefix the string with a 16-bit length word.
	/// </summary>
	/// <param name="value"></param>
	public void WriteUTFBytes(string value)
	{
		_protoWriter.WriteUTFBytes(value);
	}

	#endregion
}
