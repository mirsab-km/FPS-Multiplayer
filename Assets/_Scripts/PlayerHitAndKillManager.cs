using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class PlayerHitAndKillManager : MonoBehaviour
{
    [Header("UI")]
    public Animation hitmarkerAnimation;
    public AudioSource hitmarkerAudioSource;
    [Space]
    public Animation killmarkerAnimation;
    public AudioSource killmarkerAudioSource;

    public void GetHit(int _damage)
    {
        hitmarkerAnimation.Stop();
        hitmarkerAnimation.Play();

        hitmarkerAudioSource.Stop();
        hitmarkerAudioSource.Play();
    }

    public void GetKill()
    {
        killmarkerAnimation.Stop();
        killmarkerAnimation.Play();

        killmarkerAudioSource.Stop();
        killmarkerAudioSource.Play();

        PhotonNetwork.LocalPlayer.AddScore(25);
        LocalPlayerKDManager.Instance.GetKill();
    }
}