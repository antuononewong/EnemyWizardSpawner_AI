using UnityEngine;

/* Script for game logic of a projectile. It will automatically disappear based on
 * a pre-defined timer. 
 */

public class ProjectileController : MonoBehaviour
{
    // Attributes
    public float moveSpeed;
    public int food;

    // Components
    private Rigidbody2D rigidBody;

    // Despawn timer
    private float maxDuration = 10f;
    private float currentDuration;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rigidBody.velocity = transform.up * moveSpeed;
    }

    private void FixedUpdate()
    {
        currentDuration += Time.deltaTime;
        if (currentDuration > maxDuration)
        {
            Destroy(gameObject);
        }
    }

}
