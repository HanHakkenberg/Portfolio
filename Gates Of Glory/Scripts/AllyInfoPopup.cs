using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class AllyInfoPopup : MonoBehaviour
{

    public static AllyInfoPopup instance;

    private Allie target;
    private Transform mainCam;

    public Transform pointerParent;
    public GameObject pointerObject;

    private int listIndex;

    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private TextMeshProUGUI damageText;
    [SerializeField]
    private TextMeshProUGUI selectedText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        mainCam = Camera.main.transform;
        pointerObject.SetActive(false);
    }

    private void Update()
    {
        if (target != null)
        {
            healthText.text = ((int)target.myStats.health.currentValue).ToString();
            damageText.text = ((int)target.myStats.damage.currentValue).ToString();

            pointerParent.transform.position = target.transform.position;
            pointerParent.transform.LookAt(mainCam);

            if (target.myStats.health.currentValue <= 0)
            {
                target = null;
            }
        }
        else
        {
            if (selectedText.text != "No Minion Selected")
            {
                selectedText.text = "No Minion Selected";
                healthText.text = "???";
                damageText.text = "???";
            }
        }
    }

    public void SetTarget(Allie ally)
    {
        target = ally;

        pointerObject.SetActive(false);
        pointerObject.SetActive(true);

        string s = target.gameObject.name;       
        selectedText.text = s.Substring(0, s.Length - 7);
    }

    public void SelectNextTarget(bool increase)
    {
        if (WaveManager.instance.alliesInScene.Count == 0)
        {
            return;
        }

        if (listIndex > WaveManager.instance.alliesInScene.Count - 1)
        {
            listIndex = 0;
        }

        if (increase)
        {
            if (listIndex == WaveManager.instance.alliesInScene.Count - 1)
            {
                listIndex = 0;
            }
            else
            {
                listIndex++;

            }
        }
        else
        {
            if (listIndex == 0)
            {
                listIndex = WaveManager.instance.alliesInScene.Count - 1;
            }
            else
            {
                listIndex--;
            }
        }

        SetTarget(WaveManager.instance.alliesInScene[listIndex]);

        #region OLD SHIT
        //float myX = target.transform.position.x;

        //Dictionary<float, Allie> distances = new Dictionary<float, Allie>();

        //if (increase)
        //{
        //    for (int i = 0; i < WaveManager.instance.alliesInScene.Count; i++)
        //    {
        //        if (WaveManager.instance.alliesInScene[i] != target)
        //        {
        //            if (Mathf.Abs(WaveManager.instance.alliesInScene[i].transform.position.x) > target.transform.position.x)
        //            {
        //                distances.Add(Vector3.Distance(WaveManager.instance.alliesInScene[i].transform.position, target.transform.position), WaveManager.instance.alliesInScene[i]);
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < WaveManager.instance.alliesInScene.Count; i++)
        //    {
        //        if (WaveManager.instance.alliesInScene[i] != target)
        //        {
        //            if (Mathf.Abs(WaveManager.instance.alliesInScene[i].transform.position.x) < target.transform.position.x)
        //            {
        //                distances.Add(Vector3.Distance(WaveManager.instance.alliesInScene[i].transform.position, target.transform.position), WaveManager.instance.alliesInScene[i]);
        //            }
        //        }
        //    }
        //}

        //float closest = distances.Keys.Min();
        //SetTarget(distances[closest]);
        #endregion
    }
}
