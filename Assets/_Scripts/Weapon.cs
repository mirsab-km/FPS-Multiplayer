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
    [Space]
    public float spread = 0.05f;
    public int pellectsCount = 1;

    [Header("Animation Setup")]
    public Animation anim;
    public AnimationClip shootClip;
    public AnimationClip reloadClip;
    public AnimationClip startClip;

    [Header("Scoping Settings")]
    public ScopeManager scopeManagerScript;
    public bool isScopedWeapon = false;
    public SkinnedMeshRenderer[] meshRenderers;
    public float spreadWhileScoped;

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
    private bool isScoped = false;
    void Start()
    {
        UpdateAmmoUI();
        SetScopeState(false);

        anim.clip = startClip;
        anim.Stop();
        anim.Play();
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

        SetScopeState(isScopedWeapon && Input.GetButton("Fire2"));
    }

    private void SetScopeState(bool _isScoped)
    {
        scopeManagerScript.SetScopeState(_isScoped);
        isScoped = _isScoped;

        foreach (var _renderer in meshRenderers)
        {
            _renderer.enabled = !_isScoped;
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

        for (int i = 0; i < pellectsCount; i++)
        {
            //Spread Create
            float _spreadToUse = isScoped ? spreadWhileScoped : spread;

            Vector3 forwardDirection = cameraTranform.forward;

            Vector3 randomSpread = cameraTranform.right * Random.Range(-_spreadToUse, _spreadToUse) + 
                                   cameraTranform.up * Random.Range(-_spreadToUse, _spreadToUse);

            Vector3 finalDirection = forwardDirection + randomSpread;
            finalDirection.Normalize();

            Ray ray = new Ray(cameraTranform.position, finalDirection);
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
                        playerHitAndKillManagerScript.GetKill(hit.transform.GetComponent<PhotonView>().Owner.NickName);
                    }
                    else
                    {
                        //Damage
                        playerHitAndKillManagerScript.GetHit(damagePerShot);
                    }

                }
                else
                {
                    PhotonNetwork.Instantiate(concreteHitParicle.name, hit.point, rotation);
                }
            }
        }
    }
}