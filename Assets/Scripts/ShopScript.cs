using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour {

    public static int PricePerItem = 500;
    public Text priceText;

    public static bool allitemssold = false;

	// Use this for initialization
	void Start () {
        if (PricePerItem == 1400)
            allitemssold = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (PricePerItem == 1400)
            allitemssold = true;

        if (!allitemssold)
            priceText.text = "Price Per Item- $" + PricePerItem;
        else
            priceText.text = "All items are sold!";
	}

    public static void NewPrice()
    {
        if (PricePerItem == 500)
        {
            PricePerItem = 700;
        }
        else if (PricePerItem == 700)
        {
            PricePerItem = 900;
        }
        else if(PricePerItem == 900)
        {
            PricePerItem = 1100;
        }
        else if (PricePerItem == 1100)
        {
            PricePerItem = 1200;
        }
        else if (PricePerItem == 1200)
        {
            PricePerItem = 1300;
        }
        else if (PricePerItem == 1300)
        {
            PricePerItem = 1400;
            allitemssold = true;
        }
    }
}
