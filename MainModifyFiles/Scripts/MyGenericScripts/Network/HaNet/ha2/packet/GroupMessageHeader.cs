
using System;
using System.Collections;


public class GroupMessageHeader
{
	public uint oid;
	public Int64 gid;
	public uint routeType;
	public Array excludeIds;

	public int size()
	{
		return 4 + 4 + 4 + 4 * excludeIds.Length;
	}
}
