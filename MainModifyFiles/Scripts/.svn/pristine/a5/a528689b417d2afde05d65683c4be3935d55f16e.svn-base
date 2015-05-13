using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationRefController : ReferencesController< AnimationRefController >
{
#region Class of AnimationClipReference
    private class AnimationClipReference
    {
        public string animationName;

        private int refCount;

        public AnimationClip clip;

        public AnimationClipReference( string animationName ,AnimationClip clip)
        {
            this.refCount = 0;

            this.animationName = animationName;

            this.clip = clip;
        }

        public void  AddRefCount()
        {
            refCount ++;
        }

        public void SubRefCount()
        {
            refCount --;
        }

        public int RefCount { get { return refCount; } }

        //是否还有引用
        public bool IsReference()
        {
            return (this.refCount > 0);
        }

        public void Dispose()
        {
            clip = null;
        }
    }
#endregion

    //private static AnimationRefController _instance;

    //public static AnimationRefController Instance
    //{
    //    get
    //    {
    //        if (_instance == null) // Unity operator to check if object was destroyed, 
    //        {
    //            _instance = new AnimationRefController();
    //        }

    //        return _instance;
    //    }
    //}

    //动作名 -- IntanceId的对应
    private Dictionary<string, int> _instancIDDic = null;

    private Dictionary<int, AnimationClipReference> _animationClip = null;
    public AnimationRefController()
    {
        _instancIDDic = new Dictionary<string, int>();

        _animationClip = new Dictionary<int, AnimationClipReference>();
    }


    /// 添加AnimationClip
    /// </summary>
    /// <param name="clipName">clipName 的格式为： modleName/actionName</param>
    /// <returns>返回是否成功</returns>  
    public bool AddAnimationClip( Animation anim, string clipName)
    {
        if( anim == null )
        {
            return false;
        }

        if (_instancIDDic.ContainsKey(clipName))
        {
            int intanceID = _instancIDDic[clipName];

            AnimationClipReference reference = _animationClip[intanceID];

            string[] formats = clipName.Split('/');
            string action    = formats[1];
            if (anim[action] == null)
            {
                anim.AddClip(reference.clip, action);
                reference.AddRefCount();

                AnimationClipRefComponent controller = anim.gameObject.GetComponent<AnimationClipRefComponent>();
                if (controller == null)
                {
                    controller = anim.gameObject.AddComponent<AnimationClipRefComponent>();
                    if (controller != null)
                    {
                        controller.Setup(this);
                    }
                }
                controller.AddInstanceID(intanceID);
            }
            return true;
        }
        return false;
    }

    /// 判断动作是否已经存在
    /// </summary>
    /// <param name="clipName">clipName 的格式为： modleName/actionName</param>
    /// <returns>返回是否存在</returns>
    public bool IsAnimationClipExist(string clipName)
    {
        return _instancIDDic.ContainsKey(clipName);
    }

    /// 设置AnimationClip的保存
    /// <param name="clip">AnimationClip 数据</param>
    /// <param name="clipName">AnimationClip保存名字</param>
    public bool SetAnimationClip(AnimationClip clip, string clipName)
    {
        if (_instancIDDic.ContainsKey(clipName))
        {
            return false;
        }

        //加入列表
        int instanceID = clip.GetInstanceID();
        GameDebuger.Log("AnimationRefController Add state : " + clipName + " : " + instanceID);

        _instancIDDic.Add(clipName, instanceID);
        _animationClip.Add(instanceID, new AnimationClipReference(clipName,clip));

        return true;
    }


    public override void SetupReference(List<int> instanceID)
    {
        GameDebuger.Log("AnimationRefController SetupReference");
    }


    public override void DeleteReference(List<int> instanceID)
    {
        GameDebuger.Log("AnimationRefController DeleteReference");

        foreach (int iID in instanceID)
        {
            if (_animationClip.ContainsKey(iID))
            {
                AnimationClipReference reference = _animationClip[iID];
                reference.SubRefCount();

                GameDebuger.Log(" ======== Sub Aniamtion References " + reference.animationName + " : " + reference.RefCount);
                if (!_animationClip[iID].IsReference())
                {
                    GameDebuger.Log(" ======== Remove Animation References  " + reference.animationName);

                    _instancIDDic.Remove(reference.animationName);
                    _animationClip.Remove(iID);
                    reference.Dispose();
                }
            }
        }
    }
}
