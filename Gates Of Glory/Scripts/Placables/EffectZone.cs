using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class EffectZone : MonoBehaviour
{

    private Transform mainCam;

    [HideInInspector]
    public bool canEffect;

    protected int myHealth;
    public int myMaxHealth;

    public Animator anim;

    public string myObjectPool;

    public GameObject statsPopupPanel;
    public TextMeshProUGUI statsText;

    private void Awake()
    {
        mainCam = Camera.main.transform;
    }

    private void OnEnable()
    {
        myHealth = myMaxHealth;
    }

    public virtual void Update()
    {
        if (statsPopupPanel.activeInHierarchy)
        {
            statsPopupPanel.transform.LookAt(mainCam);
        }
    }

    private void OnMouseEnter()
    {
        if (canEffect)
        {
            if (statsPopupPanel != null)
            {
                statsPopupPanel.SetActive(true);
            }
        }
    }

    private void OnMouseExit()
    {
        if (statsPopupPanel != null)
        {
            statsPopupPanel.SetActive(false);
        }
    }
}
