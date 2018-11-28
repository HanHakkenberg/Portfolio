using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSettings : MonoBehaviour 
{

    [SerializeField]
    private List<Light> lightsToChange = new List<Light>();
    private List<float> defaultSettings = new List<float>();
    [SerializeField]
    private float intensityMultiplier;
    [SerializeField]
    private int maxSetting = 3;
    private int currentSetting;

    private void Awake()
    {
        for (int i = 0; i < lightsToChange.Count; i++)
        {
            defaultSettings.Add(lightsToChange[i].intensity);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ChangeLightIntensity();
        }
    }

    private void ChangeLightIntensity()
    {
        if (currentSetting == maxSetting)
        {
            currentSetting = 0;
        }
        else
        {
            currentSetting++;
        }

        for (int i = 0; i < lightsToChange.Count; i++)
        {
            if (currentSetting == 0)
            {
                lightsToChange[i].intensity = defaultSettings[i];
            }
            else
            {
                lightsToChange[i].intensity *= intensityMultiplier;
            }
        }
    }
}
