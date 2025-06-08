using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneManager : MonoBehaviour
{
    public Button buttonHome;
    public Button buttonRestart;

    private void Start()
    {
        buttonHome.onClick.AddListener(LoadHomeScene);
        buttonRestart.onClick.AddListener(ReloadCurrentScene);
    }

    private void LoadHomeScene()
    {
        SceneManager.LoadScene("Menu");
    }

    private void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
