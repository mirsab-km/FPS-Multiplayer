using System;
using System.Linq.Expressions;
using Photon.Pun;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class LocalPlayerKDManager : MonoBehaviour
{
    public static LocalPlayerKDManager Instance { get; private set; }
    public int localPlayerKills;
    public int localPlayerDeaths;

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

    public void GetKill()
    {
        localPlayerKills++;
        SetHashes();
    }

    public void OnDied()
    {
        localPlayerDeaths++;
        SetHashes();
    }

    private void SetHashes()
    {
        try
        {
            Hashtable hash = new Hashtable
            {
                ["Kills"] = localPlayerKills,
                ["Deaths"] = localPlayerDeaths
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
        catch(Exception e)
        {
            Debug.LogError("Error setting player properties: " + e.Message);
        }
    }
}