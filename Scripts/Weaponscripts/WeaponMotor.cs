using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMotor : MonoBehaviour
{
    public GunData gunData;
    public Transform muzzle;

    float timeSinceLastShot;
    public GameObject bulletImpact;

    WaitForSeconds rapidFireWait;

    bool rapidFire = false;

    private void Start()
    {
        gunData.currentAmmo = 10;
    }

    private void Awake()
    {
        rapidFireWait = new WaitForSeconds(1 / gunData.fireRate);
    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    public void Shoot()
    {
        if (gunData.currentAmmo > 0)
        {
            if (CanShoot())
            {
                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }

        }
    }

    public void StartReload()
    {
        if (!gunData.reloading)
        {
            if (gunData.currentAmmo != gunData.magSize)
                StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {

        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        gunData.currentAmmo = gunData.magSize;

        gunData.reloading = false;
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        Debug.DrawRay(muzzle.position, muzzle.forward);
    }

    private void OnGunShot()
    {
        if (Physics.Raycast(muzzle.position, transform.forward, out RaycastHit hitInfo, gunData.maxDistance))
        {
            Instantiate(bulletImpact, hitInfo.point, transform.rotation);
        }
    }

    public void SwitchFireMode()
    {
        rapidFire = !rapidFire;
        if (rapidFire == true)
        {
        }
        else if (rapidFire == false)
        {
        }
    }

    public IEnumerator RapidFire()
    {
        if (rapidFire == true)
        {
            while (true)
            {
                Shoot();
                yield return new WaitForSeconds(1 / gunData.fireRate);
            }
        }
        else
        {
            Shoot();
            yield return null;
        }
    }
}
