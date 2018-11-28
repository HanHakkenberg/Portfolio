using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class WaveManager : MonoBehaviour {
    public string terrorist;
    bool terroristBool;
    public List<string> enemyTypes = new List<string>();
    public List<int> enemyAmountRight = new List<int>();
    public List<int> enemyAmountLeft = new List<int>();
    public Wave thisWave;
    public float waveSize = 10;
    public float minWaveMultiplier, maxWaveMultiplier;
    public float HealthMultiplier;
    public float DamageMultiplier;
    public int maxStageSize;
    public int waveHealth;
    public AudioSource hornAudio;

    public static WaveManager instance;
    bool waveDone;
    public int currentWave;
    int currentStage;
    int currentSoldier;
    public float wavePause;
    public float maxSoldierWait, minSoldierWait;
    public float maxStageWait, minStageWait;
    public List<Transform> spwanPoints = new List<Transform>();
    public float SpawnsetOff;
    public List<Enemy> enemiesInScene = new List<Enemy>();
    public List<Allie> alliesInScene = new List<Allie>();

    public int baseTerroristChance;
    public int terroristChanceIncrese;
    int currentTerroristChance = 100;

    [Header("Scroll Animation")]
    public Animator scrollAnim;
    public TextMeshProUGUI waveText;

    [Header("Wave Pause Animation")]
    public Animator waveTimerAnim;
    public Slider waveTimerSlider;
    private bool skipWaiting;

    [HideInInspector]
    public List<CastleGate> allCastleGates = new List<CastleGate>();

    public Animator waveInfoAnim;
    public TextMeshProUGUI waveInfoLeftText;
    public TextMeshProUGUI waveInfoRightText;

    void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    void Start() {
        for(int i = 0; i < BattleManager.instance.newDeffensePoints.Count; i++) {
            allCastleGates.Add(BattleManager.instance.newDeffensePoints[i].castleGate);
        }
    }

    void Update() {
        if(Input.GetKeyDown("1")) {
            SpawnEnemy("Enemy Knight");
        }
        if(Input.GetKeyDown("2")) {
            SpawnEnemy("Enemy Bowman");
        }
        if(Input.GetKeyDown("3")) {
            SpawnEnemy("Enemy Spearman");
        }
        if(Input.GetKeyDown("4")) {
            SpawnEnemy("Enemy Giant");
        }
        if(Input.GetKeyDown("5")) {
            SpawnEnemy("Enemy Terrorist");
        }
        if(Input.GetKeyDown("m")) {
            currentWave++;
        }
        if(enemiesInScene.Count <= 0 && waveDone == true) {
            waveDone = false;
            NextWave();
            ResourceManager.instance.AddGold(10);
        }
    }

    void SpawnEnemy(string enemyToSpawn) {
        GameObject newEnemy = ObjectPooler.instance.GrabFromPool(enemyToSpawn, new Vector3(spwanPoints[0].transform.position.x + Random.Range(-SpawnsetOff, SpawnsetOff), 2, Random.Range(-SpawnsetOff, SpawnsetOff)), spwanPoints[0].transform.rotation);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.myStats.damage.currentValue = (DamageMultiplier * currentWave) * enemyScript.myStats.damage.baseValue;
        enemyScript.myStats.health.currentValue = (DamageMultiplier * currentWave) * enemyScript.myStats.health.baseValue;
    }

    [System.Serializable]
    public struct NewSoldier {
        public int Side;
        public string Soldier;
    }


    public void RemoveEnemyFromScene(Enemy _enemyToRemove) {
        instance.enemiesInScene.Remove(_enemyToRemove);
    }

    public void NextWave() {
        HealthMultiplier += 0.01f * waveSize;
        DamageMultiplier += 0.01f * waveSize;
        for(int i = 0; i < BattleManager.instance.newDeffensePoints.Count; i++) {
            for(int ii = 0; ii < CastleUpgradeManager.instance.allBuiltRooms.Count; ii++) {
                if(CastleUpgradeManager.instance.allBuiltRooms[ii].roomType == CastleRoom.RoomType.Heal) {
                    if(CastleUpgradeManager.instance.allBuiltRooms[ii].info.myLevel >= 5) {
                        if(allCastleGates[i].locked) {
                            allCastleGates[i].locked = false;
                        }

                        allCastleGates[i].Heal(100);
                    }
                }
            }
        }

        currentWave++;
        StartCoroutine(WaveTimer());
        StartCoroutine(GeneradeWave());
    }

    IEnumerator GeneradeWave() {

        for(int i = 0; i < enemyAmountLeft.Count; i++) {
            enemyAmountLeft[i] = 0;
            enemyAmountRight[i] = 0;
        }

        waveSize = waveSize * Random.Range(minWaveMultiplier, maxWaveMultiplier);
        thisWave = new Wave();
        for(int w = 0; w < Mathf.Round(waveSize); w++) {
            int stageSize = Random.Range(Mathf.Clamp(Mathf.RoundToInt(currentWave / 2f), 1, maxStageSize), Mathf.Clamp(Mathf.RoundToInt(currentWave / 0.5f), 1, maxStageSize));

            stageSize = Mathf.Clamp(stageSize - Random.Range(0, 3), 1, maxStageSize);
            Stage _newStage = new Stage();

            for(int s = 0; s < stageSize; s++) {
                NewSoldier newSoldier = new NewSoldier();

                int currentSoldier = Mathf.Clamp(Mathf.RoundToInt(Mathf.Clamp(currentWave / 5f, 0, enemyTypes.Count - 1) - Random.Range(0, enemyTypes.Count)), 0, enemyTypes.Count - 1);

                if(currentWave > 3) {
                    newSoldier.Side = Random.Range(0, 2);
                }
                else {
                    newSoldier.Side = 0;
                }

                if(newSoldier.Side == 0) {
                    enemyAmountLeft[currentSoldier]++;
                }
                else {
                    enemyAmountRight[currentSoldier]++;
                }

                newSoldier.Soldier = enemyTypes[currentSoldier];
                _newStage.soldiers.Add(newSoldier);
            }
            thisWave.atackStage.Add(_newStage);

            yield return null;
        }

        if(currentTerroristChance < baseTerroristChance) {
            currentTerroristChance = baseTerroristChance;
        }

        ShowWaveInfo();
    }

    IEnumerator WaveTimer() {
        waveTimerSlider.maxValue = wavePause;
        waveTimerAnim.SetTrigger("Open");

        float timeToWait = wavePause;

        while(timeToWait > 0) {
            waveTimerSlider.value = (waveTimerSlider.maxValue - timeToWait);
            timeToWait -= Time.deltaTime;

            if(skipWaiting) {
                timeToWait = 0;
                skipWaiting = false;
            }

            yield return null;
        }

        waveTimerAnim.SetTrigger("Close");
        hornAudio.Play();
        ShowWaveNumber();
        HideWaveInfo();
        if(currentWave > 5 && Random.Range(1, 101) <= currentTerroristChance) {
            terroristBool = true;
        }
        StartCoroutine(StageTimer());
    }

    IEnumerator StageTimer() {
        yield return new WaitForSeconds(Random.Range(minStageWait, maxStageWait));

        if(currentStage < thisWave.atackStage.Count) {
            StartCoroutine(SoldierTimer());
        }
        else {
            waveDone = true;
            currentStage = 0;
        }
    }

    IEnumerator SoldierTimer() {
        yield return new WaitForSeconds(Random.Range(minSoldierWait, maxSoldierWait));

        if(currentSoldier < thisWave.atackStage[currentStage].soldiers.Count) {
            GameObject newEnemy = null;
            Enemy enemyScript = null;

            if(terroristBool) {
                terroristBool = false;
                newEnemy = ObjectPooler.instance.GrabFromPool(terrorist, new Vector3(spwanPoints[Random.Range(0, 2)].transform.position.x + Random.Range(-SpawnsetOff, SpawnsetOff), 2, Random.Range(-SpawnsetOff, SpawnsetOff)), spwanPoints[Random.Range(0, 2)].transform.rotation);
                enemyScript = newEnemy.GetComponent<Enemy>();
                enemyScript.myStats.health.currentValue = (DamageMultiplier * currentWave) * enemyScript.myStats.health.baseValue;

                enemyScript.agent.speed = Random.Range(1.75f, 2.25f);
                enemyScript.myAudiosource.volume = Random.Range(0.01f, 0.08f);
                newEnemy.transform.localScale *= Random.Range(0.9f, 1.1f);
                currentTerroristChance = 0;
                yield return null;
            }

            newEnemy = ObjectPooler.instance.GrabFromPool(thisWave.atackStage[currentStage].soldiers[currentSoldier].Soldier, new Vector3(spwanPoints[thisWave.atackStage[currentStage].soldiers[currentSoldier].Side].transform.position.x + Random.Range(-SpawnsetOff, SpawnsetOff), 2, Random.Range(-SpawnsetOff, SpawnsetOff)), spwanPoints[thisWave.atackStage[currentStage].soldiers[currentSoldier].Side].transform.rotation);
            enemyScript = newEnemy.GetComponent<Enemy>();
            enemyScript.myStats.damage.currentValue = (DamageMultiplier * currentWave) * enemyScript.myStats.damage.baseValue;
            enemyScript.myStats.health.currentValue = (DamageMultiplier * currentWave) * enemyScript.myStats.health.baseValue;

            currentSoldier++;
            enemiesInScene.Add(enemyScript);
            StartCoroutine(SoldierTimer());
        }
        else {
            if(currentStage >= thisWave.atackStage.Count) {
                Debug.Log("k");
                waveDone = true;
            }
            currentSoldier = 0;
            currentStage++;
            StartCoroutine(StageTimer());
        }
    }

    private void ShowWaveNumber() {
        waveText.text = "Wave\n" + currentWave;
        UIManager.instance.waveText.text = currentWave.ToString();
        scrollAnim.SetTrigger("Open");
    }

    public void SkipWaitingForNextWaveButton() {
        skipWaiting = true;
    }

    private void ShowWaveInfo() {
        waveInfoLeftText.text = "";

        for(int i = 0; i < enemyAmountLeft.Count; i++) {
            if(enemyAmountLeft[i] > 0) {
                waveInfoLeftText.text += "<color=green>" + enemyAmountLeft[i] + "x</color> " + enemyTypes[i] + "\n";
            }
        }

        waveInfoRightText.text = "";

        for(int i = 0; i < enemyAmountRight.Count; i++) {
            if(enemyAmountRight[i] > 0) {
                waveInfoRightText.text += "<color=green>" + enemyAmountRight[i] + "x</color> " + enemyTypes[i] + "\n";
            }
        }

        waveInfoAnim.SetTrigger("Trigger");
    }

    private void HideWaveInfo() {
        waveInfoAnim.SetTrigger("Trigger");
    }
}
