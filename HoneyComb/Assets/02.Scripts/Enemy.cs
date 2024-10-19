using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [Header("속도")]
    public float speed;
	private Rigidbody2D rb;//중력
	private Player player;//플레이어
	private PlayerAction playerAction;

    private Rigidbody2D target;

	[Header("감지 범위")]
    public int SenserRangeX = 3;
    public int SenserRangeY = 3;
    //private PlayerAction playerAction;

    [Header("체력")]
    public int maxHP;
    public int nowHP;
	//public TextMeshProUGUI HP_UI;

	[Header("이건 나도 몰루")]
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
		// Scene에서 Player 태그를 가진 오브젝트를 찾습니다.
		player = FindObjectOfType<Player>();//무조건 해줘야됨 (초기화)
		SetEnemyStatus(100);
	}

	void Awake()
    {
		rigid = GetComponent<Rigidbody2D>();
        target = GetComponent<Rigidbody2D>();
		spriter = GetComponent<SpriteRenderer>();
        scanner = GetComponent<Scanner>(); //근거리 공격용 스캔
        scanner2 = GetComponent<Scanner2>(); //원거리 공격용 스캔
		playerAction = FindObjectOfType<Player>().GetComponent<PlayerAction>();
	}

	void FixedUpdate()
    {
		
		Vector2 direction = player.transform.position - transform.position;

		int X = Math.Abs(Mathf.RoundToInt(direction.x));
		int Y = Math.Abs(Mathf.RoundToInt(direction.y));

		if (X <= SenserRangeX && Y <= SenserRangeY)//가까이 있을때
		{
			//따라간다.
			Vector2 nextVec = direction.normalized * speed * Time.fixedDeltaTime;
			rigid.MovePosition(rigid.position + nextVec);
			rigid.velocity = Vector2.zero;

			//왼쪽 오른쪽 플립해주는 것
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
			//Debug.Log("현재 적 체력" + nowHP);
		}
        if(nowHP <= 0)//적 사망
        {
            nowHP = 0;
			Destroy(gameObject);
        }
	}
}
