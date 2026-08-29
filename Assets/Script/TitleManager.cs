using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.ResetHP();

        SceneManager.LoadScene("ExplorationScene");
    }
}