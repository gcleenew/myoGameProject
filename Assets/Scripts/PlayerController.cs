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


    private Rigidbody rb;
	public float speed;
    public float xGap;
    public float yGap;
    public int countVictory;
    public Text countText;
	public Text winText;
    public Text levelText;

	private int count;
	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		count = 0;
		SetCountText ();
		winText.text = "";
        levelText.text = "";

        


	}

	void FixedUpdate ()
	{
        var JointObject =  GameObject.Find("Stick");
        float x = JointObject.transform.rotation.eulerAngles.x;
        float y = JointObject.transform.rotation.eulerAngles.y;
        float z = JointObject.transform.rotation.eulerAngles.z;
        float moveHorizontal = Input.GetAxis ("Horizontal");
        float moveVertical = Input.GetAxis ("Vertical");
        float moveUp = 0;

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
        if (Input.GetKeyDown ("j")) {
            moveUp = 100;
        }

        Vector3 movement = new Vector3 (moveHorizontal, moveUp, moveVertical);  

		rb.AddForce (movement * speed);

        if (count >= 0)
        {
            if (Input.GetKeyDown("return"))
            {

                Application.LoadLevel("minigame");
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
            levelText.text = "Press Enter to continue";
		}
	}

   

    

    


}
