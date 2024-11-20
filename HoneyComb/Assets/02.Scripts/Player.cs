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

    [Header("����")]
    public int Atk;
    public TextMeshProUGUI Atk_UI;

    [Header("���")]
    public int Defense; // ����
    public TextMeshProUGUI Defense_UI;

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
	public int Money = 0; // ��ȭ
	public TextMeshProUGUI MoneyTxt;

	[Header("����")]
	public int round = 0; // ����
	public TextMeshProUGUI RoundTxt;

	Animator animator;        // Animator ������Ʈ ����

	[Header("���� �� ������")]
	public RoomGenerator roomGenerator; // ���� �� ������

	// ����Ʈ �������ͽ� (�����)
	int DefaultMaxHP = 0;
	int DefaultAtk = 0;
	int DefaultMoney = 0;
	int DefaultRound = 0;

	[Header("���� ���� ��")]
	public TextMeshProUGUI gameRoundTMP;
	// �� �渶�� ���� UP, 5���� ���� -> 6���� ������
	static public int gameRound = 0;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}
	private void Start()
    {
        nowHP = Mathf.FloorToInt(maxHP); // �ִ� ü���� ������ ����
        PlayerHpShow = GetComponent<PlayerHpShow>();

        // ����� ����
        //ApplyDebuff();

        // ���� ���۽� ���� �ʱ�ȭ
        gameRound = 0;

        // ����Ʈ �������ͽ� ����
        DefaultMaxHP = maxHP;
        DefaultAtk = Atk;
        DefaultMoney = Money;
        DefaultRound = round;
	}

    private void ApplyDebuff()// ����� ����
    {
        // ü�� ����� ����: 95% ���Ҹ� ���� ���
        nowHP = Mathf.FloorToInt(maxHP * 0.05f); // ���� ü�� = 1000 * 0.05

        // ���� ����� ����
        float debuffedDefense = Defense * (1 - defenseDebuff1);
        debuffedDefense *= (1 - defenseDebuff2);
        debuffedDefense *= (1 - defenseDebuff3);
        debuffedDefense *= (1 - defenseDebuff4);
        Defense = Mathf.FloorToInt(debuffedDefense);

        Debug.Log("����� ���� �Ϸ�: ");
        Debug.Log($"ü��: {nowHP}");
        Debug.Log($"����: {Defense}");
    }

    public void Dead()// ����Ƚ�� �߰�
    {
		DeadCount++;
		StartCoroutine(DeadShow());
    }
    IEnumerator DeadShow()// ���� �ൿ
    {
        player.GetComponent<PlayerInput>().enabled = false; // ����
        animator.SetTrigger(AnimationStrings.DeadTrigger);  // dead �ִϸ��̼� ����
        yield return new WaitForSeconds(5); // 5�� ���� ��޷�
		isDead = true;
		Dead_set.SetActive(true);
		// ���� ��ġ�� �̵�
		player.position = DeadPoint.position;
		prefabSpawner.HideTP();
		prefabSpawner.isSpawnned = false;
		EnmeyDown = true; // �� ��� ��Ȱ��ȭ
		yield return null;
	}

	void OnTriggerEnter2D(Collider2D collision)// �ǰ� ���ҽ�
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
		float incomingDamage = collision.GetComponent<FarATK>().damage;
        float finalDamage = Mathf.Max(incomingDamage - Defense, 1); // ���� ���� �� �ּ� ���� 1�� ����

        nowHP -= Mathf.FloorToInt(finalDamage); // ���ط��� ������ ����

        if (nowHP <= 0)
        {
            Dead();
        }
    }

    private void Update()
    {
        // ���� ���¿��� �����̽��ٸ� ������ ��Ȱ ó�� // #####################
        if (isDead && Input.GetKeyDown(KeyCode.Space))
        {
            // �÷��̾� �������ͽ� ������� ��, ���� ����
            maxHP = DefaultMaxHP;
            Atk = DefaultAtk;
            round = DefaultRound;

            // ���� ���� ���� ���� �� 0���� �ʱ�ȭ
            gameRound = 0;

			player.GetComponent<PlayerInput>().enabled = true; // �ٽ� �����ϼ� �ְ�
            nowHP = Mathf.FloorToInt(maxHP); // ��Ȱ �� ü���� �ִ�ġ�� ����
            isDead = false;
            player.position = Home.position;
            Dead_set.SetActive(false);

            // ���� �� �ٽ� �Ⱥ��̰� �����
            roomGenerator.DestroyDoor();

			foreach (var npc in npcObjects)
            {
                npc.isDialogged = true;
            }
            EnmeyDown = false;//�� ���� ���� ����
            prefabSpawner.RoomEnemyCount = 0;// �� ���� �� 0���� �ʱ�ȭ
        }

        if (isDead)
            player.position = DeadPoint.position;


        HP_UI.text = nowHP.ToString() + "/" + maxHP.ToString();     // ü��
		Atk_UI.text = Atk.ToString();                               // ���ݷ�
		MoneyTxt.text = Money.ToString();                           // ��ȭ
		RoundTxt.text = round.ToString();                           // ����
		gameRoundTMP.text = gameRound.ToString();                   // ���� ���� ���� ��


		DeadCount_UI.text = "���� Ƚ�� : " + DeadCount.ToString();
    }
}
