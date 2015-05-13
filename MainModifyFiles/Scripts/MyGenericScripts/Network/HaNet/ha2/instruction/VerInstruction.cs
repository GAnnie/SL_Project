
/**
 * 主动发送版本信息到ha.net上面，以便确认协议版本是否匹配
 */



using UnityEngine;

public class VerInstruction : MarshalableObject
{

	private string family;
	private uint currentVersion;
	private uint leastVersion;

	public VerInstruction()
{
		setRequestHead( (int)InstructionDefine.VER );
		
		HaConfiguration config = HaApplicationContext.getConfiguration();
		
		family = "xlands.ha2.internal";
		currentVersion = config.getHaVersion();
		leastVersion = config.getHaVersion();
		
		putString(family);
		putUnsignedInt32(currentVersion);
		putUnsignedInt32(leastVersion);
	}
	
	public PacketHeader getPacketHeader()   {
		return header;
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

	public void fromBytes(ProtoByteArray bytes)
	{
	
	}
}
