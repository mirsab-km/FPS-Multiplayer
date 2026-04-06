using UnityEngine;
using Photon.Pun;
using System.Collections;
public class ParticlesLifeTime : MonoBehaviourPun
{
    public float lifeTime;
    void Start()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(DelayedDestroy());
        }
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        PhotonNetwork.Destroy(gameObject);
    }
}