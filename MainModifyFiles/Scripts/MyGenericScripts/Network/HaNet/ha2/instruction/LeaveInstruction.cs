
/**
 * 主动离开ha.net
 */


public class LeaveInstruction : MarshalableObject
{
	public LeaveInstruction()
	{
		setRequestHead((int)InstructionDefine.JOIN_REQ);
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

