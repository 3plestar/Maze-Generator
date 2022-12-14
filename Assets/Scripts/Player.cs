using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController characterController;

    public float speed;
    public float jumpStrength;

    public Transform cameraTransform;

    public float mouseSensitivity;
    private float xRotation = 0;

    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //movement controls
        Vector3 walk = transform.right *Input.GetAxis("Horizontal") + transform.forward* Input.GetAxis("Vertical");
        characterController.Move( walk * Time.deltaTime * speed);

        if (Input.GetButtonDown("Jump"))
        {
          //add jump
        }

        //camera controls
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation,-90,90);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * mouseX); 
    }
}
