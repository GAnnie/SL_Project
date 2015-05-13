using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProxyActorPopoModule
{
	private const string NAME = "Prefabs/Module/ActorPopoModule/ActorPopoModuleView";

	private static Dictionary< long, ActorPopoModule > _popoMaps; //原数据

	/// <summary>
	/// Open the specified popoId, tran, msg, showCamera and offY.
	/// </summary>
	/// <param name="popoId">聊天泡泡唯一ID</param>
	/// <param name="tran">泡泡对位位置</param>
	/// <param name="msg">内容</param>
	/// <param name="showCamera">显示用的摄像机 LayerManager.Instance.BattleCamera or LayerManager.Instance.GameCamera</param>
	/// <param name="offY">Y轴偏移</param>
	public static void Open(long popoId, Transform tran, string msg, Camera showCamera, float offY = 0)
	{
		if (_popoMaps == null)
		{
			_popoMaps = new Dictionary<long, ActorPopoModule>();
		}

		GameObject modulePrefab = ResourcePoolManager.Instance.SpawnUIPrefab( NAME ) as GameObject;
		GameObject module = NGUITools.AddChild(LayerManager.Instance.uiModuleRoot,modulePrefab);
		
		var controller = module.GetMissingComponent<ActorPopoModule>();
		controller.InitView ();
		controller.Open ( popoId, tran, msg, showCamera, offY );

		_popoMaps.Add (popoId, controller);
	}
	
	public static void Close(long popoId)
	{
		if (_popoMaps != null && _popoMaps.ContainsKey(popoId))
		{
			ActorPopoModule module = _popoMaps[popoId];
			module.Dispose();
			GameObject.Destroy(module.gameObject);
			_popoMaps.Remove(popoId);
		}
	}
	
//	public static void Hide()
//	{
//		UIModuleManager.Instance.HideModule(NAME);	
//	}
//	
//	public static void Show()
//	{
//		UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.BaseModule, false);
//	}
}