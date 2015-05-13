// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  WindowManager.cs
// Author   : willson
// Created  : 2013/3/4 
// Porpuse  : 
// **********************************************************************
using System;
using System.Collections.Generic;

using UnityEngine;

public class WindowManager
{
    public static bool isToolUse = false;
	public delegate void CallBackOkHandler();
	public delegate void CallBackCanelHandler();
	public delegate void CallBackCloseHandler();
    public delegate void CallBackIsChecked(bool b);
	public delegate void CallBackIsChecked_2( bool b );

    public enum WINDOW_LEVEL
    {
		TOP_LEVEL    = 10, //topLevel在CloseAllTheConfirmViewExceptTopLevel的时候不会主动关闭,请慎用
        MIDDLE_LEVEL = 7,
        NORMAL_LEVEL = 5,
        LOW_LEVEL    = 1,
    }


    //private static GameObject _view = null;
    private static int MAX_WIN_NUMBER = 3;
    private static List<GameTipsWindow> _viewList = null;

    private class GameTipsWindow
    {
        public GameObject view = null;

        public WINDOW_LEVEL level = 0;
		
		public int windowHandleID { get; private set; }

        public GameTipsWindow(GameObject view, WINDOW_LEVEL level)
        {
            this.view  = view;
			this.windowHandleID = view.GetHashCode();
            this.level = level; 
        }

        public void SetActive(bool flag)
        {
            if (view != null)
            {
                view.SetActive(flag);
            }
        }

        public void Destroy()
        {
            if (view != null)
            {
                GameObject.Destroy(view);
                view = null;
            }
        }
    }

