using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameScript : MonoBehaviour {

    public Text usernameText, codeText;

	// Use this for initialization
	void Start () {
        usernameText.text = UsernameScript.username.username;
        codeText.text = UsernameScript.username.userId + "";
	}
	
}
