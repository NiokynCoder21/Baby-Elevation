using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed; //how fast the player moves
    public GameObject player; //the player gameObject
    public Rigidbody2D rb; //the rigidbody attached to the player
    public float jumpForce = 5f; //how powerful is the player jump
    bool isGrounded = true; //check if player is grounded
    public Transform orientation; //check the direction the player is facing
    public float rightForce; //move the player to the right in air and ground
    public float leftForce; //move the player to the left in air and ground
    public float downForce; //drop ballon force
    public float upForce; //float amount
    public float upAirGravity; //gravity amount when floating
    public float upDash;

    public int startBallons = 0; //how many ballons the player starts with
    public int ballonCount; //how many ballons the player currently has 
    public int loss; // how many ballons lost 
    public int numberOne; //if player carrying one ballon
    public int numberTwo; //if player carrying two ballons
    public int numberThree; //if player carrying three ballons 

    public PlayersHealth health; //player health script
    public bool hasBeenDowned = false; //this ensure the player does not get stuff on objects after lossing a ballon and falls down
    public GameObject[] balloons; //this is to hold the ballons that the player can pick up in game 

    private Vector2 move;
    //public BallonPool pool;
   // public Vector2[] ballonPositions;

   // private List<GameObject> activeBalloons = new List<GameObject>();

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>(); //this detects input along the vector and allows movement 
    }

    public void onJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jump();
        }

    }

    public void onDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Drop();
        }

    }

    void Update()
    {
        /*if (Input.GetKey("a")) //when the player presses A
        {
            rb.AddForce(-orientation.right * rightForce, ForceMode2D.Force); //add force to the left to allow the player to move to the left
            transform.eulerAngles = new Vector3(0, 180, 0); //rotates the player 180 on the z - this flips the sprite
        }

        else if (Input.GetKey("d")) //when the player presses B
        {
            rb.AddForce(orientation.right * leftForce, ForceMode2D.Force); //add force to the right to allow the player to move to the right
            transform.eulerAngles = new Vector3(0, 0, 0f); //rotates the player 0 on x,y,z - this flips the sprite
        }*/

        /*if (Input.GetKeyDown("s") && ballonCount > 0) //when the player presses s and has more 0 ballons
        {
            ballonCount -= loss; //this reduces loss from balloonCount and assigns the current ballonCount
            Vector2 upwardVelocity = orientation.up * downForce * ballonCount; //slows the player down when they drop a player
        }*/


        /*if (Input.GetKeyDown(KeyCode.Space) && isGrounded) //when the player presses space and are grounded they can jump
        {
            Jump(); 
        }*/

        // Deactivate balloons if they exceed the current balloonCount

        Move();

        if (ballonCount > 0 && !isGrounded && !hasBeenDowned) //when the player has more zero ballons and is not on the floor and is not hasbeendowned 
        {
            AirMoveWithVelocity();
        }

        for (int i = 0; i < balloons.Length; i++) 
        {
            if (i < ballonCount)
            {
                balloons[i].SetActive(true); //this will make the relevant ballon appear on the player
            }
            else
            {
                balloons[i].SetActive(false); //this will make the relevant ballon disappear on the player 
            }
        }
    }

    private void Start()
    {
        ballonCount = startBallons; //set the balloonCount to zero
        AudioSource audio = GetComponent<AudioSource>(); //get component audio source and store as audio
    }

    public void Move()
    {
        if (move.x > 0)
        {
            rb.AddForce(-orientation.right * rightForce, ForceMode2D.Force); //add force to the left to allow the player to move to the left
            transform.eulerAngles = new Vector3(0, 0, 0); //rotates the player 180 on the z - this flips the sprite
        }

        if (move.x < 0)
        {
            rb.AddForce(orientation.right * leftForce, ForceMode2D.Force); //add force to the right to allow the player to move to the right
            transform.eulerAngles = new Vector3(0, 180, 0f); //rotates the player 0 on x,y,z - this flips the sprite
        }

    }

    public void Drop()
    {
        if (ballonCount > 0)
        {
            ballonCount -= loss; //this reduces loss from balloonCount and assigns the current ballonCount
            Vector2 upwardVelocity = orientation.up * downForce * ballonCount; //slows the player down when they drop a player
        }
        
    }

    void AirMoveWithVelocity()
    {
        Vector2 upwardVelocity = orientation.up * upForce * ballonCount; //this makes the player float 
        Vector2 gravityEffect = -orientation.up * upAirGravity * ballonCount; //this acts as grafvity that adds negative force to the the floating to make it feel good

        rb.velocity = new Vector2(rb.velocity.x, upwardVelocity.y + gravityEffect.y);
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce); //add jump force to the player rb
        isGrounded = false; //player is not on the ground
    }

    public void getBallons(int Ballons) //a function that is made to be called on ballons the player collects 
    {
        ballonCount += Ballons;
    }

    public void loseBallons(int Ballons) //a function that is made to be called when the player losses a ballon 
    {
        if (ballonCount > 0) //if there are more 0 ballons 
        {
            ballonCount -= Ballons;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform")) //if player collides with game object with tag platform
        {
            isGrounded = true; //the player can jump
            hasBeenDowned = false; //this is to reset the bool 
        }

        if (collision.gameObject.CompareTag("Killer")) ///if player collides with game object with tag killer
        {
            GameObject player = GameObject.FindWithTag("Player"); //store object as player
            hasBeenDowned = true; //this will ensure that the player falls when they hit this object with the tag Killer 
            
            if(player != null) 
            {
                PlayersHealth health = player.GetComponent<PlayersHealth>(); //get playerhealth script from player game object  
                
                if(health != null)
                {
                    if(ballonCount == 1) //if the player has 1 balloon
                    {
                        health.LossHealth(numberOne); //this controls how much health they lose 
                    }

                    if (ballonCount == 2) //if the player has 2 balloon
                    {
                        health.LossHealth(numberTwo); //this controls how much health they lose 
                    }

                    if (ballonCount >= 3) //if the player has 3 balloon
                    {
                        health.LossHealth(numberThree); //this controls how much health they lose 
                    }
                }
            }
        }
    }

}
