using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{

    public static ResourceManager instance;

    [Header("Gold")]
    public int goldToStartWith;
    public Transform goldSpawn;
    public float goldSpawnInterval;
    private bool canDumpGold = true;
    private int goldToDump;
    public List<GoldCoin> goldPrefabsInScene = new List<GoldCoin>();
    public Animator goldAnim;
    public TextMeshProUGUI goldText;

    [Header("Enemy Kill Rewards")]
    public int normalEnemyGoldReward;
    public int terroristGoldReward;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        AddGold(goldToStartWith);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddGold(50);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            RemoveGold(5, false);
        }

        goldText.text = goldPrefabsInScene.Count.ToString();

        if (goldToDump > 0)
        {
            if (canDumpGold)
            {
                StartCoroutine(DumpGold());
            }

            goldAnim.ResetTrigger("Close");
            goldAnim.SetTrigger("Open");
        }
        else
        {
            goldAnim.ResetTrigger("Open");
            goldAnim.SetTrigger("Close");
        }
    }

    public void AddGold(int amount)
    {
        goldToDump += amount;

        int extraGoldToSpawn = amount;

        for (int i = 0; i < extraGoldToSpawn; i++)
        {
            GameObject newGold = ObjectPooler.instance.GrabFromPool("gold", goldSpawn.transform.position, Random.rotation);
            GoldCoin newCoin = newGold.GetComponent<GoldCoin>();

            goldPrefabsInScene.Add(newCoin);
            Notary.goldAccumulated++;
        }
    }

    private IEnumerator DumpGold()
    {
        canDumpGold = false;

        for (int i = 0; i < goldPrefabsInScene.Count; i++)
        {
            if (goldPrefabsInScene[i].myRb.isKinematic)
            {
                goldPrefabsInScene[i].myRb.isKinematic = false;
                goldPrefabsInScene[i].myCollider.enabled = true;
                goldPrefabsInScene[i].StartRBTimer();

                goldToDump--;

                yield return new WaitForSeconds(goldSpawnInterval);
            }
        }

        canDumpGold = true;
    }

    public void RemoveGold(int amount, bool spentByPlayer)
    {
        if (goldPrefabsInScene.Count < amount)
        {
            return;
        }

        Notary.goldSpent += spentByPlayer ? amount : 0;
        Notary.goldStolen += spentByPlayer ? 0 : amount;

        goldToDump = (goldToDump >= amount) ? goldToDump -= amount : 0;
        int goldPrefabsToDelete = amount;

        for (int i = 0; i < goldPrefabsToDelete; i++)
        {
            goldPrefabsInScene[goldPrefabsInScene.Count - 1].myRb.isKinematic = true;
            goldPrefabsInScene[goldPrefabsInScene.Count - 1].myCollider.enabled = false;

            ObjectPooler.instance.AddToPool("gold", goldPrefabsInScene[goldPrefabsInScene.Count - 1].gameObject);
            goldPrefabsInScene.RemoveAt(goldPrefabsInScene.Count - 1);
        }
    }

    public bool HasEnoughGold(int price)
    {
        if (goldPrefabsInScene.Count >= price)
        {
            return true;
        }
        else
        {
            UIManager.instance.DisplayNotEnoughGoldIcon();
            return false;
        }
    }
}
