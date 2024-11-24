using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHP : MonoBehaviour
{
    private Player player;  //�÷��̾�
    private Rigidbody2D rb; //�߷�
    private PlayerAction playerAction;
    private PrefabSpawner prefabSpawner;

    [Header("ü��")]
    public int maxHP; // �ִ� ü�� ����
    public int nowHP; // ���� ü�� ����

    [Header("�̰��� �����ΰ�?")]
    public bool isBoss;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        playerAction = FindObjectOfType<Player>().GetComponent<PlayerAction>();
        prefabSpawner = FindAnyObjectByType<PrefabSpawner>();
    }

    private void SetEnemyStatus(int _maxHP)
    {
        maxHP = _maxHP;
        nowHP = _maxHP;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();// ������ ����ߵ� (�ʱ�ȭ)
        SetEnemyStatus(maxHP);              // ü�� ��ġ ����

        // prfHpBar �������� �̿��� canvas���ٰ� ü�¹� ����.
        //bghp_bar = Instantiate(prfHpBar, GameObject.Find("Canvas").transform).GetComponent<RectTransform>(); // bghp_bar����
        //hp_bar = bghp_bar.transform.GetChild(0).GetComponent<Image>(); // bghp_bar�� �ڽ� ������Ʈ ������Ʈ ��������
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int damage = player.Atk;                // �÷��̾� �����


            //StartCoroutine(ShowDamageText(damage)); // ������� ǥ��
            nowHP = nowHP - damage;
        }

        // �Ϲݸ� �״� �Լ�
        if (nowHP < 0 && isBoss == false)			// ü���� 0���� ������
        {
            EnemyDead();
        }
        // �� �״� �Լ�
        else if (nowHP < 0 && isBoss == true)
        {
            BossDead();
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
}