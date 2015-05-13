using UnityEngine;
using System.Collections;

public class OneShotUIEffect : MonoBehaviour {
	
	private GameObject _effGo;

	public IEnumerator Play(string effName,Vector3 worldPos,float effTime=1f,float scaleFactor=1f,System.Action callBackFinish=null)
	{
		ResourcePoolManager.Instance.Spawn(effName,
        delegate(UnityEngine.Object inst)
        {
			GameObject effectGo = inst as GameObject;
			if(effectGo == null)
				return;

			_effGo = GameObjectExt.AddPoolChild(this.gameObject,effectGo);
			_effGo.transform.position = new Vector3(worldPos.x,worldPos.y,_effGo.transform.position.z);
			
			ParticleScaler scaler = _effGo.GetComponent<ParticleScaler>();
	        if (scaler != null)
	        {
				scaler.SetscaleMultiple(scaleFactor);
	        }
			
			Renderer []renderList = _effGo.GetComponentsInChildren<Renderer>(true);
			foreach(Renderer render in renderList)
			{
				render.sharedMaterial.renderQueue = UILayerType.RenderQueue_UIEffect;
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
	
	public static OneShotUIEffect Begin(string effName,Vector3 worldPos,float effTime=1f,float scaleFactor=1f,System.Action callBackFinish=null)
	{
		OneShotUIEffect controller = LayerManager.Instance.EffectsAnchor.AddComponent<OneShotUIEffect>();
		controller.StartCoroutine(controller.Play(effName,worldPos,effTime,scaleFactor,callBackFinish));
		
		return controller;
	}
}
