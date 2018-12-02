using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouDied : MonoBehaviour {
    
    public void startClosing()
    {
        Invoke("activeClosing", 1f);
    }

    public void activeClosing()
    {
        levelFailed();
    }

    public void levelFailed()
    {
        DimmerScript.RoomToGoTo = 1;
        GameObject.Find("Leaver").GetComponent<DimmerScript>().Begin();
    }
}
