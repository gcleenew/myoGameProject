﻿using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {
	
	
	private ShowPanel showPanel;						//Reference to the ShowPanels script used to hide and show UI panels
	private bool isPaused;								//Boolean to check if the game is paused or not
	//private StartOptions startScript;					//Reference to the StartButton script
	
	//Awake is called before Start()
	void Awake()
	{
		//Get a component reference to ShowPanels attached to this object, store in showPanels variable
		showPanel = GetComponent<ShowPanel> ();
		//showPanel.HidePausePanel ();
		//Get a component reference to StartButton attached to this object, store in startScript variable
		//startScript = GetComponent<StartOptions> ();
	}
	
	// Update is called once per frame
	void Update () {

		//Check if it is the last level
		/*if (Application.loadedLevel + 1 == null) {

		}*/
		
		//Check if the Cancel button in Input Manager is down this frame (default is Escape key) and that game is not paused, and that we're not in main menu
		if (Input.GetButtonDown ("Cancel") && !isPaused ) 
		{
			//Call the DoPause function to pause the game
			DoPause();
		} 
		//If the button is pressed and the game is paused and not in main menu
		else if (Input.GetButtonDown ("Cancel") && isPaused ) 
		{
			//Call the UnPause function to unpause the game
			UnPause ();
		}
	}
	
	
	public void DoPause()
	{
		//Set isPaused to true
		isPaused = true;
		//Set time.timescale to 0, this will cause animations and physics to stop updating
		Time.timeScale = 0;
		//call the ShowPausePanel function of the ShowPanels script
		showPanel.ShowPausePanel ();
	}
	
	
	public void UnPause()
	{
		//Set isPaused to false
		isPaused = false;
		//Set time.timescale to 1, this will cause animations and physics to continue updating at regular speed
		Time.timeScale = 1;
		//call the HidePausePanel function of the ShowPanels script
		showPanel.HidePausePanel ();
	}
	
	
}
