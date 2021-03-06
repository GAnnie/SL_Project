﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using LITJson;
using System;
using System.IO;
using System.Linq;

public class StoryEditor : EditorWindow {

	[MenuItem ("Window/StoryEditor")]
	public static void  ShowWindow ()
	{     
//		var window = EditorWindow.GetWindow<StoryEditor> (false, "StoryEditor", true);
		Rect  wr = new Rect (0,0,1000,700);
		StoryEditor window = (StoryEditor)EditorWindow.GetWindowWithRect (typeof (StoryEditor),wr,true,"story");	
		window.Show ();
	}

	
	List < StoryInfo > storyList = new List<StoryInfo>();



	void OnGUI(){

		EditorGUILayout.BeginHorizontal();
		DrawMenu();     //top
		EditorGUILayout.EndHorizontal();
		
		
		EditorGUILayout.BeginHorizontal();
		DrawContent();      //middle
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal ();
		DrawTimeLine ();
		EditorGUILayout.EndHorizontal ();


	}

	#region 菜单

	int storyid;         //剧情id
	int storySceneId;      //场景id
	float storyDelayTime;       //剧情时长
//	bool bstory =false;   //是否显示附属菜单
	int curStoryId = -1;      //当前点击的是哪个剧情
	enum NPCAcion{

		Talk,
		Move,
		Anim,
		Turn,
		Eff,

	}

	enum ScreenType{
		Shock,
		Mask,
		Presure,
	}



	NPCAcion npcs = NPCAcion.Talk;
	ScreenType scrType = ScreenType.Shock;
	public void DrawMenu(){
		EditorGUILayout.BeginVertical();
		EditorGUILayout.BeginHorizontal(GUILayout.Width(150f));
		
		if(GUILayout.Button("NewStory")){
			curInst = -1;
			StoryInfo story = new StoryInfo();
			story.instList = new List<BaseStoryInst>();

			storyList.Add(story);


		}

		if (GUILayout.Button ("Save")) {
			Save();	
		}
		if (GUILayout.Button ("Load")) {
			curInst = -1;
			Load();
		}

		EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
	}
	#endregion


	#region 中间
	public Vector2 scr;
	public Vector2 scr1;
	public Vector2 scr2;
	Color matColor  = Color.white;
	public void DrawContent(){


		EditorGUILayout.BeginHorizontal(GUILayout.Width(100f),GUILayout.Height(300f));
		scr = EditorGUILayout.BeginScrollView (scr, "TextField");
		for(int i = 0 ; i < storyList.Count; i++){
			EditorGUILayout.BeginHorizontal();

			if(GUILayout.Button(storyList[i].id.ToString())){
				curStoryId = i;
//				UpdateIDAndNpcId();
				EditorGUIUtility.editingTextField = false;

			}
			GUI.color = Color.red;
			if(GUILayout.Button("X",GUILayout.Width(20f))){
				curInst = -1;
				curStoryId = -1;
				storyList.RemoveAt(i);
				storyid--;
				EditorGUIUtility.editingTextField = false;
			}
			GUI.color = Color.white;
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndScrollView ();
		EditorGUILayout.EndHorizontal();






		EditorGUILayout.BeginVertical();




		EditorGUILayout.BeginHorizontal();


		if(curStoryId != -1 && storyList.Count >0){
			storyList[curStoryId].id =  EditorGUILayout.IntField("ID:",storyList[curStoryId].id);
			storyList[curStoryId].sceneid = EditorGUILayout.IntField("场景ID:",storyList[curStoryId].sceneid);
			storyList[curStoryId].delayTime = EditorGUILayout.FloatField("时长:",storyList[curStoryId].delayTime);
		}
		EditorGUILayout.EndHorizontal();


		EditorGUILayout.BeginHorizontal ();
		scr1 = EditorGUILayout.BeginScrollView (scr1,"TextField",GUILayout.Width(300f),GUILayout.MaxHeight(275f));
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Npc")){
			if(curStoryId != -1){
				addNpcAppear();
			}
				
		}
		EditorGUILayout.EndHorizontal ();

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("镜头")) {
			TransLation();
		}
		EditorGUILayout.EndHorizontal ();




		EditorGUILayout.BeginVertical ();

		if (GUILayout.Button ("特效")) {
			EffAppearInst inst = new EffAppearInst();
			inst.type = EffAppearInst.TYPE;
			storyList [curStoryId].instList.Add (inst);
		}
		if (GUILayout.Button ("音乐")) {
			MusicPlayInst inst = new MusicPlayInst();
			inst.type = MusicPlayInst.TYPE;
			storyList [curStoryId].instList.Add (inst);
		}
		if (GUILayout.Button ("音效")) {
			AudioPlayInst inst = new AudioPlayInst();
			inst.type = AudioPlayInst.TYPE;
			storyList [curStoryId].instList.Add (inst);
		}

