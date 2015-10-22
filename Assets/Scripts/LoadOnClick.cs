using UnityEngine;
using System.Collections;

public class LoadOnClick : MonoBehaviour {
	
	public GameObject loadingImage;
	
	public void LoadScene(int level)
	{
		//loadingImage.SetActive(true);
		Application.LoadLevel(level);
	}

	public void Restart(){
		Application.LoadLevel (Application.loadedLevel);
	}

	public void NextLevel(){
		int currentLevel = Application.loadedLevel;
		Application.LoadLevel (currentLevel + 1);
	}
}