
	/**
	 * 返回是否加入ha.net网络
	 */

using System;
using System.Collections;
using System.Collections.Generic;
//

using UnityEngine;

	public class JoinAckInstruction : MarshalableObject
{

		private Dictionary< string, string > properties;
		private uint result;
		private uint gateid;
		private uint stage;
		private string externalAddress; //格式为 xxx.xxx.xxx.xxx:xxx
		private ArrayList gates;
		
		public JoinAckInstruction(){
		}

		public PacketHeader getPacketHeader()
		{
			return header;
		}
		
		public DirectMessageHeader getDirectMessageHeader() {
			return null;
			
		}
		
		public GroupMessageHeader getGroupMessageHeader()  {
			return null;
			
		}
		
		public BroadcastMessageHeader getBroadcastMessageHeader() {
			return null;
			
		}
		
		public ProtoByteArray getBodyMessage()  {
			return null;
		}
		
		public void execute( HaConnector haconn ) {
			// 设置node-id和stage
			
			HaConfiguration config = HaApplicationContext.getConfiguration();
			config.setGateId(header.oid);
			config.setClientId(header.eid);			
			haconn.setState(stage);

			EventObject obj = new EventObject(EventObject.Event_Joined);
			haconn.AddEventObj(obj);
		}

		public ProtoByteArray toBytes() {
			return null;			
		}

		public void fromBytes(ProtoByteArray bytes) {
			recivedBytes(bytes);
			
			properties = readMap();
			result = readUnsignedInt32();
			gateid = readUnsignedInt32();
			GameDebuger.Log("clientid="+gateid);
			stage = readUnsignedInt8();
			externalAddress = readString();
			string[] address = externalAddress.Split(':');
			HaApplicationContext.getConfiguration().setLocalIp(address[0]);
			HaApplicationContext.getConfiguration().setLocalPort( int.Parse( address[1] ) );
			gates = readArrayString();
		}
	}
