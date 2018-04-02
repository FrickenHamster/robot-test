using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	public Button StartButton;

	void Start () {
		StartButton.onClick.AddListener(StartGame);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void StartGame() {
		SceneManager.LoadScene("Game");
	}
}
