using System;
using System.Collections;
using System.Collections.Generic;



/**
 * 接收到来自远端的消息
 */
public class GroupMessageReceivedInstruction : MarshalableObject
{

	private GroupMessageHeader groupMessageHeader;
	private ProtoByteArray message;

	public GroupMessageReceivedInstruction(){
		groupMessageHeader = new GroupMessageHeader();
	}

	public PacketHeader getPacketHeader()   {
		return null;			
	}
	
	public DirectMessageHeader getDirectMessageHeader()  {
		return null;
	}
	
	public GroupMessageHeader getGroupMessageHeader()  {
		return null;
	}
	
	public BroadcastMessageHeader getBroadcastMessageHeader()   {
		return null;
	}
	
	public ProtoByteArray getBodyMessage()   {
		return null;
	}
	
	public void execute(HaConnector haconn ) {
		EventObject obj = new EventObject(EventObject.Event_Message);
		obj.protoByteArray = message;
		haconn.AddEventObj(obj);
	}

	public ProtoByteArray toBytes() {
		return null;
		
	}

	public void fromBytes(ProtoByteArray bytes) {
		recivedBytes(bytes);

		groupMessageHeader.oid = header.oid;
		groupMessageHeader.gid = (long)readDouble();
		groupMessageHeader.routeType = readUnsignedInt32();
		readArrayUnsignedInt32();
		
		message = readBytes();
//			try{
			message.uncompress();
//			}catch(e:Error){			
//				trace(e.toString());
//			}	*/	
	}
	}
