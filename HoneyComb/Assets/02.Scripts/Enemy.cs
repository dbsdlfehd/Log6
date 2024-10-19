using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [Header("�ӵ�")]
    public float speed;
	private Rigidbody2D rb;//�߷�
	private Player player;//�÷��̾�
	private PlayerAction playerAction;

    private Rigidbody2D target;

	[Header("���� ����")]
    public int SenserRangeX = 3;
    public int SenserRangeY = 3;
    //private PlayerAction playerAction;

    [Header("ü��")]
    public int maxHP;
    public int nowHP;
	//public TextMeshProUGUI HP_UI;

	[Header("�̰� ���� ����")]
    public Scanner scanner;
    public Scanner2 scanner2;

    bool isLive;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    private void SetEnemyStatus(int _maxHP)
    {
        maxHP = _maxHP;
        nowHP = _maxHP;
    }

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		// Scene���� Player �±׸� ���� ������Ʈ�� ã���ϴ�.
		player = FindObjectOfType<Player>();//������ ����ߵ� (�ʱ�ȭ)
		SetEnemyStatus(100);
	}

	void Awake()
    {
		rigid = GetComponent<Rigidbody2D>();
        target = GetComponent<Rigidbody2D>();
		spriter = GetComponent<SpriteRenderer>();
        scanner = GetComponent<Scanner>(); //�ٰŸ� ���ݿ� ��ĵ
        scanner2 = GetComponent<Scanner2>(); //���Ÿ� ���ݿ� ��ĵ
		playerAction = FindObjectOfType<Player>().GetComponent<PlayerAction>();
	}

	void FixedUpdate()
    {
		
		Vector2 direction = player.transform.position - transform.position;

		int X = Math.Abs(Mathf.RoundToInt(direction.x));
		int Y = Math.Abs(Mathf.RoundToInt(direction.y));

		if (X <= SenserRangeX && Y <= SenserRangeY)//������ ������
		{
			//���󰣴�.
			Vector2 nextVec = direction.normalized * speed * Time.fixedDeltaTime;
			rigid.MovePosition(rigid.position + nextVec);
			rigid.velocity = Vector2.zero;

			//���� ������ �ø����ִ� ��
			if(direction.x > 0)
			{
				spriter.flipX = false;
			}
			else if(direction.x < 0)
			{
				spriter.flipX = true;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
            nowHP = nowHP - player.Atk;
			//Debug.Log("���� �� ü��" + nowHP);
		}
        if(nowHP <= 0)//�� ���
        {
            nowHP = 0;
			Destroy(gameObject);
        }
	}
}
