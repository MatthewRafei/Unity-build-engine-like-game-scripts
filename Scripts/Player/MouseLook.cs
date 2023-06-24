using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 250f;

    [SerializeField]
    private Transform playerBody;

    private float xRotation = 0f;
    
    private float mouseX, mouseY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Get Mouse point X & Y
        mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

		xRotation -= (mouseY * Time.deltaTime);

		// Clamp camera look to 90 degrees
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);

		transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

		playerBody.Rotate(Vector3.up * (mouseX * Time.deltaTime));
	}
}
