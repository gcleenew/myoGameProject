using UnityEngine;
using System.Collections;

public class WallMove : MonoBehaviour {

	// Update is called once per frame
	// Update is called once per frame
	float amplitudeZ = 0.01f;
	float omegaZ = 1.0f;
	float index;
	public void Update(){
	    index += Time.deltaTime;
	    float z = amplitudeZ*Mathf.Cos(omegaZ*index);
	    float rotateY = transform.rotation.eulerAngles.y;
	    transform.localPosition += new Vector3(0,0,Mathf.Cos(rotateY)*z);
	}
}
