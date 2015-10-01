using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;
using System;

public class PlayerController : MonoBehaviour {
    public JointOrientation Joint = null;

    public GiveRotation getJointRotation;

    private Rigidbody rb;
	public float speed;
    public float xMin;
    public float xGap;
    public float yMin;
    public float yGap;
    public Text countText;
	public Text winText;

	private int count;
	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		count = 0;
		SetCountText ();
		winText.text = "";

        JointOrientation Joint = Joint.GetComponent<JointOrientation> ();


	}

	void FixedUpdate ()
	{
        float x = Joint.transform.rotation.eulerAngles.x;
        float y = Joint.transform.rotation.eulerAngles.y;
        float z = Joint.transform.rotation.eulerAngles.z;
        
        float moveHorizontal = 0;
        float moveVertical = 0;
        print("x:"+ x);
        print("y:"+ y);
        print("z:"+ z);
        /*if (x >= xMin + xGap)
        {
            moveHorizontal = -1;
        }
        else if (x <= xMin )
        {
            moveHorizontal = 1;
        }
        else
        {
            moveHorizontal = 0;
        }

        if (y >= yMin + yGap)
        {
            moveVertical = -1;
        }
        else if (y <= yMin)
        {
            moveVertical = 1;
        }
        else
        {
            moveVertical = 0;
        }*/

        Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed); 
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
		if (count >= 13) {
			winText.text = "You Win!";
		}
	}

    

    


}
