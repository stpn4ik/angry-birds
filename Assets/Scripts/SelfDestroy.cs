using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float health;
    private Rigidbody2D rigidbody;
    public static float sensitivity = 0.5f;
    private GameManager gameManager;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        gameManager = Object.FindFirstObjectByType<GameManager>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (rigidbody.linearVelocity.magnitude > 3f)
        {
            health -= rigidbody.linearVelocity.magnitude * rigidbody.mass * sensitivity;
        }
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (rb.linearVelocity.magnitude > 1f)
            {
                health -= rb.linearVelocity.magnitude;
            }
        }
        if (health <= 0)
        {
            if (gameObject.tag == "Pig")
            {
                gameManager.ChangePigsCount();
            }
            Destroy(gameObject);
        }

    }
}
