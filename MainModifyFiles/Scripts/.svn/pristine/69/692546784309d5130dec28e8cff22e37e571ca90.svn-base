using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules;
using com.nucleus.commons.message;

public class PlayerChangeNameController : MonoBehaviour,IViewController
{
	private PlayerChangeNameView _view;


	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	public void InitView(){
		_view = gameObject.GetMissingComponent<PlayerChangeNameView> ();
		_view.Setup(this.transform);

		RegisterEvent ();
	}
	
	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView中调用
	/// </summary>
	public void RegisterEvent(){
		EventDelegate.Set (_view.OkBtn.onClick, OnOkBtnClick);
		EventDelegate.Set (_view.CancelBtn.onClick, OnCancelBtnClick);
		EventDelegate.Set (_view.CloseButton.onClick, OnCloseBtnClick);
	}

	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	public void Dispose(){
	}

	private bool _fromProp;

	public void SetData(bool fromProp)
	{
		_fromProp = fromProp;
		if (fromProp)
		{
			_view.TipLabel.text = "本次修改名字 完全免费";
		}
		else
		{
			int lv = PlayerModel.Instance.GetPlayerLevel();
			int RENAME_FACTOR1 = DataHelper.GetStaticConfigValue(H1StaticConfigs.RENAME_FACTOR1);
			int RENAME_FACTOR2 = DataHelper.GetStaticConfigValue(H1StaticConfigs.RENAME_FACTOR2);
			int RENAME_FACTOR3 = DataHelper.GetStaticConfigValue(H1StaticConfigs.RENAME_FACTOR3);

			int money = RENAME_FACTOR1+(lv-RENAME_FACTOR2)*RENAME_FACTOR3;

			_view.TipLabel.text = string.Format("本次修改角色名字需要消耗{0}{1}", money,ItemIconConst.Ingot );
		}
	}


	private void OnOkBtnClick()
	{
		string name = _view.NameInput.value;
		if (PassNameLenght(name) == false)
		{
			TipManager.AddTip("角色名字长度只能为2~5个文字");
			return;
		}

		if (!string.IsNullOrEmpty(name))
		{
			if (_fromProp)
			{
				ServiceRequestAction.requestServer(PlayerService.propsRename(name), "", (e)=>{
					PlayerModel.Instance.UpdatePlayerName(name);
				});
			}
			else
			{
				ServiceRequestAction.requestServer(PlayerService.rename(name), "", (e)=>{
					PlayerModel.Instance.UpdatePlayerName(name);
				});

			}
			OnCloseBtnClick();
		}
	}

	private bool PassNameLenght(string name)
	{
		int len = name.Length;
		if (len >= 2 && len <= 5)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private void OnCancelBtnClick()
	{
		OnCloseBtnClick ();
	}

	private void OnCloseBtnClick()
	{
		ProxyPlayerPropertyModule.ClosePlayerChangeNameView ();
	}
}
