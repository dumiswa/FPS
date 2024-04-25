using System;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;

    public GameObject grenadePrefab;  // Assign this in the inspector
    public Transform throwPoint;  // A point from where the grenade is thrown
    public float throwForce;

    [SerializeField] private GameObject rpgRocket;
    [SerializeField] private GameObject rocketPrefab;
      [SerializeField] private GameObject loadedRocket; 
    [SerializeField] private Transform rocketSpawnPoint;
    [SerializeField] private float rocketForce;
    private bool rocketEquipped;

    [SerializeField] AudioSource grenadeOut;

    void Awake()
    {
        slot1.SetActive(true);
        slot2.SetActive(false);
        slot3.SetActive(false);
        loadedRocket = this.rpgRocket;
    }

    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            Equip1();
            rocketEquipped = false;
        }

        if (Input.GetKeyDown("2"))
        {
            Equip2();
            rocketEquipped = false;
        }

        if (Input.GetKeyDown("3"))
        {
            Equip3();
            rocketEquipped = true;
        }

        if (rocketEquipped && Input.GetMouseButtonDown(0) && loadedRocket != null)
        {
            ShootRocket();
        }

        if (Input.GetKeyDown(KeyCode.R) && rocketEquipped)
        {
            ReloadRocket();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowGrenade();
        }
    }

    private void ReloadRocket()
    {
        if (rocketPrefab != null && loadedRocket == null)
        {
            loadedRocket = Instantiate(rocketPrefab, rocketSpawnPoint.position, rocketSpawnPoint.rotation);
            loadedRocket.transform.localScale = Vector3.one;  // Ensure the scale is reset
            loadedRocket.GetComponent<Rigidbody>().isKinematic = true; // Make it kinematic until shot
            //Debug.Log($"Rocket Scale after Instantiation: {loadedRocket.transform.localScale}");
        }
    }

    private void ShootRocket()
    {
        if (loadedRocket != null)
        {
            loadedRocket.transform.parent = null; // Detach from the spawn point
            Rigidbody rocketRb = loadedRocket.GetComponent<Rigidbody>();
            rocketRb.isKinematic = false;
            rocketRb.AddForce(rocketSpawnPoint.forward * rocketForce, ForceMode.VelocityChange);
            loadedRocket = null; // Clear the loaded rocket
        }
    }

    void Equip1()
    {
        slot1.SetActive(true);
        slot2.SetActive(false);
        slot3.SetActive(false);
    }

    void Equip2()
    {
        slot1.SetActive(false);
        slot2.SetActive(true);
        slot3.SetActive(false);
    }

    void Equip3()
    {
        slot1.SetActive(false);
        slot2.SetActive(false);
        slot3.SetActive(true);
    }

    public void ThrowGrenade()
    {
        GameObject grenade = Instantiate(grenadePrefab, throwPoint.position, throwPoint.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(throwPoint.forward * throwForce, ForceMode.VelocityChange);
        grenadeOut.Play();
    }
}