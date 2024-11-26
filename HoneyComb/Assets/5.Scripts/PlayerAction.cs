using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;
using UnityEditor;
using Unity.VisualScripting;


public class PlayerAction : MonoBehaviour
{
    public Player playerScript;

    public float walkSpeed = 5f;              // �ȱ� �ӵ�
    public TextMeshProUGUI speed_ui;          // �̵��ӵ� �����ֱ� 
    public TextMeshProUGUI As_ui;             // ���ݼӵ� �����ֱ�

    public float defaultSpeed = 5f;           // �⺻ �ӵ�(�ȱ� �ӵ��� �����ؾ� ��)
    public float slideSpeed = 10f;            // �����̵� �ӵ�
    public float slideDuration = 0.3f;        // �����̵� ���� �ð� (0.3��)
    public float slideCooldown = 1f;          // �����̵� �� ��Ÿ�� (1��)
    private float slideTime = 0f;             // ���� �����̵� �ð�
    private float cooldownTime = 0f;          // ���� ��Ÿ�� �ð�
    public float skillAttackCooldown = 2f;    // ù ��° ��ų ��Ÿ�� 2��
    public float skillAttack2Cooldown = 3f;   // �� ��° ��ų ��Ÿ�� 3��
    private float skillAttackTime = 0f;       // ù ��° ��ų ��� ���� �ð�
    private float skillAttack2Time = 0f;      // �� ��° ��ų ��� ���� �ð�

    public Collider2D playerCollider2D;

    Vector2 moveInput;  // �÷��̾��� �Է��� �����ϴ� ����
    Vector3 dirVec;     // �÷��̾��� ������ �����ϴ� ����

    [Header("�Ǽ� ������")]
    public float Length = 0.7f;  // ������ �Ÿ�

    [Header("���� ����")]
    public BoxCollider2D left;      // ���� ���� ���� �ݶ��̴�
    public BoxCollider2D right;     // ������ ���� ���� �ݶ��̴�
    public BoxCollider2D LargeLeft; // �� ���� ������ ���� ���� ���� ���� �ݶ��̴�
    public BoxCollider2D LargeRight; // �� ���� ������ ���� ���� ���� ���� �ݶ��̴�

	[Header("��ȭâ")]
    //public TextMeshProUGUI Dialog_UI_text;
    //private string dialog_text;
    public TalkManager talkManager;  // ��ȭ �Ŵ��� ����
    public GameObject DialogSet;     // ��ȭâ

    [Header("�� ���� �ð�")]
    public float Jab;

	[Header("��ų ���� �ð�")]
	public float Skill;

	[Header("�ñر� ���� �ð�")]
	public float Ultimite;

	[Header("���� ������ ����")]
	public bool isAtking = false;

	Rigidbody2D rigid;               // Rigidbody2D ������Ʈ ����
    Animator animator;               // Animator ������Ʈ ����
    SpriteRenderer sp;               // SpriteRenderer ������Ʈ ����

    private GameObject scanObject;   // ������ ������Ʈ

    private GameObject scanTP_Object;  // ������ TP ������Ʈ

    private Collider2D playerCollider; // �÷��̾��� �ݶ��̴�

	public Transform player;

    [Header("������")]
    public Collider2D[] myItemColliders; // Ŭ���ؼ� �Ѿ�� ��

	[Header("���� ī�޶�")]
	public Camera mainCamera; // ���� ī�޶� (ȭ�� ��ǥ ��ȯ��)

    [Header("���� ���� ��ųor�ñر⸦ ��� ���ΰ�?")]
    public bool isUsingSkillorUltimate;


    public int originalLayerID = 6; // �⺻ ���̾� ID (Default�� 0)
    public int shiftedLayerID = 10; // Shift Ű�� ������ �� ������ ���̾� ID

    private SpriteRenderer spriteRenderer;
    private Coroutine revertLayerCoroutine;

