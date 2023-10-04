using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector2(-2f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, new Vector2(-1, 0),.25f);

        if (ray.collider.tag == "Building")
        {
            rb.velocity = new Vector2(0f, 0f);
        }
    }
}
