using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class CharacterController2D : Agent
{
  // public Animator animator;
  [SerializeField] private float m_JumpForce = 400f;              // Amount of force added when the player jumps.
  [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;      // Amount of maxSpeed applied to crouching movement. 1 = 100%
  [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
  [SerializeField] private bool m_AirControl = false;             // Whether or not a player can steer while jumping;
  [SerializeField] private LayerMask m_WhatIsGround;              // A mask determining what is ground to the character
  [SerializeField] private Transform m_GroundCheck;             // A position marking where to check if the player is grounded.
  [SerializeField] private Transform m_CeilingCheck;              // A position marking where to check for ceilings
  [SerializeField] private Collider2D m_CrouchDisableCollider;        // A collider that will be disabled when crouching

  [SerializeField] private string sceneName;
  const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
  private bool m_Grounded;            // Whether or not the player is grounded.
  const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
  private Rigidbody2D m_Rigidbody2D;
  private bool m_FacingRight = true;  // For determining which way the player is currently facing.
  private Vector3 m_Velocity = Vector3.zero;

  public float horizontalMove;

  public GameManager1 GameManager;

  [Header("Events")]
  [Space]

  public UnityEvent OnLandEvent;

  [System.Serializable]
  public class BoolEvent : UnityEvent<bool> { }

  public BoolEvent OnCrouchEvent;
  private bool m_wasCrouching = false;
  public override void Initialize()
  {
    m_Rigidbody2D = GetComponent<Rigidbody2D>();

    if (OnLandEvent == null)
      OnLandEvent = new UnityEvent();

    if (OnCrouchEvent == null)
      OnCrouchEvent = new BoolEvent();
  }

  // private void FixedUpdate()
  // {
  //   // if (jumpIsReady)
  //   //   RequestDecision();
  // }

  public override void OnActionReceived(float[] vectorAction)
  {
    //jump n
    if (Mathf.FloorToInt(vectorAction[0]) == 1)
      Move(0.0f, false, true);
    // Debug.Log("jump n");
    // jump r
    if (Mathf.FloorToInt(vectorAction[0]) == 2)
      Move(1.0f, false, true);
    // AddReward(-0.002f);
    // Debug.Log("-0.002");
    // Debug.Log("jump r");
    // jump l
    if (Mathf.FloorToInt(vectorAction[0]) == 3)
      Move(1.0f, false, true);
    // Debug.Log("jump l");
    //jump courch
    if (Mathf.FloorToInt(vectorAction[0]) == 4)
      // Move(0.0f, true, true);




      //crouch n
      if (Mathf.FloorToInt(vectorAction[1]) == 1)
        // Move(0.0f, true, false);
        // Debug.Log("Cruoch");
        //crouch r
        if (Mathf.FloorToInt(vectorAction[1]) == 2)
          // Move(1.0f, true, false);
          //crouch l
          if (Mathf.FloorToInt(vectorAction[1]) == 3)
            // Move(-1.0f, true, false);
            //crouch j
            if (Mathf.FloorToInt(vectorAction[1]) == 4)
              // Move(0.0f, true, true);


              //right
              if (Mathf.FloorToInt(vectorAction[2]) == 1)
                Move(1.0f, false, false);

    // Debug.Log("R");
    //right j
    if (Mathf.FloorToInt(vectorAction[2]) == 2)
      Move(1.0f, false, true);
    // Debug.Log("R J");
    //right c
    if (Mathf.FloorToInt(vectorAction[2]) == 3)
      // Move(1.0f, true, false);

      //left
      if (Mathf.FloorToInt(vectorAction[3]) == 1)
        Move(1.0f, false, false);
    // Debug.Log("L");
    //left j
    if (Mathf.FloorToInt(vectorAction[3]) == 2)
      Move(1.0f, false, true);
    // Debug.Log("L J");
    //left c
    if (Mathf.FloorToInt(vectorAction[3]) == 3)
      // Move(1.0f, true, false);

      //nothing
      if (Mathf.FloorToInt(vectorAction[4]) == 1)
        Move(0.0f, false, false);

  }

  public override void Heuristic(float[] actionsOut)
  {
    actionsOut[0] = 0;
    actionsOut[1] = 0;
    actionsOut[2] = 0;
    actionsOut[3] = 0;
    actionsOut[4] = 0;




    if (Input.GetButton("Jump"))
    {
      actionsOut[0] = 1;
      actionsOut[1] = 4;
      actionsOut[2] = 2;
      actionsOut[3] = 2;
    }


    // if (Input.GetKeyDown("down"))
    // {
    //   actionsOut[0] = 4;
    //   actionsOut[1] = 1;
    //   actionsOut[2] = 3;
    //   actionsOut[3] = 3;
    // }

    if (Input.GetKeyDown("right"))
    {
      horizontalMove = Input.GetAxisRaw("Horizontal") * 40f;
      // animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
      actionsOut[2] = 1;
      actionsOut[0] = 2;
      actionsOut[1] = 2;

    }
    if (Input.GetKeyDown("left"))
    {
      horizontalMove = Input.GetAxisRaw("Horizontal") * 40f;
      // animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
      actionsOut[3] = 1;
      actionsOut[0] = 3;
      actionsOut[1] = 3;
    }
    else
    {
      actionsOut[4] = 1;
      actionsOut[3] = 1;
      actionsOut[2] = 1;
      actionsOut[1] = 1;
      actionsOut[0] = 1;
    }

  }

  public override void OnEpisodeBegin()
  {
    GameManager.RestartGame();
  }


  private void FixedUpdate()
  {
    RequestDecision();
    bool wasGrounded = m_Grounded;
    m_Grounded = false;

    // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
    // This can be done using layers instead but Sample Assets will not overwrite your project settings.
    Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
    for (int i = 0; i < colliders.Length; i++)
    {
      if (colliders[i].gameObject != gameObject)
      {
        m_Grounded = true;
        if (!wasGrounded)
          OnLandEvent.Invoke();
      }
    }
  }


  public void Move(float move, bool crouch, bool jump)
  {
    // If crouching, check to see if the character can stand up
    if (!crouch)
    {
      // If the character has a ceiling preventing them from standing up, keep them crouching
      if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
      {
        crouch = true;
      }
    }

    //only control the player if grounded or airControl is turned on
    if (m_Grounded || m_AirControl)
    {

      // If crouching
      if (crouch)
      {
        if (!m_wasCrouching)
        {
          m_wasCrouching = true;
          OnCrouchEvent.Invoke(true);
        }

        // Reduce the speed by the crouchSpeed multiplier
        move *= m_CrouchSpeed;

        // Disable one of the colliders when crouching
        if (m_CrouchDisableCollider != null)
          m_CrouchDisableCollider.enabled = false;
      }
      else
      {
        // Enable the collider when not crouching
        if (m_CrouchDisableCollider != null)
          m_CrouchDisableCollider.enabled = true;

        if (m_wasCrouching)
        {
          m_wasCrouching = false;
          OnCrouchEvent.Invoke(false);
        }
      }

      // Move the character by finding the target velocity
      Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
      // And then smoothing it out and applying it to the character
      m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

      // If the input is moving the player right and the player is facing left...
      if (move > 0 && !m_FacingRight)
      {
        // ... flip the player.
        Flip();
      }
      // Otherwise if the input is moving the player left and the player is facing right...
      else if (move < 0 && m_FacingRight)
      {
        // ... flip the player.
        Flip();
      }
    }
    // If the player should jump...
    if (m_Grounded && jump)
    {
      // Add a vertical force to the player.
      m_Grounded = false;
      m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
    }
  }


  private void Flip()
  {
    // Switch the way the player is labelled as facing.
    m_FacingRight = !m_FacingRight;

    // Multiply the player's x local scale by -1.
    Vector3 theScale = transform.localScale;
    theScale.x *= -1;
    transform.localScale = theScale;
  }
  private void OnTriggerEnter2D(Collider2D other)
  {

    if (other.transform.tag == "Coin")
    {
      AddReward(0.5f);
      Debug.Log("Coin + 0.5");

    }
    if (other.gameObject.tag == "gap")
    {
      AddReward(0.1f);
      Debug.Log("Jumped gap + 0.1");
    }
    if (other.gameObject.tag == "Door")
    {
      AddReward(1.0f);
      EndEpisode();
      Debug.Log("Door + 1.0");
    }

  }
  void OnCollisionEnter2D(Collision2D other)
  {

    if (other.gameObject.tag == "KillBox")
    {
      // GameManager.RestartGame();
      AddReward(-0.1f);
      EndEpisode();
      Debug.Log("Killbox -0.1");

      SceneManager.LoadScene(sceneName);

    }


  }
}