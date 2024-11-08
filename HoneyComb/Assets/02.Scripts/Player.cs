using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class Player : MonoBehaviour
{
    PlayerHpShow PlayerHpShow;

    public List<_Object> npcObjects = new List<_Object>();


    public PrefabSpawner prefabSpawner;

    [Header("���� Ƚ��")]
    public int DeadCount = 0;
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
    public float maxHP = 1000f; // �ִ� ü��
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


	Animator animator;        // Animator ������Ʈ ����

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
    }

    // ������� �����ϴ� �Լ�
    private void ApplyDebuff()
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

    // ���� �Լ�
    public void Dead()
    {
		DeadCount++;
		StartCoroutine(DeadShow());
        
    }
    IEnumerator DeadShow()
    {
		animator.SetTrigger(AnimationStrings.attackTrigger);  // dead �ִϸ��̼� ����

		isDead = true;
		Dead_set.SetActive(true);
		// ���� ��ġ�� �̵�
		player.position = DeadPoint.position;
		prefabSpawner.HideTP();
		prefabSpawner.isSpawnned = false;
		EnmeyDown = true; // �� ��� ��Ȱ��ȭ
		yield return null;
	}


	// Trigger �̺�Ʈ���� ������ ����� ���� ó��
	void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("weapon"))
            return;

        // ������ ����� ���� ���ط� ���
        float incomingDamage = collision.GetComponent<FarATK>().damage;
        float finalDamage = Mathf.Max(incomingDamage - Defense, 1); // ���� ���� �� �ּ� ���� 1�� ����

        nowHP -= Mathf.FloorToInt(finalDamage); // ���ط��� ������ ����

        if (nowHP <= 0)
        {
            Dead();
        }
    }

    // ������Ʈ �޼ҵ�
    private void Update()
    {
        // ���� ���¿��� �����̽��ٸ� ������ ��Ȱ ó��
        if (isDead && Input.GetKeyDown(KeyCode.Space))
        {
            nowHP = Mathf.FloorToInt(maxHP); // ��Ȱ �� ü���� �ִ�ġ�� ����
            isDead = false;
            player.position = Home.position;
            Dead_set.SetActive(false);

            foreach (var npc in npcObjects)
            {
                npc.isDialogged = true;
            }
            EnmeyDown = false;//�� ���� ���� ����
            prefabSpawner.RoomEnemyCount = 0;// �� ���� �� 0���� �ʱ�ȭ
        }

        if (isDead)
            player.position = DeadPoint.position;

        // UI ����
        HP_UI.text = nowHP.ToString() + "/" + maxHP.ToString();
        Atk_UI.text = Atk.ToString();

        DeadCount_UI.text = "���� Ƚ�� : " + DeadCount.ToString();
    }
}
