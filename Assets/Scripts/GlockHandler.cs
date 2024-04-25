using System.Collections;
using UnityEngine;

public class GlockHandler : MonoBehaviour
{
    private Recoil Recoil_Script;
    public GameObject muzzleFlashPrefab;
    public Transform vfx;

    Animator glock18;
    public Transform glockHolder;

    [SerializeField] public AudioSource glockShooting;
    public AudioClip glockShotSound;

    private float glockshotCooldown = 0.1f;
    private float lastGlockShotTime;
    public float recoilThrowBack = -2.5f;

    public Quaternion originalRotation;
    public Quaternion targetRotation;
    private Quaternion randomRotation;

    public bool IsMouseButtonDown { get; private set; }

    //private bool isShooting;

    float newZPosition;
    float originalZPosition;

    bool isPistolEquipped;
    bool isRifleEquipped;

    private void Awake()
    {
       
    }

    private void Start()
    {
        glock18 = GetComponent<Animator>();
        Recoil_Script = GameObject.Find("CameraRecoil").GetComponent<Recoil>();

        glockShooting.clip = glockShotSound;
        glockShooting.playOnAwake = false;

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
        if (Input.GetMouseButtonDown(0) && Time.time - lastGlockShotTime >= glockshotCooldown)
        {
            IsMouseButtonDown = true;
            ShootPistol();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            IsMouseButtonDown = false;
        }
    }

    void ShootPistol()
    {
        Vector3 targetPosition = transform.localPosition;


        glock18.SetTrigger("Shoot");
        glockShooting.PlayOneShot(glockShotSound);
        lastGlockShotTime = Time.time;
        targetPosition.z += recoilThrowBack;
        //isShooting = true;

        GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, vfx.position, transform.rotation);
        if (glockHolder != null)
        {
            muzzleFlash.transform.parent = glockHolder;
        }

        Destroy(muzzleFlash, 0.1f);


        /* float randomXRotation = Random.Range(-7f, 7f);
         float randomYRotation = Random.Range(-3f, 3f);
         float randomZRotation = Random.Range(-2f, 2f);        
         randomRotation = Quaternion.Euler(randomXRotation, randomYRotation, randomZRotation);     
         targetRotation *= randomRotation;*/


        StartCoroutine(ResetGunTransform());


        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * 2f);
        //transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * 2f);

        Recoil_Script.GunRecoil();

    }

    IEnumerator ResetGunTransform()
    {
        float elapsedTime = 0f;
        Vector3 originalPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, originalZPosition);

        while (elapsedTime < 0.1f) 
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, elapsedTime / 0.1f);
            //transform.localRotation = Quaternion.Slerp(transform.localRotation, originalRotation, elapsedTime / 0.1f);

            elapsedTime += Time.deltaTime;

            //isShooting = false;
            yield return null;
        }

        transform.localPosition = originalPosition;

        //transform.localRotation = originalRotation;
        //randomRotation = Quaternion.identity;

       
    }
}