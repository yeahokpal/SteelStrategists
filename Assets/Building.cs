using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    List<Enemy> enemies = new List<Enemy>();

    [SerializeField] int Damage;
    [SerializeField] float AttackInterval;
    bool canAttack = true;
    
    void Update()
    {
        if (enemies.Count > 0 && canAttack)
        {
            canAttack = false;
            StartCoroutine(Attack(enemies[0]));
        }
    }

    private IEnumerator Attack(Enemy enemy)
    {
        if (enemy.Health > 0)
        {
            enemy.Health -= Damage;
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
