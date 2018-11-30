using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMaster : MonoBehaviour {

    public GameObject AttackButton;
    public GameObject BlockButton;
    public GameObject ReloadButton;

    private Animator AttackButtonAnimator;
    private Animator BlockButtonAnimator;
    private Animator ReloadButtonAnimator;
    private PUNPlayer_Script ps;
    private PlayerScript pss;

    // Use this for initialization
    void Start() {
        AttackButtonAnimator = AttackButton.GetComponent<Animator>();
        BlockButtonAnimator = BlockButton.GetComponent<Animator>();
        ReloadButtonAnimator = ReloadButton.GetComponent<Animator>();
        ps = this.GetComponent<PUNPlayer_Script>();
        if(ps == null)
        {
            pss = this.GetComponent<PlayerScript>();
        }

        if (ps != null)
        {
            AttackButton.GetComponent<ButtonRipper>().rip();
            BlockButton.GetComponent<ButtonRipper>().rip();
            ReloadButton.GetComponent<ButtonRipper>().rip();
        }
    }

    public void setGreenButton(int newenemystate)
    {
        if (newenemystate == 1)
        {
            BlockButtonAnimator.SetBool("IsGreen", true);
        }
        else if (newenemystate == 2)
        {
            ReloadButtonAnimator.SetBool("IsGreen", true);
        }
        else
        {
            AttackButtonAnimator.SetBool("IsGreen", true);
        }
    }

    public void TurnOffGreenButtons()
    {
        AttackButtonAnimator.SetBool("IsGreen", false);
        ReloadButtonAnimator.SetBool("IsGreen", false);
        BlockButtonAnimator.SetBool("IsGreen", false);
    }

    public void resetButtons()
    {
        BlockButtonAnimator.SetBool("Clicked", false);
        AttackButtonAnimator.SetBool("Clicked", false);
        ReloadButtonAnimator.SetBool("Clicked", false);
    }

    public void SwapButton(string name)
    {
        if (name == "AttackButton")
        {
            BlockButtonAnimator.SetBool("Clicked", false);
            AttackButtonAnimator.SetBool("Clicked", true);
            ReloadButtonAnimator.SetBool("Clicked", false);

            SoundManager.current.AttackButtonSound();

            if (ps == null)
                pss.buttonShoot();
            else
                ps.buttonShoot();
        }
        else if (name == "BlockButton")
        {
            BlockButtonAnimator.SetBool("Clicked", true);
            AttackButtonAnimator.SetBool("Clicked", false);
            ReloadButtonAnimator.SetBool("Clicked", false);

            SoundManager.current.BlockButtonSound();

            if (ps == null)
                pss.buttonBlock();
            else
                ps.buttonBlock();
        }
        else if (name == "ReloadButton")
        {
            BlockButtonAnimator.SetBool("Clicked", false);
            AttackButtonAnimator.SetBool("Clicked", false);
            ReloadButtonAnimator.SetBool("Clicked", true);

            SoundManager.current.ReloadButtonSound();

            if (ps == null)
                pss.buttonReload();
            else
                ps.buttonReload();
        }
    }

    public void KillAll()
    {
        AttackButton.SetActive(false);
        BlockButton.SetActive(false);
        ReloadButton.SetActive(false);
    }
}
