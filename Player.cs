using UnityEngine;
using System.Collections;

/*
 * Note:
 *	if high speeds ever used, be wary that physics, specificaly colliders, may skip space, causing no-clip bugs with entities
 *  attempting to collide with this player
 **/

public class Player : MonoBehaviour {
	
	public GameObject Potato;
	public float rateOfFire;
	public float sensitivity;
	public float speed;
	public bool lockCursor;
	public float camDist;
	public float camHeight;
	public float maxYRot;
	public float minYRot;
	public bool invertMouse;
	
	private GameObject cam;
	private float virtualYRot;
	private float lastShot;
	
	// Use this for initialization
	void Start () {
		cam = GameObject.FindWithTag("MainCamera");
		virtualYRot = 45.0f;
		lastShot = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		Screen.lockCursor = lockCursor;
		
		//
		//movement and rotation
		//
		
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");
		
		Quaternion initDirection = transform.rotation;
		Quaternion initCamDirection = cam.transform.rotation;
		
		float lookX = Input.GetAxis("Mouse X");
		float lookY = Input.GetAxis("Mouse Y");
		
		lookX *= sensitivity;
		lookY *= sensitivity;
		
		if(!invertMouse)
			lookY *= -1;
				
		if((virtualYRot + lookY <= minYRot && lookY < 0) || (virtualYRot + lookY >= maxYRot && lookY > 0))
			lookY = 0;
		
		virtualYRot += lookY;
		if(virtualYRot > 360)
			virtualYRot -=360;
		if(virtualYRot < 0)
			virtualYRot += 360;
		
		transform.rotation *= Quaternion.AngleAxis(lookX, transform.InverseTransformDirection(Vector3.up));
		
		//camera rotation
		cam.transform.position = transform.position;
		cam.transform.rotation = Quaternion.AngleAxis(lookY, initCamDirection * Vector3.left);
		cam.transform.rotation *= Quaternion.AngleAxis(lookX, cam.transform.InverseTransformDirection(Vector3.up));
		cam.transform.rotation *= initCamDirection;
		cam.transform.position -= cam.transform.forward * camDist;
		cam.transform.position += cam.transform.up * camHeight;
		
		
		Vector3 moveVector = new Vector3(x,0,y);
		moveVector = transform.rotation * moveVector;
		moveVector.y = 0;
		if(moveVector.magnitude > 0)
			moveVector *= speed;
		
		if(!Physics.Raycast(transform.position, moveVector, speed + 1.0f))
			transform.position += moveVector;
		
		//
		//attacks/ special moves
		//
		
		bool fire1 = (Input.GetAxis("Fire1") > 0);
		
		if(fire1 && (Time.time - lastShot) > rateOfFire)
		{
			GameObject.Instantiate(Potato, transform.position + transform.up * camHeight + transform.forward * camHeight, cam.transform.rotation);
			lastShot = Time.time;
		}
	}
}
