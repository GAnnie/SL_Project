
/**
 * 接入ha.net，需要定时发送keepalive到ha.net中
 */


using System;
using System.Collections;
using System.Collections.Generic;

public class KeepaliveInstruction : MarshalableObject
{
	public KeepaliveInstruction()
	{
		setRequestHead( (int)InstructionDefine.KEEPALIVE);
	}

	public PacketHeader getPacketHeader()
	{
		return null;
	}
	
	public DirectMessageHeader getDirectMessageHeader()
	{
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
		return null;
	}
	
	public void execute(HaConnector haconn)
{
		haconn.writeBytes(toBytes());
	}

	public ProtoByteArray toBytes()
	{
		Dictionary<string, string> d = new Dictionary<string,string>();
		putMap(d);
		putDouble(0);
		
		_bytes.Position = 0;
		header.size = _bytes.Length;
		putUnsignedInt32(header.size);
		_bytes.Position = 0;
		return _bytes;
	}
	
	public void fromBytes(ProtoByteArray bytes)
	{
		
	}
}
