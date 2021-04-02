//------------------------------------------------------------------------------
//
// File Name:	PlayerMovementController.cs
// Author(s):	Jeremy Kings (j.kings) - Unity Project
//              Nathan Mueller - original Zero Engine project
//              Andrew Dahlman-Oeth
//              Sim McQueen
// Project:		Endless Runner
// Course:		WANIC VGP
//
// Copyright © 2021 DigiPen (USA) Corporation.
//
//------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public float MoveSpeed = 10;
    public int MaxHealth = 3;
    public float JumpHeight = 5;
    public int MaxNumberOfJumps = 2;
    public KeyCode JumpKey = KeyCode.Space;
    public KeyCode SlideKey = KeyCode.LeftShift;
    public List<AudioClip> audioClips = new List<AudioClip>();
    public ParticleSystem dustPS;
    public ParticleSystem jumpPS;
    private int jumpsRemaining = 0;
    private bool floorCheck = true;
    private int currentHealth = 0;
    private string nameOfHealthDisplayObject = "HealthBar";
    private string nameOfDistanceLabelObject = "DistanceLabel";
    private GameObject healthBarObj = null;
    private GameObject distanceObj = null;
    private float startingX = 0;
    private PlayerAnimationManager animationManager;
    private float invulnerableTime = -5f;
    public SpadeScript SS;
    private AudioSource audioSource = null;

    // Start is called before the first frame update
    void Start()
    {
        PlayDust();
        healthBarObj = GameObject.Find(nameOfHealthDisplayObject);
        distanceObj = GameObject.Find(nameOfDistanceLabelObject);
        animationManager = GetComponent<PlayerAnimationManager>();
        if (healthBarObj != null)
        {
          healthBarObj.GetComponent<FeedbackBar>().SetMax(MaxHealth);
        }

        // Take the square root of the jump height so that the math for gravity works
        // to make the number the user enters the number of units the player will
        // actually be able to jump
        JumpHeight = Mathf.Sqrt(2.0f * Physics2D.gravity.magnitude * JumpHeight);
        
        currentHealth = MaxHealth;
        startingX = transform.position.x;

        // Reset score
        PlayerSaveData.DistanceRun = 0;


        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        bool grounded = IsGrounded();

        // Jumping
        if (Input.GetKeyDown(JumpKey))
        {
            if (jumpsRemaining > 0)
            {
                StopDust();
                PlayJump();
            }
            if (jumpsRemaining == 2)
            {               
                audioSource.PlayOneShot(audioClips[0], 0.4f);
                floorCheck = false;
                animationManager.SwitchTo(PlayerAnimationStates.Jump);
                var jump_vec = new Vector3(0,JumpHeight,0);
                gameObject.GetComponent<Rigidbody2D>().velocity = jump_vec;
                jumpsRemaining -= 1;
            }
            // Only difference is the jump sound
            else if (jumpsRemaining == 1)
            {
                audioSource.PlayOneShot(audioClips[1], 0.7f);
                floorCheck = false;
                animationManager.SwitchTo(PlayerAnimationStates.Jump);
                var jump_vec = new Vector3(0,JumpHeight,0);
                gameObject.GetComponent<Rigidbody2D>().velocity = jump_vec;
                jumpsRemaining -= 1;
            }
        }
        // Sliding
        // else if (Input.GetKey(SlideKey) && grounded)
        // {
        //     animationManager.SwitchTo(PlayerAnimationStates.Slide);
        // }
        // Running
        else if (grounded)
        {
            PlayDust();
            animationManager.SwitchTo(PlayerAnimationStates.Run);
        }
        // Falling
        else
        {
            animationManager.SwitchTo(PlayerAnimationStates.Jump);
        }

        // Lock the player to X = StartingX;
        gameObject.transform.position = new Vector3(startingX, transform.position.y, transform.position.z);

        // Update the Distance travelled
        PlayerSaveData.DistanceRun += MoveSpeed * Time.deltaTime;
        if (distanceObj != null)
        {
            if (distanceObj.GetComponent<TextMeshProUGUI>() != null)
            {
                string distText = string.Format("{0,4:F1}", PlayerSaveData.DistanceRun);
                distanceObj.GetComponent<TextMeshProUGUI>().text = "Distance: " 
                    + distText + " m";
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Hit an Obstacle
        if (collision.collider.gameObject.CompareTag("Obstacle"))
        {
            Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
            if (invulnerableTime >= (Time.time - 5))
            {
                print("HIT BUT JOKER ACTIVE");
            }
            else
            {
                

                if (obstacle != null)
                {
                    audioSource.PlayOneShot(audioClips[3], 0.3f);
                    currentHealth -= obstacle.Damage;
                    // Game Over
                    if (currentHealth <= 0)
                    {
                        // Load score level
                        UnityEngine.SceneManagement.SceneManager.LoadScene("ScoreScreen");
                    }
                    
                    if (healthBarObj != null)
                    {

                        healthBarObj.GetComponent<FeedbackBar>().SetValue(currentHealth);
                        animationManager.SwitchTo(PlayerAnimationStates.Hurt);
                    }
                }
            }
            if (obstacle.DestroyOnPlayerCollision)
            {
                Destroy(collision.collider.gameObject);
            }

        }
        if (collision.collider.gameObject.CompareTag("Destructible"))
        {
            DestructibleObstacles destructible = collision.gameObject.GetComponent<DestructibleObstacles>();
            if (invulnerableTime >= (Time.time - 5))
            {
                print("HIT BUT JOKER ACTIVE");
            }
            else
            {


                if (destructible != null)
                {
                    audioSource.PlayOneShot(audioClips[3], 0.3f);
                    currentHealth -= destructible.Damage;
                    // Game Over
                    if (currentHealth <= 0)
                    {
                        // Load score level
                        UnityEngine.SceneManagement.SceneManager.LoadScene("ScoreScreen");
                    }

                    if (healthBarObj != null)
                    {
                        healthBarObj.GetComponent<FeedbackBar>().SetValue(currentHealth);
                        animationManager.SwitchTo(PlayerAnimationStates.Hurt);
                        
                    }
                }
            }

            if (destructible.DestroyOnPlayerCollision)
            {
                Destroy(collision.collider.gameObject);
            }
        }
        // Hit the floor
        if (collision.collider.gameObject.CompareTag("Floor"))
        {
            jumpsRemaining = MaxNumberOfJumps;
            if(!floorCheck)
            {
                audioSource.PlayOneShot(audioClips[2], 0.1f);
                floorCheck = true;
                
            }
            
        }
        if (collision.collider.gameObject.CompareTag("Prop")) {
            Prop prop = collision.gameObject.GetComponent<Prop>();
            if (prop != null)
            {
                if (currentHealth != 3)
                {
                    audioSource.PlayOneShot(audioClips[4], 0.1f);
                    currentHealth += 1;
                    if (healthBarObj != null)
                    {
                        healthBarObj.GetComponent<FeedbackBar>().SetValue(currentHealth);
                        animationManager.SwitchTo(PlayerAnimationStates.Hurt);
                        
                    }
                }
                Destroy(collision.collider.gameObject);
                
            }
        }
        if(collision.collider.gameObject.CompareTag("JokerCard"))
        {
            audioSource.PlayOneShot(audioClips[4], 0.2f);
            invulnerableTime = Time.time;
            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.gameObject.CompareTag("Spade"))
        {
            audioSource.PlayOneShot(audioClips[4], 0.1f);
            SS.SpadeAmmo += 1;
            print("Spade Ammo: " + SS.SpadeAmmo);
            Destroy(collision.collider.gameObject);
        }


    }

    public bool IsGrounded()
    {
        return jumpsRemaining == MaxNumberOfJumps;
    }
    // Particle Systems

    void PlayDust(){
        dustPS.Play();
    }
    void StopDust(){
        dustPS.Stop();
    }

    void PlayJump(){
        jumpPS.Play();
    }
}
