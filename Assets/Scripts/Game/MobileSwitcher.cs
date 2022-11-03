using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameObject pcObj;
    [SerializeField]
    private GameObject mobileObj;

    private void OnEnable() { GameManager.Instance.OnMobileStatusChange += SwitchObjs; }
    private void OnDisable() { GameManager.Instance.OnMobileStatusChange -= SwitchObjs; }

    private void Start()
    {
        SwitchObjs(GameManager.Instance.IsMobile);
    }

    public void SwitchObjs(bool isMobile)
    {
        pcObj.SetActive(!isMobile);
        mobileObj.SetActive(isMobile);
    }
}
