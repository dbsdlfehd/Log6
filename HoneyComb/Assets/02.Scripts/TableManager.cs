using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
	// �̱��� �ν��Ͻ�
	public static TableManager Instance;

	[Header("�Ϲ� ��")]
	[SerializeField] private int enemy1HP; // Inspector���� ���� ����
	[SerializeField] private int enemy1Atk;

	[Header("���� ��")]
	[SerializeField] private int bossHP;
	[SerializeField] private int bossATK;

	// Static ����
	public static int Enemy1HP { get; private set; }
	public static int Enemy1Atk { get; private set; }
	public static int BossHP { get; private set; }
	public static int BossATK { get; private set; }

	private void Awake()
	{
		// �̱��� ���� ����
		if (Instance == null)
		{
			Instance = this;
			//DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ����
		}
		else
		{
			Destroy(gameObject);
		}

		// �ʱ�ȭ (���� ���� �� static ���� ������Ʈ)
		UpdateStaticValues();
	}

	private void UpdateStaticValues()
	{
		// Static ������ Inspector �� �Ҵ�
		Enemy1HP = enemy1HP;
		Enemy1Atk = enemy1Atk;
		BossHP = bossHP;
		BossATK = bossATK;
	}

	// Inspector���� ���� ����� ������ static ���� ����
	private void OnValidate()
	{
		if (Instance == this)
		{
			UpdateStaticValues();
		}
	}
}
