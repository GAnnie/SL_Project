// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  FloatingJoystick.cs
// Author   : willson
// Created  : 2014/12/3 
// Porpuse  : 
// **********************************************************************

using UnityEngine;
using com.nucleus.h1.logic.core.modules.scene.data;

public class FloatingJoystick : MonoBehaviour
{
	public Transform joystick;
	public Transform joystickBackground;
	public float radius = 40;
	public float angle = 0;

	public delegate void OnDragEvent( float angle );
	public OnDragEvent onDragEvent;
	
	public delegate void OnDragEventEnd();
	public OnDragEventEnd onDragEventEnd;

	
	private bool isDragging = false;
	private Vector3 targetPosition;
	private float lerpSpeed = 0.6f;
	private BoxCollider boxCollider;
	
	private Vector3 bottomRight;
	private Vector3 upperLeft;
	private Vector2 absolutePosition = Vector2.zero;

	private GameObject mCachedGameObject;
	
	//for Npc Click//
	bool	   mNpcHited;
	Vector3    mPressPosition;
	GameObject mHitObject;
	//for Player Click//
	bool 	   mPlayerHited;
	GameObject mPlayerHitObj;
	//for Terrain Click//
	bool 	   mTerrained;
	Vector3	   mMousePosition;
	
	void Start()
	{
		boxCollider = GetComponent< BoxCollider >();
		mCachedGameObject = gameObject;
		ResetJoystick();
	}
	
	void OnDisable()
	{
		ResetJoystick();
	}
	
	public void SetCollider( bool enable )
	{
		if (boxCollider != null)
		{
			boxCollider.enabled = enable;
		}
	}
	
//	void OnDragStart() 
//	{
//
//	}
	
	void OnDrag( Vector2 delta)
	{
		if(JoystickModule.DisableMove)	return;
		if( mNpcHited ) return;
		if( mPlayerHited) return;

		UpdateJoystick();
		isDragging = true;
	}
	
//	void OnDragEnd() 
//	{
//
//	}
	
	void Update()
	{
		if ( isDragging )
		{
			if ( onDragEvent != null )
			{
				// 停止挂机
				PlayerModel.Instance.StopAutoFram();
				onDragEvent( angle );
			}
			
			Vector3 lerpPos = Vector3.Lerp( joystickBackground.position, targetPosition, lerpSpeed );
			joystickBackground.position = lerpPos;
			absolutePosition.x = lerpPos.x;
			absolutePosition.y = lerpPos.y;
			
		}
	}
	
//	public void HideJoystick()
//	{
//	}
//	
//	public void ShowJoystick( float x, float y )
//	{
//		Vector3 pos = new Vector3( x, y, 0 );
//		
//		joystick.localPosition = pos;
//		joystickBackground.localPosition = pos;
//	}
	
	
	void OnPress( bool isPress )
	{
		if( isPress )
		{
			mNpcHited = false;
			mPlayerHited = false;
			//放到了其他的控件上//
			if( UICamera.hoveredObject != mCachedGameObject )
				return;
			
			Vector3 position = Input.mousePosition;
			
			Ray ray = LayerManager.Instance.GameCamera.ScreenPointToRay( position );
			
//			int layerMask = 1 << LayerMask.NameToLayer("UnitLayer");
			
			RaycastHit[] hits = Physics.RaycastAll(ray,500f);
			RaycastHit hit;
			
			for (int i=0,len=hits.Length; i<len; i++)
			{
				hit = hits[i];
				
				//NPC//
				if(IsNpc(hit.collider))
				{
					//Debug.Log ("Is Npc");
					mHitObject = hit.collider.gameObject;
					mNpcHited = true;
					mPressPosition = position;
					return;
				}
				
				if(IsPlayer(hit.collider))
				{
					mPlayerHitObj = hit.collider.gameObject;
					mPlayerHited = true;
					//mMousePosition = hit.point;
					return;
				}

				if(IsTerrain(hit.collider) && !isDragging)
				{
					mTerrained = true;
					mMousePosition = hit.point;
				}
			}

//			int layerMask = 1 << LayerMask.NameToLayer("Terrain");
//
//			if(Physics.Raycast(ray,out hit,/*float.PositiveInfinity*/500f,layerMask))
//			{
//				//地图点击的操作
//				if( IsTerrain( hit.collider ) && !isDragging  )
//				{
//					mTerrained = true;
//					mMousePosition = hit.point;
//					//这里不能return， 不然会导致遥控控制不灵
//				}
//			}
			//触发,NPC操作//
			
			//重置摇杆的位置//
			
//			UICamera.MouseOrTouch touch = UICamera.currentTouch;
//			ray = UICamera.currentCamera.ScreenPointToRay( touch.pos );
//			Vector3 currentPos = ray.GetPoint( 0 );
//			currentPos.z = 0;
//			
//			if ( touch != null )
//			{
//				if ( joystick != null )
//				{
//					absolutePosition = currentPos;
//					joystick.position = currentPos;
//					joystickBackground.position = currentPos;
//					targetPosition = currentPos;
//				}
//			}
		}
		else
		{
			//如果可以点击到NPC//
			if ( mNpcHited )
			{
				HandleNpcClick();
				ResetNpcClick();
			}
			else if(mPlayerHited)
			{
				HandlePlayerClick();
				ResetPlayerClick();
			}
			else if( mTerrained && !isDragging )
			{
				HandleTerrainClick(mMousePosition);
				ResetTerrainClick();
				ResetJoystick();
				MainUIViewController.Instance.ShowPlayerInfoBtn(null);
			}
			else
			{
				if ( onDragEventEnd != null) {
					onDragEventEnd();
				}

				ResetJoystick();
				
			}
		}
	}
	
