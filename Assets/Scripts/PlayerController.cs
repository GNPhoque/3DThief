using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	//SELF COMPONENTS
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    Transform t;
    [SerializeField]
    Transform stepRayLower;
    [SerializeField]
    Transform stepRayUpper;
	[SerializeField]
	PlayerMovementStateMachine moveStateMachine;
	[SerializeField]
	Animator animator;
	[SerializeField]
	CapsuleCollider capsule;

	//OTHER REFERENCES
	[SerializeField]
    Inputs inputs;

	//INSPECTOR VARIABLES
	[SerializeField][Range(0f,60f)]
	float slopeLimit;
	[SerializeField]
	float jumpForce;
	[SerializeField]
	float rotationSpeed;
	[SerializeField]
	float walkSpeed;
	[SerializeField]
	float sprintSpeed;
	[SerializeField]
	float sneakSpeed;
	[SerializeField]
	float rayLength;
	[SerializeField]
	float stepSmooth;
	[SerializeField]
	float stepHeight;
	[SerializeField]
	LayerMask groundLayer;

	//PRIVATE FIELDS
	[SerializeField]
	bool _isGrounded;
	[SerializeField]
	bool _isJumping;
	bool isSliding;
	bool isFacingStairs;
	float currentSpeed;
	[SerializeField]
	float currentGroundAngle;
	[SerializeField]
	float angleInFront;

	//PUBLIC PROPERTIES
	public Vector3 velocity
	{
		get { return rb.velocity; }
	}

	public bool isGrounded
	{
		get { return _isGrounded; }
		set { _isGrounded = value; }
	}


	private void Start()
	{
		stepRayUpper.position = new Vector3(stepRayUpper.position.x, t.position.y + stepHeight, stepRayUpper.position.z);
		currentSpeed = walkSpeed;
	}

	void Update()
	{
		SetCurrentSpeed();
		animator.SetFloat("PlayerDirectionX", inputs.movement.x);
		animator.SetFloat("PlayerDirectionY", inputs.movement.y);
	}

	void FixedUpdate()
	{
		if (!isSliding)
		{
			Move();
		}
		if (inputs.movement != Vector2.zero)
		{
			Vector3 cameraForward = Camera.main.transform.forward;
			cameraForward = new Vector3(cameraForward.x, 0f, cameraForward.z);
			rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, Quaternion.LookRotation(cameraForward), rotationSpeed));
		}
		CheckGround();
		if (!isSliding)
		{
			StepClimb();  
		}
		if (inputs.jump && isGrounded && !_isJumping)
		{
			Debug.Log("Jump");
			rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
			isGrounded = false;
			_isJumping = true;
		}
	}

	private void SetCurrentSpeed()
	{
		switch (moveStateMachine.currentState)
		{
			case MovementState.IDLE:
				currentSpeed = walkSpeed;
				break;
			case MovementState.WALK:
				currentSpeed = walkSpeed;
				break;
			case MovementState.SPRINT:
				currentSpeed = sprintSpeed;
				break;
			case MovementState.SNEAK:
				currentSpeed = sneakSpeed;
				break;
			default:
				break;
		}
	}

	private void Move()
	{
		Vector3 moveX = t.right * inputs.movement.x * currentSpeed;
		Vector3 moveZ = t.forward * inputs.movement.y * currentSpeed;
		Vector3 move = moveX + moveZ;
		rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
		//rb.velocity = new Vector3(move.x, (_isJumping || rb.velocity.y < -0.1f) ? rb.velocity.y : 0f, move.z);
	}

	void CheckGround()
	{
		isGrounded = false;
		float capsuleHeight = Mathf.Max(capsule.radius * 2f, capsule.height);
		Vector3 capsuleBottom = transform.TransformPoint(capsule.center - Vector3.up * capsuleHeight / 2f);
		float radius = transform.TransformVector(capsule.radius, 0f, 0f).magnitude;

		Ray ray = new Ray(capsuleBottom + transform.up * .01f, -transform.up);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, radius * 5f))
		{
			float normalAngle = Vector3.Angle(hit.normal, transform.up);
			currentGroundAngle = normalAngle;
			if (normalAngle < slopeLimit)
			{
				float maxDist = radius / Mathf.Cos(Mathf.Deg2Rad * normalAngle) - radius + (_isJumping?.02f:.5f);
				if (hit.distance < maxDist)
				{
					isGrounded = true;
					_isJumping = false;
					isSliding = false;
				}
			}
			else
			{
				isSliding = true;
			}
		}
	}

	void StepClimb()
	{
		Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

		angleInFront = 0f;
		RaycastHit hitLower;
		Debug.DrawRay(stepRayLower.position, flatVelocity.normalized * (capsule.radius + .2f), Color.red);
		if (Physics.Raycast(stepRayLower.position, flatVelocity + transform.TransformDirection(Vector3.forward), out hitLower, capsule.radius + 0.1f))
		{
			float normalAngle = Vector3.Angle(hitLower.normal, transform.up);
			Debug.Log(normalAngle);
			angleInFront = normalAngle;
			//if (Mathf.Approximately(normalAngle, 90f))
			if (normalAngle > slopeLimit)
			{
				RaycastHit hitUpper;
				Debug.DrawRay(stepRayUpper.position, ((flatVelocity + transform.TransformDirection(Vector3.forward)).normalized) * (hitLower.distance + .01f), Color.red);
				if (!Physics.Raycast(stepRayUpper.position, flatVelocity + transform.TransformDirection(Vector3.forward), out hitUpper, hitLower.distance + .01f))
				{
					rb.position += new Vector3(0f, stepSmooth * currentSpeed * Time.deltaTime, 0f);
					isGrounded = true;
				}
			}
		}

		//RaycastHit hitLower45;
		//Debug.DrawRay(stepRayLower.position, (flatVelocity + transform.TransformDirection(1.5f, 0, 1)).normalized * (capsule.radius + .2f), Color.red);
		//if (Physics.Raycast(stepRayLower.position, flatVelocity + transform.TransformDirection(1.5f, 0, 1), out hitLower45, capsule.radius + 0.2f))
		//{
		//	float normalAngle = Vector3.Angle(hitLower.normal, transform.up);
		//	//if (Mathf.Approximately(normalAngle, 90f))
		//	if (normalAngle > slopeLimit && normalAngle < 91f)
		//	{
		//		Debug.Log(normalAngle);
		//		RaycastHit hitUpper45;
		//		Debug.DrawRay(stepRayUpper.position, ((flatVelocity + transform.TransformDirection(1.5f, 0, 1)).normalized) * (hitLower.distance + .01f), Color.red);
		//		if (!Physics.Raycast(stepRayUpper.position, flatVelocity + transform.TransformDirection(1.5f, 0, 1), out hitUpper45, hitLower45.distance + .01f))
		//		{
		//			rb.position += new Vector3(0f, stepSmooth * currentSpeed * Time.deltaTime, 0f);
		//			isGrounded = true;
		//		}
		//	}
		//}

		//RaycastHit hitLowerMinus45;
		//Debug.DrawRay(stepRayLower.position, (flatVelocity + transform.TransformDirection(-1.5f, 0, 1)).normalized * (capsule.radius + .2f), Color.red);
		//if (Physics.Raycast(stepRayLower.position, flatVelocity + transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, capsule.radius + 0.2f))
		//{
		//	float normalAngle = Vector3.Angle(hitLower.normal, transform.up);
		//	//if (Mathf.Approximately(normalAngle, 90f))
		//	if (normalAngle > slopeLimit && normalAngle < 91f)
		//	{
		//		Debug.Log(normalAngle);
		//		RaycastHit hitUpperMinus45;
		//		Debug.DrawRay(stepRayUpper.position, ((flatVelocity + transform.TransformDirection(-1.5f, 0, 1)).normalized) * (hitLower.distance + .01f), Color.red);
		//		if (!Physics.Raycast(stepRayUpper.position, flatVelocity + transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, hitLowerMinus45.distance + .01f))
		//		{
		//			rb.position += new Vector3(0f, stepSmooth * currentSpeed * Time.deltaTime, 0f);
		//			isGrounded = true;
		//		}
		//	}
		//}
	}

	//void StepClimb()
	//{
	//	RaycastHit hitLower;
	//	if (Physics.Raycast(stepRayLower.position, transform.TransformDirection(Vector3.forward), out hitLower, capsule.radius + 0.01f))
	//	{
	//		float normalAngle = Vector3.Angle(hitLower.normal, transform.up);
	//		if (normalAngle == 90f)
	//		{
	//			Debug.Log(normalAngle);
	//			RaycastHit hitUpper;
	//			if (!Physics.Raycast(stepRayUpper.position, transform.TransformDirection(Vector3.forward), out hitUpper, capsule.radius + 0.2f))
	//			{
	//				rb.position += new Vector3(0f, stepSmooth * currentSpeed * Time.deltaTime, 0f);
	//				isGrounded = true;
	//			} 
	//		}
	//	}

	//	RaycastHit hitLower45;
	//	if (Physics.Raycast(stepRayLower.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, capsule.radius + 0.2f))
	//	{
	//		float normalAngle = Vector3.Angle(hitLower.normal, transform.up);
	//		if (normalAngle == 90f)
	//		{
	//			Debug.Log(normalAngle);
	//			RaycastHit hitUpper45;
	//			if (!Physics.Raycast(stepRayUpper.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, capsule.radius + 0.2f))
	//			{
	//				rb.position += new Vector3(0f, stepSmooth * currentSpeed * Time.deltaTime, 0f);
	//			}
	//		}
	//	}

	//	RaycastHit hitLowerMinus45;
	//	if (Physics.Raycast(stepRayLower.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, capsule.radius + 0.2f))
	//	{
	//		float normalAngle = Vector3.Angle(hitLower.normal, transform.up);
	//		if (normalAngle == 90f)
	//		{
	//			Debug.Log(normalAngle);
	//			RaycastHit hitUpperMinus45;
	//			if (!Physics.Raycast(stepRayUpper.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, capsule.radius + 0.2f))
	//			{
	//				rb.position += new Vector3(0f, stepSmooth * currentSpeed * Time.deltaTime, 0f);
	//			}
	//		}
	//	}
	//}
}
