using UnityEngine;
using System.Collections;

public class AnimationClipRefComponent : ReferencesComponent
{
    public void Setup(AnimationRefController controller)
    {
        this.SetupIReferencesCtl(controller);         
    }
}
