using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tutorial : MonoBehaviour {

	public GameObject panel;
	public Text text;
	private bool isPaused=false;
	private int counter = 0;
	float TmStart;
	float TmLen=1f;

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
		if (Input.GetKeyDown("space")) {
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
