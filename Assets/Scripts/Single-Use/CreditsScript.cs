using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScript : MonoBehaviour {

    public GameObject archingCreditsUI;

	// Update is called once per frame
	void Update () {
		
	}

    public void SetupCredits()
    {
        archingCreditsUI.GetComponent<UIscript>().StartOpenings();
    }

    public void TakeDownCredits()
    {
        archingCreditsUI.GetComponent<UIscript>().BeginDeactiving();
    }
}
