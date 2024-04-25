using UnityEngine;

public class BulletImpactHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        var damageable = collision.gameObject.GetComponent<WallPieceScript>();
        if (damageable != null)
        {
            damageable.TakeDamage(transform.position);
        }
        Destroy(gameObject); 
    }
}