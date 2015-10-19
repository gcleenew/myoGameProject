using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class CanvasController : MonoBehaviour {

	// Myo game object to connect with.
	// This object must have a ThalmicMyo script attached.
	public GameObject myo = null;
	private int selected = 0;

	private Button[] buttons = new Button[10];

	// The pose from the last update. This is used to determine if the pose has changed
	// so that actions are only performed upon making them rather than every frame during
	// which they are active.
	private Pose _lastPose = Pose.Unknown;
	// Update is called once per frame

    void Start()
    {
        buttons = FindObjectsOfType(typeof(Button)) as Button[];
        ColorBlock cb = new ColorBlock();
        cb = buttons[0].colors;
        cb.normalColor = new Color32(220, 74, 59, 250);
        buttons[0].colors = cb;
    }
	void Update () {
		// Access the ThalmicMyo component attached to the Myo game object.
		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();
		LoadOnClick load = GetComponent<LoadOnClick>();
		// Check if the pose has changed since last update.
		// The ThalmicMyo component of a Myo game object has a pose property that is set to the
		// currently detected pose (e.g. Pose.Fist for the user making a fist). If no pose is currently
		// detected, pose will be set to Pose.Rest. If pose detection is unavailable, e.g. because Myo
		// is not on a user's arm, pose will be set to Pose.Unknown.
		if (thalmicMyo.pose != _lastPose) {
				_lastPose = thalmicMyo.pose;
				buttons = FindObjectsOfType(typeof(Button)) as Button[];
				ColorBlock cb = new ColorBlock();

        // Vibrate the Myo armband when a fist is made.
        if (thalmicMyo.pose == Pose.Fist) {
					thalmicMyo.Vibrate (VibrationType.Medium);
					buttons[selected].onClick.Invoke();
				}
			 if (thalmicMyo.pose == Pose.WaveIn) {
						if (selected > 0) {

							cb = buttons[selected].colors;
							cb.normalColor = Color.white;
							buttons[selected].colors = cb;

							selected -= 1;

							cb = buttons[selected].colors;
							cb.normalColor = new Color32(220, 74, 59, 250);
              cb.highlightedColor = Color.green;
              buttons[selected].colors = cb;
						}

						ExtendUnlockAndNotifyUserAction (thalmicMyo);
				}
				 if (thalmicMyo.pose == Pose.WaveOut) {
						if (selected < buttons.Length) {
							cb = buttons[selected].colors;
							cb.normalColor = Color.white;
							buttons[selected].colors = cb;

							selected += 1;

							cb = buttons[selected].colors;
							cb.normalColor = new Color32(220, 74, 59, 250);
              cb.highlightedColor = Color.green;
              buttons[selected].colors = cb;
						}

						ExtendUnlockAndNotifyUserAction (thalmicMyo);
				}
		}
	}

	// Extend the unlock if ThalmcHub's locking policy is standard, and notifies the given myo that a user action was
	// recognized.
	void ExtendUnlockAndNotifyUserAction (ThalmicMyo myo)
	{
			ThalmicHub hub = ThalmicHub.instance;

			if (hub.lockingPolicy == LockingPolicy.Standard) {
					myo.Unlock (UnlockType.Timed);
			}

			myo.NotifyUserAction ();
	}
}