		EditorGUILayout.BeginHorizontal ();
		GUILayout.Box ("屏幕:");
		scrType = (ScreenType)EditorGUILayout.EnumPopup (scrType, GUILayout.Width (50f));
		if (GUILayout.Button ("Add",GUILayout.Width(50f))) {
			createScreenAction();
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndVertical ();

		EditorGUILayout.EndScrollView ();
//		EditorGUILayout.EndHorizontal ();
//
//
//
//
//		EditorGUILayout.BeginHorizontal();

		if (curInst != -1 && storyList.Count >0&& curStoryId != -1) {
			if(storyList[curStoryId].instList[curInst].type.Equals(NpcAppearInst.TYPE) ){

				NpcAppearInst inst = storyList[curStoryId].instList[curInst] as NpcAppearInst;
				List<BaseStoryInst> InstList = new List<BaseStoryInst>();
				for(int k = 0; k <storyList[curStoryId].instList.Count; k ++){
//					if(storyList[curStoryId].instList[k].type.Equals("NpcAppearInst") && (storyList[curStoryId].instList[k] as  NpcAppearInst).npcid ==inst.npcid
//					   || storyList[curStoryId].instList[k].type.Equals("NpcTalkInst")&& (storyList[curStoryId].instList[k] as  NpcTalkInst).npcid ==inst.npcid
//					   || storyList[curStoryId].instList[k].type.Equals("NpcMoveInst")&& (storyList[curStoryId].instList[k] as  NpcMoveInst).npcid ==inst.npcid
//					   || storyList[curStoryId].instList[k].type.Equals("NpcActionInst")&& (storyList[curStoryId].instList[k] as  NpcActionInst).npcid ==inst.npcid
//					   || storyList[curStoryId].instList[k].type.Equals("NpcDeleteInst")&& (storyList[curStoryId].instList[k] as  NpcDeleteInst).npcid ==inst.npcid){
//						InstList.Add(storyList[curStoryId].instList[k]);
//					}
					if(storyList[curStoryId].instList[k].type.Equals(NpcAppearInst.TYPE) && (storyList[curStoryId].instList[k] as  NpcAppearInst).npcid ==inst.npcid
					   || storyList[curStoryId].instList[k].type.Equals(NpcTalkInst.TYPE)&& (storyList[curStoryId].instList[k] as  NpcTalkInst).npcid ==inst.npcid ){
						InstList.Add(storyList[curStoryId].instList[k]);
					}
				}

				InstList.Sort((a,b)=>a.id.CompareTo(b.id));
				scr2 = EditorGUILayout.BeginScrollView (scr2,"TextField",GUILayout.Width(300f),GUILayout.MaxHeight(275f));

				inst.npcid = EditorGUILayout.IntField("npcid",inst.npcid);
				inst.model = EditorGUILayout.IntField("模型",inst.model);
				inst.name = EditorGUILayout.TextField("名字:",inst.name);
				inst.posX = EditorGUILayout.FloatField("初始位置X:",inst.posX);
//				inst.posY = EditorGUILayout.FloatField("初始位置Y:",inst.posY);
				inst.posZ = EditorGUILayout.FloatField("初始位置Z:",inst.posZ);
				inst.rotY = EditorGUILayout.FloatField("朝向:",inst.rotY);
				inst.startTime = EditorGUILayout.FloatField("出现时间:",inst.startTime);
				inst.defaultAnim = EditorGUILayout.TextField("默认动作",inst.defaultAnim);
				inst.copyHero =  EditorHelper.DrawToggle("匹配:",inst.copyHero);
				inst.scaleX = EditorGUILayout.FloatField("模型大小X:",inst.scaleX);
				inst.scaleY = EditorGUILayout.FloatField("模型大小Y:",inst.scaleY);
				inst.scaleZ = EditorGUILayout.FloatField("模型大小Z:",inst.scaleZ);

				for(int j = 0; j <InstList.Count; j++){
					if(InstList[j].type.Equals("NpcTalkInst")){
						GUILayout.Box("说话");
						NpcTalkInst talk = InstList[j] as NpcTalkInst;
//						talk.npcid = EditorGUILayout.IntField("npcid",inst.npcid);
						talk.talkStr = EditorGUILayout.TextField("内容:",talk.talkStr);
						talk.startTime = EditorGUILayout.FloatField("开始时间:",talk.startTime);
						talk.offY = EditorGUILayout.FloatField("气泡高度:",talk.offY);
						talk.existTime = EditorGUILayout.FloatField("气泡存在时长:",talk.existTime);
					}
//					if(InstList[j].type.Equals("NpcMoveInst")){
//						NpcMoveInst move = InstList[j] as NpcMoveInst;
//						//						move.npcid = EditorGUILayout.IntField("npcid",inst.npcid);
//						GUILayout.Box("移动");
//						move.goPosX = EditorGUILayout.FloatField("要去的位置X:",move.goPosX);
//						move.goPosZ = EditorGUILayout.FloatField("要去的位置Z:",move.goPosZ);
//						move.startTime = EditorGUILayout.FloatField("开始时间:",move.startTime);
//					}
//					if(InstList[j].type.Equals("NpcActionInst")){
//						NpcActionInst ac = InstList[j] as NpcActionInst;
//						//						ac.npcid = EditorGUILayout.IntField("npcid:",inst.npcid);
//						GUILayout.Box("动作");
//						ac.anim = EditorGUILayout.TextField("动作名称:",ac.anim);
//						ac.startTime = EditorGUILayout.FloatField("开始时间:",ac.startTime);
//					}
//					
//					if(InstList[j].type.Equals("NpcDeleteInst")){
//						NpcDeleteInst del = InstList[j] as NpcDeleteInst;
//						GUILayout.Box("删除");
//						//						del.npcid = EditorGUILayout.IntField("npcid:",inst.npcid);
//						del.startTime = EditorGUILayout.FloatField("开始时间:",del.startTime);
//					}
				}

				EditorGUILayout.BeginHorizontal();
				npcs = (NPCAcion)EditorGUILayout.EnumPopup (npcs,GUILayout.Width(50f));
				if(GUILayout.Button("Add",GUILayout.Width(50f))){
					if(curStoryId != -1)
						CreateNpcInst();
				}
				EditorGUILayout.EndHorizontal ();
				EditorGUILayout.EndScrollView ();

			}
			else if(storyList[curStoryId].instList[curInst].type.Equals(NpcTalkInst.TYPE)){
				scr2 = EditorGUILayout.BeginScrollView (scr2,"TextField",GUILayout.Width(300f),GUILayout.MaxHeight(275f));
				NpcTalkInst inst = storyList[curStoryId].instList[curInst] as NpcTalkInst;
//				inst.npcid = EditorGUILayout.IntField("npcid",inst.npcid);
				inst.talkStr = EditorGUILayout.TextField("内容:",inst.talkStr);
				inst.startTime = EditorGUILayout.FloatField("开始时间:",inst.startTime);
				inst.offY = EditorGUILayout.FloatField("气泡高度:",inst.offY);
				inst.existTime = EditorGUILayout.FloatField("气泡存在时长:",inst.existTime);
				EditorGUILayout.EndScrollView ();
			}
			else if(storyList[curStoryId].instList[curInst].type.Equals(NpcMoveInst.TYPE)){
				scr2 = EditorGUILayout.BeginScrollView (scr2,"TextField",GUILayout.Width(300f),GUILayout.MaxHeight(275f));
				NpcMoveInst inst = storyList[curStoryId].instList[curInst] as NpcMoveInst;
//				inst.npcid = EditorGUILayout.IntField("npcid",inst.npcid);
				inst.goPosX = EditorGUILayout.FloatField("要去的位置X:",inst.goPosX);
				inst.goPosZ = EditorGUILayout.FloatField("要去的位置Z:",inst.goPosZ);
				inst.startTime = EditorGUILayout.FloatField("开始时间:",inst.startTime);
				EditorGUILayout.EndScrollView ();
			}

			else if(storyList[curStoryId].instList[curInst].type.Equals(EffAppearInst.TYPE)){
				scr2 = EditorGUILayout.BeginScrollView (scr2,"TextField",GUILayout.Width(300f),GUILayout.MaxHeight(275f));
				EffAppearInst inst = storyList[curStoryId].instList[curInst] as EffAppearInst;
				inst.effPath = EditorGUILayout.TextField("类型:",inst.effPath);
				inst.delayTime = EditorGUILayout.FloatField("持续时间:",inst.delayTime);
				inst.startTime = EditorGUILayout.FloatField("开始时间:",inst.startTime);
				inst.isFullScreen = EditorHelper.DrawToggle("全屏:",inst.isFullScreen);
				if(!inst.isFullScreen){
					inst.posX = EditorGUILayout.FloatField("位置X:",inst.posX);
					inst.posY = EditorGUILayout.FloatField("位置Y:",inst.posY);
					inst.posZ = EditorGUILayout.FloatField("位置Z:",inst.posZ);
				}
				EditorGUILayout.EndScrollView ();
			}

			else if(storyList[curStoryId].instList[curInst].type.Equals(CameraTranslationInst.TYPE)){
				scr2 = EditorGUILayout.BeginScrollView (scr2,"TextField",GUILayout.Width(300f),GUILayout.MaxHeight(275f));
				CameraTranslationInst inst = storyList[curStoryId].instList[curInst] as CameraTranslationInst;
				inst.startTime = EditorGUILayout.FloatField("开始时间:",inst.startTime);
				inst.delayTime = EditorGUILayout.FloatField("持续时间:",inst.delayTime);
				inst.posX = EditorGUILayout.FloatField("位置X:",inst.posX);
				inst.posY = EditorGUILayout.FloatField("位置Y:",inst.posY);
				inst.posZ = EditorGUILayout.FloatField("位置Z:",inst.posZ);
				EditorGUILayout.EndScrollView ();
			}

			else if(storyList[curStoryId].instList[curInst].type.Equals(ScreenShockInst.TYPE)){
				scr2 = EditorGUILayout.BeginScrollView (scr2,"TextField",GUILayout.Width(300f),GUILayout.MaxHeight(275f));
				ScreenShockInst inst = storyList[curStoryId].instList[curInst] as ScreenShockInst;
				inst.startTime = EditorGUILayout.FloatField("开始时间:",inst.startTime);
				inst.delayTime = EditorGUILayout.FloatField("持续时间:",inst.delayTime );
				inst.sensitive = EditorGUILayout.FloatField("摇晃程度:",inst.sensitive);
				EditorGUILayout.EndScrollView ();
			}

			else if(storyList[curStoryId].instList[curInst].type.Equals(ScreenMaskInst.TYPE)){
				scr2 = EditorGUILayout.BeginScrollView (scr2,"TextField",GUILayout.Width(300f),GUILayout.MaxHeight(275f));
				ScreenMaskInst inst = storyList[curStoryId].instList[curInst] as ScreenMaskInst;
				inst.startTime = EditorGUILayout.FloatField("开始时间:",inst.startTime);
				inst.delayTime = EditorGUILayout.FloatField("持续时间:",inst.delayTime );

				matColor = EditorGUI.ColorField(new Rect(0,150,250,25),"颜色值和透明度:",matColor);

				inst.colorR = matColor.r * 255.0f;
				inst.colorG = matColor.g * 255.0f;
				inst.colorB = matColor.b * 255.0f;
				inst.alpha = matColor.a * 255.0f;


				inst.fade = EditorHelper.DrawToggle("淡入淡出:",inst.fade);
				if(inst.fade){
					inst.fadeTime = EditorGUILayout.FloatField("淡入淡出时间:",inst.fadeTime);
				}
				EditorGUILayout.EndScrollView ();
			}

			else if(storyList[curStoryId].instList[curInst].type.Equals(ScreenPresureInst.TYPE)){
				scr2 = EditorGUILayout.BeginScrollView (scr2,"TextField",GUILayout.Width(300f),GUILayout.MaxHeight(275f));
				ScreenPresureInst inst = storyList[curStoryId].instList[curInst] as ScreenPresureInst;
				inst.startTime = EditorGUILayout.FloatField("开始时间:",inst.startTime);
				inst.delayTime = EditorGUILayout.FloatField("持续时间:",inst.delayTime );
				inst.needTime = EditorGUILayout.FloatField("压屏时间:",inst.needTime);
				inst.length = EditorGUILayout.FloatField("压屏的长度:",inst.length);
//				inst.fade = EditorHelper.DrawToggle("淡入淡出:",inst.fade);
				EditorGUILayout.EndScrollView ();
			}

			else if(storyList[curStoryId].instList[curInst].type.Equals(MusicPlayInst.TYPE)){
				scr2 = EditorGUILayout.BeginScrollView (scr2,"TextField",GUILayout.Width(300f),GUILayout.MaxHeight(275f));
				MusicPlayInst inst = storyList[curStoryId].instList[curInst] as MusicPlayInst;
				inst.startTime = EditorGUILayout.FloatField("开始时间:",inst.startTime);
				inst.delayTime = EditorGUILayout.FloatField("持续时间:",inst.delayTime );
				inst.musicPath = EditorGUILayout.TextField("文件路径:",inst.musicPath);
				EditorGUILayout.EndScrollView ();
			}

			else if(storyList[curStoryId].instList[curInst].type.Equals(AudioPlayInst.TYPE)){
				scr2 = EditorGUILayout.BeginScrollView (scr2,"TextField",GUILayout.Width(300f),GUILayout.MaxHeight(275f));
				AudioPlayInst inst = storyList[curStoryId].instList[curInst] as AudioPlayInst;
				inst.startTime = EditorGUILayout.FloatField("开始时间:",inst.startTime);
//				inst.delayTime = EditorGUILayout.FloatField("持续时间:",inst.delayTime );
				inst.audioPath = EditorGUILayout.TextField("文件路径:",inst.audioPath);
				EditorGUILayout.EndScrollView ();
			}
			else if(storyList[curStoryId].instList[curInst].type.Equals(NpcDeleteInst.TYPE)){
				scr2 = EditorGUILayout.BeginScrollView (scr2,"TextField",GUILayout.Width(300f),GUILayout.MaxHeight(275f));
				NpcDeleteInst inst = storyList[curStoryId].instList[curInst] as NpcDeleteInst;
				inst.npcid = EditorGUILayout.IntField("npcid:",inst.npcid);
				inst.startTime = EditorGUILayout.FloatField("开始时间:",inst.startTime);
				EditorGUILayout.EndScrollView ();
			}
			else if(storyList[curStoryId].instList[curInst].type.Equals(NpcActionInst.TYPE)){
				scr2 = EditorGUILayout.BeginScrollView (scr2,"TextField",GUILayout.Width(300f),GUILayout.MaxHeight(275f));
				NpcActionInst inst = storyList[curStoryId].instList[curInst] as NpcActionInst;
				inst.npcid = EditorGUILayout.IntField("npcid:",inst.npcid);
				inst.anim = EditorGUILayout.TextField("动作名称:",inst.anim);
				inst.startTime = EditorGUILayout.FloatField("开始时间:",inst.startTime);
				EditorGUILayout.EndScrollView ();
			}

			else if(storyList[curStoryId].instList[curInst].type.Equals(NpcTurnaroundInst.TYPE)){
				scr2 = EditorGUILayout.BeginScrollView (scr2,"TextField",GUILayout.Width(300f),GUILayout.MaxHeight(275f));
				NpcTurnaroundInst inst = storyList[curStoryId].instList[curInst] as NpcTurnaroundInst;
				inst.npcid = EditorGUILayout.IntField("npcid:",inst.npcid);
				inst.startTime = EditorGUILayout.FloatField("开始时间:",inst.startTime);
				inst.TurnAngle = EditorGUILayout.FloatField("旋转角度:",inst.TurnAngle);
				inst.turnSpeed = EditorGUILayout.FloatField("旋转速度:",inst.turnSpeed);
				EditorGUILayout.EndScrollView ();
			}

			else if(storyList[curStoryId].instList[curInst].type.Equals(NpcEffInst.TYPE)){
				scr2 = EditorGUILayout.BeginScrollView (scr2,"TextField",GUILayout.Width(300f),GUILayout.MaxHeight(275f));
				NpcEffInst inst = storyList[curStoryId].instList[curInst] as NpcEffInst;
				inst.npcid = EditorGUILayout.IntField("npcid:",inst.npcid);
				inst.NpcEffPath = EditorGUILayout.TextField("类型",inst.NpcEffPath);
				inst.startTime = EditorGUILayout.FloatField("开始时间:",inst.startTime);
				inst.delayTime = EditorGUILayout.FloatField("持续时间:",inst.delayTime);
				EditorGUILayout.EndScrollView ();
			}


		}
		EditorGUILayout.EndHorizontal ();



		EditorGUILayout.EndVertical ();







	}
	#endregion

