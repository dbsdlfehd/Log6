using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarATK : MonoBehaviour
{
    public int damage;
    public int per;
    public float speed = 1f;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
	}

    public void Init(int damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;
        if(rigid != null)
        {
			if (per > -1)
			{
				rigid.velocity = dir * speed; //공격이 쫓아오는 속도
			}
		}
		

	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || per == -1)
            return;

        per--;

        if (per == -1)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
