using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    //宣告:生命值、爆炸特效、替代物
    public int hp = 100;
    public Transform explosion;
    public Rigidbody deadReplacement;

    //如果生命值=0，執行功能"Dead"
    void ApplyDamage(int damage)
    {
        if (hp <= 0)
            return;

        if (enabled)
            hp -= damage;

        if (hp <= 0.0f)
            Dead();
    }

    //刪除自己
    //產生爆炸特效、產生替代物
    void Dead()
    {
        Destroy(gameObject);

        if (explosion)
            Instantiate(explosion, transform.position, transform.rotation);

        if (deadReplacement)
        {
            Rigidbody dead = Instantiate(deadReplacement, transform.position, transform.rotation) as Rigidbody;

            dead.velocity = GetComponent<Rigidbody>().velocity;
            dead.angularVelocity = GetComponent<Rigidbody>().angularVelocity;
        }
    }
}
