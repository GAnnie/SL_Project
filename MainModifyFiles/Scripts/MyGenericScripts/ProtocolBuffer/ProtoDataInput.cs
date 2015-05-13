using System;

/// <summary>
/// This type supports the Fluorine infrastructure and is not intended to be used directly from your code.
/// </summary>
class ProtoDataInput// : IDataInput
{
	private ProtoReader _protoReader;

	public ProtoDataInput(ProtoReader protoReader)
	{
		_protoReader = protoReader;
	}

	#region IDataInput Members

	/// <summary>
	/// Reads a Boolean from the byte stream or byte array. 
	/// </summary>
	/// <returns></returns>
	public bool ReadBoolean()
	{
		return _protoReader.ReadBoolean();
	}
	/// <summary>
	/// Reads a signed byte from the byte stream or byte array. 
	/// </summary>
	/// <returns></returns>
	public byte ReadByte()
	{
		return _protoReader.ReadByte();
	}
	/// <summary>
	/// Reads length bytes of data from the byte stream or byte array. 
	/// </summary>
	/// <param name="bytes"></param>
	/// <param name="offset"></param>
	/// <param name="length"></param>
	public void ReadBytes(byte[] bytes, uint offset, uint length)
	{
		byte[] tmp = _protoReader.ReadBytes((int)length);
		for(int i = 0; i < tmp.Length; i++)
			bytes[i+offset] = tmp[i];
	}
	/// <summary>
	/// Reads an IEEE 754 double-precision floating point number from the byte stream or byte array. 
	/// </summary>
	/// <returns></returns>
	public double ReadDouble()
	{
		return _protoReader.ReadDouble();
	}
	/// <summary>
	/// Reads an IEEE 754 single-precision floating point number from the byte stream or byte array. 
	/// </summary>
	/// <returns></returns>
	public float ReadFloat()
	{
		return _protoReader.ReadFloat();
	}
	/// <summary>
	/// Reads a signed 32-bit integer from the byte stream or byte array. 
	/// </summary>
	/// <returns></returns>
	public int ReadInt()
	{
		return _protoReader.ReadInt32();
	}
	/// <summary>
	/// Reads an object from the byte stream or byte array, encoded in AMF serialized format. 
	/// </summary>
	/// <returns></returns>
	public object ReadObject( byte[] data )
	{
        return _protoReader.ReadData( data );
	}
	/// <summary>
	/// Reads a signed 16-bit integer from the byte stream or byte array. 
	/// </summary>
	/// <returns></returns>
	public short ReadShort()
	{
		return _protoReader.ReadInt16();
	}
	/// <summary>
	/// Reads an unsigned byte from the byte stream or byte array. 
	/// </summary>
	/// <returns></returns>
	public byte ReadUnsignedByte()
	{
		return _protoReader.ReadByte();
	}
	/// <summary>
	/// Reads an unsigned 32-bit integer from the byte stream or byte array. 
	/// </summary>
	/// <returns></returns>
	public uint ReadUnsignedInt()
	{
		return (uint)_protoReader.ReadInt32();
	}
	/// <summary>
	/// Reads an unsigned 16-bit integer from the byte stream or byte array. 
	/// </summary>
	/// <returns></returns>
	public ushort ReadUnsignedShort()
	{
		return _protoReader.ReadUInt16();
	}
	/// <summary>
	/// Reads a UTF-8 string from the byte stream or byte array. 
	/// </summary>
	/// <returns></returns>
	public string ReadUTF()
	{
		return _protoReader.ReadString();
	}
	/// <summary>
	/// Reads a sequence of length UTF-8 bytes from the byte stream or byte array, and returns a string. 
	/// </summary>
	/// <param name="length"></param>
	/// <returns></returns>
	public string ReadUTFBytes(uint length)
	{
		return _protoReader.ReadUTF((int)length);
	}

	#endregion
}
