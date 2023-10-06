using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    RaycastHit2D ray;
    bool attacking = false;
    // Start is called before the first frame update
    void Start()
    {
        ray = Physics2D.Raycast(new Vector3(transform.position.x - .5f, transform.position.y, transform.position.z), -Vector3.right, .25f);
        rb.velocity = new Vector2(-2f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(new Vector3(transform.position.x - .5f, transform.position.y, transform.position.z), -Vector3.right, Color.red, .25f, false);
        if (ray.collider.tag == "Building" && !attacking)
        {
            rb.velocity = new Vector2(0f, 0f);
            StartCoroutine(AttackBuildings());
        }
    }

    IEnumerator AttackBuildings()
    {
        attacking = true;
        // Add attacking code once buildings are made

        yield return new WaitForSeconds(1f);
        attacking = false;
    }
}
