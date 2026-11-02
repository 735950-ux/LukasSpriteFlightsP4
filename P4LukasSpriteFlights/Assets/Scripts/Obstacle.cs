using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float minSize = 1f; 
    public float maxSize = 2f;
    public float minSpeed = 150f;
    public float maxSpeed = 400f;
    public float maxSpinSpeed = 10f;
    public Rigidbody2D rb;
    public GameObject bounceEffectPrefab; // Reference to the bounce effect prefab
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        float randomSpeed = Random.Range(minSpeed, maxSpeed);
        Vector2 randomDirection = Random.insideUnitCircle;

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(randomDirection * randomSpeed);

        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randomTorque);

       
    }
    // Update is called once per frame
    void Update()
    {

    }
    void OncollosionEnter2D(Collision2D collision)
    {
      Vector2 contactPoint = collision.GetContact(0).point; // Get the contact point of the collision
        GameObject bounceEffect = Instantiate(bounceEffectPrefab, contactPoint, Quaternion.identity); // Instantiate the bounce effect at the contact point
    }
}
