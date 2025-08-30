using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameUI : MonoBehaviour
{
    public void StartGame()
    {
        // Load the next scene (change "GameScene" to your actual scene name)
        SceneManager.LoadScene("GameScene");
    }
}
