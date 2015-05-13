using UnityEngine;
using System.Collections;


public class iOSUtility
{
	
	[System.Runtime.InteropServices.DllImport ("__Internal")]
	private static extern void _XcodeLog ( string message );
	
	[System.Runtime.InteropServices.DllImport ("__Internal")]
	private static extern uint _GetFreeMemory ( );
	
	[System.Runtime.InteropServices.DllImport ("__Internal")]
	private static extern uint _GetTotalMemory ( );
	
	[System.Runtime.InteropServices.DllImport ("__Internal")]
	private static extern float _GetTotalDiskSpaceInBytes ( );
	
	[System.Runtime.InteropServices.DllImport ("__Internal")]
	private static extern float _GetFreeDiskSpaceInBytes ( );

	[System.Runtime.InteropServices.DllImport ("__Internal")]
	private static extern bool _IsBattleCharging ( );

	[System.Runtime.InteropServices.DllImport ("__Internal")]
	private static extern float _GetBatteryLevel ( );

	[System.Runtime.InteropServices.DllImport ("__Internal")]
	private static extern float _ExcludeFromBackupUrl ( string url );
	

	public static void XCodeLog ( string message )
	{
		if ( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			_XcodeLog ( message );
		}
	}
	//in Kbytes
	public static uint GetFreeMemory ( )
	{
		if ( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			return _GetFreeMemory ( );
		}
		return 0;
	}
	
	//in Kbytes
	public static uint GetTotalMemory ( )
	{
		if ( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			return _GetTotalMemory ( );
		}
		return 0;
	}
	
	public static float GetTotalDiskSpaceInBytes ( )
	{
		if ( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			return _GetTotalDiskSpaceInBytes ( );
		}
		return 0f;
	}
	
	public static float GetFreeDiskSpaceInBytes ( )
	{
		if ( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			return _GetFreeDiskSpaceInBytes ( );
		}
		return 0f;
	}

	public static int GetBatteryLevel ( )
	{
		if ( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			float level = _GetBatteryLevel();
			return (int)level;
		}
		return 100;
	}

	public static bool IsBattleCharging ( )
	{
		if ( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			return _IsBattleCharging ( );
		}
		return false;
	}

	public static void ExcludeFromBackupUrl( string url )
	{
		if ( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			_ExcludeFromBackupUrl ( url );
		}
	}
	
}
