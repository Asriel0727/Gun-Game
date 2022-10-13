using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    bool doorOpened = false;//開門狀態(是/否)
    float openTime = 0;//開門計時器
    Transform door;//門
    GameObject[] box;//寶箱

    //開關門音效
    public AudioClip doorOpenAudio;
    public AudioClip doorCloseAudio;

    static public int boxCount;//寶箱數量

    //遊戲初始化
    //計算box在遊戲中的總數量
    void Awake()
    {
        try
        {
            box = GameObject.FindGameObjectsWithTag("box");
            boxCount = box.Length;
        }
        catch (System.Exception)
        {
            Debug.LogWarning("no box tag");
        }       
    }  

    //每個 Frame 持續執行(開門計時器)
    //如果開門狀態為 "是"，產生計時秒數，如果計時秒數大於 2 秒時，執開關門功能
    void Update()
    {
        if (doorOpened)
            openTime += Time.deltaTime;

        if (openTime >= 2)
            Close();
    }

    //如果角色撞擊到有door標籤的物件，(並且開門狀態為"否")，門=碰撞到的物件
    //如果門的父級標籤為 endDoor ，且如果box獲取數量=box總數量，執行開門功能
    //如果父級標籤不是 endDoor ，執行開門功能
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "door" && doorOpened == false)
        {
            door = hit.transform;
            if (door.parent.tag == "endDoor" && BoxCollect.GetCount != boxCount)
                return;
            
            Open();
        }
    }

    //將開門狀態設定為"是"，播放房子的開門動畫，並播放開門播放器的聲音
    void Open()
    {
        doorOpened = true;
        door.parent.GetComponent<Animator>().Play("doorOpen");

        if (doorOpenAudio)
            AudioSource.PlayClipAtPoint(doorOpenAudio, door.transform.position);

    }

    //將開門狀態設定為 "否"，播放房子的關門動畫，並播放關門播放器的聲音，將計時秒數設定為 0
    void Close()
    {
        openTime = 0;
        doorOpened = false;
        door.parent.GetComponent<Animator>().Play("doorClose");

        if (doorCloseAudio)
            AudioSource.PlayClipAtPoint(doorCloseAudio, door.transform.position);        
    }
}
