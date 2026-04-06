using UnityEngine;
public class ReloadSoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    [Space]
    [Header("Mag Audio")]
    public AudioClip magInSound;
    public AudioClip magOutSound;
    public AudioClip slideSound;
    public void PlayMagInSFX()
    {
        audioSource.PlayOneShot(magInSound);
    }

    public void PlayMagOutSFX()
    {
        audioSource.PlayOneShot(magOutSound);
    }

    public void PlaySlidingSFX()
    {
        audioSource.PlayOneShot(slideSound);
    }
}