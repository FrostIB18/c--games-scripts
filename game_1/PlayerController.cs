 using System;
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 using UnityEngine.UI;

 public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 8f;

    public Transform groundCheck; // Empty object attached to the player's feet
    public float groundCheckRadius; // Circle around player's feet
    public LayerMask groundLayer; // Layers for specifying what is ground
    
    private bool _isTouchingGround;
    private bool _isTouchingLadder;

    private float _direction;

    private Rigidbody2D _player;

    private Animator _playerAnimation;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int OnGround = Animator.StringToHash("OnGround");
    private static readonly int OnLadder = Animator.StringToHash("OnLadder");

    // For respawn
    private Vector3 _respawnPoint;
    public GameObject fallDetector;
    
    // For UI
    public Text score;
    
    // For health bar
    public HealthBar healthBar;
    
    // For climbing
    private float _verticalDirection;
    
    // Start is called before the first frame update
    private void Start()
    {
        // Getting access to the in-game elements
        _player = GetComponent<Rigidbody2D>();
        _playerAnimation = GetComponent<Animator>();

        _respawnPoint = transform.position;

        score.text = "Score: " + Scoring.totalScore;
    }

    // Update is called once per frame
    private void Update()
    {
        // Function for preventing infinite jumping
        _isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Adds walking
        _direction = Input.GetAxis("Horizontal");

        var velocity = _player.velocity;
        switch (_direction)
        {
            case (> 0f):
                velocity = new Vector2(_direction * speed, velocity.y);
                transform.localScale = new Vector2(0.25754f, 0.25754f);
                break;
            case (< 0f):
                velocity = new Vector2(_direction * speed, velocity.y);
                transform.localScale = new Vector2(-0.25754f, 0.25754f);
                break;
            default:
                velocity = new Vector2(0, velocity.y);
                break;
        }
        _player.velocity = velocity;

        // Adds jumping
        if (Input.GetButtonDown("Jump") && _isTouchingGround)
        {
            _player.velocity = new Vector2(_player.velocity.x, jumpSpeed);
        }
        
        // Animation Control
        _playerAnimation.SetFloat(Speed, Mathf.Abs(velocity.x));
        _playerAnimation.SetBool(OnGround, _isTouchingGround);
        _playerAnimation.SetBool(OnLadder, _isTouchingLadder);

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "FallDetector":
                transform.position = _respawnPoint;
                break;
            case "Checkpoint":
                _respawnPoint = transform.position;
                break;
            case "NextLevel":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                _respawnPoint = transform.position;
                break;
            case "PreviousLevel":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                _respawnPoint = transform.position;
                break;
            case "Crystal":
                Scoring.totalScore += 1;
                score.text = "Score: " + Scoring.totalScore;
                collision.gameObject.SetActive(false);
                break;
            
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Spike":
                healthBar.Damage(0.01f);
                break;
            case "Ladder":
                _isTouchingLadder = true;

                _verticalDirection = Input.GetAxis("Vertical");

                _player.velocity = new Vector2(0, _verticalDirection * speed + 0.2f);
                break;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ladder")
        {
            _isTouchingLadder = false;
        }
        
    }
}