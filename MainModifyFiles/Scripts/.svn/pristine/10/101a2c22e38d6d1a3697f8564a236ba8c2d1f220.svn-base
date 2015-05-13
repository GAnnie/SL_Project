/**
 * 当连接成功时，客户端会尝试和服务器端建立安全通道
 */

using System;
using System.Collections;
using System.Collections.Generic;
//
using UnityEngine;

public class SSLOpenRequestInstruction : MarshalableObject
{

	public SSLOpenRequestInstruction() {
		setRequestHead((int)InstructionDefine.SSL_OPEN_REQ);			
	}
	
	public PacketHeader getPacketHeader() {
		return header;
	}
	
	public DirectMessageHeader getDirectMessageHeader() {
		return null;
	}
	
	public GroupMessageHeader getGroupMessageHeader()  {
		return null;
	}
	
	public BroadcastMessageHeader getBroadcastMessageHeader()  {
		return null;
	}
	
	public ProtoByteArray getBodyMessage()  {
		_bytes.Position = header.headersize(); // 需要先设置header位置
		return _bytes;
	}
	
	public void execute(HaConnector haconn ) {
		haconn.writeBytes(toBytes());
	}

	public ProtoByteArray toBytes() {			
		_bytes.Position = 0;
		header.size = _bytes.Length;
		putUnsignedInt32(header.size);
		_bytes.Position = 0;
		return _bytes;
	}

	public void fromBytes(ProtoByteArray bytes) {
		GameDebuger.Log("do not call me");
	}

	public void setProperties(Dictionary<string, string> s) {
		putMap(s);
	}
	
	public void setSupportEncryptMethods(ArrayList s) {
		putArrayUnsignedInt32(s);
	}
	
	public void setAllowedMethod(uint a) {
		putUnsignedInt32(a);
	}
	
	public void setEncryptKey(ProtoByteArray k) {
		putBytes(k);
	}
}
