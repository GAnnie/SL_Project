using UnityEngine;
using System.Collections;

public class PlayAnim : MonoBehaviour 
{
    public string AniName = "";
    public bool ForceLoop = false;
    public float StartTime = 1.0f;
    
    bool Played = false;

    void Awake()
    {
        Played = false;

        if (AniName.Length <= 0)
        {
            foreach (AnimationState st in animation)
            {
                AniName = st.name;
                if (ForceLoop) st.wrapMode = WrapMode.Loop;

                break;
            }
        }

        if (AniName.Length <= 0) Played = true;
    }

    void Update()
    {
        if (!Played)
        {
            StartTime -= Time.deltaTime;
            if (StartTime <= 0)
            {
                animation.Play(AniName);
                Played = true;
            }
        }
    }
}
