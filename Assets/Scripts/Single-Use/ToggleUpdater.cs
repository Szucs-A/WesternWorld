using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUpdater : MonoBehaviour {

    public Text musictoggler, soundtoggler, challengetoggler;

	// Use this for initialization
	void Start () {


    }

    public void ResetToggles()
    {
        if (SettingsScript.MusicOn)
        {
            musictoggler.text = "On";
        }
        else
        {
            musictoggler.text = "Off";
        }

        if (SettingsScript.SoundOn)
        {
            soundtoggler.text = "On";
        }
        else
        {
            soundtoggler.text = "Off";
        }

        if (SettingsScript.AcceptChallenges)
        {
            challengetoggler.text = "On";
        }
        else
        {
            challengetoggler.text = "Off";
        }
    }
}
