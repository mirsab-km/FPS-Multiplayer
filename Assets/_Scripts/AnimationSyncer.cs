using UnityEngine;
using Photon.Pun;

public class AnimationSyncer : MonoBehaviour, IPunObservable
{
    public Animator animator;

    [Range(-2, 2)]
    public float horizontal, vertical;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(horizontal);
            stream.SendNext(vertical);
        }
        else
        {
            horizontal = (float)stream.ReceiveNext();
            vertical = (float)stream.ReceiveNext();
        }
    }

    [PunRPC] 
    public void RPC_PlayerJump()
    {
        animator.SetTrigger("Jump");
    }  

    void Update()
    {
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
    }
}


