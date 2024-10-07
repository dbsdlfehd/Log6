using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Tilemaps;

public class Enemy : MonoBehaviour
{
    [Header("속도")]
    public float speed;
	private Rigidbody2D rb;//중력
	private Player player;//플레이어
	private PlayerAction playerAction;

    private Rigidbody2D target;
    public int SenserRangeX = 3;
    public int SenserRangeY = 3;
    //private PlayerAction playerAction;

    [Header("체력")]
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
		// Scene에서 Player 태그를 가진 오브젝트를 찾습니다.
		player = FindObjectOfType<Player>();
		SetEnemyStatus(100);
	}

	void Awake()
    {
		rigid = GetComponent<Rigidbody2D>();
        target = GetComponent<Rigidbody2D>();
		spriter = GetComponent<SpriteRenderer>();
		// PlayerAction 컴포넌트를 찾아 변수에 저장
		//playerAction = FindObjectOfType<PlayerAction>();
        scanner = GetComponent<Scanner>(); //근거리 공격용 스캔
        scanner2 = GetComponent<Scanner2>(); //원거리 공격용 스캔
		playerAction = FindObjectOfType<Player>().GetComponent<PlayerAction>();
	}

	void FixedUpdate()
    {
		if (playerAction.attacked)
		{
			Debug.Log("공격했다.");
		}
		Vector2 direction = player.transform.position - transform.position;
		//rb.velocity = direction.normalized * speed;

		int X = Math.Abs(Mathf.RoundToInt(direction.x));
		int Y = Math.Abs(Mathf.RoundToInt(direction.y));

		if (X <= SenserRangeX && Y <= SenserRangeY)//가까이 있을때
		{
			//따라간다.
			Vector2 nextVec = direction.normalized * speed * Time.fixedDeltaTime;
			rigid.MovePosition(rigid.position + nextVec);
			rigid.velocity = Vector2.zero;

			//Debug.Log($"{direction.x}");
			if(direction.x > 0)
			{
				spriter.flipX = false;
				Debug.Log("오른쪽");
			}
			else if(direction.x < 0)
			{
				Debug.Log("왼쪽");
				spriter.flipX = true;
			}
		}

		

		//      //체력 UI 표시
		//      HP_UI.text = "현재 적 체력 : " + nowHp;

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
			//Debug.Log($"적이 플레이어에게 공격을 받음 현재 적 체력 {nowHp}");
		}
        if(nowHp <= 0)//적 사망
        {
            nowHp = 0;
			//HP_UI.text = "현재 적 체력 : " + nowHp;
			//Debug.Log($"적이 사망하였습니다.");
			Destroy(gameObject);
        }
	}
}
