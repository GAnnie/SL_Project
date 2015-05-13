	/**
	 * 请求服务所在节点信息
	 */

//
using System.Collections.Generic;

public class ServiceQueryRequestInstruction  : MarshalableObject
{

	private Dictionary<string, string> map;
	private string type;

	public ServiceQueryRequestInstruction()
	{
		setRequestHead((int)InstructionDefine.SERVICE_QUERY_REQ);
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
	
	public void execute( HaConnector haconn )
	{
		haconn.writeBytes( toBytes() );
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

	public void setProperties( Dictionary<string, string> s)
	{
		putMap(s);
	}
	
	public void setType( string t)
	{
		putString(t);
	}
}