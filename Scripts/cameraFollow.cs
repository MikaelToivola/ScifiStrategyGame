using UnityEngine;
using System.Collections;

public class cameraFollow : MonoBehaviour {

	private GameObject player;
	public float cameraDistance = 15f;

	void Start(){
		player = GameObject.FindGameObjectWithTag ("Player");

	}

	void Update () {
		
		transform.position = new Vector3(player.transform.position.x, player.transform.position.y + cameraDistance, player.transform.position.z);
	
	}
}
