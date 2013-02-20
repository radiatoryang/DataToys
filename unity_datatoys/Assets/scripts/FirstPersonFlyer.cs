using UnityEngine;
using System.Collections;

public class FirstPersonFlyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * 5f;
		transform.position += transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * 5f;
	}
}
