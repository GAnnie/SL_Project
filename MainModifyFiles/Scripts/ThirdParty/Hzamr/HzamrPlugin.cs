using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

public class HzamrPlugin 
{

#if UNITY_EDITOR
	const string HZAMRDLL = "hzamr";
#elif UNITY_IPHONE 
		const string HZAMRDLL = "__Internal"; 
#else 
		const string HZAMRDLL = "hzamr"; 
#endif

    [DllImport(HZAMRDLL)]
	private static extern IntPtr PrintHello();

    [DllImport(HZAMRDLL)]
	private static extern uint wav_to_amr(byte[] wavBuf,int wavBufSize,byte[] amrBuf);

    [DllImport(HZAMRDLL, CharSet = CharSet.Ansi)]
	private static extern int amr_to_wav_file(StringBuilder fileName,byte[] amrBuf,long amrBufSize);

	static public void TestDll()
	{
		GameDebuger.Log ("TestDll");

		TipManager.AddTip(Marshal.PtrToStringAuto (PrintHello()));

		TipManager.AddTip("HzarmPlugin: " + Application.persistentDataPath);
	}

	// Use this for initialization
	static public void TestSend()
	{
		Debug.Log(Marshal.PtrToStringAuto (PrintHello()));
		Debug.Log("HzarmPlugin: " + Application.persistentDataPath);

		byte[] wavFileBuf;
		byte[] amrBuf;
		uint amrSize;

		string path = Application.dataPath + "/res/";
		string wavFile = path + "test1.wav";
		string amrFile = path + "test1.amr";
		
		wavFileBuf = File.ReadAllBytes(wavFile);
		//byte[] wavBuf = new byte[wavFileBuf.Length - 44];
		//Array.Copy(wavFileBuf,44,wavBuf,0,wavBuf.Length);
		
		amrBuf = new byte[wavFileBuf.Length];
		amrSize = HzamrPlugin.WavToAmr(wavFileBuf,wavFileBuf.Length,amrBuf);
		
		FileStream fileStream = new FileStream (amrFile, FileMode.Create, FileAccess.Write, FileShare.Read);
		fileStream.Write (amrBuf, 0, (int)amrSize);

		if(amrSize > 0)
		{
			VoiceRecognitionManager.Instance.SaveVoiceToQiniu(amrBuf,(int)amrSize,"test1");

			VoiceRecognitionManager.Instance.GetVoiceText(amrBuf,(int)amrSize,delegate(string obj) {
				string frontStr = obj.Substring( 0 , obj.Length -1 );
				string lastChar = obj.Substring( obj.Length -1 , 1 );
				if( lastChar.Equals("，") ) lastChar = "";
				obj = frontStr + lastChar;
				string baiduMsg = string.Format( "#*#{0}#*#{1}",111 , obj );
				string tempMsg= ChatModel.Instance.PieceTypeAndContent( ChatMessageFunctionType.VoiceTalk , baiduMsg );
				Debug.Log("语音:" + tempMsg);
			});
		}
	}

	static public void TestGet(string name)
	{
        GameDebuger.Log("TestGet");
		VoiceRecognitionManager.Instance.GetVoiceInQiniu(name,delegate(AudioClip clip) {
			VoiceRecognitionManager.Instance.PlayQiniuSoundByClip(clip);
		});
	}

	public static uint WavToAmr(byte[] wavBuf,int wavBufSize,byte[] amrBuf)
	{
		return wav_to_amr(wavBuf,wavBufSize,amrBuf);
	}

	public static int AmrToWavFile(string fileName,byte[] amrBuf,long amrBufSize)
	{
		return amr_to_wav_file(new StringBuilder(fileName, fileName.Length), amrBuf, amrBufSize);
	}
}
