/**
 * 当被踢掉的时候，会收到该事件
 */



using UnityEngine;

public class LeaveEventInstruction : MarshalableObject
{
	private uint result;
	private string reason;

	public LeaveEventInstruction()
	{
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
		EventObject obj = new EventObject(EventObject.Event_Leave);
		obj.msg = getReason();
		haconn.AddEventObj(obj);
	}

	public ProtoByteArray toBytes()
	{
		return null;
	}

	public void fromBytes(ProtoByteArray bytes)
	{
		recivedBytes(bytes);

		result = readUnsignedInt32();
		reason = readString();
		GameDebuger.Log("reason="+reason);
	}
	
	public uint getResult()
	{
		return result;
	}
	
	public string getReason()
	{
		return reason;
	}
}
