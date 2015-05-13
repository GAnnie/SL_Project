using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MarqueeModel : MonoBehaviour {
	
	
	private static readonly MarqueeModel instance = new MarqueeModel();
	public static MarqueeModel Instance
	{
		get{
			return instance;
		}
	}

	public event System.Action<string> ShowMarquee;

	List < string > sysMarquee = new List < string >();
	List < string > hearMarquee = new List<string>();

	public void AddSys(string str){

		sysMarquee.Add(str);
		JudgeMessageList();

	}

	public void AddHear(string str){
		if(hearMarquee.Count > 10){
			hearMarquee.RemoveAt(0);
		}
		hearMarquee.Add(str);
		JudgeMessageList();
	}

	
	/**
	 * 是否是在跑马灯展示中
	 */
	private bool _isShowingMarquee = false;

	public void SetIsShowing(bool b){
		_isShowingMarquee = b;
	}

	/**
	 * 判断是否可以激活跑马灯
	 */
	public void JudgeMessageList()
	{
		if( sysMarquee.Count > 0 )
		{
			if(!_isShowingMarquee){
				ShowMarqueeView( sysMarquee[ 0 ]  );
				sysMarquee.RemoveAt(0);
			}
		}

		else if(hearMarquee.Count >0){
			if(!_isShowingMarquee){
				ShowMarqueeView( hearMarquee[ 0 ]  );
				hearMarquee.RemoveAt(0);
			}
		}
		else 
		{
			ShowMarqueeView( null );
		}
	}
	
	/**
	 * 激活跑马灯
	 */
	public void ShowMarqueeView( string  msg )
	{
		if(string.IsNullOrEmpty(msg)) return;

		if( ShowMarquee != null )
		{
			ShowMarquee(msg);
		}
	}




}
