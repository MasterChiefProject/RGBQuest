using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    [Header("Setup")]
    public GameObject bulletPrefab;
    public Transform muzzlePoint;

    [Header("Ammo UI")]
    public Text ammoText;

    [Header("Firing")]
    public float bulletSpeed = 30f;
    public float fireRate = 0.2f;

    [Header("Rotation Offsets")]
    public Vector3 bulletRotationOffset = Vector3.zero;
    public Vector3 flashRotationOffset = Vector3.zero;

    [Header("Effects")]
    public GameObject muzzleFlashPrefab;

    [Header("Sounds")]
    public AudioSource shootingSound;
    public float shootingVolume = 1f;
    public AudioSource noAmmoSound;
    public float noAmmoVolume = 1f;

    private Animator animator;
    private float nextFireTime = 0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        updateAmmoUI();
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            if(Globals.ammo > 0)
            {
                Shoot();
            } 
            else
            {
                noAmmoSound.PlayOneShot(noAmmoSound.clip, noAmmoVolume);
            }
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            targetPoint = hit.point;
        else
            targetPoint = ray.origin + ray.direction * 1000f;

        Vector3 shootDir = (targetPoint - muzzlePoint.position).normalized;

        Quaternion bulletRot = Quaternion.LookRotation(shootDir) * Quaternion.Euler(bulletRotationOffset);

        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, bulletRot);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = shootDir * bulletSpeed;

        Destroy(bullet, 5f);

        animator.SetTrigger("Shoot");

        if (muzzleFlashPrefab != null)
        {
            Quaternion flashRot = bulletRot * Quaternion.Euler(flashRotationOffset);
            GameObject flash = Instantiate(muzzleFlashPrefab, muzzlePoint.position, flashRot);
            Destroy(flash, 0.5f);
        }

        shootingSound.PlayOneShot(shootingSound.clip, shootingVolume);
        Globals.ammo--;

        updateAmmoUI();
    }

    void updateAmmoUI()
    {
        ammoText.text = Globals.ammo.ToString() + '/' + Globals.magazineCapacity.ToString();
    }

}