	#region 时间线
	Vector2 timeScr;
	int curInst = -1;
	int curNpcId = 1;
	Dictionary <int,List< BaseStoryInst>> dicnn = new Dictionary<int, List<BaseStoryInst>>();
	public void DrawTimeLine(){
		if (curStoryId != -1 && storyList.Count> 0) {

			dicnn.Clear();

			float width = 911.0f/storyList [curStoryId].delayTime;
			EditorGUILayout.BeginVertical ();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Button ("",GUILayout.Height(5f));
			EditorGUILayout.EndHorizontal();

			timeScr = EditorGUILayout.BeginScrollView(timeScr,"TextField");               //最下面的显示区域开始

//			
			List <BaseStoryInst> list = new List<BaseStoryInst>();            
			for(int index = 0 ; index < storyList[curStoryId].instList.Count;index++){                  //把当前剧情的Npc相关的都装进list
				if(storyList[curStoryId].instList[index].type.Equals(NpcAppearInst.TYPE) 
				   || storyList[curStoryId].instList[index].type.Equals(NpcTalkInst.TYPE)
				   || storyList[curStoryId].instList[index].type.Equals(NpcMoveInst.TYPE)
				   || storyList[curStoryId].instList[index].type.Equals(NpcActionInst.TYPE)
				   || storyList[curStoryId].instList[index].type.Equals(NpcTurnaroundInst.TYPE)
				   || storyList[curStoryId].instList[index].type.Equals(NpcEffInst.TYPE)){
					list.Add(storyList[curStoryId].instList[index]);
				}
			}
			//|| storyList[curStoryId].instList[index].type.Equals("NpcDeleteInst")) 此处把删除的指令最后添加
			for(int index1 = 0 ; index1 < storyList[curStoryId].instList.Count;index1++){                //删除的指令最后添加
				if(storyList[curStoryId].instList[index1].type.Equals(NpcDeleteInst.TYPE)){
					list.Add(storyList[curStoryId].instList[index1]);
				} 
			}

			(from l in list
				group l by (l as BaseNpcInst).npcid into g select g).ToList().ForEach(g=>{             // 把NPc的指令按照NPc的Id分组
				foreach(BaseStoryInst baseStoryInst in g){
					if (dicnn.ContainsKey(((baseStoryInst as BaseNpcInst).npcid))) {
						dicnn[(baseStoryInst as BaseNpcInst).npcid].Add(baseStoryInst);
					} else {
						List<BaseStoryInst> tList = new List<BaseStoryInst>();
						tList.Add(baseStoryInst);
						dicnn.Add((baseStoryInst as BaseNpcInst).npcid, tList);
					}
				}
			}
			);

			foreach(List<BaseStoryInst> biv  in dicnn.Values){                        //显示Npc的指令

				for(int j = 0 ; j <biv.Count;j++){

					if(biv[j] is NpcAppearInst){
						GUILayout.BeginHorizontal();
						int id = (biv[j] as NpcAppearInst).npcid;
						GUILayout.Space((biv[j] as NpcAppearInst).startTime * width);
						GUI.color = Color.green;
						if(GUILayout.Button((biv[j] as NpcAppearInst).name,GUILayout.Width(50f))){

							for(int k =0;k <storyList[curStoryId].instList.Count; k++){

								if(storyList[curStoryId].instList[k] is NpcAppearInst && (storyList[curStoryId].instList[k] as NpcAppearInst).npcid== id){
									curInst = k;
									curNpcId = (storyList[curStoryId].instList[k] as NpcAppearInst).npcid;
									EditorGUIUtility.editingTextField = false;
								}
							}
						}
						GUI.color = Color.white;
						GUI.color = Color.red;
						if(GUILayout.Button("X",GUILayout.Width(20f))){
							if(EditorUtility.DisplayDialog("确认删除","删除后会把属于该npc的指令都删除","是","否")){
								for(int k =storyList[curStoryId].instList.Count -1; k > -1 ; k--){
									
									if(storyList[curStoryId].instList[k] is NpcAppearInst && (storyList[curStoryId].instList[k] as NpcAppearInst).npcid== id){
										storyList[curStoryId].instList.RemoveAt(k);
										curInst = -1;
										EditorGUIUtility.editingTextField = false;
									}
									else if(storyList[curStoryId].instList[k] is NpcDeleteInst && (storyList[curStoryId].instList[k] as NpcDeleteInst).npcid== id){
										storyList[curStoryId].instList.RemoveAt(k);
										EditorGUIUtility.editingTextField = false;
										
									}
									else if(storyList[curStoryId].instList[k] is NpcTalkInst && (storyList[curStoryId].instList[k] as NpcTalkInst).npcid== id){
										storyList[curStoryId].instList.RemoveAt(k);
										EditorGUIUtility.editingTextField = false;
									}
									else if(storyList[curStoryId].instList[k] is NpcMoveInst && (storyList[curStoryId].instList[k] as NpcMoveInst).npcid== id){
										storyList[curStoryId].instList.RemoveAt(k);
										EditorGUIUtility.editingTextField = false;
									}
									else if(storyList[curStoryId].instList[k] is NpcActionInst && (storyList[curStoryId].instList[k] as NpcActionInst).npcid== id){
										storyList[curStoryId].instList.RemoveAt(k);
										EditorGUIUtility.editingTextField = false;
									}
									else if(storyList[curStoryId].instList[k] is NpcTurnaroundInst && (storyList[curStoryId].instList[k] as NpcTurnaroundInst).npcid== id){
										storyList[curStoryId].instList.RemoveAt(k);
										EditorGUIUtility.editingTextField = false;
									}
									else if(storyList[curStoryId].instList[k] is NpcEffInst && (storyList[curStoryId].instList[k] as NpcEffInst).npcid== id){
										storyList[curStoryId].instList.RemoveAt(k);
										EditorGUIUtility.editingTextField = false;
									}
								}
							}
						}
						GUI.color = Color.white;
						GUILayout.EndHorizontal();
					}
					if(biv[j] is NpcTalkInst){
						GUILayout.BeginHorizontal();
						int id = (biv[j] as NpcTalkInst).id;
						GUILayout.Space((biv[j] as NpcTalkInst).startTime * width);
						if(GUILayout.Button("说话",GUILayout.Width(50f))){
							for(int k =0;k <storyList[curStoryId].instList.Count; k++){
								if(storyList[curStoryId].instList[k] is NpcTalkInst && (storyList[curStoryId].instList[k] as NpcTalkInst).id== id){
									curInst = k;
									EditorGUIUtility.editingTextField = false;
								}
							}
						}
						GUI.color = Color.red;
						if(GUILayout.Button("X",GUILayout.Width(20f))){
							for(int k =0;k <storyList[curStoryId].instList.Count; k++){
								if(storyList[curStoryId].instList[k] is NpcTalkInst && (storyList[curStoryId].instList[k] as NpcTalkInst).id== id){
									storyList[curStoryId].instList.RemoveAt(k);
									curInst = -1;
									EditorGUIUtility.editingTextField = false;
								}
							}
						}
						GUI.color = Color.white;
						GUILayout.EndHorizontal();
					}
					if(biv[j] is NpcMoveInst){
						GUILayout.BeginHorizontal();
						int id = (biv[j] as NpcMoveInst).id;
						GUILayout.Space((biv[j] as NpcMoveInst).startTime * width);
						if(GUILayout.Button("移动",GUILayout.Width(50f))){

							for(int k =0;k <storyList[curStoryId].instList.Count; k++){
								if(storyList[curStoryId].instList[k] is NpcMoveInst && (storyList[curStoryId].instList[k] as NpcMoveInst).id== id){
									curInst = k;
									EditorGUIUtility.editingTextField = false;
								}
							}
						}
						GUI.color = Color.red;
						if(GUILayout.Button("X",GUILayout.Width(20f))){
							for(int k =0;k <storyList[curStoryId].instList.Count; k++){
								if(storyList[curStoryId].instList[k] is NpcMoveInst && (storyList[curStoryId].instList[k] as NpcMoveInst).id== id){
									storyList[curStoryId].instList.RemoveAt(k);
									curInst = -1;
									EditorGUIUtility.editingTextField = false;
								}
							}
						}
						GUI.color = Color.white;
						GUILayout.EndHorizontal();
					}

					if(biv[j] is NpcTurnaroundInst){
						GUILayout.BeginHorizontal();
						int id = (biv[j] as NpcTurnaroundInst).id;
						GUILayout.Space((biv[j] as NpcTurnaroundInst).startTime * width);
						if(GUILayout.Button("转向",GUILayout.Width(50f))){
							
							for(int k =0;k <storyList[curStoryId].instList.Count; k++){
								if(storyList[curStoryId].instList[k] is NpcTurnaroundInst && (storyList[curStoryId].instList[k] as NpcTurnaroundInst).id== id){
									curInst = k;
									EditorGUIUtility.editingTextField = false;
								}
							}
						}
						GUI.color = Color.red;
						if(GUILayout.Button("X",GUILayout.Width(20f))){
							for(int k =0;k <storyList[curStoryId].instList.Count; k++){
								if(storyList[curStoryId].instList[k] is NpcTurnaroundInst && (storyList[curStoryId].instList[k] as NpcTurnaroundInst).id== id){
									storyList[curStoryId].instList.RemoveAt(k);
									curInst = -1;
									EditorGUIUtility.editingTextField = false;
								}
							}
						}
						GUI.color = Color.white;
						GUILayout.EndHorizontal();
					}


					if(biv[j] is NpcActionInst){
						GUILayout.BeginHorizontal();
						int id = (biv[j] as NpcActionInst).id;
						GUILayout.Space((biv[j] as NpcActionInst).startTime * width);
						if(GUILayout.Button("动作"+(biv[j] as NpcActionInst).anim,GUILayout.Width(50f))){
							
							for(int k =0;k <storyList[curStoryId].instList.Count; k++){
								if(storyList[curStoryId].instList[k] is NpcActionInst && (storyList[curStoryId].instList[k] as NpcActionInst).id== id){
									curInst = k;
									EditorGUIUtility.editingTextField = false;
								}
							}
						}
						GUI.color = Color.red;
						if(GUILayout.Button("X",GUILayout.Width(20f))){
							for(int k =0;k <storyList[curStoryId].instList.Count; k++){
								if(storyList[curStoryId].instList[k] is NpcActionInst && (storyList[curStoryId].instList[k] as NpcActionInst).id== id){
									storyList[curStoryId].instList.RemoveAt(k);
									curInst = -1;
									EditorGUIUtility.editingTextField = false;
								}
							}
						}
						GUI.color = Color.white;
						GUILayout.EndHorizontal();
					}
					if(biv[j] is NpcDeleteInst){
						GUILayout.BeginHorizontal();
						int id = (biv[j] as NpcDeleteInst).id;
						GUILayout.Space((biv[j] as NpcDeleteInst).startTime * width);
						GUI.color = Color.grey;
						if(GUILayout.Button("del",GUILayout.Width(75f))){
							
							for(int k =0;k <storyList[curStoryId].instList.Count; k++){
								if(storyList[curStoryId].instList[k] is NpcDeleteInst && (storyList[curStoryId].instList[k] as NpcDeleteInst).id== id){
									curInst = k;
									EditorGUIUtility.editingTextField = false;
								}
							}
						}
						GUI.color = Color.white;
						GUILayout.EndHorizontal();
					}

					if(biv[j] is NpcEffInst){
						GUILayout.BeginHorizontal();
						int id = (biv[j] as NpcEffInst).id;
						GUILayout.Space((biv[j] as NpcEffInst).startTime * width);
						if(GUILayout.Button("NpcEff",GUILayout.Width(50f))){
							
							for(int k =0;k <storyList[curStoryId].instList.Count; k++){
								if(storyList[curStoryId].instList[k] is NpcEffInst && (storyList[curStoryId].instList[k] as NpcEffInst).id== id){
									curInst = k;
									EditorGUIUtility.editingTextField = false;
								}
							}
						}
						GUI.color = Color.red;
						if(GUILayout.Button("X",GUILayout.Width(20f))){
							for(int k =0;k <storyList[curStoryId].instList.Count; k++){
								if(storyList[curStoryId].instList[k] is NpcEffInst && (storyList[curStoryId].instList[k] as NpcEffInst).id== id){
									storyList[curStoryId].instList.RemoveAt(k);
									curInst = -1;
									EditorGUIUtility.editingTextField = false;
								}
							}
						}
						GUI.color = Color.white;
						GUILayout.EndHorizontal();
					}
				}
					

			}

			for(int i = 0 ; i < storyList[curStoryId].instList.Count; i++){	
				if(storyList[curStoryId].instList[i].type!=null && storyList[curStoryId].instList[i].type.Equals(EffAppearInst.TYPE)){
					GUILayout.BeginHorizontal();
					GUILayout.Space((storyList[curStoryId].instList[i] as EffAppearInst).startTime * width);
					if(GUILayout.Button("特效",GUILayout.Width(50f))){
						curInst = i;
						EditorGUIUtility.editingTextField = false;
					}
					GUI.color = Color.red;
					if(GUILayout.Button("X",GUILayout.Width(20f))){
						
						curInst = -1;
						storyList[curStoryId].instList.RemoveAt(i);
					}
					GUI.color = Color.white;
					GUILayout.EndHorizontal();
				}
				else if(storyList[curStoryId].instList[i].type!=null && storyList[curStoryId].instList[i].type.Equals(CameraTranslationInst.TYPE)){
					GUILayout.BeginHorizontal();
					GUILayout.Space((storyList[curStoryId].instList[i] as CameraTranslationInst).startTime * width);
					if(GUILayout.Button("平移",GUILayout.Width(50f))){
						curInst = i;
						EditorGUIUtility.editingTextField = false;
					}
					GUI.color = Color.red;
					if(GUILayout.Button("X",GUILayout.Width(20f))){
						
						curInst = -1;
						storyList[curStoryId].instList.RemoveAt(i);
					}
					GUI.color = Color.white;
					GUILayout.EndHorizontal();
				}
				else if(storyList[curStoryId].instList[i].type!=null && storyList[curStoryId].instList[i].type.Equals(ScreenShockInst.TYPE)){
					GUILayout.BeginHorizontal();
					GUILayout.Space((storyList[curStoryId].instList[i] as ScreenShockInst).startTime * width);
					if(GUILayout.Button("震屏",GUILayout.Width(50f))){
						curInst = i;
						EditorGUIUtility.editingTextField = false;
					}
					GUI.color = Color.red;
					if(GUILayout.Button("X",GUILayout.Width(20f))){
						
						curInst = -1;
						storyList[curStoryId].instList.RemoveAt(i);
					}
					GUI.color = Color.white;
					GUILayout.EndHorizontal();
				}
				else if(storyList[curStoryId].instList[i].type!=null && storyList[curStoryId].instList[i].type.Equals(ScreenMaskInst.TYPE)){
					GUILayout.BeginHorizontal();
					GUILayout.Space((storyList[curStoryId].instList[i] as ScreenMaskInst).startTime * width);
					if(GUILayout.Button("蒙版",GUILayout.Width(50f))){
						curInst = i;
						EditorGUIUtility.editingTextField = false;
					}
					GUI.color = Color.red;
					if(GUILayout.Button("X",GUILayout.Width(20f))){
						
						curInst = -1;
						storyList[curStoryId].instList.RemoveAt(i);
					}
					GUI.color = Color.white;
					GUILayout.EndHorizontal();
				}
				else if(storyList[curStoryId].instList[i].type!=null && storyList[curStoryId].instList[i].type.Equals(ScreenPresureInst.TYPE)){
					GUILayout.BeginHorizontal();
					GUILayout.Space((storyList[curStoryId].instList[i] as ScreenPresureInst).startTime * width);
					if(GUILayout.Button("压屏",GUILayout.Width(50f))){
						curInst = i;
					}
					GUI.color = Color.red;
					if(GUILayout.Button("X",GUILayout.Width(20f))){
						
						curInst = -1;
						storyList[curStoryId].instList.RemoveAt(i);
					}
					GUI.color = Color.white;
					GUILayout.EndHorizontal();
				}

				else if(storyList[curStoryId].instList[i].type!=null && storyList[curStoryId].instList[i].type.Equals(MusicPlayInst.TYPE)){
					GUILayout.BeginHorizontal();
					GUILayout.Space((storyList[curStoryId].instList[i] as MusicPlayInst).startTime * width);
					if(GUILayout.Button("音乐",GUILayout.Width(50f))){
						curInst = i;
						EditorGUIUtility.editingTextField = false;
					}
					GUI.color = Color.red;
					if(GUILayout.Button("X",GUILayout.Width(20f))){
						
						curInst = -1;
						storyList[curStoryId].instList.RemoveAt(i);
					}
					GUI.color = Color.white;
					GUILayout.EndHorizontal();
				}
				else if(storyList[curStoryId].instList[i].type!=null && storyList[curStoryId].instList[i].type.Equals(AudioPlayInst.TYPE)){
					GUILayout.BeginHorizontal();
					GUILayout.Space((storyList[curStoryId].instList[i] as AudioPlayInst).startTime * width);
					if(GUILayout.Button("音效",GUILayout.Width(50f))){
						curInst = i;
						EditorGUIUtility.editingTextField = false;
					}
					GUI.color = Color.red;
					if(GUILayout.Button("X",GUILayout.Width(20f))){
						
						curInst = -1;
						storyList[curStoryId].instList.RemoveAt(i);
					}
					GUI.color = Color.white;
					GUILayout.EndHorizontal();
				}
			}

			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}
	}
	#endregion








