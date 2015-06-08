using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuButtonHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick() { 
		Application.LoadLevel("Main");
	}

}
