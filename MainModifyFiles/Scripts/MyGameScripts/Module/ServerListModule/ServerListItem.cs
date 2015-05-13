using UnityEngine;
using System.Collections;

public class ServerListItem : MonoBehaviour
{
    //    //平时状态
    //    private static string COMMONE_STATE_BG = "common_state";
    //
    //    //选中状态
    //    private static string SELECT_STATE_BG = "selecting_state";

//    private static string MAN_STATE = "man";
//
//    private static string WOMAN_STATE = "woman";

    //服务器名字
    public UILabel serverNameLabel = null;

    //服务器最高人物等级
    public UISprite portrait = null;

    //服务器信息
    private ServerInfo _serverMessage = null;

    public UISprite _backgroudSprite = null;

    private bool _isSelect = false;

    //服务器回调信息
    public delegate void GetServerMessageFunc(ServerListItem item);
    private GetServerMessageFunc _callBackFunc = null;


    void Awake()
    {
//        HidePlayerMessage();
    }



    // Use this for initialization
    //void Start()
    //{

    //}

    public void Setup(ServerInfo message, GetServerMessageFunc func)
    {
        this._serverMessage = message;

        this._callBackFunc = func;

        InitServerMessage();
    }

    public void SetupServerPlayerMessage(ServerPlayerMessage message)
    {
        //Debug.Log(" ----- SetupServerPlayerMessage ----- ");
        //Debug.Log( message );

        //Debug.Log(_serverMessage.name + "/" + _serverMessage.serviceId );
        if (message != null)
        {
            if (portrait != null)
            {
//                if (message.gender == 1)
//                {
//                    portrait.spriteName = MAN_STATE;
//                }
//                else
//                {
//                    portrait.spriteName = WOMAN_STATE;
//                }
//                portrait.gameObject.SetActive(true);
            }

//            if (levelLabel != null)
//            {
//                levelLabel.gameObject.SetActive(true);
//                levelLabel.text = string.Format("LV.{0}", message.grade.ToString());
//            }
        }


    }

    public void HidePlayerMessage()
    {
////        if (levelLabel != null)
////            levelLabel.gameObject.SetActive(false);
//
//        if (portrait != null)
//            portrait.gameObject.SetActive(false);
    }
	
	private Color _selectColor = new Color(167/256f,112/256f,36/256f,255/256f);
    /// <summary>
    /// 选择服务器
    /// </summary>
    public void SelectServer()
    {
        //维护状态的服务器不给选择
        if (_serverMessage.dboState == 2)
        {
            return;
        }

        if (_isSelect) return;
        _isSelect = true;
 

		_backgroudSprite.color = _selectColor;
    }

    /// <summary>
    /// 不选中状态
    /// </summary>
    public void UnSelectServer()
    {
        _isSelect = false;
		_backgroudSprite.color = Color.white;
    }

	public void OnListItemClick()
	{
		if (_callBackFunc != null && _serverMessage != null)
		{
			_callBackFunc(this);
		}
	}

	public ServerInfo GetServerInfo()
	{
		return _serverMessage;
	}

    /// <summary>
    /// 初始化服务器项
    /// </summary>
    private void InitServerMessage()
    {
        if (_serverMessage == null) return;

        if (serverNameLabel != null)
        {
            serverNameLabel.text = ServerNameGetter.GetServerName(_serverMessage);
			portrait.spriteName  = ServerNameGetter.GetServiceStateSpriteName( _serverMessage );
        }

    }
}
