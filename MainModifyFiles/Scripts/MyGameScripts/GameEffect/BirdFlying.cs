using UnityEngine;
using System.Collections;

public class BirdFlying : MonoBehaviour {

    Transform m_FlyTransForm = null;
    Vector3 m_position;
    public float m_Radio = 10;
    public float m_speed = 40;
    Vector3 m_temp = Vector3.zero;
    float m_time = 0.0f;
	// Use this for initialization
	void Start () 
    {
        m_FlyTransForm = gameObject.transform;
        m_position = new Vector3(m_FlyTransForm.position.x, m_FlyTransForm.position.y, m_FlyTransForm.position.z);

        //播放鸟叫音效
        gameObject.AddComponent<AudioSource>();
        AudioClip clip = (AudioClip)ResourceLoader.Load("Sound/20102");
        audio.clip = clip;
        audio.loop = true;
        audio.rolloffMode = AudioRolloffMode.Linear;
        audio.maxDistance = 70;
        audio.Play();
        m_temp = new Vector3(m_position.x + m_Radio, m_position.y, m_position.z);
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (null != m_FlyTransForm)
        {
            m_time += Time.deltaTime;
            if (m_time <= 0.1f)
            {
                return;
            }
            m_FlyTransForm.RotateAround(m_temp, Vector3.up, m_speed * m_time);
            m_time = 0.0f;
        }
	}
}
