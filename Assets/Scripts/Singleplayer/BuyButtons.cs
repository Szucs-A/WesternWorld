using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class itemsCollection
{
    public bool bg, bh, bbllts, bbuttns, bw, bs;
    public int pricerper;
}

public class BuyButtons : MonoBehaviour {
    // Steel Plating = Helmet
    // Deputy = Buttons
    // Titanium Bullets = Bullets
    // Extra Bullets = Gun
    // Green Button = Wisdom
    // Coins = Shield
    public static bool boughtGun, boughtHelmet, boughtBullets, boughtButtons, boughtWisdom, boughtShield;

    public static int CurrentBuyItem = 0;
    public static GameObject currentBuyButton;

    public Text boughtText;

    public int itemId;

    public GameObject infoButton;

    // Use this for initialization
    void Start ()
    {
        if (boughtText != null)
            boughtText.text = "";

        //Talk too much
        
        switch (itemId)
        {
            case 1:
                if (boughtBullets)
                {
                    setBoughtStart();
                }
                break;
            case 2:
                if (boughtButtons)
                {
                    setBoughtStart();
                }
                break;
            case 3:
                if (boughtGun)
                {
                    setBoughtStart();
                }
                break;
            case 4:
                if (boughtHelmet)
                {
                    setBoughtStart();
                }
                break;
            case 5:
                if (boughtShield)
                {
                    setBoughtStart();
                }
                break;
            case 6:
                if (boughtWisdom)
                {
                    setBoughtStart();
                }
                break;
        }
    }

    public void setBoughtStart()
    {
        if (infoButton != null)
        {
            Vector3 newPos = new Vector3(170, 0, 0);
            infoButton.GetComponent<RectTransform>().anchoredPosition = newPos;
            this.gameObject.SetActive(false);
            boughtText.text = "Bought!";
        }
    }

    public void setBought()
    {
        if(infoButton != null)
        {
            Vector3 newPos = new Vector3(170, 0, 0);
            infoButton.GetComponent<RectTransform>().anchoredPosition = newPos;
            this.gameObject.SetActive(false);
            boughtText.text = "Bought!";
            ShopScript.NewPrice();
        }
    }

    public void BuyFromStatic()
    {
        switch (CurrentBuyItem)
        {
            case 1:
                if (!boughtBullets)
                {
                    Coins.Money -= ShopScript.PricePerItem;
                }
                boughtBullets = true;
                break;
            case 2:
                if (!boughtButtons)
                {
                    Coins.Money -= ShopScript.PricePerItem;
                }
                boughtButtons = true;
                break;
            case 3:
                if (!boughtGun)
                {
                    Coins.Money -= ShopScript.PricePerItem;
                }
                boughtGun = true;
                break;
            case 4:
                if (!boughtHelmet)
                {
                    Coins.Money -= ShopScript.PricePerItem;
                }
                boughtHelmet = true;
                break;
            case 5:
                if (!boughtShield)
                {
                    Coins.Money -= ShopScript.PricePerItem;
                }
                boughtShield = true;
                break;
            case 6:
                if (!boughtWisdom)
                {
                    Coins.Money -= ShopScript.PricePerItem;
                }
                boughtWisdom = true;
                break;
        }

        if (currentBuyButton != null)
        {
            currentBuyButton.GetComponent<BuyButtons>().setBought();
        }

        Save();
        Coins.Save();
    }

    public void BuyButton()
    {
        switch (itemId)
        {
            case 1:
                if (!boughtBullets)
                {
                    //Subtract Money here.
                }
                boughtBullets = true;
                break;
            case 2:
                if (!boughtButtons)
                {
                    //Subtract Money here.
                }
                boughtButtons = true;
                break;
            case 3:
                if (!boughtGun)
                {
                    //Subtract Money here.
                }
                boughtGun = true;
                break;
            case 4:
                if (!boughtHelmet)
                {
                    //Subtract Money here.
                }
                boughtHelmet = true;
                break;
            case 5:
                if (!boughtShield)
                {
                    //Subtract Money here.
                }
                boughtShield = true;
                break;
            case 6:
                if (!boughtWisdom)
                {
                    //Subtract Money here.
                }
                boughtWisdom = true;
                break;
        }

        Save();
        Coins.Save();
        setBought();
    }

    public static void Save()
    {
        itemsCollection ic = new itemsCollection();
        ic.bg = boughtGun;
        ic.bh = boughtHelmet;
        ic.bbllts = boughtBullets;
        ic.bbuttns = boughtButtons;
        ic.bw = boughtWisdom;
        ic.bs = boughtShield;
        ic.pricerper = ShopScript.PricePerItem;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedItems.gd");
        bf.Serialize(file, (itemsCollection)ic);
        file.Close();
    }

    public static bool Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedItems.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedItems.gd", FileMode.Open);
            itemsCollection ice = (itemsCollection)bf.Deserialize(file);
            file.Close();

            SetItems(ice);

            return true;
        }
        else
        {
            return false;
        }
    }

    public static void SetItems(itemsCollection ic)
    {
        boughtBullets = ic.bbllts;
        boughtWisdom = ic.bw;
        boughtButtons = ic.bbuttns;
        boughtGun = ic.bg;
        boughtHelmet = ic.bh;
        boughtShield = ic.bs;
        ShopScript.PricePerItem = ic.pricerper;
    }
}
