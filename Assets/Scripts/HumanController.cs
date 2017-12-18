using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class HumanController : MonoBehaviour {
	private float m_Speed = 2.0f;
	private Animator m_Animator;
	private Rigidbody m_Rigidbody;
	private float m_JumpPower = 6f;
	private float m_GroundCheckDistance = 0.2f;
	private Vector3 m_GroundNormal;
	[SerializeField] private bool m_IsGrounded;
	private float m_RunCycleLegOffset = 0.2f;

	// Use this for initialization
	void Start () 
	{
		m_Animator = GetComponent<Animator>();
		m_Rigidbody = GetComponent<Rigidbody>();
		
		m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		float v = Input.GetAxis("Vertical");
		float h = Input.GetAxis("Horizontal");
		bool jump = Input.GetButton("Jump");

		CheckGroundStatus();

		if (m_IsGrounded && jump)
		{
			m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
		}
		UpdateAnimator(v, h);
	}

	void UpdateAnimator(float v, float h)
	{
		m_Animator.SetFloat("Forward", v, 0.1f, Time.deltaTime);
		m_Animator.SetFloat("Turn", h, 0.1f, Time.deltaTime);
		m_Animator.SetBool("IsGrounded", m_IsGrounded);
		if (!m_IsGrounded)
		{
			m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
		}

		// calculate which leg is behind, so as to leave that leg trailing in the jump animation
		// (This code is reliant on the specific run cycle offset in our animations,
		// and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
		float runCycle =
			Mathf.Repeat(
				m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
		float jumpLeg = (runCycle < 0.5f ? 1 : -1) * v;
		if (m_IsGrounded)
		{
			m_Animator.SetFloat("JumpLeg", jumpLeg);
		}
	}

	void CheckGroundStatus()
	{
		RaycastHit hitInfo;
#if UNITY_EDITOR
		// helper to visualise the ground check ray in the scene view
		Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
		// 0.1f is a small offset to start the ray from inside the character
		// it is also good to note that the transform position in the sample assets is at the base of the character
		
		if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
		{
			m_GroundNormal = hitInfo.normal;
			m_IsGrounded = true;
			m_Animator.applyRootMotion = true;
		}
		else
		{
			m_IsGrounded = false;
			m_GroundNormal = Vector3.up;
			m_Animator.applyRootMotion = false;
		}
	}
}
