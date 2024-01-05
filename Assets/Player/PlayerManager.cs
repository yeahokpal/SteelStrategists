/*
 * Programmers: Jack Gill, Caden Mesina
 * Purpose: Manage and contain player information and controls
 * Input: Player inputs
 * Output: Player movements and actions to preform
 */

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    #region Global Variables
    public float moveSpeed = 5f;
    public int woodAmount;
    public int steelAmount;
    public int electronicsAmount;

    bool isPaused = false;
    int moveDir = 3;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerControls playerControls;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private Animator animator;
    [SerializeField] GameObject placeOverlay;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameManager gm;
    private GameObject[] interactables;
    public GameObject currentBuilding;
    TextMeshProUGUI woodTxt, steelTxt, electronicsTxt;

    float pauseTimer;

    Vector2 moveInput;
    #endregion

    #region Default Methods
    private void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerControls = new PlayerControls();
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        interactables = GameObject.FindGameObjectsWithTag("Interactable");

        SceneManager.activeSceneChanged += DefineMaterialUIElements;
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

        // Updating animator parameters
        animator.SetFloat("SpeedX", moveInput.x);
        animator.SetFloat("SpeedY", moveInput.y);
        animator.SetInteger("MoveDir", moveDir);

        // Showing the overlay of the current Building
        if (currentBuilding != null)
        {
            placeOverlay.SetActive(true);
            switch (moveDir)
            {
                case 1:
                    placeOverlay.transform.position = gameObject.transform.position + new Vector3(0f, 2f, 0f);
                    break;
                case 2:
                    placeOverlay.transform.position = gameObject.transform.position + new Vector3(1f, 0f, 0f);
                    break;
                case 3:
                    placeOverlay.transform.position = gameObject.transform.position + new Vector3(0f, -2f, 0f);
                    break;
                case 4:
                    placeOverlay.transform.position = gameObject.transform.position + new Vector3(-1f, 0f, 0f);
                    break;
            }
        }

        if (woodTxt != null)
        {
            woodTxt.text = "Wood: " + woodAmount;
            steelTxt.text = "Steel: " + steelAmount;
            electronicsTxt.text = "Electronics: " + electronicsAmount;
        }
    }

    private void DefineMaterialUIElements(Scene currentScene, Scene nextScene)
    {
        if (nextScene.name == "Tutorial" || nextScene.name == "MainScene")
        {
            woodTxt = GameObject.Find("WoodAmountTxt").GetComponent<TextMeshProUGUI>();
            steelTxt = GameObject.Find("SteelAmountTxt").GetComponent<TextMeshProUGUI>();
            electronicsTxt = GameObject.Find("ElectronicsAmountTxt").GetComponent<TextMeshProUGUI>();
        }
        else
        {
            woodTxt = null;
            steelTxt = null;
            electronicsTxt = null;
        }
    }


    // Changing the current Camera Position
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
    #endregion

    #region Custom Methods
    // Getting Movement as a Vector2 from the Input Device
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // When player presses interact key, run interact script on all GameObjects with Interactable tag
    public void OnInteract()
    {
        for (int i = 0; i < interactables.Length; ++i)
        {
            interactables[i].GetComponent<Interactables>().PlayerInteracted();
        }
    }

    // Place a Building
    public void OnPlaceBuilding()
    {
        // Only let player place buildings outside
        if (currentBuilding != null && cameraManager.vcam4.Priority == 1)
        {
            placeOverlay.SetActive(false);
            // Looking at where to render the building overlay
            switch (moveDir)
            {
                case 1:
                    Instantiate(currentBuilding, gameObject.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
                    break;
                case 2:
                    Instantiate(currentBuilding, gameObject.transform.position + new Vector3(1f, 0f, 0f), Quaternion.identity);
                    break;
                case 3:
                    Instantiate(currentBuilding, gameObject.transform.position + new Vector3(0f, -2f, 0f), Quaternion.identity);
                    break;
                case 4:
                    Instantiate(currentBuilding, gameObject.transform.position + new Vector3(-1f, 0f, 0f), Quaternion.identity);
                    break;
            }

            currentBuilding = null;
        }
    }

    // Handling the pause menu
    public void OnPause()
    {
        // Unpause
        if (isPaused)
        {
            isPaused = false;
            // Continue timer progression
            gm.timerDelay += (Time.realtimeSinceStartup - pauseTimer);

            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            // Continue updating timer UI
            gm.startTimer = true;
        }
        // Pause
        else
        {
            isPaused = true;
            // Stop timer from progressing
            pauseTimer = Time.realtimeSinceStartup;

            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            // Stop updating timer UI
            gm.startTimer = false;
        }
        Debug.Log("Paused Status: " + isPaused);
    }

    // Quit to Main Menu
    public void QuitToMenu()
    {
        SceneManager.LoadScene("StartingMenu");
    }

    public void OnCancel()
    {
        GameObject.Find("MapScreen").SetActive(false);
        this.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
    }

    #endregion
}