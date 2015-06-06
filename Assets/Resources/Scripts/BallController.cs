using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

/* 
 * ボール生成用空オブジェクトに持たせる、生成、管理のコンポーネント
 */

public class BallController : MonoBehaviour {

	public GameObject targetNumberObj;//今タップすべき数値　のゲームオブジェクト
	public Text timerText; //タイマーのテキスト
	public Sprite[] sprites; // 1〜10の画像を設定
	public GameObject ResetButton; // リセットボタン

	private Text targetNumberText; //今タップすべき数値
	protected int targetNumber = 0;//今タップすべき数値（BallArrayのキー）
	protected int defaultCnt = 10;//BallArray数
	private int cnt = 10;//BallArray数
	private GameObject[] BallArray = new GameObject[10];

	// 一判定用
	private float margin = 0.1f; //マージン(画面外に出てどれくらい離れたら消えるか)を指定
	private float negativeMargin;
	private float positiveMargin;
	protected Vector3 startPosition;
	protected float addforce_x;
	protected float addforce_y;

	// スタートのカウント用
	private bool isPlaying = false; //プレイ中かどうか
	private DateTime startTime;// ゲーム開始時刻
	private TimeSpan pastTime;// 経過時間

	// Use this for initialization
	void Start () {

		targetNumberText = targetNumberObj.GetComponent<Text>();
		int targetNumberDisp = targetNumber + 1;
		targetNumberText.text = "Next : "+targetNumberDisp.ToString();
		ResetButton.SetActive (false);

		negativeMargin = 0 - margin;
		positiveMargin = 1 + margin;

		timerText.text = "00:00:00";
		StartCoroutine ("StartCount");
		//isPlaying = true;
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (isPlaying);
		if (isPlaying == true) {
			for (int i = 0; i < BallArray.Length; i++) {
				if (BallArray [i] != null) {
					if (isOutOfScreen (BallArray [i])) {
						RemoveNumber (BallArray [i]);
					}
				}
			}

			if (cnt == 0) {
				//Debug.Log(cnt);
				MakeBallArray (targetNumber);
				cnt = defaultCnt - targetNumber;
			}
			//Debug.Log(isOutOfScreen(BallArray[0]));

			// 経過時間を取得
			pastTime = DateTime.Now - startTime;
			//Debug.Log(timerText);
			timerText.text = pastTime.ToString ();
		}

	}

	void MakeBallArray (int num) {
		for (int i = num; i < BallArray.Length; i++) {
			BallArray [i] = MakeABall (i);
		}
	}

	GameObject MakeABall (int i) {
		// 画面左下のワールド座標をビューポートから取得
		Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
		// 画面右上のワールド座標をビューポートから取得
		Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

		float width = sprites [i].bounds.size.x;
		float x = UnityEngine.Random.Range (min.x + width/2, max.x - width/2);
		float y = UnityEngine.Random.Range (max.y, max.y+1.0f);

		//addforce_x = Random.Range (-10.0f, 10.0f);
		//addforce_y = Random.Range (-300.0f, -100.0f);
		addforce_y = UnityEngine.Random.Range (-200.0f, -100.0f);

		startPosition = new Vector3 (x, y, 0.0f);

		// プレハブからインスタンスを生成
		GameObject prefab = (GameObject)Resources.Load ("Prefabs/Ball");
		GameObject ball = Instantiate (prefab, startPosition, Quaternion.identity) as GameObject;
		// 落下の向き
		ball.GetComponent<Rigidbody2D>().AddForce (new Vector2 (0.0f,addforce_y));
		// ボールのテクスチャ
		ball.GetComponent<SpriteRenderer> ().sprite = sprites [i];
		// オブジェクト名 = targetNumber としておく
		ball.name = i.ToString();

		return ball;
	}

	public void Tapped (GameObject obj) {
		int i = int.Parse(obj.name);

		if (targetNumber == i) {
			RemoveNumber (obj);
			targetNumber++;
			int targetNumberDisp = targetNumber + 1;
			if (targetNumber >= 10) {
				//targetNumber = 0;
				Debug.Log ("clear!");
				isPlaying = false;
				ResetButton.SetActive (true);
			} else {
				targetNumberText.text = "Next : "+targetNumberDisp.ToString();
			}

		} else {
			// 間違った番号をタップした場合
			Debug.Log("wrong!");
		}
	}

	public void RemoveNumber (GameObject obj) {
		Destroy(obj);
		cnt--;
	}

	// 画面外に出ているかの判定
	bool isOutOfScreen(GameObject obj) 
	{
		Vector3 positionInScreen = Camera.main.WorldToViewportPoint(obj.transform.position);
		//positionInScreen.z = obj.transform.position.z;

		if (positionInScreen.x <= negativeMargin ||
			positionInScreen.x >= positiveMargin ||
			positionInScreen.y <= negativeMargin ||
			positionInScreen.y >= positiveMargin)
		{
			return true;
		} else {
			return false;
		}
	}

	IEnumerator StartCount () {
		Debug.Log ("3");
		timerText.text = "3";
		yield return new WaitForSeconds (1.0f);
		Debug.Log ("2");
		timerText.text = "2";
		yield return new WaitForSeconds (1.0f);
		Debug.Log ("1");
		timerText.text = "1";
		yield return new WaitForSeconds (1.0f);
		isPlaying = true;
		startTime = DateTime.Now;// ゲーム開始時刻
		MakeBallArray (0);

	}
}
