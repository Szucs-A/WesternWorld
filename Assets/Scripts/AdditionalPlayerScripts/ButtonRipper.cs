using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRipper : MonoBehaviour {

    public GameObject topPart, bottomPart;
    
    public void GotoTop()
    {
        this.transform.SetParent(topPart.transform);
    }

    public void gotoBottom()
    {
        this.transform.SetParent(bottomPart.transform);
    }

    public void rip()
    {
        topPart = GameObject.FindGameObjectWithTag("Parent1");
        bottomPart = GameObject.FindGameObjectWithTag("Parent2");

        if (this.transform.parent.transform.position.y <= -15)
        {
            gotoBottom();
        }
        else
        {
            GotoTop();
        }
    }
}
