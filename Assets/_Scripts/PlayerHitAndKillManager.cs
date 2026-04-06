using UnityEngine;

public class PlayerHitAndKillManager : MonoBehaviour
{
    [Header("UI")]
    public Animation hitmarkerAnimation;
    public AudioSource hitmarkerAudioSource;
    [Space]
    public Animation killmarkerAnimation;
    public AudioSource killmarkerAudioSource;

    public void GetHit()
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
    }
}