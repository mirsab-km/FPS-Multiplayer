using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public float fireRate = 10f;
    public int damagePerShot = 25;
    public float rayCastDistance = 500f;

    [Header("Animation Setup")]
    public Animation anim;
    public AnimationClip shootClip;
    public AnimationClip reloadClip;

    [Header("Hitmarker and Kills Manager")]
    public PlayerHitAndKillManager playerHitAndKillManagerScript;

    [Header("Hit Particle Setups")]
    public GameObject concreteHitParicle;
    public GameObject bloodHitParicle;

    [Header("Muzzleflash Setups")]
    public Transform muzzleFlashSpawnPoint;
    public GameObject muzzleFlashPrefab;

    [Header("Shoot SFX")]
    public PhotonSoundManager PhotonSoundManagerScript;
    [Space]
    public byte shootSoundSFX = 0;

    [Header("Ammo Setups")]
    public int magSize = 30;
    public int currentAmmoInMag = 30;
    [Space]
    public TextMeshProUGUI ammoText;
    public Image ammoImage;

    [Header("Camera Reference")]
    public Transform cameraTranform;

    private float timeUntilAllowNextShot;
    void Start()
    {
        UpdateAmmoUI();
    }

    void Update()
    {
        timeUntilAllowNextShot = Mathf.Max(0, timeUntilAllowNextShot - Time.deltaTime);

        if (Input.GetButton("Fire1") && timeUntilAllowNextShot <= 0f && currentAmmoInMag > 0 && !IsPlayingReloadClip())
        {
            HitScanShoot();
            timeUntilAllowNextShot = 1 / fireRate;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    private void Reload()
    {
        anim.clip = reloadClip;
        anim.Stop();
        anim.Play();

        currentAmmoInMag = magSize;
        UpdateAmmoUI();
    }

    private bool IsPlayingReloadClip()
    {
        return anim.isPlaying && anim.clip == reloadClip;
    }

    private void UpdateAmmoUI()
    {
        ammoText.text = $"{currentAmmoInMag}/{magSize}";
        ammoImage.fillAmount = (float)currentAmmoInMag / magSize;
    }

    void HitScanShoot()
    {
        currentAmmoInMag--;
        UpdateAmmoUI();

        anim.clip = shootClip;
        anim.Stop();
        anim.Play();

        PhotonSoundManagerScript.photonView.RPC("RPC_ShootSound", RpcTarget.All, shootSoundSFX);

        GameObject spawnedMuzzleFlash = Instantiate(muzzleFlashPrefab, muzzleFlashSpawnPoint.position, muzzleFlashSpawnPoint.rotation);
        spawnedMuzzleFlash.transform.parent = muzzleFlashSpawnPoint;
        Destroy(spawnedMuzzleFlash, 0.08f);

        Ray ray = new Ray(cameraTranform.position, cameraTranform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayCastDistance))
        {
            Quaternion rotation = Quaternion.LookRotation(hit.normal);

            if (hit.transform.CompareTag("Player"))
            {
                hit.transform.GetComponent<PhotonView>().RPC("RPC_TakeDamage", RpcTarget.AllBuffered, damagePerShot);
                PhotonNetwork.Instantiate(bloodHitParicle.name, hit.point, rotation);

                if (hit.transform.GetComponent<PlayerHealth>().health <= 0f)
                {
                    //Kill
                    playerHitAndKillManagerScript.GetKill();
                }
                else
                {
                    //Damage
                    playerHitAndKillManagerScript.GetHit();
                }

            }
            else
            {
                PhotonNetwork.Instantiate(concreteHitParicle.name, hit.point, rotation);
            }
        }
    }
}