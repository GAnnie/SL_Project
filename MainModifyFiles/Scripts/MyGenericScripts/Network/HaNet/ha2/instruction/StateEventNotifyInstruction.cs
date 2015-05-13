/**
 * 当状态改变的时候, 会收到该事件
 */


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEventNotifyInstruction : MarshalableObject
{
	public StateEventNotifyInstruction(){
	}
	
	public PacketHeader getPacketHeader()  {
		return null;
	}
	
	public DirectMessageHeader getDirectMessageHeader() {
		return null;
	}
	
	public GroupMessageHeader getGroupMessageHeader() {
		return null;
	}
	
	public BroadcastMessageHeader getBroadcastMessageHeader() 
	{
		return null;
	}
	
	public ProtoByteArray getBodyMessage()
	{
		return null;			
	}
	
	public void execute(HaConnector haconn)
	{
		haconn.setState(_state);
	}
	
	public ProtoByteArray toBytes()
	{
		return null;
	}
	
	private uint _state;
	
	public void fromBytes(ProtoByteArray bytes)
	{
		recivedBytes(bytes);
		
		_state = readUnsignedInt8();
		uint result = readUnsignedInt32();
		string reason = readString();
		
		GameDebuger.Log("状态更改原因:"+reason);
	}
}
