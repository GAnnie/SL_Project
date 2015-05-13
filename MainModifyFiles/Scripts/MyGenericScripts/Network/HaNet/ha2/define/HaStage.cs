/**
 * 定义连接状态，一个线性增长的常量，由ha.net规范定义
 */
public class HaStage
{
	public static readonly int DISCONNECT = -1;
	public static readonly uint CONNECTED = 0;
	public static readonly uint JOINED = 1;
	public static readonly uint LOGINED = 2;		
}
