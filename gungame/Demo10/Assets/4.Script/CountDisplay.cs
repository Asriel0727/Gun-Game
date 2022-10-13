using UnityEngine;
using UnityEngine.UI;

public class CountDisplay : MonoBehaviour
{
    Text ui;
    int count = -1;

    void Start()
    {
        ui = GetComponent<Text>();
    }

    void Update()
    {
        if (count != BoxCollect.GetCount)
        {
            count = BoxCollect.GetCount;
            ui.text = "BOX Collected: " + BoxCollect.GetCount + "/" + OpenDoor.boxCount;
        }        
    }
}
