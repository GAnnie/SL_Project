using UnityEngine;
using System.Collections;

public class ChannelShieldViewController : MonoBehaviour,IViewController {

	private ChannelShieldView _view;

	public void InitView(){
		_view = gameObject.GetMissingComponent<ChannelShieldView> ();
		_view.Setup (this.transform);


		_view.WorldCheckbtnToggle.value = ChatModel.Instance.GetWorldChannelShield();
		_view.GuildCheckbtnToggle.value = ChatModel.Instance.GetGuildChannelShield();
		_view.TeamCheckbtnToggle.value = ChatModel.Instance.GetTeamChannelShield();

		RegisterEvent();
	}


	public void RegisterEvent(){
		EventDelegate.Set(_view.CloseBtn.onClick,OnCloseBtnClick);
		EventDelegate.Set(_view.ConfirmBtn.onClick,OnCloseBtnClick);


		//	world
		EventDelegate.Add(_view.WorldCheckbtnToggle.onChange, () => {
			ChatModel.Instance.WorldChannelShield(_view.WorldCheckbtnToggle.value);
		});


		//	guild
		EventDelegate.Add(_view.GuildCheckbtnToggle.onChange, () => {
			ChatModel.Instance.GuildChannelShield(_view.GuildCheckbtnToggle.value);
		});


		//	team
		EventDelegate.Add(_view.TeamCheckbtnToggle.onChange, () => {
			ChatModel.Instance.TeamChannelShield(_view.TeamCheckbtnToggle.value);
		});
	}


	public void OnCloseBtnClick(){
		ProxyChannelShieldModule.Close();
	}

	public void Dispose(){

	}

}
