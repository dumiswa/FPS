using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public GameObject brokenVersionPrefab; // Prefab of the broken version for this specific object

    [SerializeField] private AudioSource destructionSound;

    // Function to handle the destruction of the object
    public void Shatter()
    {
        if (destructionSound != null)
        {
            AudioSource.PlayClipAtPoint(destructionSound.clip, transform.position);
        }

        // Instantiate the broken version
        GameObject brokenVersion = Instantiate(brokenVersionPrefab, transform.position, transform.rotation);

        // Apply a force to all Rigidbody components of the broken pieces to simulate explosion
        foreach (Transform piece in brokenVersion.transform)
        {
            Rigidbody rb = piece.GetComponent<Rigidbody>();          
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(400, transform.position, 5);
            }
        }

        // Destroy the original object
        Destroy(gameObject);
    }
}
