using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontWannaLeaveScript : MonoBehaviour {

    public GameObject popui;
    public GameObject seperateMoneyUI;

    public void CloseUI()
    {
        popui.GetComponent<UIscript>().BeginDeactiving();
    }

    public void OpenUI()
    {
        popui.GetComponent<UIscript>().StartOpenings();
    }

    public void OpenSeperate()
    {
        seperateMoneyUI.GetComponent<UIscript>().StartOpenings();
    }

    public void TestingPrice()
    {
        if(Coins.Money >= ShopScript.PricePerItem)
        {
            OpenUI();
        }
        else
        {
            OpenSeperate();
        }
    }

    public void SetBuyItem()
    {
        BuyButtons.currentBuyButton = this.gameObject;
        BuyButtons.CurrentBuyItem = this.GetComponent<BuyButtons>().itemId;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
