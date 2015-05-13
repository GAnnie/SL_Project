using System;
//using System.Xml;
using System.IO;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This type supports the Fluorine infrastructure and is not intended to be used directly from your code.
/// </summary>
using com.nucleus.h1.logic.core.modules.proto;


public class ProtoWriter : BinaryWriter
{
    static ProtoWriter()
	{
    }

	/// <summary>
	/// Initializes a new instance of the AMFReader class based on the supplied stream and using UTF8Encoding.
	/// </summary>
	/// <param name="stream"></param>
	public ProtoWriter(Stream stream) : base(stream)
	{
		Reset();
	}

    internal ProtoWriter(ProtoWriter writer, Stream stream)
        : base(stream)
    {
    }

	public void Reset()
	{
    }

    /// <summary>
    /// Writes a byte to the current position in the AMF stream.
    /// </summary>
    /// <param name="value">A byte to write to the stream.</param>
	public void WriteByte(byte value)
	{
		this.BaseStream.WriteByte(value);
	}
    /// <summary>
    /// Writes a stream of bytes to the current position in the AMF stream.
    /// </summary>
    /// <param name="buffer">The memory buffer containing the bytes to write to the AMF stream</param>
	public void WriteBytes(byte[] buffer)
	{
		for(int i = 0; buffer != null && i < buffer.Length; i++)
			this.BaseStream.WriteByte(buffer[i]);
	}
    /// <summary>
    /// Writes a 16-bit unsigned integer to the current position in the AMF stream.
    /// </summary>
    /// <param name="value">A 16-bit unsigned integer.</param>
    public void WriteShort(int value)
	{
        byte[] bytes = BitConverter.GetBytes((ushort)value);
		WriteBigEndian(bytes);
	}
//    /// <summary>
//    /// Writes an UTF-8 string to the current position in the AMF stream.
//    /// </summary>
//    /// <param name="value">The UTF-8 string.</param>
//    /// <remarks>Standard or long string header is written depending on the string length.</remarks>
//	public void WriteString(string value)
//	{
//		UTF8Encoding utf8Encoding = new UTF8Encoding(true, true);
//        int byteCount = utf8Encoding.GetByteCount(value);
//		if( byteCount < 65536 )
//		{
//			WriteByte(AMF0TypeCode.String);
//            WriteUTF(value);
//		}
//		else
//		{
//			WriteByte(AMF0TypeCode.LongString);
//            WriteLongUTF(value);
//		}
//	}
    /// <summary>
    /// Writes a UTF-8 string to the current position in the AMF stream.
    /// The length of the UTF-8 string in bytes is written first, as a 16-bit integer, followed by the bytes representing the Charactors of the string.
    /// </summary>
    /// <param name="value">The UTF-8 string.</param>
    /// <remarks>Standard or long string header is not written.</remarks>
    public void WriteUTF(string value)
	{
		//null string is not accepted
		//in case of custom serialization leads to TypeError: Error #2007: Parameter value must be non-null.  at flash.utils::ObjectOutput/writeUTF()

		//Length - max 65536.
		UTF8Encoding utf8Encoding = new UTF8Encoding();
        int byteCount = utf8Encoding.GetByteCount(value);
        byte[] buffer = utf8Encoding.GetBytes(value);
		this.WriteShort(byteCount);
		if (buffer.Length > 0)
			base.Write(buffer);
	}
    /// <summary>
    /// Writes a UTF-8 string to the current position in the AMF stream.
    /// Similar to WriteUTF, but does not prefix the string with a 16-bit length word.
    /// </summary>
    /// <param name="value">The UTF-8 string.</param>
    /// <remarks>Standard or long string header is not written.</remarks>
    public void WriteUTFBytes(string value)
	{
		//Length - max 65536.
		UTF8Encoding utf8Encoding = new UTF8Encoding();
		byte[] buffer = utf8Encoding.GetBytes(value);
		if (buffer.Length > 0)
			base.Write(buffer);
	}

    private void WriteLongUTF(string value)
	{
		UTF8Encoding utf8Encoding = new UTF8Encoding(true, true);
        uint byteCount = (uint)utf8Encoding.GetByteCount(value);
		byte[] buffer = new Byte[byteCount+4];
		//unsigned long (always 32 bit, big endian byte order)
		buffer[0] = (byte)((byteCount >> 0x18) & 0xff);
		buffer[1] = (byte)((byteCount >> 0x10) & 0xff);
		buffer[2] = (byte)((byteCount >> 8) & 0xff);
		buffer[3] = (byte)((byteCount & 0xff));
        int bytesEncodedCount = utf8Encoding.GetBytes(value, 0, value.Length, buffer, 4);
        if (buffer.Length > 0)
            base.BaseStream.Write(buffer, 0, buffer.Length);
	}

