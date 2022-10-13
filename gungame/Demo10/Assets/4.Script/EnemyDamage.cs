using UnityEngine;
using UnityEngine.UI;

public class EnemyDamage : MonoBehaviour
{
    //宣告:生命值、替代物、死亡音效、血條貼圖材質
    public int hp = 100;

    public Transform deadReplacement;
    public AudioClip dieSound;
    public Canvas hpCanvas;

    int maxHp;
    float uiDisplayTime;//UI顯示的時間
    Image hpUi;

    void Start()
    {
        maxHp = hp;

        if (hpCanvas)
        {           
            hpUi = hpCanvas.GetComponentInChildren<Image>();
            hpCanvas.worldCamera = Camera.main;
            hpCanvas.enabled = false;
        }       
    }

    void Update()
    {
        if (uiDisplayTime <= 0 || hp <= 0 || hpCanvas == null)
            return;

        uiDisplayTime -= Time.deltaTime;
        hpCanvas.enabled = true;
        hpUi.fillAmount = (float)hp / maxHp;

        if (uiDisplayTime <= 0)
            hpCanvas.enabled = false;
    }

    //如果生命值=0，執行"DeadReceiver"
    void ApplyDamage(int damage)
    {
        uiDisplayTime = 1;
        // We already have less than 0 hitpoints, maybe we got killed already?
        if (hp <= 0)
            return;

        if (enabled)
            hp -= damage;

        if (hp <= 0)
            DeadReceiver();
    }

    //刪除自己、撥放死亡音效、產生一個替代物
    void DeadReceiver()
    {      
        // Play a dying audio clip
        if (dieSound)
            AudioSource.PlayClipAtPoint(dieSound, transform.position);

        // Replace ourselves with the dead body
        if (deadReplacement)
        {
            Transform dead = Instantiate(deadReplacement, transform.position, transform.rotation) as Transform;

            // Copy position & rotation from the old hierarchy into the dead replacement
            CopyTransformsRecurse(transform, dead);
        }

        // Destroy ourselves
        Destroy(gameObject);
    }

    void CopyTransformsRecurse(Transform src, Transform dst)
    {
        dst.position = src.position;
        dst.rotation = src.rotation;
       
        for (int i = 0; i < dst.childCount; i++)
        {
            // Match the transform with the same name
            var curSrc = src.Find(dst.GetChild(i).name);
            if (curSrc)
                CopyTransformsRecurse(curSrc, dst.GetChild(i));
        }
    }
}
