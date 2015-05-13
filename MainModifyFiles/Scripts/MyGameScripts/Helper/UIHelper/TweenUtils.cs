// **********************************************************************
// Copyright (c) 2013 Baoyugame. All rights reserved.
// File     :  TweenUtils.cs
// Author   : SK
// Created  : 2014/8/12
// Purpose  : 
// **********************************************************************
using UnityEngine;
using System.Collections;

public class TweenUtils
{
	public static void PlayScaleTween(GameObject go, float fromScale, bool forward,EventDelegate.Callback callBack=null,float duration=0.3f, float delay=0f , UITweener.Method  method = UITweener.Method.EaseOut )
	{
		TweenScale tweenScale = GameObjectExt.GetMissingComponent<TweenScale>(go);
		tweenScale.duration = duration;
		
		tweenScale.from = new Vector3(fromScale, fromScale, fromScale);
		tweenScale.to = go.transform.localScale;
		tweenScale.method = method;
		tweenScale.animationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f),new Keyframe(0.7f,1.2f,0f,0f), new Keyframe(1f, 1f, 1f, 0f));

		if (forward){
			go.transform.localScale = tweenScale.from;
		}else{
			go.transform.localScale = tweenScale.to;
		}

		tweenScale.delay = delay;
		
		tweenScale.SetOnFinished(callBack);
		tweenScale.Play(forward);
	}
	
	//itween下的缩放效果//
	public static void PlayScaleByITween( GameObject go, Vector3 fromScal, Vector3 toScal, GameObject completeTarget = null ,  string completeFunction = ""   ,float duration=0.3f, float delay=0f   , iTween.EaseType easeType = iTween.EaseType.linear  )
	{
		go.transform.localScale = fromScal;
		if( string.IsNullOrEmpty( completeFunction ) )
		{
			iTween.ScaleTo(go, iTween.Hash("x", toScal.x, "y", toScal.y, "z", toScal.z, "time", duration, "easetype", easeType , "isLocal" , true));
		}
		else
		{
			iTween.ScaleTo(go, iTween.Hash("x", toScal.x, "y", toScal.y, "z", toScal.z, "time", duration, "easetype", easeType, "oncompletetarget" , completeTarget ,  "oncomplete", completeFunction  , "isLocal" , true));
		}
		
	}

	public static void PlayPositionTween(GameObject go, Vector3 fromPos, Vector3 toPos, EventDelegate.Callback callBack=null,float duration=0.3f, float delay=0f, UITweener.Method method = UITweener.Method.Linear )
	{
		go.transform.localPosition = fromPos;

		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(go, duration);
		tweenPosition.from = fromPos;
		tweenPosition.to = toPos;
		tweenPosition.delay = delay;
		tweenPosition.method = method;
		tweenPosition.SetOnFinished(callBack);
		tweenPosition.Play(true);
	}
		
	//这个是itween的效果，支持较多的method-------------------------------------------     //
	public static void PlayPositionByItween( GameObject go , Vector3 toPos  , GameObject completeTarget = null ,  string completeFunction = "" , float duration = 0.3f , float delay = 0f , iTween.EaseType easeType = iTween.EaseType.linear , bool isLocal = true  )
	{
		if( string.IsNullOrEmpty ( completeFunction ) )
		{
			iTween.MoveTo(go, iTween.Hash("x", toPos.x, "y", toPos.y, "z", toPos.z, "time", duration, "easetype", easeType,  "isLocal" , true));
		}
		else
		{
			iTween.MoveTo(go, iTween.Hash("x", toPos.x, "y", toPos.y, "z", toPos.z, "time", duration, "easetype", easeType, "oncompletetarget" , completeTarget ,  "oncomplete", completeFunction , "isLocal" , true));
		}
		
	}
	
	//---------------------------------------------------------------------------------
	
	//透明度，需要控件上面存在着UIPanel//
	public static void PlayAlphaTween( GameObject panelObj , float fromAlpha = 1f , float toAlpha = 0 , EventDelegate.Callback callBackFinish = null ,  float duration = 0.3f , float delay = 0f , UITweener.Method method = UITweener.Method.Linear  )
	{
		TweenAlpha tweenAlpha = UITweener.Begin< TweenAlpha >( panelObj , duration );
		tweenAlpha.from = fromAlpha;
		tweenAlpha.to = toAlpha;
		tweenAlpha.delay = delay;
		tweenAlpha.method = method;
		tweenAlpha.SetOnFinished(callBackFinish);
		tweenAlpha.Play( true );
	}

	//	主界面UIBtn动画效果
	public static void MainUIbtnScale(GameObject go,
	                                  float fromScale, float toScale,
	                                  float duration,
	                                  EventDelegate.Callback callBack = null,
	                                  UITweener.Method method = UITweener.Method.BounceIn,
	                                  float delay = 0f)
	{
		TweenScale tweenScale = GameObjectExt.GetMissingComponent<TweenScale>(go);
		tweenScale.duration = duration;
		
		tweenScale.from = new Vector3(fromScale, fromScale, fromScale);
		tweenScale.to = new Vector3(toScale, toScale, toScale);
		tweenScale.method = method;
		
		tweenScale.animationCurve = new AnimationCurve(
			new Keyframe(0f,0f),
			new Keyframe(0.5f,1.25f),
			new Keyframe(1f,0f)
			);
		
		tweenScale.delay = delay;
		
		tweenScale.SetOnFinished(callBack);
		tweenScale.Toggle();
	}
}

