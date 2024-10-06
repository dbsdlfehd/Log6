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
		// PlayerAction 컴포넌트를 찾아 변수에 저장
		playerAction = FindObjectOfType<PlayerAction>();
	}

    void FixedUpdate()
    {
        Vector2 dirVec = target.position - rigid.position;
        //Debug.Log($"플레이어와 몬스터의 거리 : {dirVec.x.ToString("F0")}, {dirVec.y.ToString("F0")}");


        //player좌표 - npc좌표
        int X = Math.Abs(Mathf.RoundToInt(dirVec.x));
        int Y = Math.Abs(Mathf.RoundToInt(dirVec.y));

        //가까이 있을때
        if (X <= SenserRangeX && Y <= SenserRangeY)
        {
            //따라간다.
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
			Debug.Log($"적이 플레이어에게 공격을 받음 현재 적 체력 {nowHp}");
		}
        if(nowHp < 0)//적 사망
        {
			Debug.Log($"적이 사망하였습니다.");
			Destroy(gameObject);
        }
	}
}
