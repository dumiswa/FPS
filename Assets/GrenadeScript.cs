using UnityEngine;

public class GrenadeScript : MonoBehaviour
{

    [SerializeField] Material highlightedMaterial;
    [SerializeField] Material originalMaterial;
    [SerializeField] Transform suitcaseImpact;

    public bool beingUsed = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = originalMaterial;
        gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().material = originalMaterial;
    }

    public void BeingAimed()
    {
        gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = highlightedMaterial;
        gameObject.transform.GetChild(1).GetComponent<MeshRenderer>().material = highlightedMaterial;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.gameObject.layer != LayerMask.NameToLayer("Player") && beingUsed)
        {
            Instantiate(suitcaseImpact, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
