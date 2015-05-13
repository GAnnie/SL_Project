	public class PacketHeader
	{
		public uint size;
		public uint oid;
		public uint eid;
		public int type; // 16-bits
		public int ttl; // 8-bits
		public int flags; // 8-bits
		
		public int headersize()
        {
			return 16;
		}
	}
