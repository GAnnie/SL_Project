// **********************************************************************
// Copyright  2013 Baoyugame. All rights reserved.
// File     :  BaoyugameSdk.cs
// Author   : senkay
// Created  : 6/26/2013 5:54:13 PM
// Purpose  : 
// **********************************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

public class BaoyugameSdk
{
    private const string SDK_JAVA_CLASS = "com.baoyugame.android.commons.DeviceUtils";

    private const string SDK_JAVA_ASSETSUTILS = "com.baoyugame.android.commons.AssetsUtils";

	public static int batteryLevelOfAndroid = 100;
	public static bool batteryChargingOfAndroid = false;

	//return  0-100
	public static int GetBatteryLevel()
	{
		#if (UNITY_EDITOR || UNITY_STANDALONE)
		return 100;
		#elif UNITY_ANDROID
		return batteryLevelOfAndroid;
		#elif UNITY_IPHONE
		return iOSUtility.GetBatteryLevel ( );
		#else
		return 1f;
		#endif
	}

	public static bool IsBattleCharging()
	{
		#if (UNITY_EDITOR || UNITY_STANDALONE)
		return false;
		#elif UNITY_ANDROID
		return batteryChargingOfAndroid;
		#elif UNITY_IPHONE
		return iOSUtility.IsBattleCharging ( );
		#else
		return false;
		#endif
	}

    public static void RegisterPower()
    {
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return;
#elif UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            cls.CallStatic("RegisterPower");
        }
#else
        return;
#endif
    }

    public static void UnregisterPower()
    {
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return;
#elif UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            cls.CallStatic("UnregisterPower");
        }
#else
        return;
#endif
    }

    public static long getFreeMemory()
    {
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return 0;
#elif UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            return cls.CallStatic<long>("getFreeMemory");
        }
#elif UNITY_IPHONE
		return (long)iOSUtility.GetFreeMemory ( );
#else
        return 0;
#endif
    }

    public static long getTotalMemory()
    {
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return 0;
#elif UNITY_ANDROID
		 using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            return cls.CallStatic<long>("getTotalMemory");
        }
#elif UNITY_IPHONE
		return (long)iOSUtility.GetTotalMemory ( );
#else
		return 0;
#endif
    }

    public static long getExternalStorageAvailable()
    {
		GameDebuger.Log("Unity3D getExternalStorageAvailable calling...");
#if (UNITY_EDITOR || UNITY_STANDALONE)
		return 0;
#elif UNITY_ANDROID
		using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
		{
			return cls.CallStatic<long>("getExternalStorageAvailable");
		}
#elif UNITY_IPHONE
		long freeDisk = (long)iOSUtility.GetFreeDiskSpaceInBytes ( );
		return freeDisk >> 10;
#else
		return 0;
#endif
    }

    /**
     * <pre>
     * 获取网络类型
     * 无网络: NONE
     * 未知类型: UNKNOWN
     * WIFI: WIFI
     * 2G: 2G
     * 3G: 3G
     * 4G: 4G
     * </pre>
     * @return 网络类型标识
     */
    public static string getNetworkType()
    {
        GameDebuger.Log("Unity3D getNetworkType calling...");
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return "WIFI";
#elif UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            return cls.CallStatic<string>("getNetworkType");
        }
#elif UNITY_IPHONE
		NetworkReachability ability =  Application.internetReachability;
		
		if( ability ==  NetworkReachability.NotReachable )
		{
			return "NONE";
		}
		else if( ability == NetworkReachability.ReachableViaLocalAreaNetwork)
		{
			return "WIFI";
		}
		else
		{
			return "3G";
		}
#else
        return "WIFI";
#endif
    }

    public static void RegisterGsmSignalStrength()
	{
#if (UNITY_EDITOR || UNITY_STANDALONE)
		return;
#elif UNITY_ANDROID
		using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
		{
			cls.CallStatic("RegisterGsmSignalStrength");
		}
#elif UNITY_IPHONE
		return;
#else
		return;
#endif
    }

    public static void UnregisterGsmSignalStrength()
    {
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return;
#elif UNITY_ANDROID
		using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
		{
			cls.CallStatic("UnregisterGsmSignalStrength");
		}
#elif UNITY_IPHONE
		return;
#else
		return;
#endif
    }

	public static int getWifiSignal()
	{
#if (UNITY_EDITOR || UNITY_STANDALONE)
		return 100;
#elif UNITY_ANDROID
		using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
		{
			return cls.CallStatic<int>("getWifiSignal");
		}
#elif UNITY_IPHONE
		return 100;
#else
		return 100;
#endif
	}

	/**
     * 获取本机网卡Mac地址
     * @return
     */
    public static string getLocalMacAddress()
    {
        GameDebuger.Log("Unity3D getLocalMacAddress calling.....");
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return "111-11-11-111";
#elif UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            return cls.CallStatic<string>("getLocalMacAddress");
        }
#else
        return "111-11-11-111";
#endif
    }
	
    /**
     * 获取手机号码
     * @return
     */
    public static string getLocalPhoneNumber()
    {
        GameDebuger.Log("Unity3D getLocalPhoneNumber calling.....");
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return "111111";
#elif UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            return cls.CallStatic<string>("getLocalPhoneNumber");
        }
#else
        return "111111";
