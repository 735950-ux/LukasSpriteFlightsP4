using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class PlayerController : MonoBehaviour
{
    
    private float score = 0f;
    public float scoreMultiplier = 10f;         // Multiplier for the score based on elapsed time
    private float elapsedTime = 0f;


    public float thrustForce = 1f;
    public float maxSpeed = 5f;


    public GameObject shipThruster;

    Rigidbody2D rb;

    public UIDocument uiDocument;       // Reference to the UI Document
    private Label scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {                                               // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();

        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");                     // Get the Label element from the UI Document
    }

    // Update is called once per frame
    void Update()
    {
        // Update the score based on elapsed time and score multiplier
        {
            elapsedTime += Time.deltaTime;
                Debug.Log("Score: " + score);
                score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
                scoreText.text = "Score: " + score;
        }
            
                // Check if the left mouse button is pressed
                
                    if (Mouse.current.leftButton.isPressed)
                    {                                                                     // Get the mouse position in world space
                        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
                        Vector2 direction = (mousePos - transform.position).normalized;

                                                                     // Rotate the player to face the mouse position
                        transform.up = direction;
                        rb.AddForce(direction * thrustForce);

                        if (rb.linearVelocity.magnitude > maxSpeed)
                        {                                                       // Limit the player's speed to maxSpeed
                            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
                        }
                    }


            // Activate the ship thruster when the left mouse button is pressed and deactivate it when released
             if (Mouse.current.leftButton.wasPressedThisFrame)
             {
                        shipThruster.SetActive(true);
             }
                    else if (Mouse.current.leftButton.wasReleasedThisFrame)
                    {
                        shipThruster.SetActive(false);
                    }

                

    }
    
    public void OnCollisionEnter2D(Collision2D collision) // This function is called when the player collides with another object
    {
        Destroy(gameObject);
    } 
}