using UnityEngine;
using System.Collections;

public class EventObject
{
	public const string Event_Leave = "Event_Leave";
	public const string Event_Message = "Event_Message";
	public const string Event_Close = "Event_Close";
	public const string Event_Joined = "Event_Joined";
	public const string Event_Service = "Event_Service";
	public const string Event_State = "Event_State";
	public const string Event_Timeout = "Event_Timeout";
	
	public string type = "event";

	public ProtoByteArray protoByteArray;
	public ArrayList services;
	public string msg = "";
	public uint state = 0;

	public EventObject(string _type)
	{
		type = _type;
	}
}
