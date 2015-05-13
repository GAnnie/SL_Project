using UnityEngine;
using System.Collections;

public class AutoPlayVoiceController : MonoBehaviour {


	#region 自动播发语音
	float VoicePlayingWaitingtimer;
	public void PlayVoice(string str){
		//锁住
		AutoPlayVoiceModel.Instance.SetIsPlaying(true);

		VoiceRecognitionManager.Instance.GetVoiceInQiniu(str,delegate(AudioClip obj) {
			
			if( obj != null )
			{
				VoicePlayingWaitingtimer = obj.length;
				VoiceRecognitionManager.Instance.PlayQiniuSoundByClip( obj );
			}
			else
			{
				TipManager.AddTip( "获取不到音频" +  str);
			}

			StartCoroutine("PlayingVoice");

		});

	}
	
	IEnumerator PlayingVoice() {
		yield return null;
		
		string tName = "autoPlayVoice";

		CoolDownManager.Instance.SetupCoolDown(tName, VoicePlayingWaitingtimer, null, PlayingVoiceEnd);
	}
	
	private void PlayingVoiceEnd ()  {
		
		CoolDownManager.Instance.SetupCoolDown("AutoPlayVoiceStopOneSceond", 1f,null, delegate {

			AutoPlayVoiceModel.Instance.SetIsPlaying(false);

			AutoPlayVoiceModel.Instance.JudgeVoiceList();
		});
		
	}
	#endregion
}
