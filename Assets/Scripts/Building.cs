using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    List<Enemy> enemies = new List<Enemy>();

    [SerializeField] int Damage;
    [SerializeField] float AttackInterval;
    [SerializeField] new AudioSource audio;

    public LineRenderer lr;

    bool canAttack = true;

    private void Start()
    {
        lr = gameObject.GetComponent<LineRenderer>();
    }

    [System.Obsolete]
    void Update()
    {
        if (enemies.Count > 0 && canAttack)
        {
            canAttack = false;
            StartCoroutine(Attack(enemies[0]));
        }
    }

    [System.Obsolete]
    private IEnumerator Attack(Enemy enemy)
    {
        if (enemy.Health > 0)
        {
            audio.Play();
            enemy.Health -= Damage;

            lr.enabled = true;

            lr.SetWidth(0.05f, 0.05f);
            lr.SetPosition(0, gameObject.transform.position);
            lr.SetPosition(1, enemy.gameObject.transform.position);

            yield return new WaitForSeconds(.1f);
            
            lr.enabled = false;

            yield return new WaitForSeconds(AttackInterval);
        }
        if (enemy.Health <= 0)
        {
            enemies.Remove(enemy);
        }
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemies.Add(collision.gameObject.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemies.Remove(collision.gameObject.GetComponent<Enemy>());
        }
    }
}
