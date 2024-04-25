/*
 * Programmers: Jack Gill
 * Purpose: Control the animations on the door leading outside and detecting enemy attacks
 * Inputs: Circle collider on door
 * Outputs: Change current door animation and control base health
 */
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class Door : MonoBehaviour
{
    #region Global Variables
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject Player;
    [SerializeField] private Animator anim;
    [SerializeField] private Slider slider;
    private SaveManager sm;
    private GameManager gm;
    public float Health = 100;
    #endregion

    #region Default Methods
    private void Awake()
    {
        Player = GameObject.Find("Player");
        sm = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            anim.SetTrigger("Open");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            anim.SetTrigger("Close");
        }
    }
    #endregion

    #region Custom Methods
    public void TakeDamage(float damage)
    {
        Health -= damage;
        // Changing the health bar UI
        slider.value = Health / 100;
        if (Health <= 0)
        {
            GameOver();
        }
    }

    // Everything that should happen when you game over
    void GameOver()
    {
        Debug.Log("Game Over");
        Player.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        deathScreen.SetActive(true);
        Player.GetComponent<PlayerManager>().isDead = true;

        EventSystem.current.SetSelectedGameObject(GameObject.Find("DeadQuitButton"));
    }
    public void AddToDatabase()
    {
        string Initials = GameObject.Find("InitialsTextField").GetComponent<TextMeshProUGUI>().text;
        if (Initials.Length >= 3)
        {
            Initials = Initials.Substring(0, 3);
        }
        int Score = gm.score;

        sm.Write(Initials, Score);
        Destroy(gm.gameObject);
        SceneManager.LoadScene("StartingMenu");
    }
    #endregion
}