using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Timeline;

public class PlayerMovement : MonoBehaviour
{
	[Header("Movement Parameters")]
	[SerializeField] private float speed = 20f;
	[SerializeField] private float momentum = 4f;
	[SerializeField] private float gravity = -39.2f;
	[SerializeField] private float jumpHeight = 3f;
	[SerializeField] private CharacterController controller;

	[Header("Crouch Parameters")]
	[SerializeField] private float crouchHeight = 0.5f;
	[SerializeField] private float crouchSpeed = 0.06f;
	[SerializeField] private bool isCrouching;

	[Header("Ground Parameters")]
	[SerializeField] private Transform groundCheck;
	[SerializeField] private LayerMask enviormentMask;
	[SerializeField] public bool isGrounded;

	[Header("Camera")]
	[SerializeField] private GameObject cameraObject;
	[SerializeField] private Transform cameraTransform;

	[Header("Audio")]
	[SerializeField] private AudioSource FallSoundEffect;

	private Vector3 velocity;
	private Vector3 moveDirection;
	private bool wishJump;
	private float x, z;
	private float fallTimer;

	const float GROUND_CHECK_LOCATION_WHEN_CROUCHING = -0.9f;
	const float CAMERA_POSITION_WHEN_CROUCHED = 0.5f;

	private Vector3 originalGroundCheckPos;
	private Vector3 originalCameraPos;
	private float originalControllerHeight;
	private Coroutine crouchCoroutine;

	void Start()
	{
		originalGroundCheckPos = groundCheck.localPosition;
		originalCameraPos = cameraTransform.localPosition;
		originalControllerHeight = controller.height;
		isCrouching = false;
	}

	void Update()
	{
		x = Input.GetAxisRaw("Horizontal") * Time.deltaTime;
		z = Input.GetAxisRaw("Vertical") * Time.deltaTime;

		if (Input.GetKey(KeyCode.Space) && !wishJump)
		{
			wishJump = true;
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			wishJump = false;
		}

		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			if (!isCrouching)
			{
				isCrouching = true;
			}
		}
		if (Input.GetKeyUp(KeyCode.LeftControl))
		{
			if (isCrouching)
			{
				isCrouching = false;
			}
		}


		if(isCrouching)
		{
			if (crouchCoroutine != null)
				StopCoroutine(crouchCoroutine);

			crouchCoroutine = StartCoroutine(CrouchAnimation(true));
		}
		else if (!isCrouching && HeadClearance())
		{
			if (crouchCoroutine != null)
				StopCoroutine(crouchCoroutine);

			crouchCoroutine = StartCoroutine(CrouchAnimation(false));
		}

		isGrounded = controller.isGrounded;

		Vector3 desiredMoveDirection = transform.right * x + transform.forward * z;
		desiredMoveDirection = Vector3.Normalize(desiredMoveDirection);
		moveDirection = Vector3.Lerp(moveDirection, desiredMoveDirection, momentum * Time.deltaTime);
		controller.Move(((moveDirection * speed) + velocity) * Time.deltaTime);

		if (fallTimer >= 0.9f && isGrounded)
		{
			FallSoundEffect.Play();
		}

		if (!isGrounded)
		{
			fallTimer += Time.deltaTime;
		}
		else
		{
			fallTimer = 0;
		}

		if (isGrounded && velocity.y < 0)
		{
			velocity.y = -2f;
		}

		if (isGrounded && HeadClearance())
		{
			velocity.y = wishJump ? Mathf.Sqrt(jumpHeight * -2 * gravity) : velocity.y - 1.0f;
		}
		else
		{
			velocity.y += gravity * Time.deltaTime;
		}
	}

	private bool HeadClearance()
	{
		float sphereRadius = controller.radius;
		Vector3 sphereCenter = transform.position + Vector3.up * sphereRadius * 2;

		if (Physics.CheckSphere(sphereCenter, sphereRadius, enviormentMask))
		{
			return false;
		}

		return true;
	}

	private IEnumerator CrouchAnimation(bool crouch)
	{
		float timer = 0f;
		Vector3 targetGroundCheckPos = crouch ? new Vector3(0f, GROUND_CHECK_LOCATION_WHEN_CROUCHING, 0f) : originalGroundCheckPos;
		Vector3 targetCameraPos = crouch ? new Vector3(0f, CAMERA_POSITION_WHEN_CROUCHED, 0f) : originalCameraPos;
		float targetControllerHeight = crouch ? crouchHeight : originalControllerHeight;

		while (timer < crouchSpeed)
		{
			timer += Time.deltaTime;

			float newCrouchSpeed = timer / crouchSpeed;


			// Interpolate position
			groundCheck.localPosition = Vector3.Lerp(groundCheck.localPosition, targetGroundCheckPos, newCrouchSpeed);
			cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, targetCameraPos, newCrouchSpeed);

			// Interpolate height
			controller.height = Mathf.Lerp(controller.height, targetControllerHeight, newCrouchSpeed);

			yield return null;
		}


		// Set the final position and height
		groundCheck.localPosition = targetGroundCheckPos;
		cameraTransform.localPosition = targetCameraPos;
		controller.height = targetControllerHeight;

		crouchCoroutine = null;
	}
}
