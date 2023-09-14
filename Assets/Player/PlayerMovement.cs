/*
 * Programmers: Jack Gill, Caden Mesina
 * Purpose:
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

    public int woodAmount = 0;
    public int steelAmount = 0;
    public int electronicsAmount = 0;

    public Rigidbody2D rb;
    public PlayerControls playerControls;
    public CameraManager cameraManager;
    public SaveManager saveManager;
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
    }

    // Getting Movement as a Vector2 from the Input Device
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    //When player presses interact key, run interact script on all GameObjects with Interactable tag
    public void OnInteract()
    {
        for (int i = 0; i < interactables.Length; ++i)
        {
            interactables[i].GetComponent<Interactables>().PlayerInteracted();
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
