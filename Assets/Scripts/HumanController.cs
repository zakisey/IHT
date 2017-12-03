using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour {
	private float m_Speed = 2.0f;
	private Animator m_Animator;
	
	// Use this for initialization
	void Start () 
	{
		m_Animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float v = Input.GetAxis("Vertical");
		float h = Input.GetAxis("Horizontal");
		float translation = v * m_Speed * Time.deltaTime;
		float rotation = h * 75.0f * Time.deltaTime;

		m_Animator.SetBool("IsWalking", v != 0.0f || h != 0.0f);


		transform.Translate(0, 0, translation);
		transform.Rotate(0, rotation, 0);
	}
}
