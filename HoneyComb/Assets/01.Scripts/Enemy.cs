using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;
    public int SenserRangeX = 3;
    public int SenserRangeY = 3;
    public PlayerAction playerAction;
    public int maxHp;
    public int nowHp;

    bool isLive;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    private void SetEnemyStatus(int _maxHP)
    {
        maxHp = _maxHP;
        nowHp = _maxHP;
    }

	void Start()
	{
        SetEnemyStatus(100);
	}

	void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
		// PlayerAction ������Ʈ�� ã�� ������ ����
		playerAction = FindObjectOfType<PlayerAction>();
	}

    void FixedUpdate()
    {
        Vector2 dirVec = target.position - rigid.position;
        //Debug.Log($"�÷��̾�� ������ �Ÿ� : {dirVec.x.ToString("F0")}, {dirVec.y.ToString("F0")}");


        //player��ǥ - npc��ǥ
        int X = Math.Abs(Mathf.RoundToInt(dirVec.x));
        int Y = Math.Abs(Mathf.RoundToInt(dirVec.y));

        //������ ������
        if (X <= SenserRangeX && Y <= SenserRangeY)
        {
            //���󰣴�.
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero;
        }
    }
    
    void LateUpdate()
    {
        spriter.flipX = target.position.x < rigid.position.x;
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && playerAction.attacked)
		{
            nowHp = nowHp - 10;
			Debug.Log($"���� �÷��̾�� ������ ���� ���� �� ü�� {nowHp}");
		}
        if(nowHp < 0)//�� ���
        {
			Debug.Log($"���� ����Ͽ����ϴ�.");
			Destroy(gameObject);
        }
	}
}
