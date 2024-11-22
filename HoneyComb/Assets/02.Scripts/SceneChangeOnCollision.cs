using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOnCollision : MonoBehaviour
{
	// �浹���� �� ��ȯ�� ���� �̸�
	public string sceneName;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// �浹�� ��ü�� Ư�� ������ �����ϸ� �� ��ȯ
		if (collision.gameObject.CompareTag("Player")) // ��: "Player" �±װ� ���� ��ü�� �浹 ��
		{
			// �� ��ȯ
			SceneChangeHamSu();
		}
	}

	public void SceneChangeHamSu()
	{
		SceneManager.LoadScene(sceneName);
	}
}
