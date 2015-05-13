using UnityEngine;
using System.Collections;

public class UVOffsetScript : MonoBehaviour {

    public float scrollSpeed = 1.0f;

    protected float alpha;
    protected float startTime;
    public string texturePropertyName = "_tex2";

    public int materialIndex = 0;

    public int cols = 3;
    public int rows = 3;

    public bool loop = true;

    public bool stop = false;

    protected float xStep;
    protected float yStep;
	// Use this for initialization
	void Start () {

        startTime = Time.time;
        xStep = 1.0f / cols;
        yStep = 1.0f / rows;


	}
	
	// Update is called once per frame
	void Update () {

        if (!stop)
        {
            int offset = (int)(Time.time * scrollSpeed) % (cols * rows);
            float x = xStep * (offset % cols);
            float y = 1 - yStep * (offset / cols + 1);

            
            if (y == 1)
            {
                y = 0;
            }
            
            if (!loop && (offset % cols == cols - 1) && (offset / cols  == rows-1))
            {
                stop = true;
            }
            renderer.materials[materialIndex].SetTextureOffset(texturePropertyName, new Vector2(x, y));
           
        }
	}
}
