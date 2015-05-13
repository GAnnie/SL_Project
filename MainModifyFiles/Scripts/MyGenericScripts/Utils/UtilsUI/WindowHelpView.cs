using UnityEngine;
using System.Collections;

public class WindowHelpView : MonoBehaviour
{
    public UILabel infoLabel;

    private GameObject _view;
    private System.Action _closeCallBack = null;

    void Start() {

	}

    void Update()
    {

    }

    public void Open(GameObject view, string msg, System.Action closeCallBack = null)
    {
        _view = view;
        _closeCallBack = closeCallBack;
        infoLabel.text = msg;
    }

    public void OnCloseBtn()
    {
        if (_closeCallBack != null)
        {
            _closeCallBack();
        }
    }
}
