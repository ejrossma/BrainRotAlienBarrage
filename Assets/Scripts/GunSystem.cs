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


    bool shooting, readyToShoot, reloading;

    //Reference
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
        GatherInputs();

        ammoText.SetText("Ammo Remaining: " + amountOfAmmo);
    }

    private void GatherInputs()
    {
        if (allowButtonHold)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (readyToShoot && shooting && amountOfAmmo > 0)
        {
            bulletsShot = bulletsPerShot;
            bulletsLeft = amountOfAmmo * bulletsPerShot;
            Shoot();
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
                rayHit.collider.GetComponent<Enemy>().TakeDamage(damage);
        }

        //Shake camera
        cameraShake.Shake(shakeDuration, shakeMagnitude);

        //Visual Feedback
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.FromToRotation(Vector3.forward, rayHit.normal));
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;
        Invoke(nameof(ResetShot), timeBetweenShooting);

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
