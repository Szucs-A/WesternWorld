using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    private Animator myAnimator;

    private int ButtonState = 0;

    private int[] PlayerHasNoBullets, EnemyHasNoBullets, BothHaveBullets;
    private int[] MediumPNoBullets, MediumENoBullets, MediumBBullets;
    private int[] EasyPNoBullets, EasyENoBullets, EasyBBullets;

    private int bullets = 0;

    // Use this for initialization
    void Start()
    {
        // hards
        PlayerHasNoBullets = new int[]  {1, 1, 1, 1, 1, 1, 3, 3, 3, 3 };
        EnemyHasNoBullets = new int[]   {2, 2, 2, 2, 2, 2, 2, 3, 3, 3 };
        BothHaveBullets = new int[]     {1, 1, 1, 2, 2, 2, 2, 2, 2, 3 };

        MediumPNoBullets = new int[] { 1, 1, 1, 2, 2, 3, 3, 3, 3, 3 };
        MediumENoBullets = new int[] { 3, 2, 2, 2, 2, 3, 3, 3, 3, 3 };
        MediumBBullets = new int[]   { 1, 1, 1, 1, 2, 2, 2, 3, 3, 3 };

        EasyPNoBullets = new int[] { 1, 1, 2, 2, 3, 3, 3, 3, 3, 3};
        EasyENoBullets = new int[] { 3, 2, 2, 3, 3, 3, 3, 3, 3, 3};
        EasyBBullets = new int[]   { 1, 1, 3, 1, 2, 2, 3, 3, 3, 3};

        if (SingleGameManager.currentLevel <= 3)
        {
            PlayerHasNoBullets = EasyPNoBullets;
            EnemyHasNoBullets = EasyENoBullets;
            BothHaveBullets = EasyBBullets;
        }
        else if(SingleGameManager.currentLevel <= 8)
        {
            PlayerHasNoBullets = MediumPNoBullets;
            EnemyHasNoBullets = MediumENoBullets;
            BothHaveBullets = MediumBBullets;
        }

        myAnimator = GetComponent<Animator>();

        if (!BuyButtons.boughtGun)
            ButtonState = 3;
        else
            ButtonState = 2;
    }

    public void GetNewButtonState()
    {
        int newbs = 0;

        // gets the first bullet.
        if(bullets == 0 && PlayerScript.currentPlayer.bullets == 0)
        {
            // Reload the first bullet.
            newbs = 3;
        }
        else if(bullets != 0 && PlayerScript.currentPlayer.bullets == 0)
        {
            // Play Agressive.
            newbs = FindaButtonState(PlayerHasNoBullets);
        }
        else if(bullets == 0 && PlayerScript.currentPlayer.bullets != 0)
        {
            // No extra checks here.
            // Play defensive.
            int random = Random.Range(0, 10);
            newbs = EnemyHasNoBullets[random];
        }
        else
        {
            // Play Randomly.
            newbs = FindaButtonState(BothHaveBullets);
        }
        
        SetButtonState(newbs);

        PlayerScript.currentPlayer.TestForWisdom(newbs);
    }

    public int FindaButtonState(int[] play)
    {
        bool goOn = true;
        int icon = 0;

        while (goOn)
        {
            int newbs = Random.Range(0, 10);

            icon = play[newbs];

            if(icon == 3 && bullets < 3)
            {
                goOn = false;
            }
            else if(icon == 1 && bullets >= 1)
            {
                goOn = false;
            }
            else if(icon == 2)
            {
                goOn = false;
            }
        }
        
        return icon;
    }

    public void SetButtonState(int bs)
    {
        ButtonState = bs;
    }

    public void UpdateAnimation()
    {
        if (ButtonState == 3)
        {
            bullets++;
        }

        if (ButtonState == 1)
        {
            bullets--;
        }
        
        myAnimator.SetInteger("ButtonState", ButtonState);

        jump();
        
        GetNewButtonState();
    }
    
    public void jump()
    {
        this.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 7, ForceMode2D.Impulse);
    }

    public void DirectAnimations()
    {
        myAnimator.SetInteger("ButtonState", ButtonState);
    }

    public void Die()
    {
        ButtonState = -1;
        DirectAnimations();
    }

    public void ResetPlayer()
    {
        ButtonState = 0;
        bullets = 0;
        DirectAnimations();

        if (!BuyButtons.boughtGun)
            ButtonState = 3;
        else
            ButtonState = 2;
    }
}
