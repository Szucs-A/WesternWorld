using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimmingSound : MonoBehaviour {

    public bool EnteringShop, EnteringSingle;

    public void PlaySound()
    {
        if (EnteringShop)
        {
            SoundManager.current.PlayEnteringShop();
        }
        else if (EnteringSingle)
        {
            SoundManager.current.PlayEnteringSingleplayer();
        }

        MusicScript.TurnMusicOn();
    }

    public void TurnOff()
    {
        this.gameObject.SetActive(false);
    }
}
