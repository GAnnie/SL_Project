
	/**
	 * 接收到来自远端的消息
	 */


public class DirectMessageReceivedInstruction : MarshalableObject
{

	private DirectMessageHeader directMessageHeader;
	private ProtoByteArray message;

	public DirectMessageReceivedInstruction()
	{
		directMessageHeader = new DirectMessageHeader();
	}

	public PacketHeader getPacketHeader()
	{
		return null;			
	}
	
	public DirectMessageHeader getDirectMessageHeader()  {
		return null;
	}
	
	public GroupMessageHeader getGroupMessageHeader()  {
		return null;
	}
	
	public BroadcastMessageHeader getBroadcastMessageHeader()  {
		return null;
	}
	
	public ProtoByteArray getBodyMessage()  {
		return null;
	}
	
	public void execute(HaConnector haconn)
	{
		EventObject obj = new EventObject(EventObject.Event_Message);
		obj.protoByteArray = message;
		haconn.AddEventObj(obj);
	}

	public ProtoByteArray toBytes() {
		return null;
		
	}

	public void fromBytes(ProtoByteArray bytes) {
		recivedBytes(bytes);

		directMessageHeader.oid = header.oid; 
		directMessageHeader.eid = readUnsignedInt32();
		directMessageHeader.routeType = readUnsignedInt32();
		
		message = readBytes();
//			try{
			message.uncompress();
//		}catch(e:Error){			
//			trace(e.toString());
//		}
	}
}
