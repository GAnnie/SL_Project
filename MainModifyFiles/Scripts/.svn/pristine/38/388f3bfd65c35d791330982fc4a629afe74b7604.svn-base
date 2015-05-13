using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.mail.dto;
using com.nucleus.h1.logic.core.modules;

public class EmailModel : MonoBehaviour {
	
	private static readonly EmailModel instance = new EmailModel();
	public static EmailModel Instance
	{
		get
		{
			return instance;
		}
	}


	//邮件列表
	private List<MailDto> MailList = new List<MailDto>();


	private GiftDto hasGiveInfo;


	#region 从服务器获得邮件数据
	public void GetMailListFromServer(){
		ServiceRequestAction.requestServer (MailService.mine (), "Get mail", delegate(com.nucleus.commons.message.GeneralResponse e) {

			DataList dataList = e as DataList;
			List<MailDto> mailDtoList = new List<MailDto>(dataList.items.Count);
			for(int i=0;i<dataList.items.Count;++i){
				mailDtoList.Add(dataList.items[i] as MailDto);
			}

			SetMailList(mailDtoList);
		});
	}
	#endregion

	public void SetMailList(List<MailDto> _MailList){
		MailList = _MailList;
	}

	#region 邮件通知 
	public void AddMailFromNotify(MailDto notify){
		MailList.Add (notify);
	}
	#endregion

	#region 获得邮件列表
	public List<MailDto> GetMailList(){
		if (MailList == null) {
			GetMailListFromServer();		
		}
		return MailList;
	}
	#endregion

	#region 标记已读此邮件
	public void MarkThisMail(long id){
		ServiceRequestAction.requestServer (MailService.mark (id), "mark this mail");
	}
	#endregion

	#region 提取附件
	public void GetReward(long id){
		ServiceRequestAction.requestServer (MailService.extract (id), "Get Reward from this mail",delegate {
			MarkThisMail(id);
	}
	 ,(e)=>Fail(e.ToString()));
	}
	#endregion


//	#region 删除此邮件
//	public void RemoveMail(string id){
//		ServiceRequestAction.requestServer (MailService.remove (id), "Remove these mails",
//		 (e)=>Success(e.ToString()),
//		(e)=>Fail(e.ToString()));
//	}
//	#endregion


	#region 赠送
	public void GiveThings(long toId, string things){
		ServiceRequestAction.requestServer (MailService.gift(toId,things), "give these things to sb.");
	}
	#endregion

	public void Success(string str){
//		TipManager.AddTip (str);
//		Debug.LogError (str);
//		GetMailListFromServer ();
	}
	public void Fail(string str){
		Debug.LogError (str);
//		TipManager.AddTip (str);
	}

	#region  保存玩家已赠送的数量 和价值信息
	public void SetCount_Value(GiftDto giftDto){
		hasGiveInfo = giftDto;
	}

	#endregion

	#region 返回玩家已赠送的数量 和价值信息
	public GiftDto GetCount_Value(){
		return hasGiveInfo;
	}
	#endregion

	#region 登陆的时候拿数据
	public void Setup(){
		GetMailListFromServer ();
	}
	#endregion

}
