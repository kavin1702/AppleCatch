using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameUI : MonoBehaviour
{
    public void StartGame()
    {
        
        SceneManager.LoadScene("GameScene");
    }
}
