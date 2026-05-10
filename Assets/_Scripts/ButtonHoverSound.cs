using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AudioSource audioSource;
    public AudioClip hoverSound;
    private bool hasPlayed = false;

public void OnPointerEnter(PointerEventData eventData)
{
    if (!hasPlayed)
    {
        audioSource.PlayOneShot(hoverSound);
        hasPlayed = true;
    }
}

public void OnPointerExit(PointerEventData eventData)
{
    hasPlayed = false;
}
}