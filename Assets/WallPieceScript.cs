using UnityEngine;

public class WallPieceScript : MonoBehaviour
{
    [SerializeField] Material[] materials;
    private Rigidbody rb;
    private MeshRenderer mr;
    private MeshCollider mc;
    private int amountOfHits = 0;
    private bool isDestroyed = false;
    private bool toDisappear = false;
    private float t = 0;
    [SerializeField] private AudioSource rockBreaking;
    [SerializeField] private AudioSource rockHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mr = GetComponent<MeshRenderer>();
        mc = GetComponent<MeshCollider>();
    }

    public void TakeDamage(Vector3 pointOfImpact)
    {
        Debug.Log($"Taking damage on {gameObject.name}, hits: {amountOfHits}, destroyed: {isDestroyed}, materials count: {materials.Length}");
        if ( !isDestroyed)
        {
            if (amountOfHits == 2)
            {
                //transform.localScale = transform.localScale * 0.7f;
                rb.isKinematic = false;
                mc.isTrigger = false;
                rb.AddExplosionForce(200f, pointOfImpact, 15);
                isDestroyed = true;
                Invoke("ToDisappear", 4);
                if (rockBreaking != null) rockBreaking.Play();           
            }
            else 
            {
                if (rockHit != null) rockHit.Play();
                               
                amountOfHits++;
                if(materials.Length > amountOfHits)
                {
                    mr.material = materials[amountOfHits];
                }
                
            }
        }
       
    }

    public void TakeDamageFromGrenade(Vector3 pointOfImpact)
    {
        //transform.localScale = transform.localScale * 0.7f;
        rb.isKinematic = false;
        mc.isTrigger = false;
        rb.AddExplosionForce(400, pointOfImpact, 30);
        isDestroyed = true;
        Invoke("ToDisappear", 4);

        if (rockBreaking != null && !rockBreaking.isPlaying)
            rockBreaking.Play();
    }

    private void ToDisappear()
    {
        toDisappear = true;
    }

    private void Update()
    {
        if (toDisappear)
        {
            t += Time.deltaTime / 2;

            Vector3 newScale = Vector3.Lerp(transform.localScale, new Vector3(1, 0.1f, 1), t);
            transform.localScale = newScale;

            if (transform.localScale.x <= 1.5f)
            {               
                Destroy(gameObject);
            }
        }
    }
}
