using UnityEngine;
using System.Collections;

public class ResetButtonHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//Debug.Log("clicked");
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick() { 
		//Application.LoadLevel("Main");
		//Application.LoadLevel (Application.loadedLevelName);
		Debug.Log("clicked");
	}
}
