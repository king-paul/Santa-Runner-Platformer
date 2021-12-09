/*
 * Authors: Paul King, James Kennedy
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum PlayerState { Idle, Running, Jumping, Falling, KnockBack, IdleJump }

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // public cariables
    [Header("Movement Variables")]

    public float runSpeed = 10.0f;
    public float m_JumpHeight = 5.0f;
    public float m_FallMultiplier = 2.5f;
    public float m_JumpMultiplier = 2f;
    public float m_AirJumpMultiplier = 4f;
    public float m_KnockBackSpeed = 1f;
    public bool m_airJump = false;

    // serialized private values
    [Header("Collision Checkers")]
    [SerializeField] private LayerMask m_GroundLayers = default;
    [SerializeField] private LayerMask m_WallLayers = default;
    [SerializeField] private Transform[] m_GroundChecks = null;
    [SerializeField] private Transform[] m_WallChecks = null;

    // private variables
    private bool m_JumpPressed;
    private float m_JumpTimer;
    private float m_JumpGracePeriod = 0.2f;
    private float m_HorizontalInput;
    private bool m_IsGrounded;
    private bool m_Blocked;
    private bool m_IsAlive;
    
    private Vector3 moveVelocity;
    private Vector3 m_PrevPos;
    private Vector3 m_CurrentVel;
    private PlayerState state;

    // Controllers/Managers
    CharacterController controller;
    GameManager gameManager;
    Touch touchInput;

    bool hasAirJumped;

    [Header("Events")]
    public UnityEvent onBegin;
    public UnityEvent onJump, onAirJump, onFall, onLand, onCollisionWithWall, onCollisionWithHazard, 
        onFallOffLevel, onPresentCollect, onFoodCollect;

    // Properties //

    /// <summary>
    /// Returns true or false depending on whether or not the player is on the ground
    /// </summary>
    public bool Grounded { get => controller.isGrounded; }

    /// <summary>
    /// Returns the current velocity of the player along the y axis
    /// </summary>
    public float Y_Velocity { get => m_CurrentVel.y; }

    /// <summary>
    /// Returns the current state the player is in
    /// Can be Idle, Running, Jumping, Falling, KnockBack or IdleJump
    /// </summary>
    public PlayerState playerState { get => state; }

    // Functions //
    public void SetAlive(bool _state) { m_IsAlive = _state; }
    public void StartRunning() { state = PlayerState.Running; }

    // Start is called before the first frame update
    void Start()
    {
        // try/catch block
        try
        {
            controller = GetComponent<CharacterController>();
            gameManager = GameManager.m_Instance;
            //onGround = true;
            SetAlive(true);

            state = PlayerState.Idle;
            hasAirJumped = false;
        }
        catch (Exception e)
        {
            Debug.LogError("Exception in PlayerController: " + e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.State != GameState.Running)
        {
            if (controller.enabled)
                controller.enabled = false;

            return;
        }

        UpdatePosition();
        UpdateState();

        gameManager.SetJumpMeter(m_JumpTimer, m_JumpTimer + m_JumpGracePeriod);

        //Debug.Log("On Ground: " + onGround);
    }

    private void LateUpdate()
    {
        CheckColliders();

        // check if the player has moved off the z axis and if it has, move it back
        if (transform.position.z != 0)
        {
            controller.enabled = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            controller.enabled = true;
        }
    }

    /// <summary>
    /// Checks if the colliders around the player are touching anything using the Physics sphere
    /// </summary>
    private void CheckColliders()
    {
        m_HorizontalInput = 1;

        // ground checks
        m_IsGrounded = false;
        foreach (var groundCheck in m_GroundChecks)
        {
            if (Physics.CheckSphere(groundCheck.position, 0.1f, m_GroundLayers,
                QueryTriggerInteraction.Ignore))
            {
                m_IsGrounded = true;
            }
        }

        if (state != PlayerState.Idle)
        {
            // wall checks        
            foreach (var wallCheck in m_WallChecks)
            {
                if (Physics.CheckSphere(wallCheck.position, 0.1f, m_WallLayers,
                    QueryTriggerInteraction.Ignore))
                {
                    m_Blocked = true;
                    return;
                }
            }
            m_Blocked = false;
        }

    }

    /// <summary>
    /// Sets the position of the player game object each frame
    /// </summary>
    private void UpdatePosition()
    {
        // Update Horizontal movement
        if (!m_Blocked && state != PlayerState.KnockBack)
        {            
            controller.Move(new Vector3(m_HorizontalInput * runSpeed, 0, 0) * Time.deltaTime);
        }
        else if(state == PlayerState.KnockBack)
        {
            controller.Move(Vector3.left * m_KnockBackSpeed * Time.deltaTime);
        }

        if (m_IsGrounded && moveVelocity.y < 0)
        {
            moveVelocity.y = 0f;
        }
        else if(m_IsAlive)
        {
            //Add Gravity
            moveVelocity.y += gameManager.Gravity * Time.deltaTime;
        }

        //Jumping
        m_JumpPressed = (Input.GetButtonDown("Jump") || Input.touchCount > 0);

        if (m_JumpPressed)
        {
            m_JumpTimer = Time.time;
        }

        if (m_JumpPressed || (m_JumpTimer > 0 && Time.time < m_JumpTimer + m_JumpGracePeriod))
        {
            // ground jump
            if (m_IsGrounded || (m_airJump && !hasAirJumped))
            {
                if (m_IsGrounded)
                {
                    if (state == PlayerState.Idle)
                        state = PlayerState.IdleJump;
                    else
                        state = PlayerState.Jumping;

                    onJump.Invoke();
                }
                else if(m_airJump)
                {
                    onAirJump.Invoke();
                    hasAirJumped = true;
                }
                
                moveVelocity.y += Mathf.Sqrt(m_JumpHeight * -2.0f * gameManager.Gravity);
                m_JumpTimer = -1;
            }
        }

        // Calculate current player velocity
        m_CurrentVel = (transform.position - m_PrevPos) / Time.deltaTime;
        m_PrevPos = transform.position;

        // Jump handling
        if (m_CurrentVel.y < 0)
        {
            // If falling
            moveVelocity += (Vector3.up * gameManager.Gravity * (m_FallMultiplier - 1) * Time.deltaTime);
            //Debug.Log("Falling");
        }
        else if (m_CurrentVel.y > 0 && !Input.GetButton("Jump"))
        {
            //ground jump multiplier
            if(!hasAirJumped)
                moveVelocity += (Vector3.up * gameManager.Gravity * (m_JumpMultiplier - 1)
                    * Time.deltaTime);
            else // air jump
                moveVelocity += (Vector3.up * gameManager.Gravity * (m_AirJumpMultiplier - 1)
                    * Time.deltaTime);

            //Debug.Log("LowJump");
        }

        // Detect collision above the player
        if (controller.collisionFlags == CollisionFlags.Above)
        {
            moveVelocity.y = 0f;
        }

        // Vertical velocity
        controller.Move(moveVelocity * Time.deltaTime);
    }

    /// <summary>
    /// Updates the state of the player by checking a series of conditions each frame
    /// </summary>
    private void UpdateState()
    {
        if (state == PlayerState.Running && !m_IsAlive)
        {
            SetAlive(true);
        }

        // running -> Falling
        if (state == PlayerState.Running && !m_IsGrounded && m_CurrentVel.y < 0)
        {
            state = PlayerState.Falling;
            onFall.Invoke();
        }

        // jumping -> falling
        if (state == PlayerState.Jumping && (m_CurrentVel.y <= 0))
        {
            state = PlayerState.Falling;
            onFall.Invoke();
        }
        
        if(m_Blocked && state != PlayerState.Idle && state != PlayerState.KnockBack)
        {
            // running -> idle
            if (state == PlayerState.Running)
            {
                state = PlayerState.Idle;
                onCollisionWithWall.Invoke();
            }
            // jumping or falling -> knockback
            else if (state != PlayerState.IdleJump)
            {
                state = PlayerState.KnockBack;
            }
        }
        
    }
    
    // Collision Detection
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (gameManager.State != GameState.Running)
            return;

        // check if there is a collision with the ground
        if (state != PlayerState.Running && state != PlayerState.Idle
            && (hit.gameObject.CompareTag("Ground") ||
            hit.gameObject.CompareTag("Platform")) || hit.gameObject.CompareTag("Floor"))
        {
            //moveVelocity.y = 0;
            hasAirJumped = false;
            state = PlayerState.Running;
            onLand.Invoke();
        }

        // check if there is a collision with an obstacle
        if (hit.gameObject.CompareTag("Obstacle") || hit.gameObject.CompareTag("Hazard") ||
            hit.gameObject.CompareTag("Arrow"))
        {
            SetAlive(false);
            onCollisionWithHazard.Invoke();
            gameManager.UpdateGameState(GameState.Dead);
        }

    }

    // Trigger collisions
    private void OnTriggerEnter(Collider other)
    {
        if (!m_IsAlive)
            return;

        switch(other.tag)
        {
            case "Food":
                Destroy(other.gameObject);
                onFoodCollect.Invoke();
                gameManager.AddCoin();
                break;

            case "KillBox": case "KillZone":
                gameManager.UpdateGameState(GameState.Dead);
                SetAlive(false);
                onFallOffLevel.Invoke();                
                break;
        }

    }

    /// <summary>
    /// Debug Gizmo drawing
    /// </summary>
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (var checks in m_GroundChecks)
        {
            Gizmos.DrawWireSphere(checks.transform.position, 0.1f);
        }
        Gizmos.color = Color.magenta;
        foreach (var checks in m_WallChecks)
        {
            Gizmos.DrawWireSphere(checks.transform.position, 0.1f);
        }
    }


}