	public WindowManager()
	{
	}

//    static public int openGoldWindow(string msg, 
//                                     string btnOKLabel = null,
//                                     int btnOKCost = 0,
//                                     System.Action<int> callBack = null, 
//                                     string btnCancelLabel = null,
//                                     int btnCancelCost = 0,
//                                     System.Action<int> callBackCanel = null, 
//                                     WINDOW_LEVEL level = WINDOW_LEVEL.NORMAL_LEVEL)
//	{
//		
//		int windowHandleID = 0;
//		
//	 	GameObject view = WindowManager.Open("Prefabs/window/WindowGoldPrefab", ref windowHandleID ,  level );
//
//        WindowGoldView windowWiew = view.GetComponentInChildren<WindowGoldView>();
//        windowWiew.Open(view, msg, btnOKLabel, btnOKCost, callBack, btnCancelLabel, btnCancelCost, callBackCanel, _CloseView);
//
//        _updateWinLevel();
//		
//		return windowHandleID;
//	}

//    static public int openConfirmWindowUserDefault(
//				
//                string msg,
//                string btnOKLabel = null,
//                CallBackOkHandler callBack = null,
//                string btnCancelLabel = null,
//                CallBackCanelHandler callBackCanel = null,
//                WINDOW_LEVEL level = WINDOW_LEVEL.NORMAL_LEVEL,
//				bool isThreeBtnCount = false,
//				CallBackCloseHandler callBackClose = null
//            )
//    {
//		
//        btnOKLabel = "确定";
//        if (btnCancelLabel == null)
//        {
//			btnCancelLabel = "取消";
//		}
//		GameObject view = null;
//		
//		int windowHandleID = 0;
//		if(isThreeBtnCount == false)
//		{
//			view = WindowManager.Open("Prefabs/window/WindowPrefab",ref windowHandleID , level);
//		}
//       	else
//		{
//			view = WindowManager.Open("Prefabs/window/WindowPrefabThreeBtn" ,ref windowHandleID,level);
//		}
//		
//        WindowView windowWiew = view.GetComponentInChildren<WindowView>();
//        windowWiew.Open(view, msg, callBack, callBackCanel, delegate(bool onlyClosed){
//			_CloseView();
//			if (onlyClosed && callBackClose != null){
//				callBackClose();
//				callBackClose = null;
//			}
//		});
//
//		windowWiew.okLabel.text = "[b]"+btnOKLabel;
//		windowWiew.cancelLabel.text = "[b]"+btnCancelLabel;
//
//        _updateWinLevel();
//		return windowHandleID;
//    }
	
//	static public int openConfirmWindowScrollView(string title,
//                string msg,
//                string btnOKLabel = null,
//                CallBackOkHandler callBack = null,
//                string btnCancelLabel = null,
//                CallBackCanelHandler callBackCanel = null,
//                WINDOW_LEVEL level = WINDOW_LEVEL.NORMAL_LEVEL,
//				CallBackCloseHandler callBackClose = null)
//	{
//		 btnOKLabel = "确定";
//        if (btnCancelLabel == null)
//        {
//            btnCancelLabel = "取消";
//        }
//		GameObject view = null;
//		
//		int windowHandleID = 0;
//
//		view = WindowManager.Open("Prefabs/window/WindowPrefabScrollView",ref windowHandleID , level);
//        WindowView windowWiew = view.GetComponentInChildren<WindowView>();
//        windowWiew.Open(view, msg, callBack, callBackCanel, delegate(bool onlyClosed){
//			_CloseView();
//			if (onlyClosed && callBackClose != null){
//				callBackClose();
//				callBackClose = null;
//			}
//		});
//	
//        windowWiew.okLabel.text = btnOKLabel;
//        windowWiew.cancelLabel.text = btnCancelLabel;
//
//        _updateWinLevel();
//		return windowHandleID;
//	}

//	static public int openConfirmWindowUserDefault(
//					string title
//					string msg, 
//					string btnOKLabel = null,
//					CallBackOkHandler callBack = null,
//					string btnCancelLabel = null,
//					CallBackCanelHandler callBackCanel = null,
//                    WINDOW_LEVEL level = WINDOW_LEVEL.NORMAL_LEVEL,
//					bool isThreeBtnCount = false,
//					CallBackCloseHandler callBackClose = null
//				)
//	{
//        return openConfirmWindowUserDefault(
//                                      msg,
//                                      btnOKLabel,
//                                      callBack,
//                                      btnCancelLabel,
//                                      callBackCanel,
//                                      level,
//									  isThreeBtnCount,
//									  callBackClose	);
//	}


//    static public int openConfirmCheckBoxWindowUserDefault(
//                string title,
//                string msg,
//                string btnOKLabel = null,
//                CallBackOkHandler callBack = null,
//                string btnCancelLabel = null,
//                CallBackCanelHandler callBackCanel = null,
//                string checkboxLabel = null,
//                CallBackIsChecked callBackIsChecked = null,
//                WINDOW_LEVEL level = WINDOW_LEVEL.NORMAL_LEVEL
//            )
//    {
//       	btnOKLabel = "确定";
//        if (btnCancelLabel == null)
//        {
//            btnCancelLabel = "取消";
//        }
//		
//		int windowHandleID = 0;
//		
//        GameObject view = WindowManager.Open("Prefabs/window/WindowCheckBoxPrefab",ref windowHandleID, level);
//
//        WindowCheckBoxView windowWiew = view.GetComponentInChildren<WindowCheckBoxView>();
//        windowWiew.Open(view, msg, callBack, callBackCanel, callBackIsChecked, ClostAllTheConfirmView);
//
//        windowWiew.titleLabel.text = title;
//        windowWiew.okLabel.text = btnOKLabel;
//        windowWiew.cancelLabel.text = btnCancelLabel;
//
//        if (string.IsNullOrEmpty(checkboxLabel) == false)
//        {
//            windowWiew.checkbox.gameObject.SetActive(true);
//            windowWiew.checkboxLabel.text = checkboxLabel;
//        }
//        else
//        {
//            windowWiew.checkbox.gameObject.SetActive(false);
//        }
//
//        _updateWinLevel();
//		
//		return windowHandleID;
//    }

//    static public int openConfirmCheckBoxWindowUserDefault(
//                string msg,
//                string btnOKLabel = null,
//                CallBackOkHandler callBack = null,
//                string btnCancelLabel = null,
//                CallBackCanelHandler callBackCanel = null,
//                string checkboxLabel = null,
//                CallBackIsChecked callBackIsChecked = null,
//                WINDOW_LEVEL level = WINDOW_LEVEL.NORMAL_LEVEL
//            )
//    {
//        return openConfirmCheckBoxWindowUserDefault(string.Empty,
//                                      msg,
//                                      btnOKLabel,
//                                      callBack,
//                                      btnCancelLabel,
//                                      callBackCanel,
//                                      checkboxLabel,
//                                      callBackIsChecked,
//                                      level);
//    }
	
	
//	static public int OpenConfirmWithTwoCheckBoxWindowUserDefault(
//		string title_,
//		string content_,
//		string sureBtnStr_ = null,
//		CallBackOkHandler callBack = null,
//		string checkBox_1 = null,
//		CallBackIsChecked  callBackIsChecked = null,
//		string checkBox_2 = null,
//		CallBackIsChecked_2 callBackIsChecked_2 = null,
//		WINDOW_LEVEL level = WINDOW_LEVEL.NORMAL_LEVEL
//		)
//	{
//		sureBtnStr_ = "确定";
//		
//		int windowHandleID = 0;
//		
//        GameObject view = WindowManager.Open("Prefabs/window/WindowWithTwoCheckBoxPrefab",ref windowHandleID, level);
//
//        WindowWithTwoCheckBoxView windowWiew = view.GetComponentInChildren<WindowWithTwoCheckBoxView>();
//        windowWiew.SetData(title_, content_, sureBtnStr_, callBack, checkBox_1 , callBackIsChecked ,checkBox_2  ,callBackIsChecked_2  , ClostAllTheConfirmView);
//
//        _updateWinLevel();
//		
//		return windowHandleID;
//		
//	}
	

