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
        if (Input.GetKeyDown ("j") && transform.position.y < 1) {
            moveUp = 30;
        }
        if (Input.GetKey("space") && transform.position.y < 2 && rb.position.y < 2)
            moveUp = 5;

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
      levelText.text = "Press Enter to continue";
		}
	}
    void UpDate()
    {

        //if (Input.GetKey("space") && Physics.Raycast(transform.position, -Vector3.up, 1))
        if (Input.GetKey("up"))
        {
            rb.AddForce(Vector3.right * -speed * Time.deltaTime);
            //rb.velocity.y = jumpSpeed;
            //Rigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }

        if (Input.GetKey("right"))
        {
            
            rb.AddForce(Vector3.right * speed * Time.deltaTime);
        }

        if (Input.GetKey("left"))
        {
            rb.AddForce(Vector3.right * -speed * Time.deltaTime);
        }
    }

  void SetLooseText () {
    winText.text = "You loose!";
    levelText.text = "Press Enter to reload";
  }
}

