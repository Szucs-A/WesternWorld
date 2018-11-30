using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour {

    public bool calledOnce = false;

    private GameObject player, enemy;

    public GameObject drawText;

    public bool beginning = true;

    // Use this for initialization
    void Start () {
        Stoptimer();

        enemy = GameObject.FindGameObjectWithTag("Enemy");
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && enemy != null)
            if(SingleGameManager.currentLevel != 1)
               StartTimer();
    }

    public void StartTimer()
    {
        this.GetComponent<Animator>().speed = 1;
    }

    public void Stoptimer()
    {
        this.GetComponent<Animator>().speed = 0;
    }

    public void Determine()
    {
        if (beginning)
            return;

        calledOnce = false;
        int playerState = player.GetComponent<Animator>().GetInteger("ButtonState");
        int enemyState = enemy.GetComponent<Animator>().GetInteger("ButtonState");

        if(( enemyState == 0 || enemyState == 3) && playerState == 1)
        {
            enemy.GetComponent<EnemyScript>().Die();
            //SoundManager.current.PlayRoundWin();
            SoundManager.current.PlayPlayerShotSound();
            EndGame();
        }
        else if(enemyState == 1 && (playerState == 3 || playerState == 0))
        {
            if (BuyButtons.boughtHelmet && !player.GetComponent<PlayerScript>().usedSteelplating)
            {
                player.GetComponent<PlayerScript>().UseSteelPlating();
                SoundManager.current.PlayLosingArmorSound();
            }
            else if (BuyButtons.boughtButtons && !player.GetComponent<PlayerScript>().usedDeputy)
            {
                player.GetComponent<PlayerScript>().UseDeputy();
                SoundManager.current.PlayLosingDeputySound();
            }
            else
            {
                player.GetComponent<PlayerScript>().Die();
                SoundManager.current.PlayRoundLost();
                SoundManager.current.PlayEnemyShotSound();
                playerDied();
            }
        } 
        else if(enemyState == 1 && playerState == 1 && BuyButtons.boughtBullets)
        {
            enemy.GetComponent<EnemyScript>().Die();
            EndGame();
        }
        else if(enemyState == 1 && playerState == 1 && !BuyButtons.boughtBullets)
        {
            TurnOnDrawText();
        }
        else
        {
            PlayerDetermineSounds(playerState);
            EnemyDetermineSounds(enemyState);
        }
    }

    public void EnemyDetermineSounds(int ButtonState)
    {
        if (ButtonState == 0)
        {
            SoundManager.current.DefaultSound();
        }
        else if (ButtonState == 1)
        {
            SoundManager.current.PlayEnemyShotSound();
        }
        else if (ButtonState == 2)
        {
            SoundManager.current.PlayEnemyDuckSound();
        }
        else if (ButtonState == 3)
        {
            SoundManager.current.PlayEnemyReloadSound();
        }
    }

    public void PlayerDetermineSounds(int btnState)
    {
        if (btnState == 0)
        {
            SoundManager.current.DefaultSound();
        }
        else if (btnState == 1)
        {
            SoundManager.current.PlayPlayerShotSound();
        }
        else if (btnState == 2)
        {
            SoundManager.current.PlayPlayerDuckSound();
        }
        else if (btnState == 3)
        {
            SoundManager.current.PlayPlayerReloadSound();
        }
    }

    public void TurnOnDrawText()
    {
        drawText.GetComponent<Animator>().SetBool("TurnOff", true);
    }

    public void playerDied()
    {
        this.GetComponent<Animator>().SetBool("GameEnded", true);
        GameObject.Find("SingleGameManager").GetComponent<SingleGameManager>().playerDied();
    }

    public void EndGame()
    {
        this.GetComponent<Animator>().SetBool("GameEnded", true);
        GameObject.Find("SingleGameManager").GetComponent<SingleGameManager>().endGame();
    }

    public void ResetTimer()
    {
        this.GetComponent<Animator>().SetBool("GameEnded", false);
        beginning = true;
        player.GetComponent<PlayerScript>().ResetPlayer();
        enemy.GetComponent<EnemyScript>().ResetPlayer();
    }

    public void TakeAction()
    {
        if (calledOnce)
            return;

        player.GetComponent<PlayerScript>().UpdateAnimations();
        enemy.GetComponent<EnemyScript>().UpdateAnimation();

        calledOnce = true;
        beginning = false;
    }
}
