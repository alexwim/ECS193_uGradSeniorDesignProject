﻿using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {
  private Transform player;
  private BoxCollider playerCollider;
  private NavMeshAgent navMeshAgent;

  [HideInInspector]
  public bool grabbed;

  private void Awake () {
    grabbed = false;
    player = GameObject.FindGameObjectWithTag ("Player").transform;
    playerCollider = player.GetComponent<BoxCollider> ();
    navMeshAgent = GetComponent<NavMeshAgent> ();
  }

  private void RegainControl () {
    GetComponent<Rigidbody>().isKinematic = true;
    GetComponent<Rigidbody>().useGravity = false;
    navMeshAgent.enabled = true;
  }

  private void OnCollisionEnter (Collision collision) {
    // This re-enables the navmesh once an enemy hits the ground after a long toss.
    if (!grabbed && !navMeshAgent.enabled && collision.gameObject.name == "Terrain") {
      RegainControl ();
    }
  }

  private void Update () {
    if (grabbed) {
      navMeshAgent.enabled = false;
      return;
    } else if (transform.position.y < 0.9f) { // In case the enemies can't collide against the terrain, ie they're laying the ground.
      RegainControl ();
    } else if (navMeshAgent.enabled) {
      navMeshAgent.SetDestination (playerCollider.ClosestPointOnBounds(transform.position));
    }
  }
}
