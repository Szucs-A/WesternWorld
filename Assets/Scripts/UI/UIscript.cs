using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIscript : MonoBehaviour {

    public GameObject blackScreenPart;

    public GameObject UIcover;

    public bool Opened = false;
    
    public void BeginDeactiving()
    {
        //if (this.gameObject.GetActive())
        //{
        //    this.GetComponent<Animator>().SetBool("Close", true);
        //}
        //blackScreenOUT();
        //UICoverON();
        
        this.gameObject.SetActive(false);
        blackScreenPart.SetActive(false);
        Opened = false;
        SoundManager.current.PlayUINotificationSound();
    }

    // Animation Triggers This
    public void UICoverON()
    {
        UIcover.SetActive(true);
    }

    public void UICoverOFF()
    {
        UIcover.SetActive(false);
        Opened = true;
    }

    public void PlaySound()
    {
        SoundManager.current.PlayUINotificationSound();
    }

    public void blackScreenOUT()
    {
        blackScreenPart.GetComponent<Animator>().SetBool("Out", true);
    }

    public void StartOpenings()
    {
        Opened = false;
        this.gameObject.SetActive(true);
        blackScreenPart.SetActive(true);
        PlaySound();


        UICoverOFF();
    }

    // Animation Triggers this
    public void Deactivate()
    {
        this.gameObject.SetActive(false);
        blackScreenPart.SetActive(false);
    }
}