	#region 添加npc 指令
	public void CreateNpcInst(){
		switch (npcs) {

		case NPCAcion.Talk:
			addNpcTalk();
				break;
		case NPCAcion.Move:
			addNpcMove();
			break;
		case NPCAcion.Anim:
			AddNpcAnim();
			break;
		case NPCAcion.Turn:
			AddNpcTurnAround();
			break;
		case NPCAcion.Eff:
			AddNpcEff();
			break;

		}
	}
	int npcid = 1;
	int id = 1;

	public void UpdateIDAndNpcId(){

		for(int i = 0; i < storyList [curStoryId].instList.Count; i++){
			              
				if(storyList[curStoryId].instList[i].type.Equals(NpcAppearInst.TYPE)){
					if((storyList[curStoryId].instList[i] as NpcAppearInst).npcid > npcid){
						npcid = (storyList[curStoryId].instList[i] as NpcAppearInst).npcid;
					}
					if(storyList[curStoryId].instList[i].id > id){
						id = storyList[curStoryId].instList[i].id;
					}
				}
			}
		++npcid;
		++id;
	}

	public void addNpcAppear(){
		NpcAppearInst appear = new NpcAppearInst ();
		appear.npcid = npcid;
		appear.type = NpcAppearInst.TYPE;
		appear.id = ++id;
		storyList [curStoryId].instList.Add (appear);
		addNpcDelete ();
		npcid++;
	}

