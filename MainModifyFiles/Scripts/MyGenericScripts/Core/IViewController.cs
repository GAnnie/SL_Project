using UnityEngine;
using System.Collections;

public interface IViewController{
	
	/// <summary>
	/// 从DataModel中取得相关数据对界面进行初始化
	/// </summary>
	void InitView();

	/// <summary>
	/// Registers the event.
	/// DateModel中的监听和界面控件的事件绑定,这个方法将在InitView中调用
	/// </summary>
	void RegisterEvent();
	
	/// <summary>
	/// 关闭界面时清空操作放在这
	/// </summary>
	void Dispose();
}
