/**
 * 实现该接口，支持从binary信息流，转换成有业务含义的对象
 */


public interface Packetable 
{
	PacketHeader getPacketHeader();
	
	DirectMessageHeader getDirectMessageHeader();
	
	GroupMessageHeader getGroupMessageHeader();
	
	BroadcastMessageHeader getBroadcastMessageHeader();
	
	ProtoByteArray getBodyMessage();
}
