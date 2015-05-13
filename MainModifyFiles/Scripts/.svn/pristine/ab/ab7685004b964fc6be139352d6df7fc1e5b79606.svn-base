/**
 * SSL_OPEN_REQ的返回指令，以便建立安全通道
 */

//
using System.Collections.Generic;

using UnityEngine;

public class SSLOpenResponseInstruction : MarshalableObject
{

	private Dictionary<string, string> properties;
	private uint result;
	private uint method;
	private ProtoByteArray encryptkey;
	
	public SSLOpenResponseInstruction()
	{
	}

	public PacketHeader getPacketHeader()
	{
		return header;
	}
	
	public DirectMessageHeader getDirectMessageHeader() {
		return null;
	}
	
	public GroupMessageHeader getGroupMessageHeader()
	{
		return null;
	}
	
	public BroadcastMessageHeader getBroadcastMessageHeader()
	{
		return null;
	}
	
	public ProtoByteArray getBodyMessage()
	{
		_bytes.Position = header.headersize();
		return _bytes;
	}	
	
	public void execute(HaConnector haconn)
	{
		// 首先需要初始化rc4，在连接上的时候初始化rc4，应该是可以的
		if (method > 0){
			haconn.setRc4key(encryptkey);
		}
	
		HaConfiguration config = HaApplicationContext.getConfiguration();
		config.setGateId(header.oid);
		config.setClientId(header.eid);		

		// 和ha.net交换版本
		VerInstruction ver = new VerInstruction();
		ver.execute( haconn );
	}

	public ProtoByteArray toBytes()
	{
		_bytes.Position = 0;
		return _bytes;
	}

	public void fromBytes( ProtoByteArray bytes )
	{
		recivedBytes(bytes);
		properties = readMap();
		result = readUnsignedInt32();
		method = readUnsignedInt32();
		
		//debug only
//		string tmp = "";		for ( int i = bytes.Position; i < bytes.Length; ++i ) {	tmp += bytes.GetBuffer()[i] + ",";	}		GameDebuger.Log( tmp );

		encryptkey = readBytes();
	
//		// test only delte me later--------------------------------------------------
//		byte[] byteList = new byte[]{ 0x76, 0x1f, 0xf5, 0x40, 0x8c, 0xc0, 0x0b, 0xb1, 0x99, 0xc4, 0xbe, 0xba, 0x4b, 0x1d, 0xb2, 0xb1  };
//		encryptkey = new ProtoByteArray();
//		encryptkey.WriteBytes( byteList, 0, 16 );
//		////////////////////////////////////////////////////////////////////////////////////////////////////////////
		
	}
}
