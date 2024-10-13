using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarATK : MonoBehaviour
{
    [Header("�̰͵� �ȵ�, �޸��� ����;;")]
    public int damage;
    public int per;

    [Header("�̰� ��, ź��")]
    public float speed = 1f;

    [Header("�̰� ��, �� ���� �����")]
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
				rigid.velocity = dir * speed; //������ �Ѿƿ��� �ӵ�
			}
		}

		// 10�� �Ŀ� ������Ʈ�� ��Ȱ��ȭ
		Invoke("DestroyAfterTime", disappearTime);
	}

	// 10�ʰ� ������ ������Ʈ�� ��Ȱ��ȭ�ϴ� �Լ�
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
