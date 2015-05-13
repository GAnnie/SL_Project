	/**
	 * 配置的实现，随便乱写的
	 * 
	 */
//
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaConfigurationImpl : HaConfiguration
{

	protected string _localIp = "192.168.1.97";
	protected int	 _localPort = 0;
	protected uint	 _version = 65536;
	protected string _host = "192.168.1.97";
	protected int 	 _port = 443;
	protected string _encrypt;
	protected string _encryptKey;
	protected uint 	_gateid=0; // 接入网关的id
	protected uint 	_clientid=0; // 被ha.net分配的clientid		

	public HaConfigurationImpl()
	{
	}
	
	public uint getHaVersion() {
		return _version;
	}
	
	public string getHost() {
		return _host;
	}
	
	public void setHost( string host )
	{
		_host = host;
	}
	
	public int getPort()
	{
		return _port;
	}
	
	public void setPort(int port)
	{
		_port = port;	
	}
	
	public int getConnectTimeout()
	{
		return 5*1000;
	}
	
	public ArrayList getSupportEncryptMethods()
	{
		//	支持的流加密算法
		ArrayList s = new ArrayList();
		s.Add( 1 );
		//s.push(1); // RC4
		return s;
	}
	
	public uint getAllowedEncryptMethod()
	{
		//	密钥交换算法(非对称)
		return 0;	//	for test: NO
	}
	
	public ProtoByteArray getEncryptKey()
	{
		//	pubkey
		// 128bits key
		char[] chars = "1234567890ABCDEF".ToCharArray();
		string newStr = "";
		System.Random random = new System.Random();
		for (int i=0; i<16; i++)
		{
			int index = random.Next(0, chars.Length);
			newStr += chars[index];
		}

		ProtoByteArray b = new ProtoByteArray();
		b.WriteUTFBytes(newStr);
		return b;
	}
	
	public int getIdleInterval() {
		return 20*1000; // 每20秒，发送一次keepalive事件
	}
	
	public Dictionary<string, string> getCreateChannelProperties()
	{
		Dictionary<string, string> d = new Dictionary<string, string>();
		d["name"] = "aclient";
		return d;
	}
	
	public Dictionary<string, string> getClientProperties()
	{
		Dictionary<string,string> d = new Dictionary<string, string>();
		d["name"] = "aclient";
		return d;
	}
	
	public string getClientName() {
		return "aclient";
	}
	
	public uint getClientId() {
		return _clientid;
	}
	
	public uint getGateId() {
		return _gateid;
	}
	
	public void setClientId(uint id) {
		_clientid = id;
	}
	
	public void setGateId(uint id) {
		_gateid = id;
	}		
	
	public void setLocalIp(string ip){
		_localIp = ip;
	}
	
	public string getLocalIp(){
		return _localIp;
	}
	
	public void setLocalPort(int port){
		_localPort = port;
	}
	
	public int getLocalPort(){		
		return _localPort;
	}
	
//	public object getConfigObject()
//	{
//		object obj = new object();
//		obj.clientId = getClientId();
//		obj.gateId = getGateId();
//		obj.host = getHost();
//		obj.localIp = getLocalIp();
//		obj.localPort = getLocalPort();
//		obj.port = getPort();
//		return obj;
//	}
}
