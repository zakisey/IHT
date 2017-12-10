using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class HumanController : MonoBehaviour {
	private float m_Speed = 2.0f;
	private Animator m_Animator;
	private Rigidbody m_Rigidbody;
	
	// Use this for initialization
	void Start () 
	{
		m_Animator = GetComponent<Animator>();
		m_Rigidbody = GetComponent<Rigidbody>();
		m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float v = Input.GetAxis("Vertical");
		float h = Input.GetAxis("Horizontal");

		m_Animator.SetFloat("Forward", v);
		m_Animator.SetFloat("Turn", h);
	}
}
