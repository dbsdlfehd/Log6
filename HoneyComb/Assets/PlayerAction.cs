using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
	public float speed;

	Rigidbody2D rigid;
	float hweang;
	float jong;

	void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();	
	}

	void Update()
	{
		hweang = Input.GetAxisRaw("Horizontal");
		jong = Input.GetAxisRaw("Vertical");
	}

	void FixedUpdate()
	{
		rigid.velocity = new Vector2 (hweang, jong) * speed;
	}
}
