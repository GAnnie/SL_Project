#pragma strict
import System.Collections.Generic;
import System.Linq;
import System.Text;
import System.IO;

class CheckMark {}

/*
class CheckMark extends EditorWindow{
	class Monodata{
		var mono:MonoScript;
		function Ini(obj:Object){
			mono = obj as MonoScript;
		}
		function GetName(){
			if(mono)
				return mono.name;
			else
				return "";
		}
	}
	class monoLine{
		enum linetype{
			Normal,
			Var,
			Function,
			void,
		}
		var Linetype:linetype;
		var lineid:int;
		var text:String;
		var backtext:String;
		var nexttext:String = "";
		var showlab:String = "";
		var W20:GUILayoutOption = GUILayout.MaxWidth(30);
		var W30:GUILayoutOption = GUILayout.MaxWidth(30);
		var W60:GUILayoutOption = GUILayout.MaxWidth(60);
		var W100:GUILayoutOption = GUILayout.MaxWidth(100);
		var W1000:GUILayoutOption = GUILayout.MaxWidth(1000);
		var color:Color;
		var Gaixie:boolean;
		var Gaixietext:String;
		var type:IOType;
		function ShowLabel(){
			var strs:String[];
			var str:String;
			var strs2:String[];
			var bool:boolean;
			showlab = "";
			if(Linetype == 0)
				showlab = text;
			if(Linetype == 1){
				strs = text.Split("//"[0]);
				if(strs.Length > 1){
					showlab = "有注释::"+strs[1]+"::源代码::"+text;
					color = Color.green;
					bool = true;
				}
				else{
					showlab = "没有注释::::源代码::"+text;
					bool = false;
				}
			}
			if(Linetype == 2){
				if(backtext)
					str = backtext;
				else
					str = text;
				strs = str.Split("//"[0]);
				if(strs.Length > 1){
					showlab = "有注释::"+str+"::源代码::"+text;
					bool = true;
				}
				else{
					showlab = "没有注释::::源代码::"+text;
					bool = false;
				}
			}
			if(Linetype == 3){
				if(backtext)
					str = backtext;
				else
					str = text;
				strs = str.Split("//"[0]);
				if(strs.Length > 1){
					showlab = "有注释::"+str+"::源代码::"+text;
					bool = true;
				}
				else{
					showlab = "没有注释::::源代码::"+text;
					bool = false;
				}
			}
			if(bool){
				color = Color.green;
			}
			else{
				color = Color.red;
			}
		}
		function GetColor(){
			return color;
		}
		function GetShowLabel(){
			return showlab;
		}
		function Ini(str:String,id:int,back:String,next:String){
			lineid = id;
			text = str;
			backtext = back;
			nexttext = next;
			if(text.Contains("var"))
				Linetype = linetype.Var;
			if(text.Contains("function"))
				Linetype = linetype.Function;
			if(text.Contains("void"))
				Linetype = linetype.void;	
			ShowLabel();
		}
		function Show(id:int,mShowType:ShowType,mono:MonoScript,RestCB:Function){
			if(Linetype == 0 && mShowType.Nom || Linetype == 1 && mShowType.Var || Linetype == 2 && mShowType.Fun ||Linetype == 3 && mShowType.Void){
				GUI.backgroundColor = GetColor();
				EditorGUILayout.BeginHorizontal("box");
				GUI.backgroundColor = Color.white;
					EditorGUILayout.LabelField("Line:"+lineid, W60);
					EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField(GetShowLabel(),W1000);
					EditorGUILayout.EndHorizontal();
					if(Gaixie){
						Gaixietext = EditorGUILayout.TextField(Gaixietext);
						GUI.backgroundColor = Color.green;
						if(GUILayout.Button("✔",W20)){
							Inse(id,mono);
							Gaixie = false;
							RestCB();
						}
						GUI.backgroundColor = Color.red;
						if(GUILayout.Button("✘",W20)){
							Gaixie = false;
						}
						GUI.backgroundColor = Color.white;
						
					}
//					EditorGUI.BeginDisabledGroup(Gaixie);
					if(!Gaixie){
						//添加注释
						if(GUILayout.Button("✚",W20)){
							Gaixie = !Gaixie;
							Gaixietext = "";
							type = IOType.Inse;
						}
						//重写
						if(GUILayout.Button("〓",W20)){
							Gaixie = !Gaixie;
							Gaixietext = "";
							type = IOType.FuGai;
						}
						//删除
						if(GUILayout.Button("✘",W20)){
							Gaixie = !Gaixie;
							Gaixietext = "";
							type = IOType.Del;
						}
					}
//					EditorGUI.EndDisabledGroup();
					if(GUILayout.Button("➽", W20))
						coOpen(mono);
				EditorGUILayout.EndHorizontal();
			}
		}
		function coOpen(mono:MonoScript){
			AssetDatabase.OpenAsset(mono,lineid);
		}
		//插入
		function Inse(id:int,mono:MonoScript){
			if(type == IOType.Inse)
				Gaixietext = "//"+Gaixietext;
			if(Linetype == linetype.Var){
				type = IOType.FuGai;
				var tests:String[];
					tests = text.Split("/"[0]);
					if(tests.Length > 0)
						text = tests[0];
				Gaixietext = text+"//"+Gaixietext;
			}
				IO_Inse(type,mono,Gaixietext,id);
		}
		enum IOType{
			Inse,
			FuGai,
			Del,
		}
		//插入航
		function IO_Inse(iotype:IOType,mono:MonoScript,text:String,line:int){
			var path:String;
			var Text:String;
			var sr:StreamReader;
			var sw:StreamWriter;
			var strLine:String;
			var newstr:String;
			var aFile:FileStream;
			var bFile:FileStream;
			var strs:String[];
			var i:int;
			var newpath:String;
				path = AssetDatabase.GetAssetPath(mono);
				Text = mono.text;
				newpath = Path.GetDirectoryName(path)+"/"+Path.GetFileNameWithoutExtension(path)+"_1"+Path.GetExtension(path);
				aFile = new FileStream(path,FileMode.OpenOrCreate);
				bFile = new FileStream(newpath,FileMode.OpenOrCreate);
				sr = new StreamReader(aFile);
				sw = new StreamWriter(bFile);
				strLine = sr.ReadLine();
				while(strLine != null){
					if(i == line - 1 && iotype == IOType.Inse ){
						strs = strLine.Split("/"[0]);
						if(strs.Length == 0)
							sw.WriteLine(strLine);	
					}
					else{
						if(i == line){
							switch(iotype){
								case IOType.Inse:	sw.WriteLine(text);
													sw.WriteLine(strLine);
													break;
								case IOType.FuGai:	sw.WriteLine(text);
													break;
								case IOType.Del	: 	break;
							}
						}
						else{
							sw.WriteLine(strLine);	
						}
					}
						
					
					i++;
					strLine = sr.ReadLine();
				}
				sw.Close();
				sw.Dispose();
				sr.Close();
				FileUtil.ReplaceFile(newpath, path);
				FileUtil.DeleteFileOrDirectory(newpath);
				AssetDatabase.Refresh();
		}
	}
	class TargetMonoData{
		var mono:MonoScript;
		var text:String;
		var Line:List.<monoLine>;
		var ve:Vector2;
		function Ini(mo:MonoScript){
			mono = mo;
			text = mono.text;
			var spr:String[];
				spr = text.Split("\n\r"[0]);
				spr = GetLine(mo);
				
			if(Line)
				Line.Clear();
			else
				Line = new List.<monoLine>();
			for(var i = 0;i < spr.length;i++){
				var newmonoline:monoLine;
					newmonoline = new monoLine();
					var back:String;
					var next:String;
					if(i!=0)
						back = spr[i-1];
					if(i!=spr.length-1)
						next = spr[i+1];
							
					newmonoline.Ini(spr[i],i,back,next);
					Line.Add(newmonoline);
			}
		}
		function ShowEditor(mShowType:ShowType,RestCB:Function){
			ve = EditorGUILayout.BeginScrollView(ve);
				for(var i = 0;i < Line.Count;i++){
					Line[i].Show(i,mShowType,mono,RestCB);
				}
			EditorGUILayout.EndScrollView();
		}
		function GetLine(mono:MonoScript){	
			var aFile:FileStream;
			var sr:StreamReader;
			var path:String;
			var strLine:String;
			var arr = new Array();
			path = AssetDatabase.GetAssetPath(mono);
			aFile = new FileStream(path,FileMode.OpenOrCreate);
			sr = new StreamReader(aFile);
			strLine = sr.ReadLine();
			while(strLine != null){
				arr.push(strLine);
				strLine = sr.ReadLine();
			}
			sr.Close();
			var strs:String[];
				strs = arr;
			return strs;
		}
	}
	static var window;
	var AllMS:List.<Monodata>;
	var ShowMS:List.<Monodata>;
	var target:TargetMonoData;
	var line:int;
	var FindScriptname:String;
	var mShowType:ShowType = new ShowType();
	var W20:GUILayoutOption = GUILayout.MaxWidth(20);
	var W50:GUILayoutOption = GUILayout.MaxWidth(50);
	var AllLine:int;
	var saveid:int;
	class ShowType{
		var W50:GUILayoutOption = GUILayout.MaxWidth(50);
		var Nom:boolean = true;
		var Var:boolean = true;
		var Fun:boolean = true;
		var Void:boolean = true;
		function Show(){
			EditorGUILayout.BeginHorizontal("box");
				EditorGUILayout.LabelField("普通行",W50);
				Nom = EditorGUILayout.Toggle(Nom);
				EditorGUILayout.LabelField("变量",W50);
				Var = EditorGUILayout.Toggle(Var);
				EditorGUILayout.LabelField("函数",W50);
				Fun = EditorGUILayout.Toggle(Fun);
				EditorGUILayout.LabelField("Void",W50);
				Void = EditorGUILayout.Toggle(Void);
			EditorGUILayout.EndHorizontal();
		}
	}
	@MenuItem ("Assets/CheckMark")	
	static function Init(){
		window = EditorWindow.GetWindow(CheckMark);
	}
	function LoadMono(){
		AllLine= 0;
		var obj:Object[];
		obj = Resources.FindObjectsOfTypeAll(MonoScript);
		AllMS = new List.<Monodata>();
		for(var i = 0;i < obj.length;i++){
			var newdata:Monodata;
				newdata = new Monodata();
				newdata.Ini(obj[i]);
			if(newdata.GetName() != ""){
				AllMS.Add(newdata);
			}
		}
	}
	function OnGUI(){
		if(GUILayout.Button("获取"))
			LoadMono();
		if(AllMS)
			EditorGUILayout.LabelField(AllMS.Count.ToString());
		ShowFind();
		ShowTarget(RestCB);//显示目标
		if(GUI.changed)
			RestShowMS();
	}
	function ShowTarget(RestCB:Function){
		mShowType.Show();
		if(target){
			target.ShowEditor(mShowType,RestCB);
		}
	}
	function RestShowMS(){
		var i:int;
		if(!ShowMS)	
			ShowMS = new List.<Monodata>();
		else
			ShowMS.Clear();
		if(AllMS){
			for( i = 0;i < AllMS.Count;i++){
				if(AllMS[i].GetName().ToLower().Contains(FindScriptname)){
					ShowMS.Add(AllMS[i]);
				}
			}	
		}
	}
	function ShowFind(){
		var i:int;
		FindScriptname = EditorGUILayout.TextField("搜索名字",FindScriptname);
		if(ShowMS){
			for( i = 0;i < Mathf.Min( ShowMS.Count,5);i++){
				EditorGUILayout.BeginHorizontal("box");
					EditorGUILayout.LabelField(ShowMS[i].GetName());
					if(GUILayout.Button("➽", W20))
						coOpen(i);
					if(GUILayout.Button("✔",W20))
						SelectMono(i);
				EditorGUILayout.EndHorizontal();
			}
		}
	}
	function coOpen(id:int){
		AssetDatabase.OpenAsset(ShowMS[id].mono,0);
		
	}
	function RestCB(){
		SelectMono(saveid);
	}
	function SelectMono(id:int){
		saveid = id;
		if(target == null)
			target = new TargetMonoData();
		target.Ini(ShowMS[id].mono);
	}
}
*/