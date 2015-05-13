//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using System.Collections;

/// <summary>
/// Generates a safe wrapper for FriendView.
/// </summary>
public class FriendView : BaseView
{
	public UIInput ChatInputLbl;
	public GameObject ChatBtnGroupObj;
	public UITable ItemTable;
	public GameObject RightPanelObj;
	public GameObject LeftPanelObj;
	public UIGrid TypeTabBtnGrid;
	public UIButton GiftBtn;
	public UIButton HomeBtn;
	public UIButton AddFriendBtn;
	public GameObject FunctionBtnsObj;
	public UIGrid itemGrid;
	public GameObject TipsObj;
	public UILabel TipsLbl;
	public UISprite TitleSprite;
	public UIButton FaceBtn;
	public GameObject RelationObj;
	public UILabel RelationShipLbl;
	public UIButton addBtn;
	public UILabel FriendlyDegreeLbl;
	public GameObject EmailObj;
	public UILabel EmailContentLbl;
	public UIButton CloseBtn;
	public GameObject ChatContentObj;
	public UIGrid RewardItemGrid;
	public UIButton RewardBtn;
	public GameObject NoMailTextObj;
	public UILabel WarmLbl;
	public GameObject RewardSplitObj;
	public GameObject NoFriendTextObj;
	public UILabel FriendWarmLbl;
	public UISprite tipSpriteObj;
	public GameObject AddFriendsObj;
	public UIButton ChangeBtn;
	public UIInput InputLbl;
	public UIButton SeachBtn;
	public UIButton CloseAddFriendViewBtn;
	public UIScrollView RewardScrollView;
	public UISprite SpeedOrInputBg;
	public UIEventListener SpeedBtn;
	public UIButton SpeedOrInputBtn;
	public UIButton SendBtn;
	public GameObject SpeechAnchorObj;
	public GameObject SpeechObj;
	public UISprite EnergyBar;
	public GameObject GiveUpObj;
	public GameObject ShortVoiceObj;
	public UILabel VoiceTipLbl;
	public UITable RecommendFriendsTable;

