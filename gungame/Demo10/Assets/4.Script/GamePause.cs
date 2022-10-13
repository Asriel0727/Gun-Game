using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GamePause : MonoBehaviour
{
    public GameObject tip1Ui;
    public GameObject tip2Ui;    
    public BlurEffect blur;
    public GameObject[] otherUi;
    public MonoBehaviour[] othrtComponent;

    //初始化
    void Start()
    {    
        SetPause(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //如果按下滑鼠右鍵，滑鼠游標隱藏
    //如果滑鼠游標沒隱藏且被鎖定，"SetPause"=是
    //如果滑鼠游標隱藏且沒鎖定，"SetPause"=否
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SetPause(true);
        }

        if (Input.GetMouseButton(0) && Cursor.visible)
        {

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            SetPause(false);
        }      
    }

    //時間縮放=1(1表示正常0表示暫停)
    void OnApplicationQuit()
    {
        Time.timeScale = 1;
    }

    void SetPause(bool pause)
    {
        Time.timeScale = pause ? 0.0001f : 1;

        tip1Ui.SetActive(!pause);
        tip2Ui.SetActive(pause);
        blur.enabled = pause;
        foreach (var item in otherUi)
            item.SetActive(!pause);
        foreach (var item in othrtComponent)
            item.enabled = !pause;
    }   
}
