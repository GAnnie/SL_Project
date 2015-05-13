using UnityEngine;
using System.Collections;
using com.nucleus.player.msg;

public class BattleItemUseViewController : UseLeftViewCell
{

	private const string ViewName = "Prefabs/Module/ItemUseModule/IdentifyItemUseView";
	
	public static BattleItemUseViewController Setup(GameObject pos)
	{
		GameObject prefab = ResourcePoolManager.Instance.SpawnUIPrefab( ViewName ) as GameObject;
		GameObject module = GameObjectExt.AddChild(pos,prefab);
		BattleItemUseViewController leftView = module.GetMissingComponent<BattleItemUseViewController>();
		leftView.InitView();
		UIHelper.AdjustDepth(module,1);
		return leftView;
	}
	
	private IdentifyItemUseView _view;
	
	override public void InitView()
	{
		_view = gameObject.GetMissingComponent<IdentifyItemUseView> ();
		_view.Setup (this.transform);
	}
	
	override public void RegisterEvent()
	{
	}
	
	override public void SetUseDto(PackItemDto dto)
	{
		_view.EmptyInfo.SetActive(false);

		//_view.CountLabel.text = string.Format("剩余使用次数：{0}",10);
	}
	
	override public void SetData(PackItemDto dto)
	{
		_dto = dto;
		_view.EmptyInfo.SetActive(false);
		ItemTextTipManager.Instance.ShowItem(dto,_view.EquipmentTipLabel, true);
	}
	
	override public void Dispose()
	{
	}

	public void UpdateItemUsedCount(int count)
	{
		_view.CountLabel.text = string.Format("剩余使用次数：{0}",10-count);
	}
}

