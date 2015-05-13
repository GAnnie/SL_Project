using System;
using System.Collections.Generic;


/// <summary>
/// 服务器列表
/// </summary>
public class ServerMessageList
{
	public string gservice;

	public string host;

	public string loginUrl;

	public string payUrl;
	
    public List<ServerInfo> list = null;

    public ServerMessageList()
    {}
}