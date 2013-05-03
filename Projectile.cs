using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public float BulletSpeed;
	public float killTime;
	
	private float start;
	
	// Use this for initialization
	void Start () {
		rigidbody.velocity = transform.forward * BulletSpeed;
		start = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		print (start + " : " + Time.time);
		if((Time.time - start) > killTime)
			GameObject.Destroy(this.gameObject);
	}
}
