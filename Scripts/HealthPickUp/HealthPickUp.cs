using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
	private AudioSource pickUpSound;
	private SpriteRenderer healthSpriteRender;
	private SphereCollider sphereCollider;

	private float delayTime = 1;

	public sbyte healthAmount = 5;


	void Start()
	{
		pickUpSound = GetComponent<AudioSource>();
		healthSpriteRender = GetComponent<SpriteRenderer>();
		sphereCollider = GetComponent<SphereCollider>();

	}

	// Object was given a sphere collider because player is really a 3d Object
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			// Play pickup noise and add ammo amount to specific gun


			pickUpSound.Play();
			sphereCollider.enabled = false;
			healthSpriteRender.enabled = false;
			StartCoroutine(DestroyAmmoPickup());
		}
	}


	/*
		To optimize the use of coroutines in scenarios with many ammo pickups, you can consider a couple of approaches:

		Batch Processing: Rather than starting a separate coroutine for each ammo pickup, you can batch process them. 
		For example, you can have a single coroutine that handles the destruction of multiple ammo pickups after a delay. 
		This approach reduces the number of active coroutines and can help improve performance.
	*/
	IEnumerator DestroyAmmoPickup()
	{
		yield return new WaitForSeconds(delayTime);
		Destroy(gameObject);
	}

}