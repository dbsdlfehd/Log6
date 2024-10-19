using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAction : MonoBehaviour
{
	[Header("속도")]
	public float speed;

	[Header("실선 추적기")]
	public float Length = 0.7f;

	[Header("공격 범위")]
	public BoxCollider2D left; // 왼쪽 콜라이더
	public BoxCollider2D right; // 오른쪽 콜라이더

	SpriteRenderer sp;
	Rigidbody2D rigid;
	Animator animator;
	float garo;
	float sero;
	GameObject scanObject;

	Vector3 dirVec;

	public bool attacked; // 공격 중인지 확인하는 플래그

	void AttackTrue()
	{
		attacked = true;
	}
	void AttackFalse()
	{
		attacked = false;
	}

	void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		sp = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		garo = Input.GetAxisRaw("Horizontal");
		sero = Input.GetAxisRaw("Vertical");

		// 플레이어의 방향과 애니메이션 업데이트
		if (garo < 0) // 왼쪽
		{
			sp.flipX = true;
			animator.SetTrigger("run");
		}
		else if (garo > 0) // 오른쪽
		{
			sp.flipX = false;
			animator.SetTrigger("run");
		}
		else if (sero != 0)
		{
			animator.SetTrigger("run");
		}

		// 방향 변수 설정
		if (garo < 0) dirVec = Vector3.left; // 왼쪽
		else if (garo > 0) dirVec = Vector3.right; // 오른쪽

		// 오브젝트 스캔
		if (Input.GetButtonDown("Jump") && scanObject != null)
		{
			Debug.Log("이것은: " + scanObject.name);
		}

		// 공격 입력
		if (Input.GetKeyDown(KeyCode.Space) && !attacked)
		{
			Attack();
		}
	}

	void Attack()
	{
		AttackTrue(); // 공격 중 플래그 설정

		// 방향에 따라 적절한 BoxCollider2D 활성화
		if (sp.flipX)
		{
			left.enabled = true;
			StartCoroutine(DisableCollider(left));
		}
		else
		{
			right.enabled = true;
			StartCoroutine(DisableCollider(right));
		}
	}

	IEnumerator DisableCollider(BoxCollider2D collider)
	{
		yield return new WaitForSeconds(0.1f); // 콜라이더를 0.1초 동안 활성화
		collider.enabled = false;
		AttackFalse(); // 공격 중 플래그 리셋
	}

	void FixedUpdate()
	{
		rigid.velocity = new Vector2(garo, sero) * speed;

		// 플레이어 주위의 얇은 선으로 감지
		Debug.DrawRay(rigid.position, dirVec * Length, new Color(0, 1, 0));
		RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, Length, LayerMask.GetMask("Enemy"));

		// 무언가 감지됨!
		if (rayHit.collider != null) scanObject = rayHit.collider.gameObject;
		else scanObject = null;
	}
}