using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FPSPlayer : MonoBehaviour
{
    //宣告:
    //生命值、子彈GUI、火箭彈GUI、血條GUI、受傷聲音、死亡聲音  
    public int hp = 300;
    public Image hpUi;   
    public Image rocketUi;
    public Text bulletUi;
    public AudioClip painLittle;
    public AudioClip painBig;
    public AudioClip die;

    MachineGun machineGun;
    RocketLauncher rocketLauncher;

    float gotHitTimer = -1;
    int maxHp;    
    int maxRocket = 20;

    AudioSource myAudio;

    Rect hpRect;

    //初始化
    void Awake()
    {
        maxHp = hp;
        machineGun = GetComponentInChildren<MachineGun>(true);
        rocketLauncher = GetComponentInChildren<RocketLauncher>(true);
        myAudio = GetComponent<AudioSource>();
    }

    // 更新UI
    void Update()
    {
        hpUi.fillAmount = (float)hp / maxHp;

        if (machineGun)
            bulletUi.text = machineGun.GetBulletsLeft().ToString() + "/" + machineGun.Getclips().ToString();
    
        if (rocketLauncher)
            rocketUi.fillAmount = (float)rocketLauncher.ammoCount / maxRocket;
    }

    //撥放受傷音效
    //如果生命值=0，執行功能"Die"
    void ApplyDamage(int damage)
    {
        if (hp < 0.0f)
            return;

        // Apply damage
        hp -= damage;        

        // Play pain sound when getting hit - but don't play so often
        if (Time.time > gotHitTimer && painBig && painLittle)
        {
            // Play a big pain sound
            if (hp < maxHp * 0.2f || damage > 20f)
            {
                myAudio.PlayOneShot(painBig, 1.0f / myAudio.volume);
                gotHitTimer = Time.time + Random.Range(painBig.length * 2, painBig.length * 3);
            }
            else
            {
                // Play a small pain sound
                myAudio.PlayOneShot(painLittle, 1.0f / myAudio.volume);
                gotHitTimer = Time.time + Random.Range(painLittle.length * 2, painLittle.length * 3);
            }
        }

        // Are we dead?
        if (hp <= 0)
            Die();
    }

    //撥放死亡音效
    //發送訊息到功能"DidPause"
    //產生白色畫面後，重新讀取關卡
    void Die()
    {
        if (die)
            AudioSource.PlayClipAtPoint(die, transform.position);
   
        LevelLoadFade.FadeAndLoadLevel(SceneManager.GetActiveScene().buildIndex, Color.white, 2.0f);
    }   
}
