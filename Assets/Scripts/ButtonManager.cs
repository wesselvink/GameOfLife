﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

	public void Play(){
		SceneManager.LoadScene ("Game");
	}
	public void MainMenu(){
		SceneManager.LoadScene ("MainMenu");
	}
}