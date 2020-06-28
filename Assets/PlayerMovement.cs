using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PlayerMovement : Agent
{

  public CharacterController2D controller;
  public Animator animator;

  float horizontalMove = 0f;
  public float runSpeed = 40f;
  bool jump = false;
  bool crouch = false;

  public override void Initialize()
  {

  }
  public override void OnActionReceived(float[] vectorAction)
  {
    if (Mathf.FloorToInt(vectorAction[0]) == 1)
    {
      jump = true;
      animator.SetBool("IsJumping", true);
    }
  }
  public override void OnEpisodeBegin()
  {

  }

  public override void Heuristic(float[] actionsOut)
  {
    actionsOut[0] = 0;
    horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
    animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

    if (Input.GetButton("Jump"))
    {
      actionsOut[0] = 1;
      animator.SetBool("IsJumping", true);
    }

    if (Input.GetButtonDown("Crouch"))
    {
      crouch = true;
    }
    else if (Input.GetButtonUp("Crouch"))
    {
      crouch = false;
    }
  }


  public void OnLanding()
  {
    animator.SetBool("IsJumping", false);
  }

  public void OnCrouching(bool isCrouching)
  {
    animator.SetBool("IsCrouching", isCrouching);
  }

  void FixedUpdate()
  {
    // Moving our character
    controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
    jump = false;

  }





}
