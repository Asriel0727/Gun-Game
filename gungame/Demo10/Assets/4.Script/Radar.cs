using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radar : MonoBehaviour
{
    //宣告:雷達底圖，敵人底圖，雷達中心點物件
    //雷達各項參數:各敵人間隔、偵測範圍、雷達底圖位置、雷達貼圖大小、敵人底圖位置、敵人貼圖大小
    public RectTransform radarUi;
    public Sprite enemyIcon;   

    [System.Serializable]
    public class RadarParameter
    {
        public float distScale = 5;
        public int radarDist = 20;
        public Vector2 enemyIconSize = new Vector2(20, 20);
    }

    public RadarParameter parameter;

    Transform centerObject;
    GameObject[] enemys;
    List<RectTransform> enemyUiList = new List<RectTransform>();

    // Use this for initialization
    void Start()
    {
        if (centerObject == null)
            centerObject = GameObject.FindWithTag("Player").transform;

        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemys != null)
        {
            for (int i = 0; i < enemys.Length; i++)
            {
                var enemyUi = new GameObject("enemy " + i).AddComponent<Image>();
                enemyUi.transform.SetParent(radarUi);
                enemyUi.sprite = enemyIcon;

                var enemyUiRtr = enemyUi.GetComponent<RectTransform>();
                enemyUiRtr.sizeDelta = parameter.enemyIconSize;
                enemyUiList.Add(enemyUiRtr);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemys == null)
            return;

        for (int i = 0; i < enemys.Length; i++)
        {
            enemyUiList[i].gameObject.SetActive(false);
            if (enemys[i] == null) 
                continue;               

            float dist = Vector3.Distance(centerObject.position, enemys[i].transform.position);
            if (dist > parameter.radarDist)
                continue;

            enemyUiList[i].gameObject.SetActive(true);

            Vector3 dir = enemys[i].transform.position - centerObject.position;
            float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg + centerObject.eulerAngles.y;
            Vector2 pos = new Vector2(dist * Mathf.Cos(angle * Mathf.Deg2Rad), dist * Mathf.Sin(angle * Mathf.Deg2Rad)) * parameter.distScale;
            enemyUiList[i].anchoredPosition = pos;
        }
    }
}
