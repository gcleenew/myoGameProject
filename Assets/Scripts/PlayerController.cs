using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;
using System;

public class PlayerController : MonoBehaviour {
  public JointOrientation JointObject = null;

  public GameObject myo = null;
  private bool sameMovement = false;
  private Pose _lastPose = Pose.Unknown;
  private Rigidbody rb;
  public float speed;
  public float xGap;
  public float yGap;
  public int countVictory;
  public Text countText;
  public Text winText;
  public Text timerText;

  private float startTime;

	private int count;
	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		count = 0;
    startTime = 0f;
		SetCountText ();
    timerText.text = "";
		winText.text = "";

	}

	void FixedUpdate ()
	{
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();
        LoadOnClick load = GetComponent<LoadOnClick>();
        // Check if the pose has changed since last update.
        // The ThalmicMyo component of a Myo game object has a pose property that is set to the
        // currently detected pose (e.g. Pose.Fist for the user making a fist). If no pose is currently
        // detected, pose will be set to Pose.Rest. If pose detection is unavailable, e.g. because Myo
        // is not on a user's arm, pose will be set to Pose.Unknown.
        if (thalmicMyo.pose != _lastPose) {
            _lastPose = thalmicMyo.pose;
            if (thalmicMyo.pose == Pose.WaveOut) {
                sameMovement = true;
            }
          }
        else {
          sameMovement = false;
        }



        var JointObject =  GameObject.Find("Stick");
        float x = JointObject.transform.rotation.eulerAngles.x;
        float y = JointObject.transform.rotation.eulerAngles.y;
        float z = JointObject.transform.rotation.eulerAngles.z;
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");
        float moveUp = 0;

        startTime += Time.deltaTime;
        setTimerText ();

        if (0 + xGap < x && x < 180)
        {
            moveVertical = 1;
        }
        else if (180  < x && x < 360 - xGap)
        {
            moveVertical = -1;
        }


        if (0 + yGap < y && y < 180)
        {
            moveHorizontal = 1;
        }
        else if (180  < y && y < 360 - yGap)
        {
            moveHorizontal = -1;
        }
        if (sameMovement) {
            moveUp = 30;
        }


        Vector3 movement = new Vector3 (moveHorizontal, moveUp, moveVertical);

		    rb.AddForce (movement * speed);
        if (transform.position.y < -100) {
          SetLooseText();
          movement = new Vector3 (0, 0, 0);
        }


        if (Input.GetKeyDown("return"))
        {
            if (count >= countVictory)
            {
              Application.LoadLevel("SandDune");
            }

            if (transform.position.y < -100) {
              Application.LoadLevel(Application.loadedLevel);
            }
        }
    }


    //functions
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("Pick Up"))
		{
			other.gameObject.SetActive (false);
			count = count + 1;
			SetCountText();
		}
	}

	void SetCountText ()
	{
		countText.text = "Count: " + count.ToString ();
		if (count >= countVictory) {
			winText.text = "You Win!";
		}
	}

  void SetLooseText () {
    winText.text = "You loose! Make a fist to continue";
  }

  void setTimerText ()
  {
    timerText.text = Math.Round(startTime, 1, MidpointRounding.AwayFromZero)+" s";
  }
 
}
