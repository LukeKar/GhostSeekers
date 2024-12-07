using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public float moveSpeed = 5f; // Speed of player movement
    public float mouseSensitivity = 2f;
    
    private Vector3 rotatecam;
    private Vector3 rotateplayer;
    public Transform cameraTransform; // Reference to the camera transform

    void Start() {
        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
        HandleMovement();
        HandleCameraRotation();
    }

    void HandleMovement() {
        // Get input from WASD or arrow keys
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxis("Vertical");     // W/S or Up/Down
        
        transform.Translate(horizontal * moveSpeed * Time.deltaTime, 0.0f, vertical * moveSpeed * Time.deltaTime);
    }

    void HandleCameraRotation() {

        float MouseX = Input.GetAxis("Mouse X");
        float MouseY = Input.GetAxis("Mouse Y");

       
        rotateplayer = new Vector3(MouseY, -MouseX * mouseSensitivity, 0);
        this.transform.eulerAngles -= rotateplayer;
        





    }   
    

}