	public void addNpcTalk(){
		NpcTalkInst talk = new NpcTalkInst ();
		talk.npcid = curNpcId;
		talk.id =  ++id;
		talk.type = NpcTalkInst.TYPE;
		storyList [curStoryId].instList.Add (talk);
	}
	public void addNpcMove(){
		NpcMoveInst move = new NpcMoveInst ();
		move.npcid= curNpcId;
		move.id = ++id;
		move.type = NpcMoveInst.TYPE;
		storyList [curStoryId].instList.Add (move);
	}
	public void addNpcDelete(){
		NpcDeleteInst del = new NpcDeleteInst ();
		del.id = ++id;
		del.npcid = npcid;
		del.type = NpcDeleteInst.TYPE;
		storyList [curStoryId].instList.Add (del);
	}
	public void AddNpcAnim(){
		NpcActionInst inst = new NpcActionInst ();
		inst.npcid = curNpcId;
		inst.id = ++id;
		inst.type = NpcActionInst.TYPE;
		storyList [curStoryId].instList.Add (inst);
	}

	public void AddNpcTurnAround(){
		NpcTurnaroundInst inst = new NpcTurnaroundInst ();
		inst.npcid = curNpcId;
		inst.id = ++id;
		inst.type = NpcTurnaroundInst.TYPE;
		storyList [curStoryId].instList.Add (inst);
	}

