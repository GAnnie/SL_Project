using UnityEngine;
using System.Collections;
using System.IO;
//using System.Xml;
using System.ComponentModel;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Net.Sockets;


public class ARC4
{
	private int i = 0;
	private int j= 0;
	private ProtoByteArray S;
	private static readonly uint psize= 256;

	public ARC4( ProtoByteArray key = null )
    {
		S = new ProtoByteArray();
		if (key != null )
		{
			init(key);
		}
	}
	public uint getPoolSize() {
		return psize;
	}
	
	public void init(ProtoByteArray key)
    {
		int i;
		int j;
		int t;
		
		for (i=0; i<256; ++i)
		{
			S.Position = i;
			S.WriteByte( (byte)i );
			//S[i] = i;
		}
			
		j=0;
		
		for (i=0; i<256; ++i)
		{
			S.Position = i;
			byte si = S.ReadByte();
			
			key.Position = (int)(i % key.Length);
			byte keyVal = key.ReadByte();
			
			j = ( j + si + keyVal ) & 255;
			//j = (j + S[i] + key[i%key.length]) & 255;
			t = si;
			//t = S[i];
			
			S.Position = j;
			byte sj = S.ReadByte();
			S.Position = i;
			S.WriteByte( sj );
			
			//S[i] = S[j];
			
			S.Position = j;
			S.WriteByte( (byte)t );
			//S[j] = t;
		}
		this.i=0;
		this.j=0;
		
		S.Position = 0;  // duston
		//==============================================
	}
	
	public byte next()
	{
		int t;
		i = (i+1)&255;
		
		S.Position = i;
		byte si = S.ReadByte();
		
		j = (j+si)&255;
		//j = (j+S[i])&255;
		t = si;
		//t = S[i];
		S.Position = j;
		byte sj = S.ReadByte();
		S.Position = i;
		S.WriteByte( sj );
		//S[i] = S[j];
		S.Position = j;
		S.WriteByte( (byte) t );
		//S[j] = t;
		
		S.Position = i;
		si = S.ReadByte();
		
		S.Position = (t+ si)&255;
		byte retVal = S.ReadByte();
		
		return retVal;
	}

	public uint getBlockSize() {
		return 1;
	}
	
	public void encrypt(ProtoByteArray block, int length = 0)
	{
		uint i = 0;
		
		if ( length == 0 )
			length = (int)block.Length;
		
		while (i < length)
		{
			block.Position = (int)i;
			byte tmp = block.ReadByte();
			block.Position = (int)i;
			tmp ^= next();
			block.WriteByte( tmp );
			
			++i;
			
			//block[i++] ^= next();
		}
	}
	public void decrypt(ProtoByteArray block) {
		encrypt(block); // the beauty of XOR.
	}
	public void dispose() {
		uint i = 0;
		if (S!=null) {
			for (i=0;i<S.Length;i++)
			{
//				S[i] = Random.Range( 0.0f, 256.0f );//    Math.random()*256;
				
				byte randomVal = (byte)Random.Range( 0, 256 );
				S.Position = (int)i;
				S.WriteByte( randomVal );
			}
//			S.Length=0;
			S = null;
		}
		this.i = 0;
		this.j = 0;
//		System.gc();
	}
	public string toString() {
		return "rc4";
	}
	
//	public void getObject():Object{
//		var obj:Object = new Object();
//		obj.i = i;
//		obj.j = j;
//		obj.s = S;
//		return obj;
//	}
}