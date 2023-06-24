using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
	// The speed at which the bobbing motion occurs
	[SerializeField]
	private float bobSpeed = 5f;

	// The distance the weapon sprite moves up and down during bobbing
	[SerializeField]
	private float bobDistance = 3f;

	// The Transform component representing the weapon sprite.
	//[SerializeField]
	private Transform gun;

	private float horizontal, vertical, timer, waveSlice;
	private Vector3 midPoint;

	private PlayerMovement playerMovementInstance;

	// Groundcheck logic
	private bool isGrounded;

	void Start()
	{
		gun = transform;
		// The initial position of the weapon sprite
		midPoint = gun.localPosition;

		// This is used to get the value of wether or not the player is on the ground or not.
		// probably not the best way to do this but It should work for now since the player is the only object with that class
		playerMovementInstance = FindObjectOfType<PlayerMovement>();
	}

	void Update()
	{
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");

		// Current position of the weapon sprite
		Vector3 localPosition = gun.localPosition;

		isGrounded = playerMovementInstance.isGrounded;

		if (!isGrounded)
		{
			// Slowly increment the timer to transition smoothly when in the air
			timer += bobSpeed * 0.7f * Time.deltaTime;
		}
		else if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
		{
			timer = 0.0f;
		}
		else
		{
			timer += bobSpeed * Time.deltaTime;

			if (timer > Mathf.PI * 2)
			{
				timer -= Mathf.PI * 2;
			}
		}

		float waveSlice = Mathf.Sin(timer);

		if (waveSlice != 0)
		{
			float translateChange = waveSlice * bobDistance;
			float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
			totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
			translateChange *= totalAxes * 2;
			localPosition.y = midPoint.y + translateChange;
			localPosition.x = midPoint.x + translateChange;
		}
		else
		{
			localPosition.y = midPoint.y;
			localPosition.x = midPoint.x;
		}

		gun.localPosition = localPosition;
	}
}