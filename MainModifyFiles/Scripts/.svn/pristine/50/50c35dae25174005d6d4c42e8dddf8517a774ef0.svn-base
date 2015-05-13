using UnityEngine;
using System.Collections;

public class OneShotSceneEffect : MonoBehaviour {
	
	private GameObject _effGo;

	#region PlayFollowEffect
	public IEnumerator Play(string effName,Vector3 worldPos,float effTime=1f,float scaleFactor=1f,System.Action callBackFinish=null)
	{
		ResourcePoolManager.Instance.Spawn(effName,
        delegate(UnityEngine.Object inst)
        {
			GameObject effectGo = inst as GameObject;
			if(effectGo == null)
				return;

			_effGo = GameObjectExt.AddPoolChild(this.gameObject,effectGo);
			_effGo.transform.position = new Vector3(worldPos.x,worldPos.y,worldPos.z);
			
			ParticleScaler scaler = _effGo.GetComponent<ParticleScaler>();
	        if (scaler != null)
	        {
				scaler.SetscaleMultiple(scaleFactor);
	        }
			
			Renderer []renderList = _effGo.GetComponentsInChildren<Renderer>(true);
			foreach(Renderer render in renderList)
			{
				if (render.sharedMaterial != null)
				{
					render.sharedMaterial.renderQueue = UILayerType.RenderQueue_UIEffect;
				}
			}
		},ResourcePoolManager.PoolType.DESTROY_NO_REFERENCE);
		
		yield return new WaitForSeconds(effTime);
		Finish();
		
		if(callBackFinish != null)
			callBackFinish();
	}
	
	public void Finish()
	{
		ResourcePoolManager.Instance.Despawn(_effGo,ResourcePoolManager.PoolType.DESTROY_NO_REFERENCE);
		Destroy(this);
	}
	
	public static OneShotSceneEffect Begin(string effName,Vector3 worldPos,float effTime=1f,float scaleFactor=1f,System.Action callBackFinish=null)
	{
		OneShotSceneEffect controller = LayerManager.Instance.EffectsAnchor.AddComponent<OneShotSceneEffect>();
		controller.StartCoroutine(controller.Play(effName,worldPos,effTime,scaleFactor,callBackFinish));
		
		return controller;
	}
	#endregion

	#region PlayFollowEffect
	public IEnumerator PlayFollowEffect(string effName,Transform folowTransform,float effTime=1f,float scaleFactor=1f,System.Action callBackFinish=null)
	{
		ResourcePoolManager.Instance.Spawn(effName,
		                                   delegate(UnityEngine.Object inst)
		                                   {
			GameObject effectGo = inst as GameObject;
			if(effectGo == null)
				return;
			
			_effGo = GameObjectExt.AddPoolChild(folowTransform.gameObject,effectGo);
			
			ParticleScaler scaler = _effGo.GetComponent<ParticleScaler>();
			if (scaler != null)
			{
				scaler.SetscaleMultiple(scaleFactor);
			}
			
			Renderer []renderList = _effGo.GetComponentsInChildren<Renderer>(true);
			foreach(Renderer render in renderList)
			{
				if (render.sharedMaterial != null)
				{
					render.sharedMaterial.renderQueue = UILayerType.RenderQueue_UIEffect;
				}
			}
		},ResourcePoolManager.PoolType.DESTROY_NO_REFERENCE);
		
		yield return new WaitForSeconds(effTime);
		Finish();
		
		if(callBackFinish != null)
			callBackFinish();
	}

	public static OneShotSceneEffect BeginFollowEffect(string effName,Transform folowTransform,float effTime=1f,float scaleFactor=1f,System.Action callBackFinish=null)
	{
		OneShotSceneEffect controller = LayerManager.Instance.EffectsAnchor.AddComponent<OneShotSceneEffect>();
		controller.StartCoroutine(controller.PlayFollowEffect(effName,folowTransform,effTime,scaleFactor,callBackFinish));
		
		return controller;
	}
	#endregion
}
