using UnityEngine;

using System.Collections.Generic;

public class Time2Pos
{
    public float time;
    public float posy;

    public Time2Pos(float t, float p)
    {
        time = t;
        posy = p;
    }
}
public class CameraAnim : MonoBehaviour {

    private int curPlaying = -1;

    private float curCalTime = 0.0f;
    private int num = 0;
    private float offy = 0.0f;
    Dictionary<int, Dictionary<int, Time2Pos>> cameraAnim = new Dictionary<int, Dictionary<int, Time2Pos>>();

    private GameObject player;
    private Vector3 cameraOffset;
	private Transform playerTransform;
	// Use this for initialization
	void Start () {
        Dictionary<int, Time2Pos> cal1 = new Dictionary<int, Time2Pos>();

        cal1.Add(0, new Time2Pos(0,0));
        cal1.Add(1, new Time2Pos(0.02f,0.8f));
        cal1.Add(2, new Time2Pos(0.06f,0));
        cal1.Add(3, new Time2Pos(0.09f,0));
        cal1.Add(4, new Time2Pos(0.11f,0.5f));
        cal1.Add(5, new Time2Pos(0.13f,0));
        cal1.Add(6, new Time2Pos(0.15f,0.1f));
        cal1.Add(7, new Time2Pos(0.2f,0));

	    cameraAnim.Add(0,cal1);
	}

    void SetPlayer(GameObject p)
    {
        player = p;
		playerTransform = p.transform;
    }

    void SetCameraOffset(Vector3 pos)
    {
        cameraOffset = pos;
    }
	
	// Update is called once per frame
	void Update () {
        

        if (curPlaying != -1)
        {
            curCalTime += Time.deltaTime;
            if (curCalTime >= cameraAnim[curPlaying][num].time)
            {
                Vector3 p = playerTransform.position + cameraOffset;

                p.y -= offy;
                offy = cameraAnim[curPlaying][num].posy;
                p.y += offy;
                transform.position = p;
                num++;
                if (num >= cameraAnim[curPlaying].Count)
                    curPlaying = -1;
            }
        }
        else
        {
            if (player != null)
            {
                transform.position = playerTransform.position + cameraOffset;
                foreach (Camera c in Camera.allCameras)
                {
                    if (c.tag == "SecondCamera")
                    {
                        c.transform.position = playerTransform.position + cameraOffset;
                    }
                }
            }
        }
	}

    public void playAnim(int which)
    {
        if (cameraAnim.ContainsKey(which))
        {
            curPlaying = which;
            curCalTime = 0.0f;
            num = 0;
            offy = 0.0f;
        }
    }
}
