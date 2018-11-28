using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Throne : CastleDeffensePoint {

    public Image throneHealthBarLeft;
    public Image throneHealthBarRight;
    public Image gateHealthBarLeft;
    public Image gateHealthBarRight;

    public CastleDeffensePoint leftGate, rightGate;

    public float lerpSpeed;

    [Space(10)]
    public Image secondThroneHealthBarFill;

    private void Start() {
        HPBar();
    }

    public override void DirectDamage(float damage) {
        myStats.health.currentValue -= damage;
        myStats.health.currentValue -= damage;
        HPBar();

        if(healthbarFill != null) {
            healthbarFill.fillAmount = (myStats.health.currentValue / myStats.health.baseValue);

            if(secondThroneHealthBarFill != null) {
                secondThroneHealthBarFill.fillAmount = (myStats.health.currentValue / myStats.health.baseValue);
            }
        }

        if(myStats.health.currentValue <= 0) {
            if(GameManager.instance.gameState == GameManager.GameState.Playing) {
                StartCoroutine(UIManager.instance.GameOver());
            }
        }
    }

    public override void TakeDamage(float damage) {
        if(ResourceManager.instance.goldPrefabsInScene.Count > 0) {
            ResourceManager.instance.RemoveGold(1, false);
        }
        else {
            damage = Mathf.Abs(ResourceManager.instance.goldPrefabsInScene.Count - Mathf.RoundToInt(damage / 10));
            ResourceManager.instance.RemoveGold(ResourceManager.instance.goldPrefabsInScene.Count, false);
            myStats.health.currentValue -= damage;
            myStats.health.currentValue -= damage;
            HPBar();

            if(healthbarFill != null) {
                healthbarFill.fillAmount = (myStats.health.currentValue / myStats.health.baseValue);

                if(secondThroneHealthBarFill != null) {
                    secondThroneHealthBarFill.fillAmount = (myStats.health.currentValue / myStats.health.baseValue);
                }
            }

            if(myStats.health.currentValue <= 0) {
                if(GameManager.instance.gameState == GameManager.GameState.Playing) {
                    StartCoroutine(UIManager.instance.GameOver());
                }
            }
        }
    }

    public void HPBar() {
        float newGateHealthBarLeft = (leftGate.myStats.health.currentValue + myStats.health.currentValue) / (leftGate.myStats.health.baseValue + myStats.health.baseValue);
        float newThroneHealthBarLeft = myStats.health.currentValue / (myStats.health.baseValue + leftGate.myStats.health.baseValue);
        float newGateHealthBarRight = (rightGate.myStats.health.currentValue + myStats.health.currentValue) / (rightGate.myStats.health.baseValue + myStats.health.baseValue);
        float newThroneHealthBarRight = myStats.health.currentValue / (myStats.health.baseValue + rightGate.myStats.health.baseValue);

        StopCoroutine("HealthBarLerp");
        StartCoroutine(HealthBarLerp(newGateHealthBarLeft, newGateHealthBarRight, newThroneHealthBarLeft, newThroneHealthBarRight));
    }

    IEnumerator HealthBarLerp(float newGateLeft, float newGateRight, float newThroneLeft, float newThroneRight) {

        while(gateHealthBarLeft.fillAmount >= newGateLeft || gateHealthBarRight.fillAmount >= newGateRight || throneHealthBarLeft.fillAmount >= newThroneLeft || throneHealthBarRight.fillAmount >= newThroneRight) {
            gateHealthBarLeft.fillAmount = Mathf.Lerp(gateHealthBarLeft.fillAmount, (leftGate.myStats.health.currentValue + myStats.health.currentValue) / (leftGate.myStats.health.baseValue + myStats.health.baseValue), lerpSpeed);
            throneHealthBarLeft.fillAmount = Mathf.Lerp(throneHealthBarLeft.fillAmount, myStats.health.currentValue / (myStats.health.baseValue + leftGate.myStats.health.baseValue), lerpSpeed);
            gateHealthBarRight.fillAmount = Mathf.Lerp(gateHealthBarRight.fillAmount, (rightGate.myStats.health.currentValue + myStats.health.currentValue) / (rightGate.myStats.health.baseValue + myStats.health.baseValue), lerpSpeed);
            throneHealthBarRight.fillAmount = Mathf.Lerp(throneHealthBarRight.fillAmount, myStats.health.currentValue / (myStats.health.baseValue + rightGate.myStats.health.baseValue), lerpSpeed);
            yield return null;
        }

    }
}
