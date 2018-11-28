using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonZone : EffectZone 
{

    public GameObject poisonObject;

    private void Awake()
    {
        poisonObject.SetActive(false);
    }

    public override void Update()
    {
        if (canEffect && !poisonObject.activeInHierarchy)
        {
            poisonObject.SetActive(true);
        }
    }
}
