using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.charactor.data;

public class CrewTavernTabContentController : MonoBehaviour {

	private CrewMainView _view;

	private const string CREWTAVERN_ITEM = "Prefabs/Module/CrewModule/CrewTavernItemWidget";

	public void InitView(CrewMainView view){
		_view = view;

		List<GeneralCharactor> generalCharactorList = DataCache.getArrayByCls<GeneralCharactor>();
		List<Crew> unRecruitedCrewInfoList = new List<Crew>(10);
		List<Crew> recruitedCrewInfoList = new List<Crew>(2);
		for(int i=0;i<generalCharactorList.Count;++i){
			if(generalCharactorList[i] is Crew){
				if(CrewModel.Instance.ContainCrew(generalCharactorList[i].id)){
					recruitedCrewInfoList.Add(generalCharactorList[i] as Crew);
				}else
					unRecruitedCrewInfoList.Add(generalCharactorList[i] as Crew);
			}
		}

		GameObject itemGrid = _view.CrewTavernGroup.transform.Find("itemGrid").gameObject;
		GameObject itemPrefab = ResourcePoolManager.Instance.SpawnUIPrefab(CREWTAVERN_ITEM) as GameObject;
		for(int i=0;i<unRecruitedCrewInfoList.Count;++i){
			GameObject tavernItem = NGUITools.AddChild(itemGrid,itemPrefab);
			CrewTavernItemController com = tavernItem.GetMissingComponent<CrewTavernItemController>();
			com.InitItem(unRecruitedCrewInfoList[i],false);
		}

		for(int i=0;i<recruitedCrewInfoList.Count;++i){
			GameObject tavernItem = NGUITools.AddChild(itemGrid,itemPrefab);
			CrewTavernItemController com = tavernItem.GetMissingComponent<CrewTavernItemController>();
			com.InitItem(recruitedCrewInfoList[i],true);
		}
	}
}