	public void AddNpcEff(){
		NpcEffInst inst = new NpcEffInst ();
		inst.type = NpcEffInst.TYPE;
		inst.id = ++id;
		inst.npcid = curNpcId;
		storyList [curStoryId].instList.Add (inst);
	}

	#endregion


	#region 保存
	public void Save(){
		DirectoryInfo di = new DirectoryInfo ("Assets/GameResources/ConfigFiles/StoryConfig");
		FileInfo []files =  di.GetFiles ();
		foreach(FileInfo file in files){
			file.Delete();
		}

		for (int i = 0; i < storyList.Count; i++) {
			StoryInfo story = storyList[i];

			string json = JsonMapper.ToJson (story);
			byte[] buff = System.Text.UTF8Encoding.UTF8.GetBytes (json);
	
			if (json.Length > 0) {
				DataHelper.SaveFile ("Assets/GameResources/ConfigFiles/StoryConfig/StoryConfig"+storyList[i].id+".bytes", buff);
			}
			
			AssetDatabase.Refresh();
		}
	}
	#endregion

	#region 加载
	public void Load(){

		storyList.Clear ();
		DirectoryInfo di = new DirectoryInfo ("Assets/GameResources/ConfigFiles/StoryConfig");
		FileInfo []files =  di.GetFiles ("*.bytes");
		for (int i =0; i <files.Length; i++) {
			JsonStoryInfo configInfo = DataHelper.GetJsonFile<JsonStoryInfo>("ConfigFiles/StoryConfig/"+files[i].Name.Split('.')[0], "bytes", false);
			StoryInfo story = new StoryInfo();
			story.id = configInfo.id;
			story.sceneid = configInfo.sceneid;
			story.delayTime = configInfo.delayTime;
			story.instList = new List<BaseStoryInst> ();

			if (configInfo != null)
			{
				JsonStoryInst[] list = configInfo.instList.ToArray();
				foreach(JsonStoryInst info in list)
				{
					story.instList.Add(info.ToBaseActionInfo());
				}
				storyList.Add(story);
			}
			storyList.Sort((a,b)=>a.id.CompareTo(b.id));
		}
	}
	#endregion


