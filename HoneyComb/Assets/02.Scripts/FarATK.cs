using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarATK : MonoBehaviour
{
    [Header("이것들 안됨, 메모장 보셈;;")]
    public int damage;
    public int per;

    [Header("이건 됨, 탄속")]
    public float speed = 1f;

    [Header("이건 됨, 몇 초후 사라짐")]
    public float disappearTime = 5f;

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

		// 10초 후에 오브젝트를 비활성화
		Invoke("DestroyAfterTime", disappearTime);
	}

	// 10초가 지나면 오브젝트를 비활성화하는 함수
	void DestroyAfterTime()
	{
		rigid.velocity = Vector2.zero;
		gameObject.SetActive(false);
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
