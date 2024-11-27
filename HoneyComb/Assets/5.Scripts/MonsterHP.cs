using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHP : MonoBehaviour
{
    private Player player;  //�÷��̾�
    private Rigidbody2D rb; //�߷�
    private PlayerAction playerAction;
    private PrefabSpawner prefabSpawner;
    private UI_MonsterHP uI_MonsterHP; // ui�� �ѷ��ִ� hp��
    private DamageTextShow damageTextShow; // ui ������ ��ġ�� ǥ�����ִ°� ���� ��ũ��Ʈ

	[Header("ü��")]
    public int maxHP; // �ִ� ü�� ����
    public int nowHP; // ���� ü�� ����

    [Header("�̰��� �����ΰ�?")]
    public bool isBoss;

    [Header("�̰��� �߰������ΰ�?")]
    public bool middleBoss;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    public int defaultMaxHp;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        playerAction = FindObjectOfType<Player>().GetComponent<PlayerAction>();
        prefabSpawner = FindAnyObjectByType<PrefabSpawner>();
		uI_MonsterHP = GetComponent<UI_MonsterHP>();
		damageTextShow = GetComponent<DamageTextShow>();// ���� ������Ʈ�� �����ִ�.
    }

    private void SetEnemyStatus(int _maxHP)
    {
        maxHP = _maxHP;
        nowHP = _maxHP;
    }

    IEnumerator SetDefaultEnemyStat()
    {
        yield return new WaitForSeconds(2.0f);
        SetEnemyStatus(defaultMaxHp);
    }


    public void SetDefault()
    {
        SetDefaultEnemyStat();
    }
    void Start()
    {
        defaultMaxHp = maxHP;
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();// ������ ����ߵ� (�ʱ�ȭ)
        SetEnemyStatus(maxHP);              // ü�� ��ġ ����

        // prfHpBar �������� �̿��� canvas���ٰ� ü�¹� ����.
        //bghp_bar = Instantiate(prfHpBar, GameObject.Find("Canvas").transform).GetComponent<RectTransform>(); // bghp_bar����
        //hp_bar = bghp_bar.transform.GetChild(0).GetComponent<Image>(); // bghp_bar�� �ڽ� ������Ʈ ������Ʈ ��������
    }

    private void Update()
    {
    }

    // ���˽�
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �� ����� �÷��̾� �±��Ͻ� && �÷��̾ �������Ͻ�
        if (other.CompareTag("Player") && playerAction.isAtking)
        {
            int damage = player.Atk;                // �÷��̾� �����
			damageTextShow.ShowDamage(damage);
			//StartCoroutine(ShowDamageText(damage)); // ������� ǥ��
			nowHP = nowHP - damage;
        }

        // �Ϲݸ� �״� �Լ�
        //if (nowHP < 0 && isBoss == false)			// ü���� 0���� ������
        //{
        //    EnemyDead();
        //}
        //// �� �״� �Լ�
        //else if (nowHP < 0 && isBoss == true)
        //{
        //    BossDead();
        //}
        if (nowHP < 0 && middleBoss) // �߰�����
        {
            MiddleBossDead();
		}
    }

    void BossDead()
    {
        Destroy(gameObject);
        //prefabSpawner.RoomEnemyCount++;        // �� ���� Ƚ�� 1 �þ
        //Destroy(bghp_bar.gameObject);          // ü�¹� ����
    }

    void EnemyDead()
    {
        nowHP = 0;
        if (isBoss == false)
        {
            Destroy(gameObject);
		}
        //prefabSpawner.RoomEnemyCount++;        // �� ���� Ƚ�� 1 �þ
        //Destroy(bghp_bar.gameObject);          // ü�¹� ����

    }

	void MiddleBossDead()
	{
		nowHP = 0;
		uI_MonsterHP.DestroyHP_UI();// ü�¹� ����
		Destroy(gameObject);    // �ڱ� �ڽ��� ����
	}
}