	#region 镜头指令

	//平移
	public void TransLation(){
		CameraTranslationInst inst = new CameraTranslationInst ();
		inst.type = CameraTranslationInst.TYPE;
		storyList [curStoryId].instList.Add (inst);
	}

	#endregion

	#region 屏幕指令
	public void createScreenAction(){
		switch (scrType) {
		case ScreenType.Shock:
			Shock();
			break;
		case ScreenType.Mask:
			Mask();
			break;
		case ScreenType.Presure:
			Presure();
			break;
		}
	}


	//真屏
	public void Shock(){
		ScreenShockInst inst = new ScreenShockInst ();
		inst.type = ScreenShockInst.TYPE;
		storyList [curStoryId].instList.Add (inst);
	}

	//蒙版
	public void Mask(){
		ScreenMaskInst inst = new ScreenMaskInst ();
		inst.type = ScreenMaskInst.TYPE;
		storyList [curStoryId].instList.Add (inst);
	}

	//压屏
	public void Presure(){
		ScreenPresureInst inst = new ScreenPresureInst ();
		inst.type = ScreenPresureInst.TYPE;
		storyList [curStoryId].instList.Add (inst);
	}
	#endregion

	void OnDestroy ()
	{
//		if(EditorUtility.DisplayDialog("是否保存","","OK","No")){
//			Save();
//		}
//		AssetDatabase.Refresh();
	}
}
