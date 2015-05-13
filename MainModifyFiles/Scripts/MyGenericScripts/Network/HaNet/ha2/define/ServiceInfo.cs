/**
 * 查询接口服务所在节点时，会返回serviceinfo信息
 */
using System.Collections;
using System.Collections.Generic;

public class ServiceInfo
{
	public Dictionary<string, string> 	properties;
	public string	type ;
	public string	id ;
	public string	name ;
	public uint		version ;
	public uint		leastVersion ;
	public uint		highestVersion ;
	public uint		access ;
	public uint		query ;
	public string	doc ;

}