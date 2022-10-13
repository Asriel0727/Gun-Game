using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePass : MonoBehaviour
{
    //遊戲初始化 : 解除滑鼠游標鎖定
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Play()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }  
}
