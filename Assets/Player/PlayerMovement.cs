/*
 * Programmers: Jack Gill, Caden Mesina
 * Purpose: Manage and contain
 * Inputs:
 * Outputs:
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int woodAmount;
    public int steelAmount;
    public int electronicsAmount;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerControls playerControls;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private SaveManager saveManager;
    int moveDir = 3;
    [SerializeField] private Animator animator;
    private GameObject[] interactables;

    Vector2 moveInput;

    private void Awake()
    {
        playerControls = new PlayerControls();
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        interactables = GameObject.FindGameObjectsWithTag("Interactable");
    }

    void Start()
    {
        
    }

    void Update()
    {
        // Movement Calculation
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);

        // Finding the current facing direction
        // North = 1, East = 2, South = 3, West = 4
        if (moveInput.x > .25 && moveInput.y < .25)
            moveDir = 2;
        else if (moveInput.x < -.25 && moveInput.y < .25)
            moveDir = 4;
        else if (moveInput.x < .25 && moveInput.y > .25)
            moveDir = 1;
        else if (moveInput.x < .25 && moveInput.y < -.25)
            moveDir = 3;

        animator.SetFloat("SpeedX", moveInput.x);
        animator.SetFloat("SpeedY", moveInput.y);
        animator.SetInteger("MoveDir", moveDir);
    }

    // Getting Movement as a Vector2 from the Input Device
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    //When player presses interact key, run interact script on all GameObjects with Interactable tag
    public void OnInteract()
    {
        if (true)
        {
            for (int i = 0; i < interactables.Length; ++i)
            {
                interactables[i].GetComponent<Interactables>().PlayerInteracted();
            }
        }
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
            case "CameraTrigger3":
                cameraManager.SwitchPriority(3);
                break;
            case "CameraTrigger4":
                cameraManager.SwitchPriority(4);
                break;
        }
    }
}
