using UnityEngine;
using System.Collections;

public class Zoom : MonoBehaviour {

	const float speed = 10.0f;
	const float minSize = 5.0f;
	const float maxSize = 50.0f;

	const float delta = 0.5f;
	const float range = 10f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		float scroll = Input.GetAxis ("Mouse ScrollWheel");
		float newSize = Camera.main.orthographicSize - scroll * speed;
		if (scroll != 0.0f) {
			if ((newSize >= minSize) &&
					(newSize <= maxSize))
				Camera.main.orthographicSize = newSize;
			else if (newSize <= minSize)
				Camera.main.orthographicSize = minSize;
			else if (newSize >= maxSize)
				Camera.main.orthographicSize = maxSize;
		}
		if (Input.GetKey(KeyCode.D)){
			if (transform.position.x <= range){
				transform.position += new Vector3( delta, 0, 0);
			}
		}
		
		if (Input.GetKey(KeyCode.A)){
			if (transform.position.x >= -range){
				transform.position -= new Vector3( delta, 0, 0);
			}
		}
		
		if (Input.GetKey(KeyCode.W)){
			if (transform.position.y <= range){
				transform.position += new Vector3( 0, delta, 0);
			}
		}
		
		if (Input.GetKey(KeyCode.S)){
			if (transform.position.y >= -range){
				transform.position -= new Vector3( 0, delta, 0);
			}
		}
	}
}
