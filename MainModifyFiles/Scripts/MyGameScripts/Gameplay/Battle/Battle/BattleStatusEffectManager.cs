using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.core.modules.battle.data;

public sealed class BattleStatusEffectManager
{
    static readonly BattleStatusEffectManager instance=new BattleStatusEffectManager();
	
    //private float timeCounter = 0.0f;     //GET WARNING
	
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static BattleStatusEffectManager()
    {
    }

    BattleStatusEffectManager()
    {
    }

    public static BattleStatusEffectManager Instance
    {
        get
        {
            return instance;
        }
    }
	
	public void InitBattleStatusEffectManager()
	{
		
		

	}

	public void AddEffect( MonsterController mc )
	{
		BattleController battleController = BattleController.Instance;
		if ( battleController == null )
			return;
		
		Camera currentCamera = battleController.GetCurrentCamera();
		
		Transform hudTransform = mc.GetMountDamageffect();
		

		if ( mc.existMessageEffect( ( MonsterController.ShowMessageEffect.CRITICAL) ) == true  ){
			ShowBattleStatusEffect( "baoji", currentCamera, hudTransform );
		}
		else if(mc.existMessageEffect( ( MonsterController.ShowMessageEffect.DODGE) ) == true)
			ShowBattleStatusEffect( "shanduo", currentCamera, hudTransform );

	}
	
	public void PlayDamage(string msg, Color color, GameObject target, float duration, int fontType, float scale = 1f)
	{
		BattleController battleController = BattleController.Instance;
		if ( battleController == null )
			return;
		
		Camera currentCamera = battleController.GetCurrentCamera();
		
		GameObject statusPrefab = ResourcePoolManager.Instance.SpawnUIPrefab("Prefabs/Module/Battle/BattleUI/BattleFontPrefab" ) as GameObject;
		
		if(statusPrefab == null)
			return;
		
		GameObject effectObject = NGUITools.AddChild( LayerManager.Instance.battleHudTextAnchor, statusPrefab );
		if ( effectObject == null )
			return;
		
		UIBattleStatusEffect bse = effectObject.GetComponent< UIBattleStatusEffect >();
		if ( bse != null )
		{
			if ( target != null )
				bse.ShowDamage(currentCamera, target, msg, fontType);
		}
	}

	public void PlaySkillName(MonsterController mc, Skill skill)
	{
		BattleController battleController = BattleController.Instance;
		
		Camera currentCamera = battleController.GetCurrentCamera();
		
		GameObject statusPrefab = ResourcePoolManager.Instance.SpawnUIPrefab("Prefabs/Module/Battle/BattleUI/BattleFontPrefab" ) as GameObject;
		
		if(statusPrefab == null)
			return;
		
		GameObject effectObject = NGUITools.AddChild( LayerManager.Instance.battleHudTextAnchor, statusPrefab );
		
		UIBattleStatusEffect bse = effectObject.GetComponent< UIBattleStatusEffect >();
		if ( bse != null )
		{
			Transform hudTransform = mc.GetMountBattleEffect();
			
			bse.ShowSkillName(skill.name, currentCamera, hudTransform);
		}

		string attackType = "";

		if (skill.skillAttackType == Skill.SkillAttackType_Phy)
		{
			attackType = "phy";
		}
		else
		{
			attackType = "magic";
		}

		string soundName = string.Format ("pet_{0}_{1}", mc.GetModel(), attackType);

		AudioManager.Instance.PlaySound ("sound_pet/"+soundName);
	}

	public void PlayMsg(MonsterController mc, string msg)
	{
		BattleController battleController = BattleController.Instance;
		
		Camera currentCamera = battleController.GetCurrentCamera();
		
		GameObject statusPrefab = ResourcePoolManager.Instance.SpawnUIPrefab("Prefabs/Module/Battle/BattleUI/BattleFontPrefab" ) as GameObject;
		
		if(statusPrefab == null)
			return;
		
		GameObject effectObject = NGUITools.AddChild( LayerManager.Instance.battleHudTextAnchor, statusPrefab );
		
		UIBattleStatusEffect bse = effectObject.GetComponent< UIBattleStatusEffect >();
		if ( bse != null )
		{
			Transform hudTransform = mc.GetMountBattleEffect();
			
			bse.ShowMsg(msg, currentCamera, hudTransform);
		}
	}

	void Update()
	{
	}
	
	private void ShowCriticalFlash(){
		//AudioManager.Instance.PlaySound("sound_effect/souond_baoji");
		
//		CameraMask.Instance.ShowWhite();
	}
	
	private void ShowBattleStatusEffect( string effectName, Camera cam, Transform mountPoint )
	{
		if ( cam == null )
			return;
		
		GameObject statusPrefab = ResourcePoolManager.Instance.SpawnUIPrefab("Prefabs/BattleFontPrefab/BattleFontPrefab" ) as GameObject;
		
		if(statusPrefab == null)
			return;
		
		GameObject effectObject = NGUITools.AddChild( LayerManager.Instance.battleHudTextAnchor, statusPrefab );
		if ( effectObject == null )
			return;
		
		UIBattleStatusEffect bse = effectObject.GetComponent< UIBattleStatusEffect >();
		if ( bse != null )
		{
			if ( mountPoint != null )
				bse.ShowStatusEffect( cam, mountPoint.gameObject, effectName );
		}
	}
}
