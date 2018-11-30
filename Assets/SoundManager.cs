using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager current;

    public AudioClip ButtonClickSound, UINotificationSound1, UINotificationSound2, UsedAItem1, UsedAItem2
        , GameWin, RoundWin, RoundLoss, BoughtItem, EnteringShop, EnteringSingleplayer, ScrollSound1, 
        ScrollSound2, ScrollSound3, DrawSound, ToggleSound, InfoButton, PlayerShot, PlayerDuck, PlayerReload, DefaultStance, 
        EnemyShot, EnemyReload, EnemyDuck, LevelSelectSound, AttackBtn, BlockBtn, ReloadBtn;

    private AudioSource ad;

	// Use this for initialization
	void Start () {
        Application.targetFrameRate = 60;
        current = this;
        DontDestroyOnLoad(this.gameObject);
        ad = this.GetComponent<AudioSource>();

        GameObject.Find("UsernameManager").GetComponent<UsernameScript>().startup();
	}
	
    // Play One shot for sound effects so that it can play multiple sounds.
    public void PlayBtnClickSound()
    {
        if(SettingsScript.SoundOn)
            ad.PlayOneShot(ButtonClickSound, 0.8f);
    }

    public void PlayUINotificationSound()
    {
        if (!SettingsScript.SoundOn)
            return;

        int r = Random.Range(1, 3);
        if(r == 1)
        {
            ad.PlayOneShot(UINotificationSound1, 1f);
        }
        else
        {
            ad.PlayOneShot(UINotificationSound2, 1f);
        }
    }

    public void PlayLosingArmorSound()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(UsedAItem1);
    }

    public void PlayLosingDeputySound()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(UsedAItem2);
    }

    public void PlayGameWin()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(GameWin);
    }

    public void PlayRoundWin()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(RoundWin);
    }

    public void PlayRoundLost()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(RoundLoss);
    }

    public void PlayBoughtItem()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(BoughtItem);
    }

    public void PlayEnteringShop()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(EnteringShop);
    }

    public void PlayEnteringSingleplayer()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(EnteringSingleplayer);
    }

    public void PlayScrollingSound()
    {
        if (!SettingsScript.SoundOn)
            return;

        int r = Random.Range(1, 4);

        if(r == 1)
        {
            ad.PlayOneShot(ScrollSound1);
        }
        else if(r == 2)
        {
            ad.PlayOneShot(ScrollSound2);
        }
        else
        {
            ad.PlayOneShot(ScrollSound3);
        }
    }

    public void PlayInfoButtonSound()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(InfoButton);
    }

    public void PlayLevelSelect()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(LevelSelectSound);
    }

    public void PlayEnemyShotSound()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(EnemyShot);
    }

    public void PlayPlayerShotSound()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(PlayerShot);
    }

    public void PlayPlayerDuckSound()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(PlayerDuck);
    }

    public void PlayEnemyDuckSound()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(EnemyDuck);
    }

    public void PlayPlayerReloadSound()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(PlayerReload);
    }

    public void PlayEnemyReloadSound()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(EnemyReload);
    }

    public void PlayDrawSound()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(DrawSound);
    }

    public void PlayToggleSound()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(ToggleSound);
    }

    public void AttackButtonSound()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(AttackBtn);
    }

    public void ReloadButtonSound()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(ReloadBtn);
    }

    public void BlockButtonSound()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(BlockBtn);
    }

    public void DefaultSound()
    {
        if (SettingsScript.SoundOn)
            ad.PlayOneShot(DefaultStance);
    }
}
