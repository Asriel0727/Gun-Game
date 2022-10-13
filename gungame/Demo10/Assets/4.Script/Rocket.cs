using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Rocket : MonoBehaviour
{
    //宣告:爆炸特效、終止時間
    public GameObject explosion;
    public float timeOut = 3.0f;

    //功能:遊戲初始化
    //刪除物件
    void Start()
    {
        Destroy(gameObject, timeOut);
    }

    //功能:觸發區域
    //在碰撞到的地方動態產生一個爆炸特效
    //刪除物件
    void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        var rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Instantiate(explosion, contact.point, rotation);

        Destroy(gameObject);
    }    
}
