using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M16Handler : MonoBehaviour
{
    private Recoil Recoil_Script;
    public GameObject muzzleFlashPrefab;
    public Transform vfx;

    public Camera playerCamera;  // Assign the main camera or a camera that follows the gun
    public float shootRange = 100.0f;  // Max distance the gun can shoot
    public LayerMask shootableLayer;  // Layers on which the shootable objects are placed
    public LayerMask assetsLayer;

    public Transform M16Holder;

    [SerializeField] public AudioSource M16Shooting;
    public AudioClip M16ShotSound;

    private float gunShotCooldown = 0.1f;
    private float lastGunShotTime;
    public float recoilThrowBack;

    public Quaternion originalRotation;
    public Quaternion targetRotation;
    private Quaternion randomRotation;

    [SerializeField] private GameObject bulletColissionEffect;

    //public bool isShooting;
    public bool IsMouseButtonDown { get; private set; }

    float newZPosition;
    float originalZPosition;

    bool isPistolEquipped;
    bool isRifleEquipped;

    public Vector3 originalPosition;


    private void Awake()
    {     
        
    }

    private void Start()
    {

        originalPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);

        Recoil_Script = GameObject.Find("CameraRecoil").GetComponent<Recoil>();

        originalZPosition = transform.localPosition.z;
        newZPosition = transform.localPosition.z + recoilThrowBack;

        originalRotation = transform.localRotation;
        targetRotation = originalRotation;

        isPistolEquipped = true;
        isRifleEquipped = false;
        //isShooting = false;
        IsMouseButtonDown = false;


    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time - lastGunShotTime >= gunShotCooldown)
        {
            IsMouseButtonDown = true;
            ShootGun();
        }     
        else if (Input.GetMouseButtonUp(0))
        {
            IsMouseButtonDown = false;
        }

       TestMethod();
    }

    void ShootGun()
    {
        Recoil_Script.GunRecoil();
        Vector3 targetPosition = transform.localPosition;

        M16Shooting.PlayOneShot(M16ShotSound);
        Debug.Log("Played sound");
        lastGunShotTime = Time.time;
        targetPosition.z += recoilThrowBack;
        //isShooting = true;

        GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, vfx.position, transform.rotation);
        if (M16Holder != null)
        {
            muzzleFlash.transform.parent = M16Holder;
        }

        Destroy(muzzleFlash, 0.1f);

       
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * 5f);

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, shootRange, shootableLayer))
        {
            Debug.Log("Hit " + hit.transform.name);  // Log the name of the hit object

            // Check if the hit object has the WallPieceScript and call TakeDamage
            WallPieceScript wallPieceScript = hit.collider.GetComponent<WallPieceScript>();
            if (wallPieceScript != null)
            {
                wallPieceScript.TakeDamage(hit.point);  // Pass the point of impact
                
                if( bulletColissionEffect != null )
                {
                    Instantiate(bulletColissionEffect, hit.point, Quaternion.identity);
                }
            }
        }

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, shootRange, assetsLayer))
        {
            DestructibleObject destructible = hit.collider.GetComponent<DestructibleObject>();
            if (destructible != null)
            {
                destructible.Shatter(); // Directly shatter the object
            }
        }
    }

   

    private void TestMethod()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, 8 * Time.deltaTime);
    }

}