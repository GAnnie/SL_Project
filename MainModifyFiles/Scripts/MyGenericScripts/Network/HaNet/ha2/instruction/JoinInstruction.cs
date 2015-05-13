/**
 * 加入到ha.net网络，建立了加密通道以后，会发送该指令
 */

using System;
using System.Collections;
using System.Collections.Generic;

//

using UnityEngine;

public class JoinInstruction : MarshalableObject
{

	public JoinInstruction()
	{
		setRequestHead((int)InstructionDefine.JOIN_REQ);
	}

	public PacketHeader getPacketHeader() {
		return null;
	}
	
	public DirectMessageHeader getDirectMessageHeader()  {
		return null;
	}
	
	public GroupMessageHeader getGroupMessageHeader()  {
		return null;			
	}
	
	public BroadcastMessageHeader getBroadcastMessageHeader()
	{
		return null;
	}
	
	public ProtoByteArray getBodyMessage()  {
		return null;
	}
	
	public void execute( HaConnector haconn) {
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
	
	public void setClientId(uint id) {
		putUnsignedInt32(id);
	}
	
	public void setName(string name) {
		putString(name);
	}
	
	public void setLocalIps(ArrayList ips) {
		putArrayString(ips);
	}
	
	public void setHost(string host) {
		putString(host);
	}
}
