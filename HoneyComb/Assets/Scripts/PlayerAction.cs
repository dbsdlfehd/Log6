using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
	public float speed;

	SpriteRenderer sp;
	Rigidbody2D rigid;
	Animator animator;
	float garo;
	float sero;

	void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sp = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		garo = Input.GetAxisRaw("Horizontal");
		sero = Input.GetAxisRaw("Vertical");
        if (garo < 0)
		{
			sp.flipX = true;
			animator.SetTrigger("run");
        }
		else if(garo > 0)
        {
			sp.flipX = false;
			animator.SetTrigger("run");
		}
		else if (sero != 0)
        {
			animator.SetTrigger("run");
		}
	}

	void FixedUpdate()
	{
		rigid.velocity = new Vector2 (garo, sero) * speed;
	}
}
