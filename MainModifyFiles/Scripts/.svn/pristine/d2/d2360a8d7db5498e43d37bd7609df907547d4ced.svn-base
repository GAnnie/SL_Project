using UnityEngine;
using System.Collections;
using com.nucleus.commons.message;
using com.nucleus.h1.logic.core.modules.team.dto;

public class ApplicationItemController : MonoBehaviour
{

	ApplicationItemWidget _view;
	private GeneralResponse _data;

	public void InitItem ()
	{
		_view = gameObject.GetMissingComponent<ApplicationItemWidget> ();
		_view.Setup (this.transform);

		EventDelegate.Set (_view.applyBtn.onClick, OnClickApplyBtn);
	}

	public void SetData (GeneralResponse data)
	{
		_data = data;
		if (data is TeamInviteNotify) {
			TeamInviteNotify inviteInfo = data as TeamInviteNotify;
			SetNameLbl(inviteInfo.leaderNickname);
			SetFactionInfoLbl(string.Format("{0}级{1}",inviteInfo.leaderLevel,inviteInfo.leaderFaction.name));
		} else if (data is JoinTeamRequestNotify) {
			JoinTeamRequestNotify joinInfo = data as JoinTeamRequestNotify;
			SetNameLbl(joinInfo.requestJoinPlayerNickname);
			SetFactionInfoLbl(string.Format("{0}级{1}",joinInfo.requestJoinPlayerLevel,joinInfo.requestJoinPlayerFaction.name));
		}
	}

	private void SetNameLbl (string name)
	{
		_view.nameLbl.text = name;
	}

	private void SetFactionInfoLbl (string info)
	{
		_view.factionInfoLbl.text = info;
	}

	private void OnClickApplyBtn ()
	{
		if (_data is TeamInviteNotify) {
			TeamInviteNotify inviteInfo = _data as TeamInviteNotify;
			TeamModel.Instance.ApproveInviteMember(inviteInfo.teamUniqueId);
		} else if (_data is JoinTeamRequestNotify) {
			JoinTeamRequestNotify joinInfo = _data as JoinTeamRequestNotify;
			TeamModel.Instance.ApproveJoinTeam(joinInfo.requestJoinPlayerId);
		}
	}
}
