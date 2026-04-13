using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviourPun
{
    [Header("Health Setup")]
    public int maxHealth = 100;
    [HideInInspector] public int health = 100;

    [Header("UI Setup")]
    public TextMeshProUGUI healthText;
    public Image healthImage;

    private void Start()
    {
        health = maxHealth;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        healthText.text = $"{health}/{maxHealth}";
        healthImage.fillAmount = (float)health / maxHealth;
    }

    [PunRPC]
    public void RPC_TakeDamage(int _damage)
    {
        health = Mathf.Max(0, health -= _damage);

        if (photonView.IsMine)
        {
            UpdateHealthUI();

            if (health <= 0f)
            {
                LocalPlayerKDManager.Instance.OnDied();
                //Death Code
                RoomManager.Instance.RespawnPlayer();
                PhotonNetwork.Destroy(gameObject);
            }
        }
        else
        {
            if (health <= 0f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}