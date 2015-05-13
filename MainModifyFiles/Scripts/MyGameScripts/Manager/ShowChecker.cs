using UnityEngine;
using System.Collections;
using System;

[ExecuteInEditMode]
[RequireComponent( typeof( MeshRenderer))]
public class ShowChecker : MonoBehaviour 
{
	//当前时间
	private float _curtTime = 0.0f;
	private float _lastTime = 0.0f;

	private GameObject _showGO = null;

	// Update is called once per frame
	void Update () 
	{
		_lastTime = Time.time;

		//以20FPS来计算是否切实不在摄像机里面
		isRealRendering = (( _lastTime - _curtTime ) < 0.05f);

		//如果最新时间和渲染时间大于1.0秒， 则表示已经不在摄像机里面
		if( _lastTime - _curtTime > 4.0f )
		{
			if( isRendering )
			{
				isRendering = false;
				GameObjectDisplayManager.Instance.DeleteShowCommandItem( this, _checkerType );
			}
		}
		else
		{
			if(  !isRendering )
			{
				isRendering = true;
				GameObjectDisplayManager.Instance.AddShowCommandItem( this, _checkerType );
			}
		}
	}

	void OnDestroy()
	{
		GameObjectDisplayManager.Instance.DeleteFromList( this, _checkerType );
	}

	void OnWillRenderObject()
	{
		_curtTime=Time.time;
	}

	private const string defaultType = "hero";
	private string _checkerType = defaultType;
	private Action< GameObject > newCallBack = null;
	private Action< GameObject > deleteCallBack = null;
	public void Setup( Action< GameObject > newCallBack  , Action< GameObject >  deleteCallBack, string checkerType = defaultType)
	{
		_checkerType = checkerType;

		this.newCallBack = newCallBack;
		this.deleteCallBack = deleteCallBack;
	}

	//表示checker是否在摄像机里面，  和isRealRendering不同， 判断不在摄像机里面的条件是相隔4秒
	public bool isRendering {  get ; private set;}
	//表示checker是否在摄像机里面的真实情况
	public bool isRealRendering{get; private set;}
	//当前物体是否显示主渲染提
	public bool isShowObj { get; private set;}

	/// <summary>
	/// 生成显示的物体
	/// </summary>
	public void ShowObj()
	{
		if( isShowObj )
		{
			return;
		}

		isShowObj = true;
		//加载物体

		//这里会存在一个问题， 如果checker之前存在于显示列表， 由于又一个checker隐藏后才回调这个方法， 但是当前checker已经不在摄像机
		//这是如果回调了newCallback的时候， checker立马调用deleteCallBack, 
		//因为资源是异步加载， 这样有可能会导致资源加载的错误， 如果如果出现调用此方法时， 已经不在摄像机里面的时候， 主动增加1.0s来防止
		//上面的现象

		if( !isRealRendering )
		{
			_curtTime += 1.0f;
		}


		if( newCallBack != null )
		{
			newCallBack( this.gameObject );
		}

	}

	/// <summary>
	/// 删除显示的物体
	/// </summary>
	public void HideObj()
	{
		if( ! isShowObj)
		{
			return;
		}

		isShowObj = false;

		//卸载物体
		if( deleteCallBack != null )
		{
			deleteCallBack( this.gameObject);
		}
	}

	public void Reset()
	{
		isShowObj = false;
	}

	public void RemoveChecker()
	{
		isRendering = false;
		GameObjectDisplayManager.Instance.DeleteShowCommandItem( this, _checkerType );
	}
}