	bool IsNpc(Collider collider_)
	{
		if(collider_ == null) return false;
		return collider_.CompareTag(GameTag.Tag_Npc); //|| collider_.CompareTag("NpcDialogueBubble");
	}
	
	bool IsPlayer(Collider collider_)
	{
		if(collider_ == null) return false;
		return collider_.CompareTag(GameTag.Tag_Player);
	}
	
	bool IsTerrain( Collider collider_ )
	{
		if( collider_ == null || !gameObject.activeSelf )	return false;
		return collider_.CompareTag( GameTag.Tag_Terrain );
	}
	
	void HandleNpcClick()
	{
		if(JoystickModule.DisableMove) return;
		Vector3 position = Input.mousePosition;
		Vector3 delta = position - mPressPosition;

		if ( delta.sqrMagnitude > 1600f )  //40 * 40
		{
			return;
		}

		INpcUnit inNpcUnit = WorldManager.Instance.GetNpcViewManager().GetNpcUnit(mHitObject);
		if(inNpcUnit == null)
		{
			inNpcUnit = WorldManager.Instance.GetNpcViewManager().GetNpcUnit(mHitObject.transform.parent.parent.gameObject);
		}
		if(inNpcUnit == null)
		{
			return;
		}
		
		Npc npc = inNpcUnit.GetNpc();
		if(npc == null)
			return;
		if(npc is NpcDoubleTeleport)
		{
			return;
		}

		inNpcUnit.Trigger ();
		//MissionGuidePathFinder.Instance.FindNpc(npc);
	}
	
	private void HandlePlayerClick()
	{
		if(mPlayerHitObj != null)
		{
			PlayerView playerView = mPlayerHitObj.GetComponent<PlayerView>();
			if(playerView == null)
				return;

			if (playerView.GetPlayerDto().id != PlayerModel.Instance.GetPlayerId())
			{
				MainUIViewController.Instance.ShowPlayerInfoBtn(playerView.GetPlayerDto());
				string effpath = GameEffectConst.GetGameEffectPath (GameEffectConst.Effect_CharactorClick);
				OneShotSceneEffect.BeginFollowEffect (effpath, playerView.transform, 2f, 1f);
			}
			HandleTerrainClick(mMousePosition);
		}
	}
	
	private void HandleTerrainClick( Vector3 tempPostion )
	{
		if(JoystickModule.DisableMove) return;
		HeroView heroView = WorldManager.Instance.GetHeroView();
		if(heroView)
		{
			// 停止挂机状态
			PlayerModel.Instance.StopAutoFram();
			heroView.WalkToPoint( tempPostion );
			MissionDataModel.Instance.HeroCharacterControllerEnable(true);
		}

		ResourcePoolManager.Instance.Spawn(GameEffectConst.GetGameEffectPath(GameEffectConst.Effect_TerrainClick), 
		                                   delegate(UnityEngine.Object inst)
		                                   {
			GameObject go = inst as GameObject;
			if (go != null)
			{
				EffectTime effectTime = go.AddMissingComponent<EffectTime>();
				effectTime.time = 2;
				GameObjectExt.AddPoolChild(LayerManager.Instance.EffectsAnchor, go);
				PlayGroundClickEffect(go, tempPostion);
			}
			
		}, ResourcePoolManager.PoolType.DONT_DESTROY);
	}

	private void PlayGroundClickEffect(GameObject go, Vector3 tempPostion)
	{
		go.transform.position = tempPostion;
		go.transform.localScale = Vector3.one;
		go.SetActive (false);
		go.SetActive (true);
	}

	public void ResetJoystick()
	{
		if ( joystick == null )
			return;
		isDragging = false;
		joystick.position = Vector3.zero;
	}
	
	
	public void ResetNpcClick()
	{
		mHitObject = null;
		mNpcHited = false;
	}
	
	public void ResetPlayerClick()
	{
		mPlayerHitObj = null;
		mPlayerHited = false;
	}
	
	public void ResetTerrainClick()
	{
		mTerrained = false;
	}
	
	void UpdateJoystick()
	{
//		UICamera.MouseOrTouch touch = UICamera.currentTouch;
//		Ray ray = UICamera.currentCamera.ScreenPointToRay( touch.pos );
//		Vector3 currentPos = ray.GetPoint( 0 );
		Vector3 currentPos = UICamera.lastWorldPosition;
		currentPos.z = 0;
		
		float x = currentPos.x - absolutePosition.x;
		float y = currentPos.y - absolutePosition.y;
		
		angle = Mathf.Atan2( y, x );
		
		float revertAngle = angle + Mathf.PI;
		
		float magnitude = new Vector2( x, y ).magnitude;
		
		Vector3 tmpVec = collider.bounds.center;
		bottomRight = new Vector3( tmpVec.x + Screen.width * 0.5f, tmpVec.y - Screen.height * 0.5f, 0 );
		upperLeft = new Vector3( tmpVec.x - Screen.width * 0.5f, tmpVec.y + Screen.height  * 0.5f, 0 );

		if ( magnitude > radius )
		{
			lerpSpeed = 1.2f;
			
			magnitude = radius;
			
			x = currentPos.x + Mathf.Cos( revertAngle ) * magnitude;
			y = currentPos.y + Mathf.Sin( revertAngle ) * magnitude;
			
			if ( x < upperLeft.x )
				x = upperLeft.x;
			else if ( x > bottomRight.x )
				x = bottomRight.x;
			if ( y < bottomRight.y )
				y = bottomRight.y;
			else if ( y > upperLeft.y )
				y = upperLeft.y;
			
			targetPosition = new Vector3( x, y, 0 );
		}
	}
}