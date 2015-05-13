
/**
 * 用于保存程序的上下文，共享的数据会放在这里
 */
public class HaApplicationContext
{
	private static HaConnector _connector;
	private static HaConfiguration _configuration;

	public static void setConnector(HaConnector connector)
	{
		_connector = connector;
	}

	public static HaConnector getConnector()
	{
		return _connector;
	}
	
	public static HaConfiguration getConfiguration()
	{
		if (_configuration == null)
			_configuration = new HaConfigurationImpl();
		
		return _configuration;
	}
}

