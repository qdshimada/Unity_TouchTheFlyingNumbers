using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/* 
 * ボール生成用空オブジェクトに持たせる、生成、管理のコンポーネント
 */

public class BallController : MonoBehaviour {
	protected Vector3 startPosition;
	protected float addforce_x;
	protected float addforce_y;

	public GameObject targetNumberObj;
	private Text targetNumberText; //タイマーのテキスト
	protected int targetNumber = 0;//今タップすべき数値（BallArrayのキー）
	protected int defaultCnt = 10;//BallArray数
	private int cnt = 10;//BallArray数
	private GameObject[] BallArray = new GameObject[10];

	public Sprite[] sprites;


	// 一判定用
	private float margin = 0.1f; //マージン(画面外に出てどれくらい離れたら消えるか)を指定
	private float negativeMargin;
	private float positiveMargin;

	// スタートのカウント用
	private bool isPlaying = false; //プレイ中かどうか
	public GameObject timer; //タイマーとなるオブジェクト
	private Text timerText; //タイマーのテキスト
	private int timeLimit = 60; //制限時間
	private int countTime = 5; //カウントダウンの秒数

	// Use this for initialization
	void Start () {

		targetNumberText = targetNumberObj.GetComponent<Text>();
		int targetNumberDisp = targetNumber + 1;
		targetNumberText.text = "Next : "+targetNumberDisp.ToString();

		timerText = timer.GetComponent<Text>(); //タイマーを取得
		CountDown(); //カウントダウン開始

		MakeBallArray (0);

		negativeMargin = 0 - margin;
		positiveMargin = 1 + margin;
	}

	// Update is called once per frame
	void Update () {
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
		float x = Random.Range (min.x + width/2, max.x - width/2);
		float y = Random.Range (max.y, max.y+1.0f);

		//addforce_x = Random.Range (-10.0f, 10.0f);
		//addforce_y = Random.Range (-300.0f, -100.0f);
		addforce_y = Random.Range (-200.0f, -100.0f);

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
			targetNumberText.text = "Next : "+targetNumberDisp.ToString();
			if (targetNumber >= 10) {
				//targetNumber = 0;
				Debug.Log("clear!");
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

	private IEnumerator CountDown () {
		Debug.Log ("hoge");
		int count = countTime;
		while (count > 0) {
			WriteTimerText(count.ToString());//カウントダウンのテキストを変更
			yield return new WaitForSeconds (1.0f); //1秒待つ
			count -= 1; //カウントを1つ減らす
		}
		WriteTimerText("Start!");
		isPlaying = true;
		yield return new WaitForSeconds (1.0f);
		StartTimer(); //制限時間のカウントを開始
	}

	private IEnumerator StartTimer() {
		var count = timeLimit;
		while (count > 0) {
			WriteTimerText(count.ToString());
			yield return new WaitForSeconds (1.0f);
			count -= 1;
		}
		WriteTimerText("Finish");
		//OnDragEnd();
		isPlaying = false;
	}

	private void WriteTimerText(string str) {
		timerText.text = str;
	}
}