	/// <summary>
    /// Serializes object graphs in Action Message Format (AMF).
	/// </summary>
    /// <param name="objectEncoding">AMF version to use.</param>
    /// <param name="data">The Object to serialize in the AMF stream.</param>
	public void WriteData( object data )
	{
		try
		{
			byte[] byteData =  ProtobufUtilsNet.packIntoData( data );
			
			base.Write( byteData );
		}
		catch (Exception e)
        {
			GameDebuger.Log( e.Message );
        }
		
		
		
//		//If we have ObjectEncoding.AMF3 anything that serializes to String, Number, Boolean, Date will use AMF0 encoding
//		//For other types we have to switch the encoding to AMF3
//		if( data == null )
//		{
//			WriteNull();
//			return;
//		}
//		Type type = data.GetType();
//
//		if( _amf0ObjectReferences.ContainsKey( data ) )
//		{
//			WriteReference( data );
//			return;
//		}
//
//        IAMFWriter amfWriter = null;
//        if( AmfWriterTable[0].ContainsKey(type) )
//            amfWriter = AmfWriterTable[0][type] as IAMFWriter;
//		//Second try with basetype (enums and arrays for example)
//        if (amfWriter == null && AmfWriterTable[0].ContainsKey(type.BaseType))
//            amfWriter = AmfWriterTable[0][type.BaseType] as IAMFWriter;
//
//		if( amfWriter == null )
//		{
//			lock(AmfWriterTable)
//			{
//                if (!AmfWriterTable[0].ContainsKey(type))
//				{
//					amfWriter = new AMF0ObjectWriter();
//					AmfWriterTable[0].Add(type, amfWriter);
//				}
//				else
//					amfWriter = AmfWriterTable[0][type] as IAMFWriter;
//			}
//		}
//
//		if( amfWriter != null )
//		{
//			if( objectEncoding == ObjectEncoding.AMF0 )
//				amfWriter.WriteData(this, data);
//			else
//			{
//				if( amfWriter.IsPrimitive )
//					amfWriter.WriteData(this, data);
//				else
//				{
//					WriteByte(AMF0TypeCode.AMF3Tag);
//					WriteAMF3Data(data);
//				}
//			}
//		}
//		else
//		{
//            string msg = __Res.GetString(__Res.TypeSerializer_NotFound, type.FullName);
//		}
	}

    /// <summary>
    /// Writes a null type marker to the current position in the AMF stream.
    /// </summary>
	public void WriteNull()
	{
		WriteByte( 1 );
	}
    /// <summary>
    /// Writes a double-precision floating point number to the current position in the AMF stream.
    /// </summary>
    /// <param name="value">A double-precision floating point number.</param>
    /// <remarks>No type marker is written in the AMF stream.</remarks>
	public void WriteDouble(double value)
	{
        byte[] bytes = BitConverter.GetBytes(value);
        WriteBigEndian(bytes);
	}
    /// <summary>
    /// Writes a single-precision floating point number to the current position in the AMF stream.
    /// </summary>
    /// <param name="value">A double-precision floating point number.</param>
    /// <remarks>No type marker is written in the AMF stream.</remarks>
    public void WriteFloat(float value)
	{
		byte[] bytes = BitConverter.GetBytes(value);			
		WriteBigEndian(bytes);
	}
    /// <summary>
    /// Writes a 32-bit signed integer to the current position in the AMF stream.
    /// </summary>
    /// <param name="value">A 32-bit signed integer.</param>
    /// <remarks>No type marker is written in the AMF stream.</remarks>
    public void WriteInt32(int value)
	{
		byte[] bytes = BitConverter.GetBytes(value);
		WriteBigEndian(bytes);
	}
    /// <summary>
    /// Writes a 32-bit signed integer to the current position in the AMF stream using variable length unsigned 29-bit integer encoding.
    /// </summary>
    /// <param name="value">A 32-bit signed integer.</param>
    /// <remarks>No type marker is written in the AMF stream.</remarks>
    public void WriteUInt24(int value)
    {
        byte[] bytes = new byte[3];
        bytes[0] = (byte)(0xFF & (value >> 16));
        bytes[1] = (byte)(0xFF & (value >> 8));
        bytes[2] = (byte)(0xFF & (value >> 0));
        this.BaseStream.Write(bytes, 0, bytes.Length);
    }
    /// <summary>
    /// Writes a Boolean value to the current position in the AMF stream.
    /// </summary>
    /// <param name="value">A Boolean value.</param>
    /// <remarks>No type marker is written in the AMF stream.</remarks>
    public void WriteBoolean(bool value)
	{
        this.BaseStream.WriteByte(value ? ((byte)1) : ((byte)0));
	}
    /// <summary>
    /// Writes a 64-bit signed integer to the current position in the AMF stream.
    /// </summary>
    /// <param name="value">A 64-bit signed integer.</param>
    /// <remarks>No type marker is written in the AMF stream.</remarks>
    public void WriteLong(long value)
	{
        byte[] bytes = BitConverter.GetBytes(value);
		WriteBigEndian(bytes);
	}

