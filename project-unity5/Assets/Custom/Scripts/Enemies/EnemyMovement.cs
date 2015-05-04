﻿using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
  	private Collider playerCollider;
  	private NavMeshAgent navMeshAgent;
	private EnemyHealth enemyHealth;
	private Rigidbody rigidBody;
  	private bool grabbed;
	private Vector3 droppedPosition;

  	private void Awake () {
    	grabbed = false;
    	playerCollider = GameObject.FindGameObjectWithTag ("Player").transform.GetComponent<Collider>();
    	navMeshAgent = GetComponent<NavMeshAgent> ();
		enemyHealth = GetComponent<EnemyHealth> ();
		rigidBody = GetComponent<Rigidbody> ();
  	}

	public void Pinch() {
		grabbed = true;
		navMeshAgent.enabled = false;
		rigidBody.isKinematic = false;
		rigidBody.useGravity = true;
	}

	public void Release() {
		grabbed = false;
		droppedPosition = transform.position;
	}

  	private void RegainControl () {
  	  	GetComponent<Rigidbody>().isKinematic = true;
    	GetComponent<Rigidbody>().useGravity = false;
    	navMeshAgent.enabled = true;
  	}

	private void OnCollisionEnter (Collision collision) {
		if (!grabbed && !navMeshAgent.enabled && collision.gameObject.name == "Terrain") {
			ContactPoint contact = collision.contacts[0];
			Vector3 normal = contact.normal;
			Vector3 relativeVelocity = collision.relativeVelocity;
			
			int damage = (int) Mathf.Abs(Vector3.Dot (normal, relativeVelocity) * GetComponent<Rigidbody>().mass);
			
			enemyHealth.TakeDamage (damage);
			
			droppedPosition.y = 0; // reset after drop

			RegainControl ();
		}
	}

  	private void Update () {
    	if (navMeshAgent.enabled) {
      		navMeshAgent.SetDestination (playerCollider.ClosestPointOnBounds(transform.position));
    	}
  	}
}
