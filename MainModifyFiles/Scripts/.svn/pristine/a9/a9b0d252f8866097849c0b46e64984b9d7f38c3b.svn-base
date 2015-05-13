// **********************************************************************
//	Copyright (C), 2011-2015, CILU Game Company Tech. Co., Ltd. All rights reserved
//	Work:		For H1 Project With .cs
//  FileName:	MissionPlotModel.cs
//  Version:	Beat R&D

//  CreatedBy:	_Alot
//  Date:		2015.05.07
//	Modify:		__

//	Url:		http://www.cilugame.com/

//	Description:
//	This program files for detailed instructions to complete the main functions,
//	or functions with other modules interface, the output value of the range,
//	between meaning and parameter control, sequence, independence or dependence relations
// **********************************************************************

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.plot.data;
using com.nucleus.h1.logic.core.modules.mission.dto;

public class MissionPlotModel {

	private static readonly MissionPlotModel instance = new MissionPlotModel();
	public static MissionPlotModel Instance {
		get { return instance; }
	}

	/// <summary>
	/// 外部剧情播放Callback （剧情ID，回调（无参）） -- The story plot callback.
	/// </summary>
	public System.Action<int, System.Action<PlayerMissionDto>> storyPlotCallback {
		get { return _storyPlotCallback; }
		set { _storyPlotCallback = value; }
	}
	private System.Action<int, System.Action<PlayerMissionDto>> _storyPlotCallback = null;
	
	//	维护一个任务为ID的剧情Dic(任务前)
	private Dictionary<int, Plot> _missionAcceptPlotDic = null;
	//	维护一个任务为ID的剧情Dic(任务后)
	private Dictionary<int, Plot> _missionSubmitPlotDic = null;

	#region 获取ID是MissionID的PlotDic（过滤）
	private void GetPlotDic() {
		_missionAcceptPlotDic = new Dictionary<int, Plot>();
		_missionSubmitPlotDic = new Dictionary<int, Plot>();

		Dictionary<int, Plot> tPlotDic = DataCache.getDicByCls<Plot>();
		if (tPlotDic == null) return;

		foreach(Plot tPlot in tPlotDic.Values) {
			if (tPlot.triggerType == Plot.PlotTriggerType_AcceptMission) {
				_missionAcceptPlotDic.Add(tPlot.triggerParam, tPlot);
			} else if (tPlot.triggerType == Plot.PlotTriggerType_SubmitMission) {
				_missionSubmitPlotDic.Add(tPlot.triggerParam, tPlot);
			}
			continue;
		}
	}
	#endregion

	#region 给MissionDataModel调用的剧情播放接口方法
	/// <summary>
	/// 给MissionDataModel调用的剧情播放接口方法 -- Stories the plot in plot model.
	/// </summary>
	/// <param name="missionID">Mission I.</param>
	/// <param name="acceptAndSubmit">If set to <c>true</c> accept and submit.</param>
	/// <param name="callback">Callback.</param>
	public void StoryPlotInPlotModel(int missionID, bool acceptAndSubmit, System.Action<PlayerMissionDto> callback) {
		if (_missionAcceptPlotDic == null || _missionSubmitPlotDic == null) {
			GetPlotDic();
		}

		//	查看是否有任务ID相关剧情
		Dictionary<int, Plot> tMissionPlotDic = acceptAndSubmit? _missionAcceptPlotDic : _missionSubmitPlotDic;
		if (tMissionPlotDic.Count <= 0) {
			GameDebuger.OrangeDebugLog(string.Format("WARNING:当前没有配置 {0} 任务剧情(ID:{1})\n_missionAcceptPlotDic Is Null",
			                                         acceptAndSubmit? "接取" : "提交", missionID));
			return;
		}

		if (_storyPlotCallback != null && tMissionPlotDic.ContainsKey(missionID)) {
			// 播放剧情
			_storyPlotCallback(tMissionPlotDic[missionID].id, callback);
		} else {
			callback(null);
		}
	}
	#endregion
}

