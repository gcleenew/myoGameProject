using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;
using System;

public class PlayerController : MonoBehaviour {
    // Myo game object to connect with.
    // This object must have a ThalmicMyo script attached.
    public GameObject myo = null;

    // A rotation that compensates for the Myo armband's orientation parallel to the ground, i.e. yaw.
    // Once set, the direction the Myo armband is facing becomes "forward" within the program.
    // Set by making the fingers spread pose or pressing "r".
    private Quaternion _antiYaw = Quaternion.identity;

    // A reference angle representing how the armband is rotated about the wearer's arm, i.e. roll.
    // Set by making the fingers spread pose or pressing "r".
    private float _referenceRoll = 0.0f;

    // The pose from the last update. This is used to determine if the pose has changed
    // so that actions are only performed upon making them rather than every frame during
    // which they are active.
    private Pose _lastPose = Pose.Unknown;

    private Rigidbody rb;
	public float speed;
	public Text countText;
	public Text winText;

	private int count;
	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		count = 0;
		SetCountText ();
		winText.text = "";


	}

	void FixedUpdate ()
	{
        // Access the ThalmicMyo component attached to the Myo object.
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();

        // Update references when the pose becomes fingers spread or the q key is pressed.
        bool updateReference = false;
        if (thalmicMyo.pose != _lastPose)
        {
            _lastPose = thalmicMyo.pose;

            if (thalmicMyo.pose == Pose.FingersSpread)
            {
                updateReference = true;

                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
        }
        if (Input.GetKeyDown("r"))
        {
            updateReference = true;
        }

        // Update references. This anchors the joint on-screen such that it faces forward away
        // from the viewer when the Myo armband is oriented the way it is when these references are taken.
        if (updateReference)
        {
            // _antiYaw represents a rotation of the Myo armband about the Y axis (up) which aligns the forward
            // vector of the rotation with Z = 1 when the wearer's arm is pointing in the reference direction.
            _antiYaw = Quaternion.FromToRotation(
                new Vector3(myo.transform.forward.x, 0, myo.transform.forward.z),
                new Vector3(0, 0, 1)
            );

            // _referenceRoll represents how many degrees the Myo armband is rotated clockwise
            // about its forward axis (when looking down the wearer's arm towards their hand) from the reference zero
            // roll direction. This direction is calculated and explained below. When this reference is
            // taken, the joint will be rotated about its forward axis such that it faces upwards when
            // the roll value matches the reference.
            Vector3 referenceZeroRoll = computeZeroRollVector(myo.transform.forward);
            _referenceRoll = rollFromZero(referenceZeroRoll, myo.transform.forward, myo.transform.up);
        }

        // Current zero roll vector and roll value.
        Vector3 zeroRoll = computeZeroRollVector(myo.transform.forward);
        float roll = rollFromZero(zeroRoll, myo.transform.forward, myo.transform.up);

        // The relative roll is simply how much the current roll has changed relative to the reference roll.
        // adjustAngle simply keeps the resultant value within -180 to 180 degrees.
        float relativeRoll = normalizeAngle(roll - _referenceRoll);

        // antiRoll represents a rotation about the myo Armband's forward axis adjusting for reference roll.
        Quaternion antiRoll = Quaternion.AngleAxis(relativeRoll, myo.transform.forward);

        
        float moveHorizontal = 0;
        float moveVertical = 0;

        /*if (relativeRoll >= 25)
        {
            moveHorizontal = -1;
        }
        else if (relativeRoll <= -25)
        {
            moveHorizontal = 1;
        }*/

        if (transform.localRotation.y >= 0.1)
        {
            moveVertical = 1;
        }
        else if (transform.localRotation.y <= -0.1)
        {
            moveVertical = -1;
        }

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

    // Extend the unlock if ThalmcHub's locking policy is standard, and notifies the given myo that a user action was
    // recognized.
    void ExtendUnlockAndNotifyUserAction(ThalmicMyo myo)
    {
        ThalmicHub hub = ThalmicHub.instance;

        if (hub.lockingPolicy == LockingPolicy.Standard)
        {
            myo.Unlock(UnlockType.Timed);
        }

        myo.NotifyUserAction();
    }

    // Compute a vector that points perpendicular to the forward direction,
    // minimizing angular distance from world up (positive Y axis).
    // This represents the direction of no rotation about its forward axis.
    Vector3 computeZeroRollVector(Vector3 forward)
    {
        Vector3 antigravity = Vector3.up;
        Vector3 m = Vector3.Cross(myo.transform.forward, antigravity);
        Vector3 roll = Vector3.Cross(m, myo.transform.forward);

        return roll.normalized;
    }

    // Compute the angle of rotation clockwise about the forward axis relative to the provided zero roll direction.
    // As the armband is rotated about the forward axis this value will change, regardless of which way the
    // forward vector of the Myo is pointing. The returned value will be between -180 and 180 degrees.
    float rollFromZero(Vector3 zeroRoll, Vector3 forward, Vector3 up)
    {
        // The cosine of the angle between the up vector and the zero roll vector. Since both are
        // orthogonal to the forward vector, this tells us how far the Myo has been turned around the
        // forward axis relative to the zero roll vector, but we need to determine separately whether the
        // Myo has been rolled clockwise or counterclockwise.
        float cosine = Vector3.Dot(up, zeroRoll);

        // To determine the sign of the roll, we take the cross product of the up vector and the zero
        // roll vector. This cross product will either be the same or opposite direction as the forward
        // vector depending on whether up is clockwise or counter-clockwise from zero roll.
        // Thus the sign of the dot product of forward and it yields the sign of our roll value.
        Vector3 cp = Vector3.Cross(up, zeroRoll);
        float directionCosine = Vector3.Dot(forward, cp);
        float sign = directionCosine < 0.0f ? 1.0f : -1.0f;

        // Return the angle of roll (in degrees) from the cosine and the sign.
        return sign * Mathf.Rad2Deg * Mathf.Acos(cosine);
    }

    // Adjust the provided angle to be within a -180 to 180.
    float normalizeAngle(float angle)
    {
        if (angle > 180.0f)
        {
            return angle - 360.0f;
        }
        if (angle < -180.0f)
        {
            return angle + 360.0f;
        }
        return angle;
    }

    


}
