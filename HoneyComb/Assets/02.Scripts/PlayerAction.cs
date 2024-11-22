using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    public Player playerScript;

    public float walkSpeed = 5f;              // 걷기 속도
    public float defaultSpeed = 5f;           // 기본 속도(걷기 속도와 동일해야 함)
    public float slideSpeed = 10f;            // 슬라이드 속도
    public float slideDuration = 0.3f;        // 슬라이드 지속 시간 (0.3초)
    public float slideCooldown = 1f;          // 슬라이드 후 쿨타임 (1초)
    private float slideTime = 0f;             // 남은 슬라이드 시간
    private float cooldownTime = 0f;          // 남은 쿨타임 시간
    public float skillAttackCooldown = 2f;    // 첫 번째 스킬 쿨타임 2초
    public float skillAttack2Cooldown = 3f;   // 두 번째 스킬 쿨타임 3초
    private float skillAttackTime = 0f;       // 첫 번째 스킬 사용 가능 시간
    private float skillAttack2Time = 0f;      // 두 번째 스킬 사용 가능 시간

    public Collider2D playerCollider2D;

    Vector2 moveInput;  // 플레이어의 입력을 저장하는 변수
    Vector3 dirVec;     // 플레이어의 방향을 저장하는 변수

    [Header("실선 추적기")]
    public float Length = 0.7f;  // 감지할 거리

    [Header("공격 범위")]
    public BoxCollider2D left;   // 왼쪽 공격 범위 콜라이더
    public BoxCollider2D right;  // 오른쪽 공격 범위 콜라이더

    [Header("대화창")]
    //public TextMeshProUGUI Dialog_UI_text;
    //private string dialog_text;
    public TalkManager talkManager;  // 대화 매니저 참조
    public GameObject DialogSet;     // 대화창

    Rigidbody2D rigid;               // Rigidbody2D 컴포넌트 참조
    Animator animator;               // Animator 컴포넌트 참조
    SpriteRenderer sp;               // SpriteRenderer 컴포넌트 참조

    private GameObject scanObject;   // 감지된 오브젝트

    private Collider2D playerCollider; // 플레이어의 콜라이더

	public Transform player;

    [Header("아이템")]
    public Collider2D myItemCollider; // 아이템의 콜라이더를 연결

    public Camera mainCamera; // 메인 카메라 (화면 좌표 변환용)

    // 이동 상태 확인 변수 (애니메이터와 연동)
    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);  // 애니메이션 상태 변경
        }
    }
	// 슬라이딩 상태 확인 변수 (애니메이터와 연동)
	[SerializeField]
    private bool _isSliding = false;
    public bool IsSliding
    {
        get
        {
            return _isSliding;
        }
        private set
        {
            _isSliding = value;
            animator.SetBool(AnimationStrings.isSliding, value);  // 애니메이션 상태 변경
        }
    }
    private bool isCooldown = false;  // 슬라이드 쿨타임 중인지 여부
    public bool _isFacingRight = true;// 캐릭터가 오른쪽을 보고 있는지 확인하는 변수


	public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1); // 캐릭터의 방향을 뒤집음
            }

            _isFacingRight = value;
        }
    }

    // canMove: 플레이어가 이동할 수 있는지 여부를 나타냄
    public bool canMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    void Awake()
    {
        // 필요한 컴포넌트들을 가져옴
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>(); // 플레이어의 콜라이더 가져오기
    }

    void Update()
    {
        if (isCooldown)
        {
            cooldownTime -= Time.deltaTime;
            if (cooldownTime <= 0)
            {
                isCooldown = false;  // 쿨타임 종료
            }
        }

        if (IsSliding)
        {
            slideTime -= Time.deltaTime;
            if (slideTime <= 0)
            {
                StopSliding();  // 슬라이드가 끝나면 슬라이딩 종료
            }
        }

        // 스킬 쿨타임 감소
        if (skillAttackTime > 0)
        {
            skillAttackTime -= Time.deltaTime;
        }

        if (skillAttack2Time > 0)
        {
            skillAttack2Time -= Time.deltaTime;
        }

    }

    // 대화창 E키
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (scanObject != null)// 대상을 찾았을 때의 대사
			{
				talkManager.DialogAction(scanObject);
			}
        }
    }

    public float tempTime = 1.0f; // 시간 텀
	IEnumerator DisableCollider(BoxCollider2D collider, int AtkStyle)// 콜라이더를 비활성화하는 코루틴
	{
		if (AtkStyle == 0 && Time.time > tempTime) // 기본공격
        {
			animator.SetTrigger(AnimationStrings.attackTrigger);  // 공격 애니메이션 실행
			tempTime = Time.time + 0.15f;
			collider.enabled = true; // 왼쪽 또는 오른쪽 공격 켜기
			yield return new WaitForSeconds(0.1f); // 콜라이더를 0.1초 동안 활성화
			collider.enabled = false;  // 왼쪽 또는 오른쪽 공격 끄기
		}
        else if(AtkStyle == 1) // 스킬 1
        {
			yield return new WaitForSeconds(0.6f);
			collider.enabled = true;
			yield return new WaitForSeconds(0.1f); // 콜라이더를 0.1초 동안 활성화
			collider.enabled = false;
			playerScript.Atk /= 2;
		}
		else if (AtkStyle == 2) // 스킬 2 (궁극기)
		{
			yield return new WaitForSeconds(0.9f);
			collider.enabled = true;
			yield return new WaitForSeconds(0.1f); // 콜라이더를 0.1초 동안 활성화
			collider.enabled = false;
			playerScript.Atk /= 4;
		}
	}

    void FixedUpdate()
	{
        // 물리 업데이트 처리 (이동 및 감지)
        //canMove는 true,
        //isDialoging(대화창 열린 여부)가 false 일때 움직일수 있음
        if (canMove && talkManager.isDialoging == false)
        {
            rigid.velocity = new Vector2(moveInput.x * walkSpeed, moveInput.y * walkSpeed);  // 이동 처리
        }
        else
        {
            rigid.velocity = new Vector2(0, 0);  // 이동을 막을 때 속도를 0으로 설정
        }

        // 플레이어 주위의 얇은 선으로 감지
        Debug.DrawRay(rigid.position, dirVec * Length, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, Length, LayerMask.GetMask("Object"));

        // 무언가 감지됨!
        if (rayHit.collider != null) scanObject = rayHit.collider.gameObject;
        else scanObject = null;

        // 왼쪽 화살표 = -1, 오른쪽 화살표 = 1
        // Debug.Log(moveInput.x);
    }

    public void OnMove(InputAction.CallbackContext context)// 이동 입력 처리
	{
        if (canMove)
        {
            moveInput = context.ReadValue<Vector2>();

            IsMoving = moveInput != Vector2.zero;  // 움직임 여부 확인

            SetFacingDirection(moveInput);  // 방향 설정
        }
    }

    void SetFacingDirection(Vector2 moveInput)// 이동 방향 설정
	{
        //x값이 0보다 큼 && 오른쪽 안바라봄 && 대화가 끝남
        if (moveInput.x > 0 && talkManager.isDialoging == false)
        {
            //IsFacingRight = true;
            dirVec = Vector3.right;  // 오른쪽 방향 설정
            sp.flipX = false; // 캐릭터를 오른쪽으로 바라보게 설정
        }
		//x값이 0보다 큼 && 오른쪽 바라봄 && 대화가 끝남
		else if (moveInput.x < 0 && talkManager.isDialoging == false)
        {
            //IsFacingRight = false;
            dirVec = Vector3.left;  // 왼쪽 방향 설정
            sp.flipX = true; // 캐릭터를 왼쪽으로 바라보게 설정
        }
    }

    public void OnSlide(InputAction.CallbackContext context)// 슬라이딩 입력 처리
	{
        if (context.started && !isCooldown && !IsSliding )
        {
            if (_isMoving)
            {
				StartSliding();  // 슬라이딩 시작
			}
        }
        else if (context.canceled && IsSliding)
        {
            StopSliding();  // 슬라이딩 종료
        }
    }

    private void StartSliding()// 슬라이딩 시작
	{
        IsSliding = true;
        slideTime = slideDuration;
        walkSpeed = slideSpeed;  // 슬라이드 속도 적용
        
	}

    private void StopSliding()// 슬라이딩 종료
	{
        IsSliding = false;
        walkSpeed = defaultSpeed;  // 기본 속도로 복귀
        isCooldown = true;
        cooldownTime = slideCooldown;  // 슬라이드 쿨타임 적용
    }

    // 마우스 좌클릭 (잽)
    public void OnAttack(InputAction.CallbackContext context)
	{
        if (context.started)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();           // 마우스 위치 가져오기
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition); // 화면 좌표 -> 월드 좌표 변환
            float playerPositionX = transform.position.x;                         // 플레이어의 현재 x 좌표
            worldPosition.z = 0f; // 카메라의 z값을 0으로 설정 (2D 공간에서의 좌표)

            int atkStyle = 0; // 기본 공격 스타일

            // 클릭한 위치를 기준으로 캐릭터 방향 전환 및 콜라이더 활성화
            if (worldPosition.x < playerPositionX)
            {
                sp.flipX = true; // 캐릭터를 왼쪽으로 바라보게 설정
                StartCoroutine(DisableCollider(left, atkStyle)); // 왼쪽 공격 활성화
            }
            else
            {
                sp.flipX = false; // 캐릭터를 오른쪽으로 바라보게 설정
                StartCoroutine(DisableCollider(right, atkStyle)); // 오른쪽 공격 활성화
            }

            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);  // Cast a ray at the click position

            if (hit.collider != null)
            {
                // Check if the object has a Teleport component
                Teleport teleport = hit.collider.GetComponent<Teleport>();
                if (teleport != null)
                {
                    teleport.MovePlayer(playerCollider2D); // Teleport the player
                }
            }


            // 아이템이 있는 콜라이더 객체 (예시로 myItemCollider를 사용)
            if (myItemCollider.bounds.Contains(worldPosition)) // 클릭한 위치가 아이템 콜라이더 범위 안에 있을 때
            {
                Teleport teleport = myItemCollider.GetComponent<Teleport>();
                if (teleport != null)
                {
                    teleport.MovePlayer(playerCollider2D); // 아이템의 OnItemClicked 함수 실행
                }

            }
        }
    }

    public void OnSkillAttack(InputAction.CallbackContext context)// 첫 번째 스킬 공격 입력 처리
	{
        if (context.started)  // 쿨타임이 0일 때만 스킬 발동
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue(); // 마우스 위치 가져오기
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition); // 화면 좌표 -> 월드 좌표 변환
            float playerPositionX = transform.position.x; // 플레이어의 현재 x 좌표

            if (skillAttackTime <= 0)
            {
                animator.SetTrigger(AnimationStrings.skillAttackTrigger);  // 스킬 애니메이션 실행
                skillAttackTime = skillAttackCooldown;  // 쿨타임 적용


				int atkStyle = 1; // 스킬 공격 스타일
				playerScript.Atk *= 2;

                if (worldPosition.x < playerPositionX)
                {
                    sp.flipX = true; // 캐릭터를 왼쪽으로 바라보게 설정
                    StartCoroutine(DisableCollider(left, atkStyle)); // 왼쪽 공격 활성화

                }
                else
                {
                    sp.flipX = false; // 캐릭터를 오른쪽으로 바라보게 설정
                    StartCoroutine(DisableCollider(right, atkStyle)); // 오른쪽 공격 활성화
                }
			}
		}
    }

    public void OnSkillAttack2(InputAction.CallbackContext context)// 두 번째 스킬 공격 입력 처리
	{
        if (context.started)  // 쿨타임이 0일 때만 스킬 발동
        {
            if (skillAttack2Time <= 0)
            {
                animator.SetTrigger(AnimationStrings.skillAttackTrigger2);  // 스킬 애니메이션 실행
                skillAttack2Time = skillAttack2Cooldown;  // 쿨타임 적용

				int atkStyle = 2;
				playerScript.Atk *= 4; // 플레이어 공격력 증가
									   // 방향에 따라 적절한 BoxCollider2D 활성화
				if (sp.flipX)
				{
					StartCoroutine(DisableCollider(left, atkStyle));  // 왼쪽 공격 활성화
				}
				else
				{
					StartCoroutine(DisableCollider(right, atkStyle));  // 오른쪽 공격 활성화
				}
			}
		}
    }
}