#endif
    }


    /**
     * 取sdcard容量与本机容量, 返回字符串(sdcard容量|手机容量)
     * @return
     */
    public static string getStorageInfos()
    {
		GameDebuger.Log("Unity3D getStorageInfos calling.....");
#if (UNITY_EDITOR || UNITY_STANDALONE)
		return "1|1";
		
#elif UNITY_ANDROID
		using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
		{
			return cls.CallStatic<string>("getStorageInfos");
		}
#elif UNITY_IPHONE
		long freeDisk = (long)iOSUtility.GetFreeDiskSpaceInBytes ( );
		freeDisk >>= 10;
		return "0|" + freeDisk.ToString( );
#else
		return "1|1";
#endif
    }


    	
	/**
	 * 判断是否带sdcard
	 * @return
	 */
    public static bool hasExternalStorage()
    {
        GameDebuger.Log("Unity3D hasExternalStorage..");
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return false;

#elif UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            return cls.CallStatic<bool>("externalStorageAvailable");
            //return cls.CallStatic<bool>("hasExternalStorage");
        }
#else
        return false;
#endif
    }
      

    /**
         * 显示安装未签名app的设置窗口
         */
    public static void showSettingsInstallNonMarketApps()
    {
        GameDebuger.Log("Unity3D showSettingsInstallNonMarketApps calling.....");

#if (UNITY_EDITOR || UNITY_STANDALONE)
#elif UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            cls.CallStatic("showSettingsInstallNonMarketApps");
        }
#endif
    }


	/**
	 * 是否可安装未签名APP
	 * @return
	 * 			0 不可安装
	 * 			1可安装
	 */
    public static int getOptionInstallNonMarketApps()
    {
        GameDebuger.Log("Unity3D getOptionInstallNonMarketApps calling.....");
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return 0;

#elif UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            return cls.CallStatic<int>("getOptionInstallNonMarketApps");
        }
#else
        return 0;
#endif
    }

/**
     * 取得生成应用的唯一串,返回字符串(新生成标记|UUID)
     * 新生成标记: 1首次生成, 0读取缓存的
     * @return
     */
    public static string getUUID()
    {
        GameDebuger.Log("Unity3D getUUID calling.....");
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return "1|111-11-11-115";
#elif UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            return cls.CallStatic<string>("getUUID");
        }
#elif UNITY_IPHONE
		return "1|"+SystemInfo.deviceUniqueIdentifier;
#else
       return "1|111-11-11-111";
#endif
    }

    /**
	 * 把asset复制成另一个目录文件
	 * @param assetName 资源名字
	 * @param toFilePath 文件全路径
	 * @param decompresses 是否解压(true解压,false正常读取)
	 * @return
	 */
    public static bool copyAssetAs(String assetName, String toFilePath, bool decompresses)
    {
#if UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_ASSETSUTILS))
        {
            return cls.CallStatic<bool>("copyAssetAs", assetName, toFilePath, decompresses);
        }
#endif
		return false;
    }
	
	
	
	/**
	 * 复制资源的批量接口
	 * assetName|toFilePath|decompresses,...
	 * @param assets
	 * @return
	 */
	public static void copyAssetAs(string assets) 
	{
#if UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_ASSETSUTILS))
        {
           cls.CallStatic("copyAssetAs", assets);
        }		
#endif 
	}
	

    /**
         * 获取Assets 目录下的资源信息
         * @return
         */
    public static byte[] readBytes(string assetPath)
    {
#if UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_ASSETSUTILS))
        {
            return cls.CallStatic<byte[]>("readBytes", assetPath);
        }
#endif
		return null;
    }


	/// <summary>
	/// Installs the apk.
	/// </summary>
	/// <param name='apkUrl'>
	/// Apk URL.
	/// </param>
	public static void InstallApk(string apkUrl)
	{
#if (UNITY_EDITOR || UNITY_STANDALONE)
#elif UNITY_ANDROID
		callSdkApi("installApk", apkUrl);
#else
#endif
    }
	
	public static void RestartGame()
	{
#if (UNITY_EDITOR || UNITY_STANDALONE)
#elif UNITY_ANDROID
		callSdkApi("RestartGame");
#else
#endif
    }

    /** 
    * 判断是否开启了自动亮度调节 
    */
    public static bool isAutoBrightness()
    {
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return false;
#elif UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            return cls.CallStatic<bool>("isAutoBrightness");
        }
#else
        return false;
#endif
    }

    /** 
     * 关闭亮度自动调节 
     */
    public static void stopAutoBrightness()
    {
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return;
#elif UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            cls.CallStatic("stopAutoBrightness");
        }
#else
        return;
#endif
    }

    /** 
     * 开启亮度自动调节 
     */
    public static void startAutoBrightness()
    {
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return;
#elif UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            cls.CallStatic("startAutoBrightness");
        }
#else
        return;
#endif
    }
    /**
     * 获取当前屏幕亮度
     */
    public static int getScreenBrightness()
    {
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return 255;
#elif UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            return cls.CallStatic<int>("getScreenBrightness");
        }
#else
        return 0;
#endif
    }

    /**
     * 省电模式,设置亮度
     */
    public static void setBrightness(int brightness)
    {
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return;
#elif UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            cls.CallStatic("setBrightness",brightness);
        }
#else
        return;
#endif
    }

    /**
     * 读取metadata
     * @return
     */
    public static string getMetaData(string name)
    {
        GameDebuger.Log("Unity3D getMetaData calling.....");
#if (UNITY_EDITOR || UNITY_STANDALONE)
        return "";

#elif UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_ASSETSUTILS))
        {
            return cls.CallStatic<string>("getMetaData", name);
        }
#else
        return "";
#endif
    }		
	
	private static void callSdkApi(string apiName, params object[] args)
	{
		GameDebuger.Log("Unity3D " + apiName + " calling...");

#if UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            cls.CallStatic(apiName, args);
        }
#endif
	}	
}
