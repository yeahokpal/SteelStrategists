using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public int woodAmount = 0;
    public int steelAmount = 0;
    public int electronicsAmount = 0;

    public Rigidbody2D rb;
    public PlayerControls playerControls;
    public CameraManager cameraManager;
    public SaveManager saveManager;

    Vector2 moveInput;

    private void Awake()
    {
        playerControls = new PlayerControls();
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
    }

    void Start()
    {

    }

    void Update()
    {
        // Movement Calculation
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    // Getting Movement as a Vector2 from the Input Device
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Changing the camera and which room that it is focused on
        switch (collision.name)
        {
            case "CameraTrigger1":
                cameraManager.SwitchPriority(1);
                break;
            case "CameraTrigger2":
                cameraManager.SwitchPriority(2);
                break;
        }
    }
}
