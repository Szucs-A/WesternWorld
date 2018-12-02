using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour {

    public static float speed = 1.5f;

    public static void SettingsTurnMusicOff()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("MusicManager");

        foreach(GameObject music in objects)
        {
            music.GetComponent<MusicScript>().Stop();
        }
    }

    public static void SettingsTurnMusicOn()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("MusicManager");

        foreach (GameObject music in objects)
        {
            music.GetComponent<MusicScript>().Replay();
        }
    }

    public static void TurnMusicOff()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("MusicManager");

        foreach (GameObject music in objects)
        {
            music.GetComponent<MusicScript>().BeginTurnOff();
        }
    }

    public static void TurnMusicOn()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("MusicManager");

        foreach (GameObject music in objects)
        {
            music.GetComponent<MusicScript>().BeginTurnOn();
        }
    }

    public bool TurningOff = false;
    public bool TurningOn = true;
    public float tParam = 0;
    public float volume = 0;
    public float MaxVolume = 1;
    public bool Off = false;

    public void Replay()
    {
        Off = false;
        this.GetComponent<AudioSource>().Play();
        BeginTurnOn();
    }

    public void Stop()
    {
        this.GetComponent<AudioSource>().Stop();
        volume = 0;
        Off = true;
    }

    public void Update()
    {
        if (Off)
        {
            return;
        }

        if (!SettingsScript.MusicOn && this.GetComponent<AudioSource>().isPlaying)
            this.GetComponent<AudioSource>().Stop();

        if (TurningOff && !TurningOn)
        {
            if (tParam < 1)
            {
                tParam += Time.deltaTime * speed;
                volume = Mathf.Lerp(MaxVolume, 0, tParam);
            }
            else
            {
                tParam = 0;
                TurningOn = false;
                TurningOff = false;
                volume = 0;
            }
        }
        else if (TurningOn && !TurningOff)
        {
            if(tParam < 1)
            {
                tParam += Time.deltaTime * speed;
                volume = Mathf.Lerp(0, MaxVolume, tParam);
            }
            else
            {
                tParam = 0;
                TurningOn = false;
                TurningOff = false;
                volume = MaxVolume;
            }
        }

        this.GetComponent<AudioSource>().volume = volume;
    }

    public void BeginTurnOff()
    {
        TurningOff = true;
        TurningOn = false;
    }

    public void BeginTurnOn()
    {
        TurningOn = true;
        TurningOff = false;
    }
    
}
