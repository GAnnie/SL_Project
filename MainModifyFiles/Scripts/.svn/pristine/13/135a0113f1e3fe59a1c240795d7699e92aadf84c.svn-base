using com.nucleus.h1.logic.core.modules.equipment.dto;
using System;

public class EquipmentBuffUpdateNotifyListener : BaseDtoListener 
{
	override protected Type getDtoClass()
	{
        return typeof(EquipmentBuffUpdateNotify);
	}
	
	override public void process( object message )
	{
        EquipmentBuffUpdateNotify dto = message as EquipmentBuffUpdateNotify;
        BackpackModel.Instance.UpDateEquipmentBuff(dto);
	}
}