using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	static public GameController Instance;
	public GameObject WinScreen;
	public GameObject LoseScreen;

	// Use this for initialization
	void Start () {
		Instance = this;
		WinScreen = GameObject.Find("WinScreen");
		WinScreen.SetActive(false);
		LoseScreen = GameObject.Find("LoseScreen");
		LoseScreen.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RestartGame() {
		SceneManager.LoadScene("Game");
	}
}
