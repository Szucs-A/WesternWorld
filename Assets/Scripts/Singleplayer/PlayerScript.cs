using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    private Animator myAnimator;

    public static PlayerScript currentPlayer;
    public int bullets = 0;
    public int btnState = 0;

    public GameObject steelPlatingItem, DeputyItem;
    public GameObject regularBullets, titaniumBullets;
    public GameObject bullet1, bullet2, bullet3;
    public GameObject tbullet1, tbullet2, tbullet3;
    public GameObject rbullet1, rbullet2, rbullet3;

    public bool usedSteelplating = false, usedDeputy = false;

    public bool showWisdom = false;

    // Use this for initialization
    void Start()
    {
        usedSteelplating = false;
        usedDeputy = false;
        currentPlayer = this;
        myAnimator = GetComponent<Animator>();

        if (BuyButtons.boughtHelmet)
        {
            steelPlatingItem.SetActive(true);
        }

        if (BuyButtons.boughtButtons)
        {
            DeputyItem.SetActive(true);
        }
        
        bullet1 = rbullet1;
        bullet2 = rbullet2;
        bullet3 = rbullet3;

        if (BuyButtons.boughtBullets)
        {
            regularBullets.SetActive(false);
            titaniumBullets.SetActive(true);

            bullet1 = tbullet1;
            bullet2 = tbullet2;
            bullet3 = tbullet3;
        }
        
        if (BuyButtons.boughtGun)
        {
            bullets = 1;
            bullet1.SetActive(true);
        }
    }

    public void UseSteelPlating()
    {
        usedSteelplating = true;
        steelPlatingItem.GetComponent<Animator>().SetBool("Used", true);
    }

    public void UseDeputy()
    {
        usedDeputy = true;
        DeputyItem.GetComponent<Animator>().SetBool("Used", true);
    }

    public void TestForWisdom(int newenemystate)
    {
        if (BuyButtons.boughtWisdom)
        {
            int tester = Random.Range(0, 10);
            if(tester == 1)
            {
                this.GetComponent<ButtonMaster>().setGreenButton(newenemystate);
                Invoke("TurnOffGreenButtons", 2f);
            }
        }
    }

    public void TurnOffGreenButtons()
    {
        this.GetComponent<ButtonMaster>().TurnOffGreenButtons();
    }

    public void buttonShoot()
    {
        btnState = 1;
    }

    public void buttonReload()
    {
        btnState = 3;
    }

    public void buttonBlock()
    {
        btnState = 2;
    }

    public void Die()
    {
        btnState = -1;
        myAnimator.SetInteger("ButtonState", btnState);
    }

    public void UpdateAnimations()
    {
        if(btnState == 1)
        {
            if(bullets >= 1)
            {
                bullets--;
            }
            else
            {
                // Play Sound here for error 
                btnState = 0;
            }
        }

        if(btnState == 3)
        {
            if (bullets < 3)
            {
                bullets++;
            }
            else
            {
                // Play Sound here for error 
                btnState = 0;
            }
        }
        
        UpdateBullets();

        myAnimator.SetInteger("ButtonState", btnState);
        jump();
    }
    
    public void UpdateBullets()
    {
        if(bullets == 0)
        {
            bullet1.SetActive(false);
            bullet2.SetActive(false);
            bullet3.SetActive(false);
        }
        else if(bullets == 1)
        {
            bullet1.SetActive(true);
            bullet2.SetActive(false);
            bullet3.SetActive(false);
        }
        else if(bullets == 2)
        {
            bullet1.SetActive(true);
            bullet2.SetActive(true);
            bullet3.SetActive(false);
        }
        else if(bullets == 3)
        {
            bullet1.SetActive(true);
            bullet2.SetActive(true);
            bullet3.SetActive(true);
        }
    }

    public void DirectAnimations()
    {
        myAnimator.SetInteger("ButtonState", btnState);
    }

    public void ResetPlayer()
    {
        bullets = 0;
        if (BuyButtons.boughtGun)
            bullets = 1;

        UpdateBullets();

        btnState = 0;
        DirectAnimations();
        this.GetComponent<ButtonMaster>().resetButtons();
    }

    public void jump()
    {
        this.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 7, ForceMode2D.Impulse);
    }
}
