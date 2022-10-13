using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    //遊戲初始化 : 選擇第一把武器
    void Start()
    {
        SelectWeapon(0);
    }

    //每個 Frame 持續執行
    //如果按下Fire1(通常是滑鼠右鍵或鍵盤Ctrl)，發送訊息到Fire(通常在武器的腳本中會有個Fire接收器)
    //如果按下鍵盤 1或2 切換武器
    void Update()
    {
        if (Input.GetButton("Fire1"))
            BroadcastMessage("Fire");

        if (Input.GetKeyDown("1"))
            SelectWeapon(0);
        else if (Input.GetKeyDown("2"))
            SelectWeapon(1);
    }

    //被選擇的武器顯示，沒被選擇的武器隱藏
    void SelectWeapon(int index)
    {
        for (var i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(i == index);            
    }
}
