using UnityEngine;

public class GrenadeImpact : MonoBehaviour
{

    [SerializeField] Transform brokenGlassPrefab;

    [SerializeField] LayerMask glassLayerMask;

    void OnTriggerStay(Collider other)
    {
        if (other.transform.gameObject.layer == LayerMask.NameToLayer("Wall") && other.transform.TryGetComponent<WallPieceScript>(out WallPieceScript wallPieceScript))
        {
            wallPieceScript.TakeDamageFromGrenade(transform.position);
            Destroy(gameObject);
        }

        if (other.transform.gameObject.layer == LayerMask.NameToLayer("Glass"))
        {

            Transform brokenGlass = Instantiate(brokenGlassPrefab, new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z), other.transform.rotation);

            foreach (Transform child in brokenGlass)
            {
                if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidBody))
                {
                    childRigidBody.AddExplosionForce(400, other.transform.position, 30);
                }
            }
            Destroy(gameObject);
        }

        if (other.transform.gameObject.layer == LayerMask.NameToLayer("Assets"))
        {
            foreach (Transform child in other.transform.parent)
            {
                if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidBody))
                {
                    childRigidBody.isKinematic = false;
                    childRigidBody.AddExplosionForce(150, transform.position, 30);
                }
            }

            Destroy(gameObject);

        }


    }
}
