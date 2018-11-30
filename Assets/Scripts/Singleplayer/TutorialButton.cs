using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MonoBehaviour {

    public GameObject tutorial;

    public void TurnOff()
    {
        GameObject timer = GameObject.FindGameObjectWithTag("CountDownTimer");
        timer.GetComponent<TimerScript>().StartTimer();

        tutorial.GetComponent<UIscript>().BeginDeactiving();

        UsernameScript.NewGame = false;
    }

}
