using UnityEngine;
using Photon.Pun;
using TMPro;
public class PlayerSetup : MonoBehaviourPun
{
    [SerializeField] private GameObject fpCamera;
    [SerializeField] private Movement movementScript;
        [Space]
    [SerializeField] private TextMeshProUGUI nameText;
    void Start()
    {
        fpCamera.SetActive(photonView.IsMine);
        movementScript.enabled = photonView.IsMine;
        nameText.gameObject.SetActive(!photonView.IsMine);

        nameText.text = photonView.Owner.NickName;
    }
}
