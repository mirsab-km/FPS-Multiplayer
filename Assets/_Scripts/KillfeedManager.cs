using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections;
public class KillfeedManager : MonoBehaviourPun
{
    public static KillfeedManager Instance { get; private set; }
    [Header("UI")]
    public Transform killfeedItemParent; 
    public GameObject killfeedItemPrefab;

    void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [PunRPC]
    public void RPC_GetKill(string _killer, string _victim)
    {
        GameObject item = Instantiate(killfeedItemPrefab, killfeedItemParent);
        item.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{_killer} killed {_victim}";
        StartCoroutine(DelayedEnableKillfieldItem(item.transform.GetChild(0).gameObject));
        Destroy(item, 1f);
    }

    private IEnumerator DelayedEnableKillfieldItem(GameObject itemText)
    {
        itemText.gameObject.SetActive(false);
        yield return null;
        itemText.gameObject.SetActive(true);
    }
}
