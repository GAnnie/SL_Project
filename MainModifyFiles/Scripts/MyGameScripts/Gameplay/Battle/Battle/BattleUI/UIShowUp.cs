using UnityEngine;
using System.Collections;

public class UIShowUp : MonoBehaviour
{
	public enum TweenType
	{
		SCALE,
		MOVE,
	}
	
	[System.Serializable]
	public class TweenNode
	{
		public TweenType type;
		public Vector3 from = new Vector3( 0, -512.8798f, 0 );
		public Vector3 to = new Vector3( 0, 0, 0 );
		public float time = 2.0f;
		public iTween.EaseType easeType;
	}
		
	public TweenNode[] tweenNode;
	private int tweenIdx = -1;
	
	public Vector3 StartScale = Vector3.one;
	
	public bool destroyWhenFinished = true;
	
	public delegate void FinishedEvent();
	private FinishedEvent finishedEvent;
	
	public void Show( FinishedEvent _finishedEvent )
	{
		finishedEvent = _finishedEvent;
		
		transform.localScale = StartScale;
		
		tweenIdx = -1;
		NextTween();
	}
	
	public void DestroyGameObject()
	{
		Destroy( gameObject );
	}
	
	public void MoveAnimation( TweenNode node )
	{
		transform.localPosition = node.from;
		
		iTween.MoveTo( gameObject, iTween.Hash( "x", node.to.x, "y", node.to.y, "z", node.to.z, "time", node.time,
										"easetype", node.easeType, "isLocal", true,  "oncomplete", "NextTween" ) );
	}
	
	private Vector3 TransformNodeScale( Vector3 node )
	{
		float x = node.x == 0 ? 0.001f : node.x;
		float y = node.y == 0 ? 0.001f : node.y;
		float z = node.z == 0 ? 0.001f : node.z;
		
		return new Vector3( x,y,z );
	}
	
	public void ScaleAnimation( TweenNode node )
	{
		transform.localScale = TransformNodeScale ( node.from );
		
		Vector3 targetPosition = TransformNodeScale( node.to );
		
		iTween.ScaleTo( gameObject, iTween.Hash( "x", targetPosition.x, "y", targetPosition.y, "z", targetPosition.z, "time", node.time,
										"easetype", node.easeType, "isLocal", true,  "oncomplete", "NextTween" ) );
	}
	
	void NextTween()
	{
		++tweenIdx;
		
		if ( tweenIdx >= tweenNode.Length )
		{
			if ( tweenIdx == tweenNode.Length )
			{
			
				if ( finishedEvent != null )
				{
					finishedEvent();
					finishedEvent = null;
				}
				
				if ( destroyWhenFinished )
					DestroyGameObject();
			}
			
			return;
		}
		
		TweenNode node = tweenNode[tweenIdx];
		
		if ( node.type == TweenType.MOVE )
			MoveAnimation( node );
		else if ( node.type == TweenType.SCALE )
			ScaleAnimation( node );
	}
}
