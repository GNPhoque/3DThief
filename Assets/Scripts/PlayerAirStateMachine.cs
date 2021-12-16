using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AirState
{
	GROUND,
	JUMP,
	FALL
}

public class PlayerAirStateMachine : MonoBehaviour
{
	[SerializeField]
	Inputs inputs;
	[SerializeField]
	Animator animator;
	[SerializeField]
	PlayerController player;
	
	[SerializeField]
	AirState currentState;

	//INSPECTOR VARIABLES
	[SerializeField]
	float fallDetectionVelocity;

	#region StateMachine
	void Start()
	{
		OnStateEnter(AirState.GROUND);
	}

	private void Update()
	{
		OnStateUpdate(currentState);
	}
	void OnStateEnter(AirState state)
	{
		switch (state)
		{
			case AirState.GROUND:
				OnEnterGround();
				break;
			case AirState.JUMP:
				OnEnterJump();
				break;
			case AirState.FALL:
				OnEnterFall();
				break;
			default:
				break;
		}
	}

	void OnStateUpdate(AirState state)
	{
		switch (state)
		{
			case AirState.GROUND:
				OnUpdateGround();
				break;
			case AirState.JUMP:
				OnUpdateJump();
				break;
			case AirState.FALL:
				OnUpdateFall();
				break;
			default:
				break;
		}
	}

	void OnStateExit(AirState state)
	{
		switch (state)
		{
			case AirState.GROUND:
				OnExitGround();
				break;
			case AirState.JUMP:
				OnExitJump();
				break;
			case AirState.FALL:
				OnExitFall();
				break;
			default:
				break;
		}
	}

	void TransitionToState(AirState toState)
	{
		OnStateExit(currentState);
		currentState = toState;
		OnStateEnter(toState);
	}
	#endregion

	#region OnStateEnter
	void OnEnterGround()
	{
		animator.SetBool("Ground", true);
	}

	void OnEnterJump()
	{
		animator.SetBool("Jump", true);
	}

	void OnEnterFall()
	{
		animator.SetBool("Fall", true);
	}
	#endregion

	#region OnStateUpdate
	private void OnUpdateGround()
	{
		if (!player.isGrounded)
		{
			if (player.velocity.y < fallDetectionVelocity)
			{
				TransitionToState(AirState.FALL);
			}
			else if (inputs.jump)
			{
				TransitionToState(AirState.JUMP);
			} 
		}
	}

	private void OnUpdateJump()
	{
		if (player.isGrounded)
		{
			TransitionToState(AirState.GROUND);
		}
		else if (player.velocity.y < fallDetectionVelocity)
		{
			TransitionToState(AirState.FALL);
		}
	}

	private void OnUpdateFall()
	{
		if (player.isGrounded)
		{
			TransitionToState(AirState.GROUND);
		}
		//For bouncy pads
		else if (player.velocity.y > 0f)
		{
			TransitionToState(AirState.JUMP);
		}
	}

	#endregion

	#region OnStateExit
	private void OnExitGround()
	{
		animator.SetBool("Ground", false);

	}

	private void OnExitJump()
	{
		animator.SetBool("Jump", false);

	}

	private void OnExitFall()
	{
		animator.SetBool("Fall", false);
	}
	#endregion
}
