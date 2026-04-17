using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    private int selectedWeapon;

    public PhotonView tpWeaponManager;
    private float timeUntilAllowSelectNextWeapon;

    private int previouslySelectedWeapon = -1;

    void Update()
    {
        timeUntilAllowSelectNextWeapon = Mathf.Max(0, timeUntilAllowSelectNextWeapon - Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedWeapon = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedWeapon = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedWeapon = 2;

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && timeUntilAllowSelectNextWeapon <= 0f)
        {
            timeUntilAllowSelectNextWeapon = 0.1f;

            if (selectedWeapon >= transform.childCount - 1)
            {
                selectedWeapon = 0;
            }
            else
            {
                selectedWeapon += 1;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && timeUntilAllowSelectNextWeapon <= 0f)
        {
            timeUntilAllowSelectNextWeapon = 0.1f;

            if (selectedWeapon <= 0)
            {
                selectedWeapon = transform.childCount - 1;
            }
            else
            {
                selectedWeapon -= 1;
            }
        }
        SelectWeapon();
    }

    private void SelectWeapon()
    {
        selectedWeapon = Mathf.Clamp(selectedWeapon, 0, transform.childCount - 1);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == selectedWeapon);
        }

        if (selectedWeapon != previouslySelectedWeapon)
        {
            tpWeaponManager.RPC("RPC_SetTPWeapon", RpcTarget.OthersBuffered, (byte)selectedWeapon);
        }
        previouslySelectedWeapon = selectedWeapon;
    }
}
