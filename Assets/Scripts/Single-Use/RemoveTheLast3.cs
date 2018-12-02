using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveTheLast3 : MonoBehaviour {

    public Text parent;
    public Text child;

    private void Update()
    {
        string s = parent.text;

        string a = s.Substring(0, s.Length - 3);
        this.GetComponent<Text>().text = a;

        string b = s.Substring(s.Length - 3);
        child.text = "#" + b;
    }
}
