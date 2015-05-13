using System;
using System.Collections;
using System.Collections.Generic;
//using FluorineFx.IO.Readers;
//using FluorineFx;
//using FluorineFx.IO;
//
/**
 * 提供连接的相关配置
 */
public interface HaConfiguration 
{
	uint getHaVersion();
	
	string getHost();
	
	void setHost(string host);

	int getPort();
	
	void setPort( int port );
	
	int getConnectTimeout();
	
	ArrayList getSupportEncryptMethods();
	
	uint getAllowedEncryptMethod();
	
	ProtoByteArray getEncryptKey();
	
	int getIdleInterval();
	
	Dictionary<string, string> getCreateChannelProperties();
	
	Dictionary<string, string> getClientProperties();
	
	string getClientName();
	
	uint getClientId();
	
	uint getGateId();
	
	void setClientId( uint id );
	
	void setGateId( uint id );
	
	void setLocalIp( string ip );
	
	string getLocalIp();
	
	void setLocalPort( int port );
	
	int getLocalPort();
	
//	object getConfigObject();
}
