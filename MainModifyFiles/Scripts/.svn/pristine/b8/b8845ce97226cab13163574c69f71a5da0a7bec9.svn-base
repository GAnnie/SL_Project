/**
 * 发送消息到某个节点上
 */


using UnityEngine;

public class DirectMessageInstruction : MarshalableObject
{
	private DirectMessageHeader directMessageHeader;
	private ProtoByteArray message;

	public DirectMessageInstruction()
	{
		directMessageHeader = new DirectMessageHeader();
		setPreRequestHead( (int)InstructionDefine.DIRECT_MESSAGE);			
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
		haconn.writeBytes( toBytes() );			
	}

	public ProtoByteArray toBytes() {
		writeHeadToBtyes();
		putUnsignedInt32(directMessageHeader.eid);
		putUnsignedInt32(directMessageHeader.routeType);
		putBytes(message);
		
		_bytes.Position = 0;
		header.size = _bytes.Length;
		putUnsignedInt32(header.size);
		_bytes.Position = 0;
		return _bytes;
	}

	public void fromBytes(ProtoByteArray bytes) {
		
	}

	public void setTargetId(uint id) {
		directMessageHeader.eid = id;		
		header.eid =  directMessageHeader.eid;
	}
	
	public void setRouteType(uint type) {
		directMessageHeader.routeType = type;
	}
	
	public void setMessage(ProtoByteArray m)
	{
		message = m;
	}
}