	static public int showMessageBox(string msg, Action callBack = null, bool isShowCloseBtn = true, WINDOW_LEVEL level = WINDOW_LEVEL.NORMAL_LEVEL, string btnOKLabel = null)
	{
        if (!isToolUse)
        {
			return openRoundWindowUserDefault( msg, callBack, btnOKLabel, isShowCloseBtn,level);
        }
		
		return 0;
	}

	static public int openRoundWindowUserDefault(
					
					string msg, 
					Action callBack = null,
					string btnOKLabel = null,
                    bool isShowCloseBtn = true,
                    WINDOW_LEVEL level = WINDOW_LEVEL.NORMAL_LEVEL,
					bool isShowOkBtn = true
				)
	{
		int windowHandleID = 0;
		ProxyWindowModule.OpenMessageWindow (msg, "", callBack,UIWidget.Pivot.Center, btnOKLabel);

        //_updateWinLevel();
		return windowHandleID;
	}

//    static public int openTimerWindowUserDefault(
//            int time,
//            string msg,string timeCompleteMsg,
//            string btnOKLabel = null,
//            System.Action<int> callBack = null,
//            string btnCancelLabel = null,
//            System.Action<int> callBackCanel = null,
//            WINDOW_LEVEL level = WINDOW_LEVEL.NORMAL_LEVEL
//        )
//    {
//
//        btnOKLabel = "确定";
//        if (btnCancelLabel == null)
//        {
//            btnCancelLabel = "取消";
//        }
//        GameObject view = null;
//
//        int windowHandleID = 0;
//
//        view = WindowManager.Open("Prefabs/window/WindowTimerPrefab", ref windowHandleID, level);
//        WindowTimerView windowWiew = view.GetComponentInChildren<WindowTimerView>();
//        windowWiew.Open(view, time, msg, timeCompleteMsg, callBack, callBackCanel, ClostAllTheConfirmView);
//
//        windowWiew.okLabel.text = btnOKLabel;
//        windowWiew.cancelLabel.text = btnCancelLabel;
//
//        _updateWinLevel();
//        return windowHandleID;
//    }

//    static public int showHelpBox(string msg,int offsetX = 0, WINDOW_LEVEL level = WINDOW_LEVEL.NORMAL_LEVEL)
//    {
//        GameObject view = null;
//
//        int windowHandleID = 0;
//
//        view = WindowManager.Open("Prefabs/window/WindowHelpPrefab", ref windowHandleID, level);
//        view.transform.localPosition = new UnityEngine.Vector3(offsetX, view.transform.localPosition.y, view.transform.localPosition.z);
//        WindowHelpView windowWiew = view.GetComponentInChildren<WindowHelpView>();
//        windowWiew.Open(view, msg, ClostAllTheConfirmView);
//
//        _updateWinLevel();
//        return windowHandleID;
//    }

    //-------------------

    public static void CleanAllView()
    {
        if (_viewList != null)
        {
            for (int i = 0; i < _viewList.Count; i++)
            {
                _viewList[i].Destroy();
            }

            _viewList.Clear();
        }
    }


    private static void _updateWinLevel()
    {
        if (_viewList != null)
        {
            for (int i = 0; i < _viewList.Count; i++)
            {
                _viewList[i].SetActive((i == _viewList.Count - 1));
            }
        }
    }

