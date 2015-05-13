
using UnityEngine;
using System;
using System.Collections.Generic;


public class GameTimeDisplay : MonoBehaviour
{

    public Rect startRect; // The rect the window is initially displayed at.
    public bool updateColor = true; // Do you want the color to change if the FPS gets low
    public bool allowDrag = true; // Do you want to allow the dragging of the FPS window
    public float frequency = 2f; // The update frequency of the fps

    private Color color = Color.white; // The color of the GUI, depending of the FPS ( R < 10, Y < 30, G >= 30 )
    private GUIStyle style; // The style the text will be displayed at, based en defaultSkin.label.

#if UNITY_DEBUG
    void Start()
    {
        startRect = new Rect(0f, Screen.height - 50f, 200f, 175f);
    }

    void OnGUI()
    {
        if (style == null)
        {
            style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = Color.green;
            style.alignment = TextAnchor.MiddleCenter;
        }

        GUI.color = updateColor ? color : Color.white;
        startRect = GUI.Window(2, startRect, DoMyWindow, "");
    }

    void DoMyWindow(int windowID)
    {
        for (int i = 0; i < timeSpans.Length; i++)
        {
            if (timeSpans[i] != null)
            {
                GUI.Label(new Rect(0, i * 25, startRect.width, 25), timeStringType[i] + timeSpans[i].Seconds + "(s)" + timeSpans[i].Milliseconds + ("(ms)"), style);
            }
        }
        if (allowDrag) GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
    }

    private static string[] timeStringType = new string[6]{
        "LOADING : ",
        "PREPROTOBUF : ",
        "SHADER_INIT : ", 
        "STATIC_DATA : ",
        "COMMON_SCENE : ",
        "BATTLE_SCENE"
    };

    private static DateTime[] dateTimes = new DateTime[6];
    private static TimeSpan[] timeSpans = new TimeSpan[6];

#endif

    public static void BeginTime(GameTimeType type)
    {
#if UNITY_DEBUG
        if (dateTimes.Length < (int)(type)) return;
        dateTimes[(int)(type)] = DateTime.Now;
#endif
    }

    public enum GameTimeType
    {
        LOADING_TIME_TYPE       = 0,
        PREPROTOBUF_DATA_TIME_TYPE = 1,
        SHADER_INIT_TIME_TYPE   = 2,
        STATIC_DATA_TIME_TYPE   = 3,
        COMMON_SCENE_TIME_TYPE  = 4,
        BATTLE_SCENE_TIME_TYPE  = 5
    }

    public static void EndTime(GameTimeType type)
    {
#if UNITY_DEBUG
        if (dateTimes.Length < (int)(type)) return;

        if ( dateTimes[(int)(type)] != null ) return;

        timeSpans[(int)(type)] = DateTime.Now.Subtract(dateTimes[(int)(type)]);
#endif
    }


}
