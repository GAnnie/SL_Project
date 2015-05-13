/**
 * 提供最基本的序列化支持，遵照ha2的序列化规则
 */

using System;
using System.Collections;
using System.Collections.Generic;

//

using UnityEngine;

public class MarshalableObject : Marshalable
{
	protected ProtoByteArray _bytes;
	private PacketHeader _requestHeader;

	public MarshalableObject()
	{
		_requestHeader = new PacketHeader();
		_bytes = new ProtoByteArray();			
	}
	
	protected void setRequestHead( int type){
		setPreRequestHead(type);
		writeHeadToBtyes();
	}
	
	protected void setPreRequestHead( int type){
		HaConfiguration haconn = HaApplicationContext.getConfiguration();
		
		_requestHeader.size = 0;
		_requestHeader.oid = haconn.getClientId();
		_requestHeader.eid = haconn.getGateId();
		_requestHeader.type = type;
		_requestHeader.ttl = 0;
		_requestHeader.flags = 0;
	}		
	
	protected void writeHeadToBtyes()
	{
		putUnsignedInt32((uint)_requestHeader.size);
		putUnsignedInt32((uint)_requestHeader.oid);
		putUnsignedInt32((uint)_requestHeader.eid);
		putUnsignedInt16((uint)_requestHeader.type);
		putUnsignedInt8((uint)_requestHeader.ttl);
		putUnsignedInt8((uint)_requestHeader.flags);		
	}
	
	/**
	 *接收数据
	 * 
	 */		
	protected void recivedBytes( ProtoByteArray bytes)
	{
		_bytes = bytes;
		header.size = readUnsignedInt32();
		header.oid = readUnsignedInt32();
		header.eid = readUnsignedInt32();
		header.type = (int)readUnsignedInt16();
		header.ttl = (int)readUnsignedInt8();
		header.flags = (int)readUnsignedInt8();
	}
	
	public void putUnsignedInt8( uint xbyte)
	{
		_bytes.WriteByte((byte)xbyte);
	}
	
	public void putInt16(int int16)
	{
		_bytes.WriteShort((short)int16);
	}
	
	public void putUnsignedInt16(uint uint16)
	{
		_bytes.WriteShort((short)uint16);
	}
	
	public void putInt32( int int32 )
	{
		_bytes.WriteInt(int32);
	}
	
	public void putUnsignedInt32(uint uint32)
	{
		_bytes.WriteUnsignedInt(uint32);
	}
	
	public void putInt64(ProtoByteArray int64) {
		//throw Error("not implement");
		GameDebuger.Log("not implement");
	}
	
	public void putUnsignedInt64(ProtoByteArray uint64) {
		//throw Error("not implement");
		GameDebuger.Log("not implement");
	}
	
	public void putFloat( float xfloat) {
		_bytes.WriteFloat(xfloat);
	}
	
	public void putDouble(double xdouble)
	{
		_bytes.WriteDouble(xdouble);
	}
	
	public void putBoolean(bool boolean)
	{
		// true:1, false:0
		if (boolean) {
			_bytes.WriteByte(1);
		} else {
			_bytes.WriteByte(0);
		}			
	}
	
//	public void putTimeStamp(DateTime date)
//	{
//		GameDebuger.Log("not implement");
//		//putDouble(date.getTime()); // FIXME 需要测试才能确认是否正确
//	}
	
	public void putString(string s) {
		// 不改变string的编码方式
		_bytes.WriteUTF(s);			
	}
	
	public void putBytesWithoutLen(ProtoByteArray bytes)
	{
		_bytes.WriteBytes(bytes.GetBuffer(), 0, (int) bytes.Length );
	}		
	
	public void putBytes(ProtoByteArray bytes) {
		_bytes.WriteUnsignedInt(bytes.Length);
		_bytes.WriteBytes(bytes.GetBuffer(), 0, (int) bytes.Length );
	}
	
	public void putArrayUnsignedInt32(ArrayList array) {
		_bytes.WriteUnsignedInt( (uint)array.Count);
		foreach(int i in array)
		{
			putUnsignedInt32( (uint)i);
		}				
	}
	
	public void putArrayString(ArrayList strings)
	{
		_bytes.WriteUnsignedInt((uint)strings.Count);
		foreach(string s in strings)
		{
			putString(s);
		}			
	}
	
	public void putMap(Dictionary<string, string> map)
	{
		uint size = (uint)map.Count;
		
		_bytes.WriteUnsignedInt( size );
		
		foreach ( KeyValuePair<string,string> k in map)
		{
			//string ks = k as String;
			//putString(map[ks]);
			
			putString( k.Key );
			putString( k.Value );
		}
	}
	
	public uint readUnsignedInt8() {
		return _bytes.ReadByte();
	}
	
	public int readInt16() {
		return _bytes.ReadShort();
	}
	
	public uint readUnsignedInt16() {
		return _bytes.ReadUnsignedShort();
	}
	
	public int readInt32() {
		return _bytes.ReadInt();
	}
	
	public uint readUnsignedInt32() {
		return _bytes.ReadUnsignedInt();
	}
	
	public ProtoByteArray readInt64() {
		GameDebuger.Log("not implement");
		return null;
	}
	
	public ProtoByteArray readUnsignedInt64() {
		GameDebuger.Log("not implement");
		return null;
	}
	
	public float readFloat() {
		return _bytes.ReadFloat();
	}
	
	public double readDouble() {
		return _bytes.ReadDouble();
	}
	
	public bool readBoolean() {
		int b = _bytes.ReadByte();
		if (b == 1) {
			return true;
		}
		return false;
	}

//	public DateTime readTimeStamp() {
//		GameDebuger.Log("not implement");
//		return null;
//	}
	public ProtoByteArray readBytesWhitoutLen() {
		uint size = (uint)_bytes.Length;
		byte[] buffer = new byte[size];
		ProtoByteArray bytes = new ProtoByteArray(buffer);
		_bytes.ReadBytes(bytes.GetBuffer(), 0, size );
		return bytes;
	}		
	
	public ProtoByteArray readBytes() {
		uint size = _bytes.ReadUnsignedInt();
		byte[] buffer = new byte[size];
		ProtoByteArray bytes = new ProtoByteArray(buffer);
		_bytes.ReadBytes(bytes.GetBuffer(), 0, size);
		return bytes;
	}
	
	public string readString() {
		string s = _bytes.ReadUTF();
		return s;
	}
	
	public ArrayList readArrayUnsignedInt32() {
		uint size = _bytes.ReadUnsignedInt();
		ArrayList array = new ArrayList();
		for (int i = 0; i < size; i++) {
			array.Add(_bytes.ReadUnsignedInt());
		}
		return array;
	}
	
	public ArrayList readArrayString() {
		uint size = _bytes.ReadUnsignedInt();
		ArrayList array = new ArrayList();
		for (int i = 0; i < size; i++) {
			array.Add(readString());
		}
		return array;			
	}
	
	public Dictionary<string, string> readMap() {
		uint size = _bytes.ReadUnsignedInt();
		Dictionary<string,string> map = new Dictionary<string, string>();
		for (int i = 0; i < size; i++)
		{
			string k = readString();
			string v = readString();
			
			map[k] = v;
		}
		return map;
	}

//	protected PacketHeader header()
//	{
//		return _requestHeader;
//	}
	
	protected PacketHeader header
	{
		get
		{
			return _requestHeader;
		}
	}
}
