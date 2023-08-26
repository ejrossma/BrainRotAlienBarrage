using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    [Header("Gun Stats")]
    [SerializeField] int damage;
    [SerializeField] float timeBetweenShooting, spread, range, timeBetweenShots;
    [SerializeField] int amountOfAmmo, maxAmmo, bulletsPerShot;
    [SerializeField] bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    [Header("Double Time")]
    private float timeBetweenShootingModifier;
    [SerializeField] float doubleTimeModifier;

    bool shooting, readyToShoot, reloading;

    [Header("Reference")]
    [SerializeField] Player p;
    [SerializeField] Camera playerCamera;
    [SerializeField] Transform attackPoint;
    [SerializeField] RaycastHit rayHit;
    [SerializeField] LayerMask enemyLayers;

    [Header("Visuals")]
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] GameObject bulletHoleGraphic;
    [SerializeField] CameraShake cameraShake;
    [SerializeField] float shakeMagnitude, shakeDuration;
    [SerializeField] TextMeshProUGUI ammoText;

    void Awake()
    {
        bulletsLeft = amountOfAmmo;
        readyToShoot = true;
    }

    void Update()
    {
        if (p.doubleTime)
            timeBetweenShootingModifier = doubleTimeModifier;
        else
            timeBetweenShootingModifier = 1f;
        GatherInputs();

        ammoText.SetText("" + amountOfAmmo);
    }

    private void GatherInputs()
    {
        if (allowButtonHold)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (readyToShoot && shooting && (amountOfAmmo > 0 || p.doubleTime) )
        {
            bulletsShot = bulletsPerShot;
            bulletsLeft = amountOfAmmo * bulletsPerShot;
            Shoot();
            if (!p.doubleTime)
                amountOfAmmo--;
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread/2, spread/2);

        //Calculate direction
        Vector3 direction = playerCamera.transform.forward + new Vector3(x, y, 0);

        //Raycast
        if (Physics.Raycast(playerCamera.transform.position, direction, out rayHit, range, enemyLayers))
        {
            if (rayHit.collider.CompareTag("Enemy"))
                rayHit.collider.GetComponentInParent<Enemy>().TakeDamage(damage);
        }

        //Shake camera
        cameraShake.Shake(shakeDuration, shakeMagnitude);

        //Visual Feedback
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.FromToRotation(Vector3.forward, rayHit.normal));
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;
        Invoke(nameof(ResetShot), timeBetweenShooting * timeBetweenShootingModifier);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke(nameof(Shoot), timeBetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    public void AddAmmo(int amount)
    {
        amountOfAmmo += amount;
        if (amountOfAmmo > maxAmmo) amountOfAmmo = maxAmmo;
    }

    public bool IsAmmoFull() { return amountOfAmmo == maxAmmo; }
}
