using UnityEngine;
using System.Collections;

public class CameraAnimOnDestroy : MonoBehaviour {

    public int id = 0;
    void OnDetroy()
    {
        Camera.main.GetComponent<CameraAnim>().playAnim(id);
    }

}
