using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{

    public GameObject explosionEffect;
    
    private float score = 0f;
    public float scoreMultiplier = 10f;         // Multiplier for the score based on elapsed time
    private float elapsedTime = 0f;
    private int highScore = 0; // Variable to store the high score


    public float thrustForce = 3.5f;
    public float maxSpeed = 8f;


    public GameObject shipThruster;
    public GameObject borderParent; // Reference to the parent object of the border

    Rigidbody2D rb;

    public UIDocument uiDocument;       // Reference to the UI Document
    private Label scoreText;
    private Label highScoreText; // reference to the high score label
    private Button restartButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {                                               // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();

        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");     
        // Get the Label element from the UI Document



        highScoreText = uiDocument.rootVisualElement.Q<Label>("HighScoreLabel");   
        // Get the high score label from the UI Document

        highScore = PlayerPrefs.GetInt("HighScore", 0);    
        // Load the high score from PlayerPrefs

        highScoreText.text = "High Score: " + highScore;   
        // Update the high score label with the loaded value

        highScoreText.style.display = DisplayStyle.None;    
        // Show the high score label



        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");  
        // Get the Button element from the UI Document
        restartButton.style.display = DisplayStyle.None;    
        // Hide the restart button initially
        restartButton.clicked += ReloadScene;       
        // Add a listener to the restart button to reload the scene when clicked
    }

    // Update is called once per frame
    void Update()
    {
        // Update the score based on elapsed time and score multiplier
        
            elapsedTime += Time.deltaTime;
                Debug.Log("Score: " + score);
                score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
                scoreText.text = "Score: " + score;

            if (score > highScore) // Check if the current score is greater than the high score
            {
                highScore = (int)score;
               highScoreText.text = "High Score: " + highScore; // Update the high score label with the new value
                PlayerPrefs.SetInt("HighScore", highScore); // Save the new high score to PlayerPrefs
            }

                // Check if the left mouse button is pressed

                if (Mouse.current.leftButton.isPressed)
                    {                                                                     // Get the mouse position in world space
                        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
                        Vector2 direction = (mousePos - transform.position).normalized;

                                                                     // Rotate the player to face the mouse position
                        transform.up = direction;
                        rb.AddForce(direction * thrustForce); // Apply force to the player in the direction of the mouse position

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
            PlayerPrefs.Save(); // Save the PlayerPrefs changes
            highScoreText.style.display = DisplayStyle.Flex; // Show the high score label


            Instantiate(explosionEffect, transform.position, transform.rotation); // Instantiate the explosion effect at the player's position and rotation

        restartButton.style.display = DisplayStyle.Flex; // Show the restart button

        Destroy(gameObject); // Destroy the player object

        borderParent.SetActive(false); // Deactivate the border parent object to hide the borders
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene)
    }
}