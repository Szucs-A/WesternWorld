using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteScript : MonoBehaviour {

    public GameObject congratz;

	// Use this for initialization
	void Start () {
        if (CompleteLevel.AllCompletedOnce)
        {
            congratz.GetComponent<UIscript>().StartOpenings();
            CompleteLevel.AllCompletedOnce = false;
        }
	}
}
