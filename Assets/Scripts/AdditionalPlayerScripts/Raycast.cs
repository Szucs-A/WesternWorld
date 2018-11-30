using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour {
    
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            CastRay();
        }
	}

    void CastRay()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        foreach(RaycastHit2D hitz in hit)
        {
            if (hitz.collider != null)
            {
                if (hitz.collider.gameObject.tag == "UI")
                {
                    ButtonMaster bm = this.gameObject.GetComponent<ButtonMaster>();
                    bm.SwapButton(hitz.collider.gameObject.name);
                    return;
                }
            }
        }
    }
}
