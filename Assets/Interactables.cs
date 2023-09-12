using UnityEngine;

public class Interactables : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CircleCollider2D detectionArea;
    [SerializeField] private float interactionDistance;
    private bool closeEnough = false;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        closeEnough = false;
        spriteRenderer.color = Color.white;
        detectionArea.radius = interactionDistance;

        if (Vector2.Distance(player.transform.position, this.transform.position) <= interactionDistance)
        {
            closeEnough = true;
            spriteRenderer.color = Color.red;
            Debug.Log("Player is in range");
        }
    }
}
