using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementState
{
	IDLE,
	WALK,
	SPRINT,
	SNEAK
}

public class PlayerMovementStateMachine : MonoBehaviour
{
	[SerializeField]
	Inputs inputs;
	[SerializeField]
	Animator animator;
	[SerializeField]
	PlayerController player;

	[SerializeField]
	MovementState _currentState;

	public MovementState currentState { get => _currentState; set => _currentState = value; }

	#region StateMachine
	void Start()
	{
		OnStateEnter(MovementState.IDLE);
	}

	private void Update()
	{
		OnStateUpdate(currentState);
	}
	void OnStateEnter(MovementState state)
	{
		switch (state)
		{
			case MovementState.IDLE:
				OnEnterIdle();
				break;
			case MovementState.WALK:
				OnEnterWalk();
				break;
			case MovementState.SPRINT:
				OnEnterSprint();
				break;
			case MovementState.SNEAK:
				OnEnterSneak();
				break;
			default:
				break;
		}
	}

	void OnStateUpdate(MovementState state)
	{
		switch (state)
		{
			case MovementState.IDLE:
				OnUpdateIdle();
				break;
			case MovementState.WALK:
				OnUpdateWalk();
				break;
			case MovementState.SPRINT:
				OnUpdateSprint();
				break;
			case MovementState.SNEAK:
				OnUpdateSneak();
				break;
			default:
				break;
		}
	}

	void OnStateExit(MovementState state)
	{
		switch (state)
		{
			case MovementState.IDLE:
				OnExitIdle();
				break;
			case MovementState.WALK:
				OnExitWalk();
				break;
			case MovementState.SPRINT:
				OnExitSprint();
				break;
			case MovementState.SNEAK:
				OnExitSneak();
				break;
			default:
				break;
		}
	}

	void TransitionToState(MovementState toState)
	{
		OnStateExit(currentState);
		currentState = toState;
		OnStateEnter(toState);
	}
	#endregion

	#region OnStateEnter
	void OnEnterIdle()
	{
		animator.SetBool("Idle", true);
	}

	void OnEnterWalk()
	{
		animator.SetBool("Walk", true);
	}

	void OnEnterSprint()
	{
		animator.SetBool("Sprint", true);
	}

	void OnEnterSneak()
	{
		animator.SetBool("Sneak", true);
	}
	#endregion

	#region OnStateUpdate
	private void OnUpdateIdle()
	{
		if (player.isGrounded && inputs.sneak)
		{
			TransitionToState(MovementState.SNEAK);
		}
		else if (inputs.movement.sqrMagnitude > 0f)
		{
			if (player.isGrounded && inputs.sprint)
			{
				TransitionToState(MovementState.SPRINT);
			}
			else
			TransitionToState(MovementState.WALK);
		}
	}

	private void OnUpdateWalk()
	{
		if (player.isGrounded && inputs.sneak)
		{
			TransitionToState(MovementState.SNEAK);
		}
		else if (inputs.movement.sqrMagnitude == 0f)
		{
			TransitionToState(MovementState.IDLE);
		}
		else if (inputs.sprint)
		{
			TransitionToState(MovementState.SPRINT);
		}
	}

	private void OnUpdateSprint()
	{
		if (player.isGrounded && inputs.sneak)
		{
			TransitionToState(MovementState.SNEAK);
		}
		else if (inputs.movement.sqrMagnitude == 0f)
		{
			TransitionToState(MovementState.IDLE);
		}
		else if (!inputs.sprint)
		{
			TransitionToState(MovementState.WALK);
		}
	}

	private void OnUpdateSneak()
	{
		if (!inputs.sneak)
		{
			if (inputs.movement.sqrMagnitude == 0f)
			{
				TransitionToState(MovementState.IDLE);
			}
			else if (inputs.sprint)
			{
				TransitionToState(MovementState.SPRINT);
			}
			else
			{
				TransitionToState(MovementState.WALK);
			}
		}
	}

	#endregion

	#region OnStateExit
	private void OnExitIdle()
	{
		animator.SetBool("Idle", false);
	}

	private void OnExitWalk()
	{
		animator.SetBool("Walk", false);
	}

	private void OnExitSprint()
	{
		animator.SetBool("Sprint", false);
	}

	private void OnExitSneak()
	{
		animator.SetBool("Sneak", false);
	}
	#endregion
}
