using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

	public GameObject canvas,firstSection,secondSection,otherPlayerPrefab,emptySection, topDownShooter,mainCamera,cutSceneCameraObject,tutorialSlider;
	public float scoreUpdateDistance = 5f,gameStartOffsetTime = 4,topDownModeDuration = 15, topDownModeStartTime = 30;
	public Text scoreText;
	public List<GameObject> sections;
	public int powerCollThreshold = 15;
	public Slider powerSlider;
	public AudioClip gameBkMusic, gameOverBkMusic;
	public Image sliderFillImage;
	public Color normalColor, fullColor, reducingColor;

	[HideInInspector]
	public int noOfOtherPlayerInGame = 0;
	[HideInInspector]
	public bool powerModeEnabled = false, gameOver =false,startGame = false;

	private int score = 0, lastCreatedSectionIndex = 0;
	private float currentScoreUpdateDistance = 1,gameStartTime;
	private bool topDownShootingMode = false;
	private GameObject player,currentSection,nextSection,lastSection,topDownShooterObject,sliderObject;

	[HideInInspector]
	public AudioSource aud;

	Animator canvasAnim,sliderAnim;

	Player playerScript;

	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerScript = player.GetComponent<Player> ();
		canvasAnim = canvas.GetComponent<Animator> ();
		aud = GetComponent<AudioSource> ();
		sliderAnim = powerSlider.gameObject.GetComponent<Animator> ();

		gameStartTime = Time.time;
		powerSlider.minValue = 0;
		powerSlider.maxValue = powerCollThreshold-1;
		ResetPowerSlider ();
		sliderObject = powerSlider.gameObject;
		sliderObject.SetActive (false);
		tutorialSlider.SetActive (false);
		currentSection = firstSection;
		nextSection = secondSection;
		scoreText.text = "";
		StartCoroutine(SpawnOtherPlayer (1,10,25));   // initially spawn 1 players;
		aud.clip = gameBkMusic;
		aud.Play ();
	}
	

	void Update () {
		if ((!startGame) && (Time.time > gameStartTime + gameStartOffsetTime)) {
			startGame = true;
			GameStarted ();
		}
		if ((startGame)&&(!gameOver)) {
			if (player.transform.position.z > currentScoreUpdateDistance) {    // Score 
				currentScoreUpdateDistance = currentScoreUpdateDistance + scoreUpdateDistance;
				ScoreIncrease (1);
			}

			if((player.transform.position.z + 75)>nextSection.transform.position.z){        // Creating new Sections
				GameObject newlyCreatedSect;
				int newSectionIndex;
				do {
					newSectionIndex = Random.Range (0, sections.Count);
				} while(newSectionIndex == lastCreatedSectionIndex);
				Vector3 pos = nextSection.transform.position;
				pos.z = pos.z + 50;     // As every section is 50 units long
			//	pos.y= pos.y -0.02f;
				if (!topDownShootingMode) {
					newlyCreatedSect = Instantiate (sections [newSectionIndex], pos, Quaternion.identity)as GameObject;
				}
				else {
					newlyCreatedSect = Instantiate (emptySection, pos, Quaternion.identity)as GameObject;
				}
				lastSection = currentSection;
				currentSection = nextSection;
				nextSection = newlyCreatedSect;
				lastCreatedSectionIndex = newSectionIndex;
				Destroy (lastSection, 5);   // Destroying last section in 2 seconds
			}

			if (noOfOtherPlayerInGame < 1) {      // Spawning other players
				StartCoroutine(SpawnOtherPlayer(1,35,50));
			}

			if ((Time.time > topDownModeStartTime + gameStartTime)&&(!topDownShootingMode)) {
				topDownShootingMode = true;
				TopDownShootingModeStart ();
			}
			if (topDownShootingMode) {
				if (Time.time > topDownModeStartTime + topDownModeDuration + gameStartTime) {
					topDownModeStartTime = Time.time + topDownModeStartTime;
					topDownShootingMode = false;
					TopDownShootingModeEnd ();
				}
			}

		}
	}

	public void ScoreIncrease(int scoreUpdate){
		score = score + scoreUpdate;
		scoreText.text = score.ToString ();
	}

	void GameStarted(){
		scoreText.text = score.ToString();
		sliderObject.SetActive (true);
		Destroy (cutSceneCameraObject);
		mainCamera.GetComponent<Camera> ().enabled = true;
		tutorialSlider.SetActive (true);
		StartCoroutine (TutorialSliderEnd ());
	}
	IEnumerator TutorialSliderEnd(){
		yield return new WaitForSeconds (2);
		tutorialSlider.SetActive (false);
	}

	void TopDownShootingModeStart(){
		Vector3 pos = player.transform.position;
		pos.z = pos.z + 80;
		pos.y = pos.y + 4f;
		topDownShooterObject = Instantiate (topDownShooter, pos, Quaternion.identity)as GameObject;
	}
	void TopDownShootingModeEnd(){
		topDownShooterObject.GetComponent<TopDownShooter> ().StopGenerating ();
		topDownShooterObject.GetComponent<TopDownShooter> ().Exit();
		Destroy (topDownShooterObject, 3);
	}

	IEnumerator SpawnOtherPlayer(int number,int posMin = 35, int posMax = 40){
		for (int i = 0; i < number; i++) {
			Vector3 pos = player.transform.position;
			pos.x = Random.Range (-2, 3);
			pos.z = pos.z + Random.Range (posMin, posMax);
			GameObject op;
			op = Instantiate (otherPlayerPrefab, pos, Quaternion.identity)as GameObject;
			OtherPlayers opScript;
			opScript = op.GetComponent<OtherPlayers> ();
			int rt = Random.Range (0, 10);
			if (rt % 2 == 0) {
				opScript.reverseTrack = true;
			}
			else {
				opScript.reverseTrack = false;
			}
			yield return null;
		}
	}

	public void PowerSliderIncrease(int x){
		if (!powerModeEnabled) {
			if (powerSlider.value == powerSlider.maxValue) {
				powerModeEnabled = true;
				sliderFillImage.color = fullColor;
				sliderAnim.SetTrigger ("Full");
			}
			else {
				powerSlider.value = powerSlider.value + x;
			}
		}
	}

	public void ResetPowerSlider(){
		powerSlider.value = powerSlider.minValue;
		powerModeEnabled = false;
		sliderFillImage.color = normalColor;
		sliderAnim.SetTrigger ("Reset");
	}

	public void GameOver(){
		gameOver = true;
		StartCoroutine (GameOverAnim ());
		aud.Stop ();
		aud.clip = gameOverBkMusic;
		aud.volume = aud.volume / 0.75f;
		aud.Play ();
	}
	IEnumerator GameOverAnim(){
		yield return new WaitForSeconds (2f);
		canvasAnim.SetTrigger ("GameOver");	
	}

	public void PowerBtn(){
		if (powerModeEnabled) {
			if (!playerScript.inPowerMode) {
				playerScript.EnterPowerMode ();
				Handheld.Vibrate ();
				StartCoroutine (DuringPowerMode ());
			}
		}
	}

	IEnumerator DuringPowerMode(){
		sliderFillImage.color = reducingColor;
		for(int i =(int) powerSlider.maxValue; i >= 0; i--){
			powerSlider.value = i;
			if (i > 0) {
				yield return new WaitForSeconds ((float)(playerScript.powerDuration /powerSlider.maxValue));
			}
		}
	}

	public void Restart(){
		SceneManager.LoadScene ("Game");
	}
}
