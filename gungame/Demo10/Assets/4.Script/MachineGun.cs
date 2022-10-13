using UnityEngine;
using System.Collections;

public class MachineGun : MonoBehaviour
{
    //宣告:
    //射程、射擊頻率、推力、傷害值、子彈數量、彈匣數量、裝填時間、火花特效、槍口閃光、下次射擊時間
    public float range = 100;
    public float fireRate = 0.05f;
    public float force = 10;
    public int damage = 5;
    public int bulletsPerClip = 40;
    public int clips = 20;
    public float reloadTime = 0.5f;

    ParticleSystem hitParticles;

    public Renderer muzzleFlash;

    int bulletsLeft = 0;
    float nextFireTime;
    int m_LastFrameShot = -1;

    //遊戲初始化
    //獲取子物件內的分子特效並隱藏、定義子彈初始數量
    void Start()
    {
        hitParticles = GetComponentInChildren<ParticleSystem>();
        if (hitParticles)
        {
            var main = hitParticles.main;
            main.loop = false;
        }
  
        bulletsLeft = bulletsPerClip;
    }

    //在Update函數調用後被調用
    //如果正在射擊，槍口閃光開啟、撥放音效、音效循環開啟，反之，槍口閃光關閉、音效循環關閉
    void LateUpdate()
    {
        if (muzzleFlash)
        {
            if (m_LastFrameShot == Time.frameCount)
            {
                muzzleFlash.transform.localRotation = Quaternion.AngleAxis(Random.value * 360, Vector3.forward);
                muzzleFlash.enabled = true;
                if (GetComponent<AudioSource>())
                {
                    if (!GetComponent<AudioSource>().isPlaying)
                        GetComponent<AudioSource>().Play();
                    GetComponent<AudioSource>().loop = true;
                }
            }
            else
            {
                muzzleFlash.enabled = false;
                enabled = false;

                if (GetComponent<AudioSource>())
                    GetComponent<AudioSource>().loop = false;
            }
        }
    }

    //如果子彈=0，執行功能"Reload"
    //迴圈，子彈不等於0且下次射擊時間<遊戲時間，執行功能"FireOneShot"
    void Fire()
    {
        if (bulletsLeft == 0)
            StartCoroutine(Reload());

        if (Time.time - fireRate > nextFireTime)
            nextFireTime = Time.time - Time.deltaTime;

        while (nextFireTime < Time.time && bulletsLeft != 0)
        {
            FireOneShot();
            nextFireTime += fireRate;
        }
    }

    //如果射到鋼體物件，傳達子彈力量給鋼體物件
    //在彈著點產生火花
    //對射擊到的物件傳達傷害值(要接受傷害值的物件腳本上需要有功能"ApplyDamage")
    //子彈數量減少
    void FireOneShot()
    {
        var direction = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, range))
        {
            if (hit.rigidbody != null)
                hit.rigidbody.AddForceAtPosition(force * direction, hit.point);

            if (hitParticles)
            {
                hitParticles.transform.position = hit.point;
                hitParticles.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                hitParticles.Emit(30);
            }

            hit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
        }
        bulletsLeft--;

        m_LastFrameShot = Time.frameCount;
        enabled = true;
    }

    //如果彈匣>0且子彈=0，彈匣減少、子彈=初始設定值
    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);

        if (clips > 0 && bulletsLeft == 0)
        {
            clips--;
            bulletsLeft = bulletsPerClip;
        }
    }

    public int GetBulletsLeft()
    {
        return bulletsLeft;
    }

    public int Getclips()
    {
        return clips;
    }
}