	public void Setup (Transform root)
	{
		ChatInputLbl = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/ChatBtnGroup/ChatInput").GetComponent<UIInput>();
		ChatBtnGroupObj = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/ChatBtnGroup").gameObject;
		ItemTable = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/ItemScrollView/ItemTable").GetComponent<UITable>();
		RightPanelObj = root.Find("ContentFrame/ContentBg/RightPanel").gameObject;
		LeftPanelObj = root.Find("ContentFrame/ContentBg/LeftPanel").gameObject;
		TypeTabBtnGrid = root.Find("ContentFrame/ContentBg/LeftPanel/TypeTabBtnGrid").GetComponent<UIGrid>();
		GiftBtn = root.Find("ContentFrame/ContentBg/LeftPanel/FunctionBtns/GiftBtn").GetComponent<UIButton>();
		HomeBtn = root.Find("ContentFrame/ContentBg/LeftPanel/FunctionBtns/HomeBtn").GetComponent<UIButton>();
		AddFriendBtn = root.Find("ContentFrame/ContentBg/LeftPanel/FunctionBtns/AddFriendBtn").GetComponent<UIButton>();
		FunctionBtnsObj = root.Find("ContentFrame/ContentBg/LeftPanel/FunctionBtns").gameObject;
		itemGrid = root.Find("ContentFrame/ContentBg/LeftPanel/ItemScrollView/itemGrid").GetComponent<UIGrid>();
		TipsObj = root.Find("ContentFrame/ContentBg/Tips").gameObject;
		TipsLbl = root.Find("ContentFrame/ContentBg/Tips/Label").GetComponent<UILabel>();
		TitleSprite = root.Find("ContentFrame/TitleSprite").GetComponent<UISprite>();
		FaceBtn = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/ChatBtnGroup/FaceBtn").GetComponent<UIButton>();
		RelationObj = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/ChatBtnGroup/Relation").gameObject;
		RelationShipLbl = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/ChatBtnGroup/Relation/RelationShip").GetComponent<UILabel>();
		addBtn = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/ChatBtnGroup/Relation/add").GetComponent<UIButton>();
		FriendlyDegreeLbl = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/ChatBtnGroup/Relation/FriendlyDegree").GetComponent<UILabel>();
		EmailObj = root.Find("ContentFrame/ContentBg/RightPanel/Email").gameObject;
		EmailContentLbl = root.Find("ContentFrame/ContentBg/RightPanel/Email/EmailContent/ContentGroup/scrollView/EmailContentLbl").GetComponent<UILabel>();
		CloseBtn = root.Find("ContentFrame/CloseBtn").GetComponent<UIButton>();
		ChatContentObj = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent").gameObject;
		RewardItemGrid = root.Find("ContentFrame/ContentBg/RightPanel/Email/EmailContent/RewardGroup/scrollView/itemGrid").GetComponent<UIGrid>();
		RewardBtn = root.Find("ContentFrame/ContentBg/RightPanel/Email/EmailContent/RewardGroup/RewardBtn").GetComponent<UIButton>();
		NoMailTextObj = root.Find("ContentFrame/ContentBg/RightPanel/Email/NoMailText").gameObject;
		WarmLbl = root.Find("ContentFrame/ContentBg/RightPanel/Email/NoMailText/WarmLbl").GetComponent<UILabel>();
		RewardSplitObj = root.Find("ContentFrame/ContentBg/RightPanel/Email/RewardSplit").gameObject;
		NoFriendTextObj = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/NoFriendText").gameObject;
		FriendWarmLbl = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/NoFriendText/WarmLbl").GetComponent<UILabel>();
		tipSpriteObj = root.Find("ContentFrame/ContentBg/Tips/tip").GetComponent<UISprite>();
		AddFriendsObj = root.Find("ContentFrame/ContentBg/AddFriends").gameObject;
		ChangeBtn = root.Find("ContentFrame/ContentBg/AddFriends/Change/ChangeBtn").GetComponent<UIButton>();
		InputLbl = root.Find("ContentFrame/ContentBg/AddFriends/PlayerId/Input").GetComponent<UIInput>();
		SeachBtn = root.Find("ContentFrame/ContentBg/AddFriends/PlayerId/SeachBtn").GetComponent<UIButton>();
		CloseAddFriendViewBtn = root.Find("ContentFrame/ContentBg/AddFriends/CloseAddFriendViewBtn").GetComponent<UIButton>();
		RewardScrollView = root.Find("ContentFrame/ContentBg/RightPanel/Email/EmailContent/RewardGroup/scrollView").GetComponent<UIScrollView>();
		SpeedOrInputBg = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/ChatBtnGroup/SpeedOrInputBtn/Background").GetComponent<UISprite>();
		SpeedBtn = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/ChatBtnGroup/SpeedBtn").GetComponent<UIEventListener>();
		SpeedOrInputBtn = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/ChatBtnGroup/SpeedOrInputBtn").GetComponent<UIButton>();
		SendBtn = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/ChatBtnGroup/SendBtn").GetComponent<UIButton>();
		SpeechAnchorObj = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/SpeechAnchor").gameObject;
		SpeechObj = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/SpeechAnchor/Speech").gameObject;
		EnergyBar = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/SpeechAnchor/Speech/EnergyBar").GetComponent<UISprite>();
		GiveUpObj = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/SpeechAnchor/GiveUp").gameObject;
		ShortVoiceObj = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/SpeechAnchor/Short").gameObject;
		VoiceTipLbl = root.Find("ContentFrame/ContentBg/RightPanel/ChatContent/SpeechAnchor/Tip").GetComponent<UILabel>();
		RecommendFriendsTable = root.Find("ContentFrame/ContentBg/AddFriends/RecommendFriends/ItemScrollView/itemGrid").GetComponent<UITable>();
	}
}