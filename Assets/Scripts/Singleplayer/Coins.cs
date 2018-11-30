using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Coins : MonoBehaviour {
    public static int Money = 0;

    public Text coinText;

    public void Start()
    {
        if (!Load())
        {
            Money = 0;
        }

        coinText.text = "$" + Money;
    }

    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedMoney.gd");
        bf.Serialize(file, (int)Money);
        file.Close();
    }

    public bool Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedMoney.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedMoney.gd", FileMode.Open);
            Money = (int)bf.Deserialize(file);
            file.Close();

            return true;
        }
        else
        {
            return false;
        }
    }
}