	private void WriteBigEndian(byte[] bytes)
	{
		if( bytes == null )
			return;
		for(int i = bytes.Length-1; i >= 0; i--)
		{
			base.BaseStream.WriteByte( bytes[i] );
		}
	}
 
//    /// <summary>
//    /// Writes an Array value to the current position in the AMF stream.
//    /// </summary>
//    /// <param name="objectEcoding">Object encoding used.</param>
//    /// <param name="value">An Array object.</param>
//    public void WriteArray( Array value)
//	{
//        if (value == null)
//			this.WriteNull();
//		else
//		{
//            for (int i = 0; i < value.Length; i++)
//			{
//                WriteData(objectEcoding, value.GetValue(i));
//			}
//		}
//	}

    /// <summary>
    /// Writes an object to the current position in the AMF stream.
    /// </summary>
    /// <param name="objectEncoding">Object encoding used.</param>
    /// <param name="obj">The object to serialize.</param>
    /// <remarks>No type marker is written in the AMF stream.</remarks>
    public void WriteObject( object obj )
	{
//		if( obj == null )
//		{
//			WriteNull();
//			return;
//		}
//		AddReference(obj);
//
//		Type type = obj.GetType();
//
//		WriteByte(16);
//		string customClass = type.FullName;
//		customClass = FluorineConfiguration.Instance.GetCustomClass(customClass);
//
//		WriteUTF( customClass );
//
//		PropertyInfo[] propertyInfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
//
//                List<PropertyInfo> properties = new List<PropertyInfo>(propertyInfos);
//        for (int i = properties.Count - 1; i >=0 ; i--)
//		{
//			PropertyInfo propertyInfo = properties[i] as PropertyInfo;
//            if( propertyInfo.GetCustomAttributes(typeof(NonSerializedAttribute), true).Length > 0 )
//				properties.RemoveAt(i);
//			if( propertyInfo.GetCustomAttributes(typeof(TransientAttribute), true).Length > 0 )
//				properties.RemoveAt(i);
//		}
//		foreach(PropertyInfo propertyInfo in properties)
//		{
//			WriteUTF(propertyInfo.Name);
//			object value = propertyInfo.GetValue(obj, null);
//			WriteData( objectEncoding, value);
//		}
//
//		FieldInfo[] fieldInfos = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
//        List<FieldInfo> fields = new List<FieldInfo>(fieldInfos);
//		for(int i = fields.Count - 1; i >=0 ; i--)
//		{
//			FieldInfo fieldInfo = fields[i] as FieldInfo;
//            if( fieldInfo.GetCustomAttributes(typeof(NonSerializedAttribute), true).Length > 0 )
//				fields.RemoveAt(i);
//			if( fieldInfo.GetCustomAttributes(typeof(TransientAttribute), true).Length > 0 )
//				fields.RemoveAt(i);
//		}
//		for(int i = 0; i < fields.Count; i++)
//		{
//            FieldInfo fieldInfo = fields[i] as FieldInfo;
//			WriteUTF(fieldInfo.Name);
//			WriteData( objectEncoding, fieldInfo.GetValue(obj));
//		}
	}

