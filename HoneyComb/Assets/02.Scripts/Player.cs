using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    PlayerHpShow PlayerHpShow;

    public List<_Object> npcObjects = new List<_Object>();

    [Header("��ȯ ��ũ��Ʈ")]
    public PrefabSpawner prefabSpawner;
    public PlayerAction playerAction;

    [Header("���� Ƚ��")]
    static public int DeadCount = 0;
    public TextMeshProUGUI DeadCount_UI;

    //�� ��� ų
    public bool EnmeyDown = false;

    //�÷��̾� ��ġ
    public Transform player;

    //�� ��ġ
    public Transform Home;

    //���� ��ġ
    public Transform DeadPoint;

    public bool isDead = false;
    public GameObject Dead_set; // ���� ����

    [Header("ü��")]
    public int maxHP = 1000; // �ִ� ü��
    public int nowHP; // ü���� int�� ����
    public TextMeshProUGUI HP_UI;

    [Header("�Ϲ� ���� (��)")]
    public int Atk;

	[Header("��ų (��¡ ��ġ) ��踸ŭ �����Ұ��ΰ�?")]
	public int SkillAtk;

	[Header("�ñر�(��ġ) ��踸ŭ ���� �Ұ��ΰ�?")]
	public int UltimitAtk;

	[Header("���� ���ݷ� UI ǥ�ÿ�")]
	public TextMeshProUGUI Atk_UI;

    [Header("����")]
    public int AR; // Armor Resistance ���� Defense -> AR�� �ٲ�
    public TextMeshProUGUI AR_UI;

    public int atkNum; // �޺� ��ȣ

    [Header("ü�� ����� �ۼ�Ʈ")]
    public float healthDebuff1 = 0.35f;       // ü�� ���� ����� 1 (35%) ��Ȳ���
    public float healthDebuff2 = 0.3f;        // ü�� ���� ����� 2 (30%)  ���强 ����
    public float healthDebuff3 = 0.2f;        // ü�� ���� ����� 3 (20%) �������
    public float healthDebuff4 = 0.1f;        // ü�� ���� ����� 4 (10%) Ż����


    [Header("���� ����� �ۼ�Ʈ")]
    public float defenseDebuff1 = 0.25f;      // ���� ���� ����� 1 (25%) ��ȸ�� �Ҿ����
    public float defenseDebuff2 = 0.14f;      // ���� ���� ����� 2 (14%) ������ ����
    public float defenseDebuff3 = 0.26f;      // ���� ���� ����� 3 (26%) ȸ���ൿ
    public float defenseDebuff4 = 0.35f;      // ���� ���� ����� 4 (35%) ���� �αٰŸ�

    [Header("��ȭ")]
    static public int Money = 0; // ��ȭ
	public TextMeshProUGUI MoneyTxt;
    public TextMeshProUGUI StoreMoneyTxt;

    [Header("����")]
	static public int round = 0; // ����
	public TextMeshProUGUI RoundTxt;

	Animator animator;        // Animator ������Ʈ ����

	[Header("���� �� ������")]
	public RoomGenerator roomGenerator; // ���� �� ������

	// ����Ʈ �������ͽ� (�����)
	int DefaultMaxHP = 0;
	int DefaultAtk = 0;
	int DefaultMoney = 0;
	int DefaultRound = 0;
    float D_speed = 0;
    float D_As = 0; // ���ݼӵ�
    int D_Ar = 0; // ���� Armor Resistance

	[Header("���� ���� ��")]
	public TextMeshProUGUI gameRoundTMP;
	// �� �渶�� ���� UP, 5���� ���� -> 6���� ������
	static public int gameRound = 0;

    public ShopManager shop;

    public ItemManager itemManager;

    public int HammerBuffedRoundCount = 0;

    // ���� �׽�Ʈ�� ��ȭ + 100
    public void AddMoneyTest()
	{
        Money += 100;
	}

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}
	private void Start()
    {
        nowHP = Mathf.FloorToInt(maxHP); // �ִ� ü���� ������ ����
        PlayerHpShow = GetComponent<PlayerHpShow>();

        // ����� ����
       // ApplyDebuff();

        // ���� ���۽� ���� �ʱ�ȭ
        gameRound = 0;

        // ����Ʈ �������ͽ� ����
        DefaultMaxHP = maxHP;
        DefaultAtk = Atk;
        DefaultMoney = Money;
        DefaultRound = round;
		D_speed = playerAction.walkSpeed; // start�Լ� ���� ����
		D_As = playerAction.jabCooldown; // ���ݼӵ�
		D_Ar = AR; // ���� Armor Resistance
	}

    private void ApplyDebuff()// ����� ����
    {
        // ü�� ����� ����: 95% ���Ҹ� ���� ���
        nowHP = Mathf.FloorToInt(maxHP * 0.05f); // ���� ü�� = 1000 * 0.05

        // ���� ����� ����
        float debuffedDefense = AR * (1 - defenseDebuff1);
        debuffedDefense *= (1 - defenseDebuff2);
        debuffedDefense *= (1 - defenseDebuff3);
        debuffedDefense *= (1 - defenseDebuff4);
        AR = Mathf.FloorToInt(debuffedDefense);

        Debug.Log("����� ���� �Ϸ�: ");
        Debug.Log($"ü��: {nowHP}");
        Debug.Log($"����: {AR}");
    }

    // �׾��� �� ���ϴ� ��
    public void Dead()
    {
		isDead = true; // �÷��̾� ���� ���� Ȱ��ȭ
		DeadCount++;
		StartCoroutine(DeadShow());
    }

    // �׾��� �� ���ϴ� ��22
    IEnumerator DeadShow()
    {
		Debug.Log("���� ��� ���� �ǰ�?");
		player.GetComponent<PlayerInput>().enabled = false; // ����
        animator.SetTrigger(AnimationStrings.DeadTrigger);  // dead �ִϸ��̼� ����
        yield return new WaitForSeconds(4); // N�� ���� ��޷�
		Dead_set.SetActive(true);
		// ���� ��ġ�� �̵�
		player.position = DeadPoint.position;
		//prefabSpawner.HideTP();
		prefabSpawner.isSpawnned = false;
		EnmeyDown = true; // �� ��� ��Ȱ��ȭ
		yield return null;
	}

    // ���� ����ü�� ���� �ǰ� ���ҽ�
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("weapon"))
            return;

		// �ǰ� �ִϸ��̼� ���
        if(playerAction.canMove == true)
            animator.SetTrigger(AnimationStrings.OuchTrigger);  // dead �ִϸ��̼� ����
		else
		{
			// ResetTrigger�� �ش� Ʈ���Ÿ� ��Ȱ��ȭ
			animator.ResetTrigger(AnimationStrings.OuchTrigger);
		}

		// ������ ����� ���� ���ط� ���
		//float incomingDamage = collision.GetComponent<FarATK>().damage;
		//      float finalDamage = Mathf.Max(incomingDamage - AR, 1); // ���� ���� �� �ּ� ���� 1�� ����

		//      nowHP -= Mathf.FloorToInt(finalDamage); // ���ط��� ������ ����

		// ������ ����� ���� ���ط� ���
		float incomingDamage = collision.GetComponent<FarATK>().damage;
        PlayerDamaged(incomingDamage);

        // ü���� 0���� ������ && �״� ���ΰ� ��Ȱ��ȭ �ɽ�
		if (nowHP <= 0 && isDead == false)
        {
            Dead();
        }
    }


    private void Update()
    {
        // ���� ���¿��� �����̽��ٸ� ������ ��Ȱ ó�� // #####################
        if (isDead && Input.GetKeyDown(KeyCode.Space))
        {
            RespawnPlayer();
            Debug.Log("�����̽� �� ����");
        }

        //if (isDead)
        //    player.position = DeadPoint.position;

        // �ӽ� ü�� ǥ�� ����
        int TempNowHP = nowHP;

        if (nowHP < 0)// ü���� ������ �� -> �����ֱ�� ü�� 0 ǥ��
		{
            TempNowHP = 0;
		}
        else if (nowHP > 0) // ��� �� ���� ���� ü������ ǥ��
        {
			TempNowHP = nowHP;
		}

        HP_UI.text = TempNowHP.ToString() + "/" + maxHP.ToString(); // (����ü��/�ִ�ü��)
		Atk_UI.text = Atk.ToString();                               // ���ݷ�
		MoneyTxt.text = Money.ToString();                           // ��ȭ
        StoreMoneyTxt.text = Money.ToString();                      // �������� ���̴� ��ȭ
        RoundTxt.text = round.ToString();                           // ����
        AR_UI.text = AR.ToString();                                 // ����

        // ���� ���� ���� ��
        string Temptext = "";
        if (gameRound == 0) // �� �϶�
        {
            Temptext = "��".ToString();
        }
        else if (gameRound <= prefabSpawner.OverRoom)
        {
            Temptext = "�������� : " + gameRound.ToString();
        }
        else if (gameRound == prefabSpawner.OverRoom + 1)
        {
            Temptext = "����";
        }
        else if (gameRound == prefabSpawner.OverRoom + 2)
        {
            Temptext = "������";
        }

        gameRoundTMP.text = Temptext.ToString();

        DeadCount_UI.text = "���� Ƚ�� : " + DeadCount.ToString();
    }

    public void RespawnPlayer()
    {
        // �÷��̾� �������ͽ� ������� ��, ���� ����
        StatDefaultPlayer();

		// ���� ���� ���� ���� �� 0���� �ʱ�ȭ
		gameRound = 0;

        player.GetComponent<PlayerInput>().enabled = true; // �ٽ� �����ϼ� �ְ�
        nowHP = Mathf.FloorToInt(maxHP); // ��Ȱ �� ü���� �ִ�ġ�� ����
        isDead = false;
        player.position = Home.position; // �� ��
        Dead_set.SetActive(false); // ���� ���� ��

        // ���� �� �ٽ� �Ⱥ��̰� �����
        roomGenerator.DestroyDoor();

        foreach (var npc in npcObjects)
        {
            npc.isDialogged = true;
        }
        
        EnmeyDown = false;//�� ���� ���� ����
        prefabSpawner.RoomEnemyCount = 0;// �� ���� �� 0���� �ʱ�ȭ
        prefabSpawner.DestroySpawnedObjects();// ������ ������ �� ���� TP ��ü ����
	}// �÷��̾� ��Ȱ

	public void StatDefaultPlayer()
    {
		maxHP = DefaultMaxHP;
		Atk = DefaultAtk;
		round = DefaultRound;
		itemManager.isNextRoundHpUp = false;    // ��¹�� ���� ����
		itemManager.DebuffHpUp();
        playerAction.walkSpeed = D_speed;
        playerAction.jabCooldown = D_As;// ���ݼӵ�
        AR = D_Ar; // ���� Armor Resistance
        itemManager.GoodByeHammerBuff();        // �ظ� ���� ����


		itemManager.OFFCrunchMode();            // ������ �� ���� ����

		if (Atk != 25)
		{
			Atk = DefaultAtk;
		}
	}// �÷��̾� �������ͽ� ����Ʈ�� ����


	const float AR_FACTOR = 0.01f;       // ���� ���Ŀ� ����
	public void PlayerDamaged(float dmg) // �÷��̾ �ްԵǴ� ������ (���� ���� ����)
    {
		// ������ ������ ��� 0���� ����
		if (AR < 0) AR = 0;

        // ������ ��� (���ϰ� �Ȱ���) (���� 100 -> 50%) (���� 200 -> 33.33%)
		int totaldmg = Mathf.Max(1, (int)(dmg / (1 + (AR * AR_FACTOR))));

		// ���� HP���� ���ط� ����
		nowHP -= totaldmg;
	}
}
