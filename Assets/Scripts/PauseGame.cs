using UnityEngine;
using System.Collections;


using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class PauseGame : MonoBehaviour {

	public GameObject myo = null;

	private ShowPanel showPanel;						//Reference to the ShowPanels script used to hide and show UI panels
	private bool isPaused;								//Boolean to check if the game is paused or not
	//private StartOptions startScript;					//Reference to the StartButton script
	private Pose _lastPose = Pose.Unknown;

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
		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();
		//Check if it is the last level
		/*if (Application.loadedLevel + 1 == null) {

		}*/
		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;
			//Check if the Cancel button in Input Manager is down this frame (default is Escape key) and that game is not paused, and that we're not in main menu
			if (thalmicMyo.pose == Pose.DoubleTap && !isPaused) {
				thalmicMyo.Vibrate (VibrationType.Medium);
				//Call the DoPause function to pause the game
				DoPause ();
			} 
		//If the button is pressed and the game is paused and not in main menu
			else if (thalmicMyo.pose == Pose.Fist && isPaused) {
				thalmicMyo.Vibrate (VibrationType.Medium);
					//Call the UnPause function to unpause the game
				//Set isPaused to false
				isPaused = false;
				//Set time.timescale to 1, this will cause animations and physics to continue updating at regular speed
				Time.timeScale = 1;
			}
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
