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
    public int SenserRangeX = 3;
    public int SenserRangeY = 3;
    //private PlayerAction playerAction;

    [Header("ü��")]
    public int maxHp;
    public int nowHp;
    //public TextMeshProUGUI HP_UI;

    public Scanner scanner;
    public Scanner2 scanner2;

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
		// PlayerAction ������Ʈ�� ã�� ������ ����
		//playerAction = FindObjectOfType<PlayerAction>();
        scanner = GetComponent<Scanner>(); //�ٰŸ� ���ݿ� ��ĵ
        scanner2 = GetComponent<Scanner2>(); //���Ÿ� ���ݿ� ��ĵ
		playerAction = FindObjectOfType<Player>().GetComponent<PlayerAction>();
	}

	void FixedUpdate()
    {
		
		Vector2 direction = player.transform.position - transform.position;
		//rb.velocity = direction.normalized * speed;

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

		

		//      //ü�� UI ǥ��
		//      HP_UI.text = "���� �� ü�� : " + nowHp;

	}
    
    void LateUpdate()
    {
        //spriter.flipX = target.position.x < rigid.position.x;
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && playerAction.attacked)
		{
            nowHp = nowHp - player.Atk;
			//Debug.Log($"���� �÷��̾�� ������ ���� ���� �� ü�� {nowHp}");
		}
        if(nowHp <= 0)//�� ���
        {
            nowHp = 0;
			//HP_UI.text = "���� �� ü�� : " + nowHp;
			//Debug.Log($"���� ����Ͽ����ϴ�.");
			Destroy(gameObject);
        }
	}
}
