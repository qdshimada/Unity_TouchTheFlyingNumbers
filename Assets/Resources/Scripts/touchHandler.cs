using UnityEngine;
using System.Collections;

/* 
 * タップ（またはクリック）動作とボールの衝突を管理するクラス
 */

public class touchHandler : MonoBehaviour {
	private GameObject targetObj;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0) || Input.GetButtonDown ("Fire1")) {

			Vector3    aTapPoint   = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D aCollider2d = Physics2D.OverlapPoint(aTapPoint);

			if (aCollider2d) {
				targetObj = aCollider2d.transform.gameObject;
				//Debug.Log(targetObj);
				//Destroy(obj);

				BallController ballController = GetComponent<BallController> ();
				ballController.Tapped (targetObj);
			}
		}
	}
}
