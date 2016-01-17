using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class Tutorial : MonoBehaviour {

	public GameObject panel;
	public Text text;
	private bool isPaused=false;
	private int counter = 0;
	float TmStart;
	float TmLen=1f;

	public GameObject myo = null;
	private Pose _lastPose = Pose.Unknown;

	//void Awake(){
		//Get a component reference to ShowPanels attached to this object, store in showPanels variable
		//showPanel = GetComponent<ShowPanel> ();}

	// Use this for initialization
	void Start () {
		TmStart = Time.time;
		Time.timeScale = 0;
		panel.SetActive (true);
		text.text = "Welcome to Roll a Ball tutorial!";
		isPaused = true;
	}
	
	// Update is called once per frame
	void Update () {
		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();
		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;
			//Check if the Cancel button in Input Manager is down this frame (default is Escape key) and that game is not paused, and that we're not in main menu
			if (thalmicMyo.pose == Pose.WaveOut) {
				thalmicMyo.Vibrate (VibrationType.Medium);
				counter += 1;
				if(counter == 1){
					text.text = "You play as the red ball.";
				}
				else if(counter == 2) {
					text.text = "Pick up all the yellow cubes in a minimum of time.";
				}else if(counter == 3) {
					text.text = "Use your arm to move the ball.";
				}else if(counter == 4) {
					text.text = "Make a fist to pause the game.";
				}else{
					text.text = "";
					panel.SetActive(false);
					Time.timeScale = 1;
					isPaused = false;
				}
			} 
		}
	}
}
