using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.mission.dto;
using com.nucleus.h1.logic.core.modules.mission.data;
using com.nucleus.h1.logic.core.modules.title.data;
using com.nucleus.h1.logic.core.modules.assistskill.dto;
using com.nucleus.h1.logic.core.modules.faction.dto;

public class HyperlinkMissionController : MonoBehaviour {

	private UIPanel mPanel;
	private HyperlinkMission _view;

	public void InitView(){
		_view = gameObject.GetMissingComponent<HyperlinkMission> ();
		_view.Setup (this.transform);
		mPanel = gameObject.GetComponent<UIPanel>();

		_view.MIssionContentObj.SetActive(false);
		_view.SkillContentObj.SetActive(false);
		_view.TitleContentObj.SetActive(false);

		RegisterEvent();
	}

	public void SetData(PlayerMissionDto dto){


		_view.MissionName.text = dto.mission.name;

		Mission mission = dto.mission;
		_view.MissionTarget.text = MissionDataModel.Instance.GetCurTargetContent(mission,false);
		_view.MissionDescription.text = MissionDataModel.Instance.GetCurDescriptionContent(mission);
		_view.MIssionContentObj.SetActive(true);
	}

	public void SetData(FactionSkillDto dto){

		_view.SkillName.text = dto.factionSkill.name;
		_view.SkillDescription.text = dto.factionSkill.description;
		_view.SkillContentObj.SetActive(true);
	}

	public void SetData(Title dto){

		_view.TitleName.text = dto.name;
		_view.TitleDescription.text = dto.desc;
		_view.TitleContentObj.SetActive(true);
	}
	

	public void SetData(AssistSkillDto dto){
		_view.SkillName.text = dto.assistSkill.name;
		_view.SkillDescription.text = dto.assistSkill.description;
		_view.SkillContentObj.SetActive(true);
	}

	public void RegisterEvent (){
		EventDelegate.Set(_view.CloseBtn.onClick,()=>{ProxyHyperlinkMissionModule.Close();});

		UICamera.onClick += ClickEventHandler;
	}

	void ClickEventHandler(GameObject clickGo){
		UIPanel panel = UIPanel.Find(clickGo.transform);
		if(panel != mPanel)
			CloseView();
	}
	
	void CloseView(){
		ProxyHyperlinkMissionModule.Close();
	}
}