    // �̵� ���� Ȯ�� ���� (�ִϸ����Ϳ� ����)
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
            animator.SetBool(AnimationStrings.isMoving, value);  // �ִϸ��̼� ���� ����
        }
    }
	// �����̵� ���� Ȯ�� ���� (�ִϸ����Ϳ� ����)
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
            animator.SetBool(AnimationStrings.isSliding, value);  // �ִϸ��̼� ���� ����
        }
    }
    private bool isCooldown = false;  // �����̵� ��Ÿ�� ������ ����
    public bool _isFacingRight = true;// ĳ���Ͱ� �������� ���� �ִ��� Ȯ���ϴ� ����

    public Transform playerPos;

    public ItemManager itemManager;     // ������ �Ŵ��� ��ũ��Ʈ

	


	public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1); // ĳ������ ������ ������
            }

            _isFacingRight = value;
        }
    }

    // canMove: �÷��̾ �̵��� �� �ִ��� ���θ� ��Ÿ��
    public bool canMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
        set
        {
			animator.SetBool(AnimationStrings.canMove, value); // �ִϸ����Ϳ� �����Ͽ� canMove ���� ����
		}
	}

    void Awake()
    {
        // �ʿ��� ������Ʈ���� ������
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>(); // �÷��̾��� �ݶ��̴� ��������
    }

    void Update()
    {

        if (isCooldown)
        {
            cooldownTime -= Time.deltaTime;
            if (cooldownTime <= 0)
            {
                isCooldown = false;  // ��Ÿ�� ����
            }
        }

        if (IsSliding)
        {
            slideTime -= Time.deltaTime;
            if (slideTime <= 0)
            {
                StopSliding();  // �����̵尡 ������ �����̵� ����
            }
        }

        // ��ų ��Ÿ�� ����
        if (skillAttackTime > 0)
        {
            skillAttackTime -= Time.deltaTime;
        }

        if (skillAttack2Time > 0)
        {
            skillAttack2Time -= Time.deltaTime;
        }
    }
    public bool RealStop = false; // ���ߴ� �� Update �Լ��� ����
	public void OnInteract(InputAction.CallbackContext context)// ��ȭâ EŰ
	{
        if (context.started)
        {
            // ��ȭ ��� ã�� ��
            if (scanObject != null)// ����� ã���� ���� ���
			{
                TicTocDealyTime = 5f;

				PleaseStopPlayer();
				talkManager.DialogAction(scanObject);
			}
        }
    }

    public bool timeStopu = false;

    public Transform tempPlayerPos;

    public float tempTime = 1.0f; // �ð� ��
	IEnumerator DisableCollider(BoxCollider2D collider, int AtkStyle)// �������� ����
	{
        isAtking = true;
		canMove = false;
		PleaseStopPlayer();
		if (AtkStyle == 0 && Time.time > tempTime) // �⺻����
        {
			animator.SetTrigger(AnimationStrings.attackTrigger);  // ���� �ִϸ��̼� ����
			tempTime = Time.time + 0.15f;
			collider.enabled = true;                              // ���� �Ǵ� ������ ���� �ѱ�
			yield return new WaitForSeconds(0.1f);                // �ݶ��̴��� 0.1�� ���� Ȱ��ȭ
			collider.enabled = false;                             // ���� �Ǵ� ������ ���� ����
		}
        else if(AtkStyle == 1) // ��ų
        {
			isUsingSkillorUltimate = true;              // ������ ��ų ����ϰ� �ִ�.
			yield return new WaitForSeconds(0.6f);      // �ִϸ��̼� Ÿ���� ������ ��ٸ��� ��
			playerScript.Atk *= playerScript.SkillAtk;  // ���ݷ� �ι� ����
			collider.enabled = true;                    // ���� �ݶ��̴� Ȱ��ȭ
			yield return new WaitForSeconds(0.1f);      // �ݶ��̴��� 0.1�� ���� Ȱ��ȭ
			collider.enabled = false;                   // ���� �ݶ��̴� ��Ȱ��ȭ
			playerScript.Atk /= playerScript.SkillAtk;  // ���ݷ� �ʱ�ȭ
			isUsingSkillorUltimate = false;             // ������ ��ų ����ϰ� ���� �ʴ�.
		}
		else if (AtkStyle == 2) // �ñر�
		{
            
			isUsingSkillorUltimate = true;                  // ������ ��ų ����ϰ� �ִ�.
			yield return new WaitForSeconds(0.9f);          // �ִϸ��̼� Ÿ���� ������ ��ٸ��� ��
			playerScript.Atk *= playerScript.UltimitAtk;    // �÷��̾� ���ݷ� ����
            itemManager.HammerBuff();                       // �ظ� ���ݷ� ��� (��, ������ �϶�)
			collider.enabled = true;                        // ���� �ݶ��̴� Ȱ��ȭ
			yield return new WaitForSeconds(0.1f);          // �ݶ��̴��� 0.1�� ���� Ȱ��ȭ
			collider.enabled = false;                       // ���� �ݶ��̴� ��Ȱ��ȭ
            itemManager.HammerDeBuff();                     // �ظ� ���ݷ� �϶� (��, ������ �϶�)
			playerScript.Atk /= playerScript.UltimitAtk;    // ���ݷ� �ʱ�ȭ
			isUsingSkillorUltimate = false;                 // ������ ��ų ����ϰ� ���� �ʴ�.
		}
		canMove = true;
        isAtking = false;
	}

    public float TicTocDealyTime;
    IEnumerator TicToc()
    {
        IsMoving = false;                       // �����̰� �ִ� ���� ����
        //canMove = false;                        // �����ϼ� �ִ� ���� ����
		//moveInput.x = 0;                        // input ���� 0 
		//moveInput.y = 0;
		playerPos = tempPlayerPos;
		timeStopu = true;
		yield return new WaitForSeconds(TicTocDealyTime);
        timeStopu = false;
        //canMove = true;
	}

	// �÷��̾� �̵�
	void FixedUpdate()
	{
        //canMove�� true,
        //isDialoging(��ȭâ ���� ����)�� false �϶� �����ϼ� ���� && Ÿ�ӽ���Ǫ�� false�϶���
        if (canMove == true && talkManager.isDialoging == false && timeStopu == false)
        {
			rigid.velocity = new Vector2(moveInput.x * walkSpeed, moveInput.y * walkSpeed);  // �̵� ó��
        }
        else if (IsMoving == false)
        {
            StartCoroutine(TicToc());
		}
		// ������ ������ ��� ����Ű �Է��� ������ �̵��ϵ��� ����
		if (canMove && !IsSliding && !isUsingSkillorUltimate)
		{
			IsMoving = moveInput != Vector2.zero;  // ��� �̵��� �� �ֵ��� ���� ������Ʈ
			SetFacingDirection(moveInput);  // �̵� ���� ����
		}
        if (IsMoving == true && moveInput.x == 0 && moveInput.y == 0)
        {
            TicToc();
        }

		// �÷��̾� ������ ���� ������ ����
		Debug.DrawRay(rigid.position, dirVec * Length, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, Length, LayerMask.GetMask("Object"));

        // ���� ������!
        if (rayHit.collider != null) scanObject = rayHit.collider.gameObject;
        else scanObject = null;

        // ���� ȭ��ǥ = -1, ������ ȭ��ǥ = 1
        // Debug.Log(moveInput.x);

        speed_ui.text = walkSpeed.ToString(); // �̵��ӵ�
        As_ui.text = (3/jabCooldown).ToString("F2");           // ���ݼӵ�
    }

    public void OnMove(InputAction.CallbackContext context)// �̵� �Է� ó��
	{
        if (canMove)
        {
            moveInput = context.ReadValue<Vector2>();

            IsMoving = moveInput != Vector2.zero;  // ������ ���� Ȯ��

            SetFacingDirection(moveInput);  // ���� ����
        }
    }

    

    void SetFacingDirection(Vector2 moveInput)// �̵� ���� ����
	{
        //x���� 0���� ŭ && ������ �ȹٶ� && ��ȭ�� ����
        if (moveInput.x > 0 && talkManager.isDialoging == false)
        {
            //IsFacingRight = true;
            dirVec = Vector3.right;  // ������ ���� ����
            sp.flipX = false; // ĳ���͸� ���������� �ٶ󺸰� ����
        }
		//x���� 0���� ŭ && ������ �ٶ� && ��ȭ�� ����
		else if (moveInput.x < 0 && talkManager.isDialoging == false)
        {
            //IsFacingRight = false;
            dirVec = Vector3.left;  // ���� ���� ����
            sp.flipX = true; // ĳ���͸� �������� �ٶ󺸰� ����
        }
    }

    public void OnSlide(InputAction.CallbackContext context) // �����̵� �Է� ó��
    {
        if (context.started && !isCooldown && !IsSliding)
        {
            if (_isMoving)
            {
                StartSliding(); // �����̵� ����
            }
        }
    }

    private void StartSliding() // �����̵� ����
    {
        IsSliding = true;
        slideTime = slideDuration; // �����̵� ���� �ð� ����
        walkSpeed = slideSpeed;    // �����̵� �ӵ� ����
        gameObject.layer = shiftedLayerID;
        // �����̵��� ������ �ڵ����� ���� ó��
        Invoke(nameof(StopSliding), slideDuration); // slideDuration��ŭ ��� �� StopSliding ȣ��
        revertLayerCoroutine = StartCoroutine(RevertLayerAfterDelay(0.5f));
    }

    private void StopSliding() // �����̵� ����
    {
        IsSliding = false;
        walkSpeed = defaultSpeed; // �⺻ �ӵ��� ����
        isCooldown = true;
        cooldownTime = slideCooldown; // �����̵� ��Ÿ�� ����

        // ��Ÿ�� Ÿ�̸Ӹ� ������Ʈ�ϴ� �ڷ�ƾ ���� ����� ���� ����
    }

    void SetLayer(int layerID)
    {
        gameObject.layer = layerID;
    }

    IEnumerator RevertLayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetLayer(originalLayerID);
    }


    public float jabCooldown = 0.3f;  // �� ���� ��Ÿ�� (������ �ǹ�)
	private float lastAttackTime = 0f;  // ������ ���� �ð��� ����� ����


	public void OnAttack(InputAction.CallbackContext context)// �Ϲ� ���� (��)
	{
        if (jabCooldown <= 0.2f)
        {
            jabCooldown = 0.2f;
        }
		// ���콺 Ŭ���� ���� �Ҷ� && ��ų�̳� �ñر⸦ ����ϰ� ���� ������ ���� ����
		if (context.started && isUsingSkillorUltimate == false && isAtking == false && Time.time >= lastAttackTime + jabCooldown)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();           // ���콺 ��ġ ��������
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition); // ȭ�� ��ǥ -> ���� ��ǥ ��ȯ
            float playerPositionX = transform.position.x;                         // �÷��̾��� ���� x ��ǥ
            worldPosition.z = 0f; // ī�޶��� z���� 0���� ���� (2D ���������� ��ǥ)

            int atkStyle = 0; // �⺻ ���� ��Ÿ��

			TicTocDealyTime = Jab;                               // ���ʵ��� �����Ǿ� ������?

			// ȭ�� �������� ���� Ŭ�� ��
			if (worldPosition.x < playerPositionX)
			{
                sp.flipX = true; // ĳ���͸� �������� �ٶ󺸰� ����
                StartCoroutine(DisableCollider(left, atkStyle)); // ���� ���� Ȱ��ȭ
            }
			// ȭ�� �������� ������ Ŭ�� ��
			else
			{
                sp.flipX = false; // ĳ���͸� ���������� �ٶ󺸰� ����
                StartCoroutine(DisableCollider(right, atkStyle)); // ������ ���� Ȱ��ȭ
            }

			lastAttackTime = Time.time;  // ������ ���� �ð� ����


			// �������� �ִ� �ݶ��̴� ��ü (���÷� myItemCollider�� ���)

			foreach (var item in myItemColliders)
            {
				if (item.bounds.Contains(worldPosition)) // Ŭ���� ��ġ�� ������ �ݶ��̴� ���� �ȿ� ���� ��
				{
					teleport teleport = item.GetComponent<teleport>();
					SceneChangeOnCollision sceneChangeOnCollision = item.GetComponent<SceneChangeOnCollision>();
					if (teleport != null)
					{
						teleport.MovePlayer(playerCollider2D); // �������� OnItemClicked �Լ� ����
					}
                    else if (teleport == null)
                    {
                        sceneChangeOnCollision.SceneChangeHamSu();
					}
				}
			}
        }
    }

    public void OnSkillAttack(InputAction.CallbackContext context)// ��ų
	{
		if (context.started)  // ��Ÿ���� 0�� ���� ��ų �ߵ�
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue(); // ���콺 ��ġ ��������
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition); // ȭ�� ��ǥ -> ���� ��ǥ ��ȯ
            float playerPositionX = transform.position.x; // �÷��̾��� ���� x ��ǥ

            if (skillAttackTime <= 0)
            {
                animator.SetTrigger(AnimationStrings.skillAttackTrigger);  // ��ų �ִϸ��̼� ����
                skillAttackTime = skillAttackCooldown;  // ��Ÿ�� ����


				int atkStyle = 1; // ��ų ���� ��Ÿ��

				TicTocDealyTime = Skill;                     // ���ʵ��� �����Ǿ� ������?

				if (worldPosition.x < playerPositionX)
                {
                    sp.flipX = true; // ĳ���͸� �������� �ٶ󺸰� ����
                    StartCoroutine(DisableCollider(LargeLeft, atkStyle)); // ���� ���� Ȱ��ȭ

                }
                else
                {
                    sp.flipX = false; // ĳ���͸� ���������� �ٶ󺸰� ����
                    StartCoroutine(DisableCollider(LargeRight, atkStyle)); // ������ ���� Ȱ��ȭ
                }
			}
		}
    }

    public void OnSkillAttack2(InputAction.CallbackContext context)// �ñر�
	{
		if (context.started)  // ��Ÿ���� 0�� ���� ��ų �ߵ�
        {
            if (skillAttack2Time <= 0)
            {
                animator.SetTrigger(AnimationStrings.skillAttackTrigger2);  // ��ų �ִϸ��̼� ����
                skillAttack2Time = skillAttack2Cooldown;  // ��Ÿ�� ����

				int atkStyle = 2;      // �ñر� ���� ����
									   // ���⿡ ���� ������ BoxCollider2D Ȱ��ȭ
				TicTocDealyTime = Ultimite;                      // ���ʵ��� �����Ǿ� ������?
				if (sp.flipX)
				{
					StartCoroutine(DisableCollider(LargeLeft, atkStyle));  // ���� ���� Ȱ��ȭ
				}
				else
				{
					StartCoroutine(DisableCollider(LargeRight, atkStyle));  // ������ ���� Ȱ��ȭ
				}
			}
		}
    }

    // ���� �Լ� Like ����
    void PleaseStopPlayer()
    {
		rigid.velocity = Vector2.zero;

		tempPlayerPos = playerPos;
		StartCoroutine(TicToc());
	}
}
