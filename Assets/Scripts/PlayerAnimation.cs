// Author: Paul King

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    // member variables
    private Animator animator;
    private PlayerController player;
    private GameManager gameManager;
    private bool falling;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerController>();
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (falling)
        {
            animator.SetFloat("Y_Velocity", player.Y_Velocity + gameManager.Gravity);
            animator.SetBool("Grounded", player.Grounded);
        }
    }

    /// <summary>
    /// Changes the animation state uses in the player's animator component
    /// </summary>
    /// <param name="trigger">The trigger used to transition to another animation</param>
    public void SetAnimation(string trigger)
    {
        animator.SetTrigger(trigger);
    }

    /// <summary>
    /// Sets all the parameters needed to transition to the running animation
    /// </summary>
    public void PlayRunningAnimation()
    {
        animator.SetBool("GameRunning", true);
        animator.SetBool("WallCollision", false);
        animator.SetBool("Grounded", true);

        falling = false;
    }

    /// <summary>
    /// Sets all the parameters needed to transition to the dead animation
    /// </summary>
    public void PlayDeadAnimation()
    {
        // play death animation once
        animator.SetTrigger("Die");
        //animator.SetBool("alive", false);
        animator.SetBool("GameRunning", false);
        falling = false;
    }

    /// <summary>
    /// Sets all the parameters needed to begin the jump animations
    /// </summary>
    public void PlayJumpSequence()
    {
        animator.SetTrigger("Jump");
        animator.SetBool("HighJump", false);
        falling = true;
    }

    /// <summary>
    /// Sets all the parameters needed to begin the air jump animation 
    /// </summary>
    public void PlayHighJumpSequence()
    {
        animator.SetTrigger("Jump");
        animator.SetBool("HighJump", true);
        falling = true;
    }

    /// <summary>
    /// Sets all the parameters needed to transition to the idle animation
    /// </summary>
    public void PlayIdleState()
    {
        animator.SetBool("WallCollision", true);
        falling = false;
    }

    /// <summary>
    /// Sets all the parameters needed to transition to the falling animtion
    /// </summary>
    public void PlayFallAnimation()
    {
        falling = true;
    }

}
