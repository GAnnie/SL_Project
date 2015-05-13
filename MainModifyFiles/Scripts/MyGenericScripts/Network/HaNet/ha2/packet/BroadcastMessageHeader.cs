
using System;
using System.Collections;


public class BroadcastMessageHeader
{
	public Array ids;
	public uint routeType;
	
	public int size()
	{
		return 4 + 4 * ids.Length + 4;
	}
}
