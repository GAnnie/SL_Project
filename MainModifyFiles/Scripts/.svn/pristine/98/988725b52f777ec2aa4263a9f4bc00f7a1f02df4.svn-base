/**
 * 序列化规则接口定义，规范序列化规则：
 * 
 * UINT8		8位无符号整数			1 byte
 * INT16		16位整数				2 bytes
 * UINT16		16位无符号整数			2 bytes
 * INT32		32位整数				4 bytes
 * UINT32		32位无符号整数			4 bytes
 * INT64		64位整数				8 bytes
 * UINT64		64位无符号整数			8 bytes
 * FLOAT		32位浮点数				4 bytes
 * DOUBLE		64位浮点数				8 bytes
 * BOOL			True: 1					1 byte
 * 				False: 0	
 * TIMESTAMP	64位无符号整数(msec)	8 bytes
 * 
 * STRING		字符串					[UINT16][String Data]
 * BYTES		二进制数据				[UINT32][Bytes Data]
 * ARRAY		序列					[UINT32][item0][item1]..[itemN]
 * MAP			表(结构化对象)			[UINT32][STRING][value1][STRING][value2]..[STRING][valueN]
 * 
 * 注：array中的item，是指任何数据类型
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
//

public interface Marshalable 
{
	void putUnsignedInt8(uint xbyte);
	void putInt16(int int16);
	void putUnsignedInt16(uint uint16);
	void putInt32(int int32);
	void putUnsignedInt32(uint uint32);
	void putInt64(ProtoByteArray int64); // FIXME
	void putUnsignedInt64(ProtoByteArray uint64); // FIXME
	void putFloat(float xfloat);
	void putDouble(double xdouble);
	void putBoolean(bool boolean);// true:1, false:0
//	void putTimeStamp( DateTime date );
	void putString( string s ); // 不改变string的编码方式
	void putBytes(ProtoByteArray bytes);
	void putArrayUnsignedInt32( ArrayList array);
	void putArrayString( ArrayList strings );
	void putMap( Dictionary<string, string> map );
	
	uint readUnsignedInt8();
	int readInt16();
	uint readUnsignedInt16();
	int readInt32();
	uint readUnsignedInt32();
	ProtoByteArray readInt64(); // FIXME
	ProtoByteArray readUnsignedInt64(); // FIXME
	float readFloat();
	double readDouble();
	bool readBoolean();
//	DateTime readTimeStamp();
	ProtoByteArray readBytes();
	string readString();
	ArrayList readArrayUnsignedInt32();
	ArrayList readArrayString();
	Dictionary<string, string> readMap();
}