    /// <summary>
    /// 创建提示窗口
    /// </summary>
    /// <param name="moduleName"></param>
    /// <param name="type"></param>
    /// <param name="depth"></param>
    /// <returns></returns>

    private static GameObject Open(string moduleName,ref int windowHandleID, WINDOW_LEVEL level, int depth =UILayerType.Dialogue)
	{
        if ( _viewList == null )
        {
            _viewList = new List<GameTipsWindow>();
        }

        if ( _viewList.Count >= MAX_WIN_NUMBER )
        {
            GameTipsWindow delectView = _viewList[0];
            _viewList.RemoveAt(0);
            delectView.Destroy();
            delectView = null;
        }

        GameTipsWindow gameView = new GameTipsWindow( _Open(moduleName, depth), level );
        _viewList.Add(gameView);

        if (_viewList.Count > 2)
        {
            _viewList.Sort(Compare);
        }
		
		windowHandleID = gameView.windowHandleID;

        return gameView.view;
	}

    private static int Compare(GameTipsWindow x, GameTipsWindow y)
    {
        if ((int)(x.level) > (int)(y.level))      return 1;
        else if ((int)(x.level) < (int)(y.level)) return -1;
        else                                      return 0;
    }

    private static GameObject _Open( string moduleName, int depth = -30 )
    {
        //GameObject returnVeiw = GameObjectExt.AddChild(LayerManager.Instance.getUILayer(), (GameObject)ResourceLoader.Load(moduleName), 0, 0, depth);
//		GameObject returnVeiw = GameObjectExt.AddChild(LayerManager.Instance.cameraUI, (GameObject)ResourceLoader.Load(moduleName), 0, 0, depth);
       
		GameObject winPrefab = ResourcePoolManager.Instance.SpawnUIPrefab( moduleName ) as GameObject;
		GameObject returnVeiw = GameObjectExt.AddChild(LayerManager.Instance.floatTipAnchor, winPrefab, 0, 0, 0);

        //Camera uiCam = LayerManager.Instance.getUICamera();
//        Camera uiCam = LayerManager.Instance.uiCamera;
//        if (uiCam == null)
//            GameDebuger.Log("Can't fetch UICamera");

//        UIAnchor anchor = returnVeiw.GetComponent<UIAnchor>();
//        if (anchor != null)
//        {
//            anchor.uiCamera = uiCam;
//        }
//        else
//        {
//            UIAnchor[] anchors = returnVeiw.GetComponentsInChildren<UIAnchor>();
//
//            foreach (UIAnchor uia in anchors)
//            {
//                if (uia != null)
//                    uia.uiCamera = uiCam;
//            }
//        }

        return returnVeiw;
    }


    private static void _CloseView(bool onlyClose=true)
    {
        if ( _viewList != null && _viewList.Count > 0)
        {
            _viewList[_viewList.Count - 1].Destroy();
            _viewList.RemoveAt(_viewList.Count - 1);

            if (_viewList.Count > 0)
            {
                _viewList[_viewList.Count - 1].SetActive(true);
            }
        }
    }
	
	public static void ClostAllTheConfirmView()
	{
		_CloseView();
	}

	public static void CloseAllTheConfirmViewExceptTopLevel()
	{
		if ( _viewList != null && _viewList.Count > 0)
		{
			//不关闭topLevel的窗口
			if (_viewList[_viewList.Count - 1].level == WINDOW_LEVEL.TOP_LEVEL){
				return;
			}

			_CloseView();
		}
	}

	public static void CloseViewByWindowHandleID(int handleId)
	{
		if ( _viewList != null && _viewList.Count > 0 )
		{
			int foundIndex = -1;
			
			for ( int i = 0 , imax = _viewList.Count ; i < imax ; ++i )
			{
				if ( _viewList[i].windowHandleID == handleId )
				{
					foundIndex = i;
					break;
				}
			}
			
			
			if ( foundIndex != -1 )
			{
				
				_viewList[foundIndex].Destroy ( );
				
				_viewList.RemoveAt (foundIndex);
				
				
				if ( foundIndex > 0 && foundIndex == _viewList.Count )
				{
					
					_viewList[foundIndex-1].SetActive(true);
					
				}
				
				
			}
			
		}
	}
}


