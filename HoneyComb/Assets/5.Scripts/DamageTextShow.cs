using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextShow : MonoBehaviour
{
	[Header("���� �� ���� �ٿ��� ��")]
	public TextMeshProUGUI damage_text;

	[Header("������ �ؽ�Ʈ ������")]
	public GameObject damage_text_prf;

	public void ShowDamage(int damage)
	{
		StartCoroutine(ShowDamageText(damage));
	}

	// ����� �ؽ�Ʈ ���� �� 1�� �� ����
	IEnumerator ShowDamageText(int damage)
	{
		// �ؽ�Ʈ ������ ����
		GameObject dmgText = Instantiate(damage_text_prf, GameObject.Find("Canvas").transform);
		TextMeshProUGUI dmgTextComponent = dmgText.GetComponent<TextMeshProUGUI>();

		// �ؽ�Ʈ ����� ��ġ ����
		dmgTextComponent.text = damage.ToString();

		// �ʱ� ��ġ ���� (���� -> ��ũ�� ��ǥ ��ȯ)
		Vector3 startPosition = Camera.main.WorldToScreenPoint(transform.position);
		dmgText.transform.position = startPosition;

		// �̵��� y�� ����
		float moveDistance = 50f; // y�� �̵� �Ÿ� (Canvas ����)
		float duration = 0.2f;    // ���� �ð�
		float elapsedTime = 0f;   // ��� �ð�

		// �ڷ�ƾ���� �ؽ�Ʈ ��ġ �̵�
		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;

			// ���� ��ġ ���
			Vector3 currentPosition = dmgText.transform.position;
			currentPosition.y = Mathf.Lerp(startPosition.y, startPosition.y + moveDistance, elapsedTime / duration);
			dmgText.transform.position = currentPosition;

			yield return null; // ���� �����ӱ��� ���
		}

		// �ؽ�Ʈ ����
		Destroy(dmgText);
	}

}
