using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoButtons : MonoBehaviour {

    public Text text;

    public string texttosay;

    public void Start()
    {

    }

    public void setText()
    {

        text.text = texttosay;
    }
}
