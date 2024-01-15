/*
 * Programmers: Jack Gill and Caden Mesina
 * Purpose: Manage enemy behavior
 * Inputs: If enemy is facing a structure or not
 * Outputs: Either attack or keep walking
 */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

public class Enemy : MonoBehaviour
{
    #region Global Variables
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameManager gm;
    [SerializeField] private new AudioSource audio;
    [SerializeField] private float damage = 1;
    [SerializeField] private Animator animationController;
    private float timerStart;
    public float Health = 5;
    private float speed = 2f;
    public bool goUp;
    #endregion

    #region Default Methods
    void Start()
    {
        this.transform.parent = null;
        timerStart = Time.realtimeSinceStartup;

        // Setting velocity if in tutorial of main game
        if (!goUp)
        {
            rb.velocity = new Vector2(-1 * speed, 0f);
        }
        else
        {
            rb.velocity = new Vector2(0f, speed);
        }

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (Time.realtimeSinceStartup - timerStart >= 100000)
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door" && collision.gameObject.GetComponent<Door>().Health > 0)
        {
            rb.velocity = new Vector2(0f, 0f);
            StartCoroutine(StartAttackingBase(collision));
        } 
    }
    #endregion

    #region Custom Methods
    // Increasing damage and health for every enemy spawned
    public void SetDamage(float modifier)
    {
        // Increasing Damage
        damage = (damage + modifier) * damage;

        // Increasing Speed
        speed += modifier;

        // Increasing Health
        Health += (modifier * 5);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        // Death
        if (Health <= 0)
        {
            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                SceneManager.LoadScene("StartingMenu");
            }

            gm.score += 100;
            gm.UpdateScoreTxt();
            Destroy(gameObject);
        }
    }

    // Code that executes when the enemy is attacking the base
    IEnumerator StartAttackingBase(Collider2D collision)
    {
        if (collision.gameObject.tag == "Door" && collision.gameObject.GetComponent<Door>().Health > 0)
        {
            animationController.SetTrigger("Attacking");
            collision.gameObject.GetComponent<Door>().TakeDamage(damage);
            audio.Play();
            yield return new WaitForSeconds(1f);
            StartCoroutine(StartAttackingBase(collision));
        }
    }
    #endregion
}
