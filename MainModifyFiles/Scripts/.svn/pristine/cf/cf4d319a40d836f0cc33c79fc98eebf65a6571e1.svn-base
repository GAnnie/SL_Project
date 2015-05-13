using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReferencesComponent : MonoBehaviour
{
    private IReferencesCtl _controller;

    void OnDestroy()
    {
        if (_controller != null)
        {
            _controller.DeleteReference(_instanceIDList);
        }
    }

    protected void SetupIReferencesCtl(IReferencesCtl controller)
    {
        this._controller = controller;
    }

    private List<int> _instanceIDList = new List<int>();
    public void AddInstanceID( int instanceID )
    {
        if (!_instanceIDList.Contains(instanceID))
        {
            _instanceIDList.Add(instanceID);
        }
    }


}
