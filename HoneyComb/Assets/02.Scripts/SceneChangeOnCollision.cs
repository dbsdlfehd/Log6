using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOnCollision : MonoBehaviour
{
	// 충돌했을 때 전환할 씬의 이름
	public string sceneName;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// 충돌한 객체가 특정 조건을 만족하면 씬 전환
		if (collision.gameObject.CompareTag("Player")) // 예: "Player" 태그가 붙은 객체와 충돌 시
		{
			// 씬 전환
			SceneChangeHamSu();
		}
	}

	public void SceneChangeHamSu()
	{
		SceneManager.LoadScene(sceneName);
	}
}
