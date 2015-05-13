using UnityEngine;
using System.Collections;

public class StoryNpcView :MonoBehaviour {
	
	
	private NavMeshAgent agent;
	private Animator _animator;
	
	private GameObject _mGo;
	private GameObject _modelGo;
	private int _modelId;
	public NpcHUDView _npcHUDView;
	
	private NpcAppearInst _dto;       
	
	void Start(){
		
	}
	
	
	void Awake(){
		
		_mGo = this.gameObject;
		
		
		agent = _mGo.GetMissingComponent<NavMeshAgent>();
		agent.radius = 0.4f;
		agent.speed = 3f;
		agent.acceleration = 1000;
		agent.angularSpeed = 1000;
		agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
	}
	
	public int GetNpcId()
	{
		return _dto.npcid;
	}
	
	public void Load(NpcAppearInst npc){

		_dto = npc;
		_modelId = npc.model;
		if(npc.copyHero){
			_dto.name = PlayerModel.Instance.GetPlayerName();
			//模型大小
			OnLoadFinish(GameObject.Instantiate(WorldManager.Instance.GetHeroView().GetPlayerModelGo()));
		}else{
			string petStylePath = ModelHelper.GetCharacterPrefabPath(_modelId);
			ResourcePoolManager.Instance.Spawn(petStylePath, OnLoadFinish, ResourcePoolManager.PoolType.DONT_DESTROY);
		}

	
	}
	
	protected void OnLoadFinish(UnityEngine.Object inst)
	{
		GameObject model = inst as GameObject;
		if (model == null)
		{
			GameDebuger.Log("PlayerView OnLoadFinish null and Use Default 2020");
			_modelId = ModelHelper.DefaultModelId;
			string petStylePath = ModelHelper.GetCharacterPrefabPath(_modelId);
			ResourcePoolManager.Instance.Spawn(petStylePath, OnLoadFinish, ResourcePoolManager.PoolType.DONT_DESTROY);
			return;
		}
		
//		GameObjectExt.RemoveChildren(_mGo);
		_modelGo = GameObjectExt.AddPoolChild(_mGo, model); //模型大小

		if(_dto.scaleX != 0f){
			_modelGo.transform.localScale = new Vector3(_dto.scaleX,_dto.scaleY,_dto.scaleZ);
		}

		
		ModelHelper.SetPetShadow (_modelGo);
//		ModelHelper.SetPetShader(_modelGo, true);
		
		if (_modelId == 11)
		{
			int wpId = 11;
			ModelHelper.UpdateModelWeapon (_modelGo, _modelId, wpId);
		}
		
		InitHUDName ();
		
		_animator = _modelGo.GetComponent<Animator>();
		
	}
	
	private void InitHUDName()
	{
		GameObject npcHUDPrefab = ResourcePoolManager.Instance.SpawnUIPrefab( "Prefabs/Module/PlayerHUDModule/NpcHUDView" ) as GameObject;
		GameObject npcHUDGo = NGUITools.AddChild(LayerManager.Instance.storyHudTextAnchor, npcHUDPrefab);
		npcHUDGo.name = "StoryNpcHUDView_"+_dto.npcid;
		
		_npcHUDView = npcHUDGo.GetMissingComponent<NpcHUDView>();
		_npcHUDView.Setup(npcHUDGo.transform);
		_npcHUDView.fightFlag_UISprite.enabled = false;
		_npcHUDView.fightFlag_UISpriteAnimation.enabled = false;
		_npcHUDView.missionType_UISprite.enabled =false;
		_npcHUDView.nameLbl_UILabel.text = string.Format("{0}",_dto.name.WrapColor(ColorConstant.Color_Battle_Enemy_Name));
		
		//	Bottom HUD
		_npcHUDView.BottomFollow_UIFollowTarget.gameCamera = LayerManager.Instance.GameCamera;
		_npcHUDView.BottomFollow_UIFollowTarget.uiCamera = LayerManager.Instance.UICamera;
		
		Transform tMountShadow = ModelHelper.GetMountingPoint(_modelGo, ModelHelper.Mount_shadow);
		_npcHUDView.BottomFollow_UIFollowTarget.target = tMountShadow;
		_npcHUDView.BottomFollow_UIFollowTarget.offset = new Vector3 (0f,-0.5f,0f);
		
		//	Top HUD
		_npcHUDView.TopFollow_UIFollowTarget.gameCamera = LayerManager.Instance.GameCamera;
		_npcHUDView.TopFollow_UIFollowTarget.uiCamera = LayerManager.Instance.UICamera;
		
		Transform tMountHUD = ModelHelper.GetMountingPoint(_modelGo, ModelHelper.Mount_hud);
		_npcHUDView.TopFollow_UIFollowTarget.target = tMountHUD;
		_npcHUDView.TopFollow_UIFollowTarget.offset = Vector3.zero;
	}
	


	void Update(){
		if (agent.hasPath)
		{
			PlayRunAnimation();
		} 
		else 
		{
			PlayIdleAnimation();
		}


		if (turn) {
			if(_mGo.transform.rotation != Quaternion.Euler(new Vector3(0,_turnAngle,0))){
				_mGo.transform.rotation = Quaternion.Lerp(_mGo.transform.rotation,Quaternion.Euler(new Vector3(0,_turnAngle,0)),turnSpeed);
			}
			else{
				turn = false;
			}
		} 
			



	}
	
	bool _isRunning = false;
//	public void PlayAnimation(string animName){
//		_animator.Play (animName);
////		ModelHelper.PlayAnimation(curAnimation, action, crossFade, null, false);
//	}
	
	public void PlayAnimation(string action, bool crossFade = false)
	{
		ModelHelper.PlayAnimation(_animator, action, crossFade, null, false);
	}
	
	public void PlayRunAnimation(){
		if (!_isRunning) 
		{
			PlayAnimation(ModelHelper.Anim_run, false);
			_isRunning = true;
		}
	}
	
	public void PlayIdleAnimation(){
		if (_isRunning) 
		{
			PlayAnimation(ModelHelper.Anim_idle, true);
			_isRunning = false;
		}
	}
	

	float _turnAngle;           //转完后的角度.
	bool turn = false;
	float turnSpeed;
	public void TurnAround(NpcTurnaroundInst inst){
		_turnAngle = inst.TurnAngle;
		turnSpeed = inst.turnSpeed;
		turn = true;
	}
	
	
	virtual public void DestroyMe()
	{
		if(_npcHUDView != null)
			GameObject.Destroy (_npcHUDView.gameObject);
		
		GameObject.Destroy(_mGo);
	}
	
	public void SetGoPosition(Vector3 position){
		agent.SetDestination (position);
	}
}
