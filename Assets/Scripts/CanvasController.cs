using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class CanvasController : MonoBehaviour {

	// Myo game object to connect with.
	// This object must have a ThalmicMyo script attached.
	public GameObject myo = null;
	public int numberOfStage;
	private int selected = 1;
	private Button selectedButton;

	// The pose from the last update. This is used to determine if the pose has changed
	// so that actions are only performed upon making them rather than every frame during
	// which they are active.
	private Pose _lastPose = Pose.Unknown;
	// Update is called once per frame

    void Start()
    {
        Button selectedButton = GameObject.Find("Level " + selected).GetComponent<Button>();
        ColorBlock cb = new ColorBlock();
        cb = selectedButton.colors;
        cb.normalColor = new Color32(220, 74, 59, 250);
        selectedButton.colors = cb;
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
				Button selectedButton = GameObject.Find("Level " + selected).GetComponent<Button>();
				ColorBlock cb = new ColorBlock();

            // Vibrate the Myo armband when a fist is made.
            if (thalmicMyo.pose == Pose.Fist) {
						thalmicMyo.Vibrate (VibrationType.Medium);

						load.LoadScene(selected);

				// Change material when wave in, wave out or double tap poses are made.
				}
			 if (thalmicMyo.pose == Pose.WaveIn) {
						if (selected > 1) {
							selected -= 1;
								cb = selectedButton.colors;
								cb.normalColor = Color.white;
								selectedButton.colors = cb;
							selectedButton = GameObject.Find("Level " + selected).GetComponent<Button>();
								cb = selectedButton.colors;
								cb.normalColor = new Color32(220, 74, 59, 250);
                    cb.highlightedColor = Color.green;
                    selectedButton.colors = cb;
						}

						ExtendUnlockAndNotifyUserAction (thalmicMyo);
				}
				 if (thalmicMyo.pose == Pose.WaveOut) {
						if (selected < numberOfStage) {
							selected += 1;
								cb = selectedButton.colors;
								cb.normalColor = Color.white;
								selectedButton.colors = cb;
							selectedButton = GameObject.Find("Level " + selected).GetComponent<Button>();
								cb = selectedButton.colors;
								cb.normalColor = new Color32(220,74,59, 250);
                    cb.highlightedColor = Color.green;
                    selectedButton.colors = cb;
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
