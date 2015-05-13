/**
 * 响应事件
 * @author senkay
 */
using com.nucleus.commons.message;


public class ResponseEvent
{
	public static readonly string EVENT_SUCCESS = "event_success";
	public static readonly string EVENT_ERROR = "event_error";
	public static readonly string EVENT_TIMEOUT = "event_timeout";
	public static readonly string EVENT_GIVEUP = "event_giveup";
	public static readonly string EVENT_FINISH = "event_finish";	
	
	private object _response;
	
	public ResponseEvent(string type, object response=null)
	{
		_response = response;
	}
	
	public ErrorResponse getErrorResponse()
	{
		return _response as ErrorResponse;
	}
	
	public GeneralResponse getResponse()
	{
		return _response as GeneralResponse;
	}
}