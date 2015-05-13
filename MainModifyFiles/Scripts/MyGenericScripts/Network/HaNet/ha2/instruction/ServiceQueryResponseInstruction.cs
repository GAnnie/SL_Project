using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//


	/**
	 * 请求service信息的返回
	 */
	public class ServiceQueryResponseInstruction : MarshalableObject
{

		private Dictionary<string, string> properties;
		private uint result;
		private ArrayList services;

		public ServiceQueryResponseInstruction(){
		}

		public PacketHeader getPacketHeader()   {
			return null;
		}
		
		public DirectMessageHeader getDirectMessageHeader()   {
			return null;
		}
		
		public GroupMessageHeader getGroupMessageHeader()  {
			return null;			
		}
		
		public BroadcastMessageHeader getBroadcastMessageHeader()   {
			return null;
		}
		
		public ProtoByteArray getBodyMessage()  {
			return null;
		}
		
		public void execute(HaConnector haconn)
		{
			EventObject obj = new EventObject(EventObject.Event_Service);
			obj.services = getServices();
			haconn.AddEventObj(obj);
		}

		public ProtoByteArray toBytes() {
			return null;
		}

		public void fromBytes(ProtoByteArray bytes) {
			recivedBytes(bytes);
			
			properties = readMap();
			result = readUnsignedInt32();

			uint size = readUnsignedInt32();
			if (size > 0){
				services = new ArrayList();
			}
			for (int i = 0; i < size; i++) {
				ServiceInfo service = new ServiceInfo();
				service.properties = readMap();
				service.type = readString();
				service.id = readUnsignedInt32().ToString();
				service.name = readString();
				service.version = readUnsignedInt32();
				service.leastVersion = readUnsignedInt32();
				service.highestVersion = readUnsignedInt32();
				service.access = readUnsignedInt8();
				service.query = readUnsignedInt8();
				service.doc = readString();
			
//				//---------------------------------------------------
//				GameDebuger.Log( "properties:" + service.properties.ToString() );
//				GameDebuger.Log( "type:" + service.type.ToString() );
//				GameDebuger.Log( "id:" + service.id.ToString() );
//				GameDebuger.Log( "name:" + service.name.ToString() );
//				GameDebuger.Log( "version:" + service.version.ToString() );
//				GameDebuger.Log( "leastVersion:" + service.leastVersion.ToString() );
//				GameDebuger.Log( "highestVersion:" + service.highestVersion.ToString() );
//				GameDebuger.Log( "access:" + service.access.ToString() );
//				GameDebuger.Log( "query:" + service.query.ToString() );
//				GameDebuger.Log( "doc:" + service.doc.ToString() );
//				//---------------------------------------------------
				
				services.Add(service);
			}
		}

		public ArrayList getServices() {
			return services;
		}
	}
