/**
 * 消息处理类，当接收到的信息没有ActionCallback对应，进入MessageProcessor处理，特别适合广播的处理
 * @author ivan
 */
public interface MessageProcessor
{
	/**
	 * 获取激发消息处理器的事件类型
	 * 一般用 getQualifiedClassName(XXXDto);
	 */
	string getEventType();

	/**
	 * 处理信息
	 */
	void process( object message );
}
