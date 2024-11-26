using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarATK : MonoBehaviour
{
    [Header("���⼭ ���� �� �������� �ڽĿ�����Ʈ���� �����ϼ�")]
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

	// N�ʰ� ������ ������Ʈ�� ��Ȱ��ȭ�ϴ� �Լ�
	void DestroyAfterTime()
	{
        if (rigid != null)
        {
			rigid.velocity = Vector2.zero;
			gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
    {
        // �ε��� ���� �÷��̾�� ���� �ƴϸ� �ǳʶٱ�
        if (!collision.CompareTag("Player") && !collision.CompareTag("Wall") || per == -1)
            return;

        per--;

        if (per == -1)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
