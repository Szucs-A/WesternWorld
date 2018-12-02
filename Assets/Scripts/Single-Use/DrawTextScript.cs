using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTextScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlaySound()
    {
        SoundManager.current.PlayDrawSound();
    }

    public void TurnOff()
    {
        this.GetComponent<Animator>().SetBool("TurnOff", false);
    }
}