    /// <summary>
    /// Serializes object graphs in Action Message Format (AMF).
    /// </summary>
    /// <param name="data">The Object to serialize in the AMF stream.</param>
    public void WriteAMF3Data(object data)
	{
//		if( data == null )
//		{
//			WriteAMF3Null();
//			return;
//		}
//		if(data is DBNull )
//		{
//			WriteAMF3Null();
//			return;
//		}
//        if (FluorineConfiguration.Instance.AcceptNullValueTypes && FluorineConfiguration.Instance.NullableValues != null)
//		{
//			Type type = data.GetType();
//			if( FluorineConfiguration.Instance.NullableValues.ContainsKey(type) && data.Equals(FluorineConfiguration.Instance.NullableValues[type]) )
//			{
//				WriteAMF3Null();
//				return;
//			}
//		}
//
//        IAMFWriter amfWriter = null;
//        if( AmfWriterTable[3].ContainsKey(data.GetType()) )
//            amfWriter = AmfWriterTable[3][data.GetType()] as IAMFWriter;
//		//Second try with basetype (Enums for example)
//        if (amfWriter == null && AmfWriterTable[3].ContainsKey(data.GetType().BaseType) )
//			amfWriter = AmfWriterTable[3][data.GetType().BaseType] as IAMFWriter;
//
//		if( amfWriter == null )
//		{
//			lock(AmfWriterTable)
//			{
//                if (!AmfWriterTable[3].ContainsKey(data.GetType()))
//                {
//                    amfWriter = new AMF3ObjectWriter();
//                    AmfWriterTable[3].Add(data.GetType(), amfWriter);
//                }
//                else
//                    amfWriter = AmfWriterTable[3][data.GetType()] as IAMFWriter;
//			}
//		}
//
//		if( amfWriter != null )
//		{
//			amfWriter.WriteData(this, data);
//		}
//		else
//		{
//			string msg = string.Format("Could not find serializer for type {0}", data.GetType().FullName);
//			//throw new FluorineException(msg);
//		}
//		//WriteByte(AMF3TypeCode.Object);
//		//WriteAMF3Object(data);
	}

//	internal void WriteByteArray(ByteArray byteArray)
//	{
//		//WriteBytes( byteArray.MemoryStream.ToArray() );
//	}

    /// <summary>
    /// Writes an object to the current position in the AMF stream.
    /// </summary>
    /// <param name="value">The object to serialize.</param>
    /// <remarks>No type marker is written in the AMF stream.</remarks>
//    public void WriteAMF3Object(object value)
//	{
//        if (!_objectReferences.ContainsKey(value))
//		{
//			_objectReferences.Add(value, _objectReferences.Count);
//
//			ClassDefinition classDefinition = GetClassDefinition(value);
//            if (classDefinition != null && _classDefinitionReferences.ContainsKey(classDefinition))
//            {
//                //Existing class-def
//                int handle = (int)_classDefinitionReferences[classDefinition];//handle = classRef 0 1
//                handle = handle << 2;
//                handle = handle | 1;
//                WriteAMF3IntegerData(handle);
//            }
//            else
//			{//inline class-def
//				
//				classDefinition = CreateClassDefinition(value);
//                _classDefinitionReferences.Add(classDefinition, _classDefinitionReferences.Count);
//				//handle = memberCount dynamic externalizable 1 1
//				int handle = classDefinition.MemberCount;
//				handle = handle << 1;
//				handle = handle | (classDefinition.IsDynamic ? 1 : 0);
//				handle = handle << 1;
//				handle = handle | (classDefinition.IsExternalizable ? 1 : 0);
//				handle = handle << 2;
//				handle = handle | 3;
//				WriteAMF3IntegerData(handle);
//				WriteAMF3UTF(classDefinition.ClassName);
//				for(int i = 0; i < classDefinition.MemberCount; i++)
//				{
//					string key = classDefinition.Members[i].Name;
//					WriteAMF3UTF(key);
//				}
//			}
//			//write inline object
//			if( classDefinition.IsExternalizable )
//			{
//				if( value is IExternalizable )
//				{
//					IExternalizable externalizable = value as IExternalizable;
//					DataOutput dataOutput = new DataOutput(this);
//					externalizable.WriteExternal(dataOutput);
//				}
//					else
//						throw new FluorineException(__Res.GetString(__Res.Externalizable_CastFail,classDefinition.ClassName));
//			}
//			else
//			{
//				for(int i = 0; i < classDefinition.MemberCount; i++)
//				{
//                    object memberValue = GetMember(value, classDefinition.Members[i]);
//                    WriteAMF3Data(memberValue);
//				}
//
//				if(classDefinition.IsDynamic)
//				{
//					IDictionary dictionary = value as IDictionary;
//					foreach(DictionaryEntry entry in dictionary)
//					{
//						WriteAMF3UTF(entry.Key.ToString());
//						WriteAMF3Data(entry.Value);
//					}
//					WriteAMF3UTF(string.Empty);
//				}
//			}
//		}
//		else
//		{
//			//handle = objectRef 0
//			int handle = (int)_objectReferences[value];
//			handle = handle << 1;
//			WriteAMF3IntegerData(handle);
//		}
//	}

}
