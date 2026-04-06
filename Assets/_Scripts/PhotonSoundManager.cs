using UnityEngine;
using Photon.Pun;
public class PhotonSoundManager : MonoBehaviourPun
{
    [Header("Shoot Audio")]
    public AudioClip[] shootSFX;
    public AudioSource audioSource;

    [PunRPC]
    public void RPC_ShootSound(byte _index)
    {
        audioSource.clip = shootSFX[_index];
        audioSource.Stop();
        audioSource.Play();
    }
}