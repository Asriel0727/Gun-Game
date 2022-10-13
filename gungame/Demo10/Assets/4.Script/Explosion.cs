using UnityEngine;

public class Explosion : MonoBehaviour
{
    //宣告:爆炸範圍、力量、傷害值、結束時間
    public float explosionRadius = 5;
    public float explosionPower = 10;
    public int explosionDamage = 100;
    public float explosionTimeout = 2;

    //初始化
    //對爆炸範圍內目標(ApplyDamage)發送傷害值，並對Rigidbody發送爆炸力量
    //刪除自己
    void Start()
    {
        var explosionPosition = transform.position;

        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

        foreach (var hit in colliders)
        {
            var closestPoint = hit.ClosestPointOnBounds(explosionPosition);
            var distance = Vector3.Distance(closestPoint, explosionPosition);

            float hitPoints = 1 - Mathf.Clamp01(distance / explosionRadius);
            hitPoints *= explosionDamage;

            hit.SendMessageUpwards("ApplyDamage", (int)hitPoints, SendMessageOptions.DontRequireReceiver);
        }

        colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Rigidbody>())
                hit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPosition, explosionRadius, 3.0f);
        }

        Destroy(gameObject, explosionTimeout);
    }
}
