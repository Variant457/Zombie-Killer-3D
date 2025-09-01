
using UnityEngine;
using TMPro;
using System.Diagnostics;


public class GunSystem : MonoBehaviour
{
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;


    //bools 
    bool shooting, readyToShoot, reloading;


    //Reference
    public Camera fpsCam;
    public Transform muzzlePoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    public AudioClip Audioclip;
    public AudioSource audioSource;

    //Graphics
    public GameObject muzzleFlash;
    public TextMeshProUGUI text;

    // Profiling
    [SerializeField] private ProfilerDataExporter profilerExporter;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    private void Update()
    {
        var stopwatch = Stopwatch.StartNew();
        MyInput();

        //SetText
        text.SetText(bulletsLeft + " / " + magazineSize);
        stopwatch.Stop();
        profilerExporter?.RecordEvent("GunSystem.Update", stopwatch.Elapsed.TotalMilliseconds);
    }
    private void MyInput()
    {
        var stopwatch = Stopwatch.StartNew();
        if (allowButtonHold) shooting = Input.GetMouseButton(0);
        else shooting = Input.GetMouseButton(0);


        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();


        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
        stopwatch.Stop();
        profilerExporter?.RecordEvent("GunSystem.MyInput", stopwatch.Elapsed.TotalMilliseconds);
    }
    private void Shoot()
    {
        var stopwatch = Stopwatch.StartNew();
        readyToShoot = false;


        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);


        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);


        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            //Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Enemy"))
            {
                rayHit.collider.GetComponentInParent<ZombieManager>().TakeDmg(damage);
            }
        }

        //Graphics
        GameObject flash = Instantiate(muzzleFlash, muzzlePoint.position, Quaternion.identity);
        Destroy(flash, 0.1f);
        
        audioSource.clip = Audioclip;
        audioSource.Play();

        bulletsLeft--;
        bulletsShot--;


        Invoke("ResetShot", timeBetweenShooting);


        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);

        stopwatch.Stop();
        profilerExporter?.RecordEvent("GunSystem.Shoot", stopwatch.Elapsed.TotalMilliseconds);
    }
    private void ResetShot()
    {
        var stopwatch = Stopwatch.StartNew();
        readyToShoot = true;
        stopwatch.Stop();
        profilerExporter?.RecordEvent("GunSystem.ResetShot", stopwatch.Elapsed.TotalMilliseconds);
    }
    private void Reload()
    {
        var stopwatch = Stopwatch.StartNew();
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
        stopwatch.Stop();
        profilerExporter?.RecordEvent("GunSystem.Reload", stopwatch.Elapsed.TotalMilliseconds);
    }
    private void ReloadFinished()
    {
        var stopwatch = Stopwatch.StartNew();
        bulletsLeft = magazineSize;
        reloading = false;
        stopwatch.Stop();
        profilerExporter?.RecordEvent("GunSystem.ReloadFinished", stopwatch.Elapsed.TotalMilliseconds);
    }
}
