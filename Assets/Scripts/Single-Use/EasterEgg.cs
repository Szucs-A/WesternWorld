using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasterEgg : MonoBehaviour {

    public float alpha = 1;

    public void Reduce()
    {
        alpha = this.GetComponent<Image>().color.a;

        this.GetComponent<Image>().color = new Color(255, 255, 255, alpha - 0.2f);
        alpha -= 0.2f;

        if(alpha <= 0.2)
        {
            this.gameObject.SetActive(false);
        }
    }
    
    public void PlayMusic()
    {
        if (this.GetComponent<AudioSource>().isPlaying || !SettingsScript.MusicOn)
            return;

        GameObject[] gos = GameObject.FindGameObjectsWithTag("MusicManager");

        foreach(GameObject go in gos)
        {
            if (go != this.gameObject)
                go.GetComponent<MusicScript>().Stop();
        }

        this.GetComponent<AudioSource>().Play();
        Invoke("Replay", GetComponent<AudioSource>().clip.length);
    }
    
    public void Replay()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("MusicManager");

        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
                go.GetComponent<MusicScript>().Replay();
        }
    }
}
