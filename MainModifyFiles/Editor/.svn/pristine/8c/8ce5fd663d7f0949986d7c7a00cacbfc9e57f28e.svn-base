using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

public class RGBTool : EditorWindow  {
	[MenuItem ("Tools/RGB")]
	static void AddWindow ()
	{       
		Rect  wr = new Rect (0,0,500,300);
		RGBTool window = (RGBTool)EditorWindow.GetWindowWithRect (typeof (RGBTool),wr,true,"ChangeToRGB");	
		window.Show();
		
	}
	private string GRBText;
	private string OxText;
	private string text;

	public Color centerColor;  
	
	private Color[] colors;  
	
	void Start () {  
		colors = new Color[1];  
		colors[0] = centerColor;    
	} 

	Color matColor  = Color.white;
	void OnGUI () 
	{
		GRBText = EditorGUILayout.TextField("输入#16进制颜色值/调色板:",GRBText);

		if(GUILayout.Button("ToRGB",GUILayout.Width(200)))
		{
			
			if(string.IsNullOrEmpty(GRBText)){
				return;
			}
			string str = GRBText.Substring(1);
			if(str.Substring(4).Length <2){
				str = "0x" + str.Substring(0,4)+"0"+str.Substring(4);
			}
			
			int c = Convert.ToInt32(str,16);
			string str1 = string.Format("{0},{1},{2}",(Convert.ToInt32(c & 0xff0000) >> 16),(Convert.ToInt32(c & 0x00ff00) >> 8),Convert.ToInt32(c  & 0x0000ff));
			matColor.r = ((Convert.ToInt32(c & 0xff0000) >> 16) /255.0f);
			matColor.g = ((Convert.ToInt32(c & 0x00ff00) >> 8 )/ 255.0f);
			matColor.b = ((Convert.ToInt32(c & 0x0000ff)) / 255.0f);

		}
		
		int r0 = (int)Math.Round(255 * matColor.r);
		int g0 =(int)Math.Round(255 * matColor.g);
		int b0 =(int)Math.Round(255 * matColor.b);
		
		
		string r =  r0.ToString("X");
		if(r.Length <2){
			r = "0"+r;
		}
		string g = g0.ToString("X");
		if(g.Length <2){
			g = "0"+g;
		}
		string b = b0.ToString("X");
		if(b.Length <2){
			b = "0"+b;
		}
		
		matColor = EditorGUI.ColorField(new Rect(0,150,250,25),"color:",matColor);
		text = r0+","+g0+","+b0;
		text = EditorGUILayout.TextField("输出:",text);
		OxText = "#"+r+g+b;
		OxText = EditorGUILayout.TextField("输出:",OxText);
	}


	/// <summary>
	/// c#自带的Math.Round是五舍六入
	/// double d1 = Math.Round(8.4);//8
	/// double d2 = Math.Round(8.5);//8
	/// double d3 = Math.Round(8.6);//9

	/// 实现数据的四舍五入法
	/// </summary>
	/// <param name="v">要进行处理的数据</param>
	/// <param name="x">保留的小数位数</param>
	/// <returns>四舍五入后的结果</returns>
	private double Round(double v, int x)
	{
		bool isNegative = false;
		//如果是负数
		if (v < 0)
		{
			isNegative = true;
			v = -v;
		}
		int IValue = 1;
		for (int i = 1; i <= x; i++)
		{
			IValue = IValue * 10;
		}
		double  Int = Math.Round(v * IValue + 0.5, 0);
		v = Int / IValue;
		if (isNegative)
		{
			v = -v;
		}
		return v;
	}

}
