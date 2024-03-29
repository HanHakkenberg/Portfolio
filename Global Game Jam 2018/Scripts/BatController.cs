﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BatController : MonoBehaviour
{

    private Rigidbody2D rb;

    public Animator anim;

    public GameObject rayRound;
    [Space(10)]
    public float moveSpeed;
    [Space(10)]
    public float hitInvinsiibilityTime;
    public bool canGetHit = true;

    [Header("Camera Shake")]
    public float shakeX;
    public float shakeY;
    public float shakeZ;
    public float shakeSpeed;
    public float shakeDuration;
    public float shakeRotate;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!BatRayController.isFiringRay && GameManager.instance.gameState == GameManager.GameState.Playing)
        {
            Movement();
        }
    }

    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 horizontal = transform.right * x;
        Vector2 vertical = transform.up * y;

        Vector2 velocity = (horizontal + vertical).normalized * (Time.deltaTime * moveSpeed);

        Vector2 currentPosition = new Vector3(transform.position.x, transform.position.y);

        rb.MovePosition(currentPosition + velocity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            if (canGetHit)
            {
                StartCoroutine(HitInvinsibility(hitInvinsiibilityTime));

                GameManager.instance.SubtractLive();
                collision.GetComponent<Obstacle>().myAnimator.SetTrigger("Highlight");

                Instantiate(rayRound, collision.transform.position, Quaternion.identity);

                anim.SetTrigger("pHurt");
                Camera.main.transform.GetComponent<CameraShake>().Shake(shakeDuration, shakeX, shakeY, shakeZ, shakeRotate, shakeSpeed);
            }
        }
        else if (collision.tag == "Heart")
        {
            GameManager.instance.AddLive();
            Destroy(collision.gameObject);
        }
        else if(collision.tag == "Web"){
            moveSpeed /= 2;
        }
        else if (collision.tag == "Boss")
        {
            BossFight.instance.isInEatRange = true;

            StartCoroutine(BossFight.instance.AttackPlayer());
        }

        if (collision.tag == "BossSecondCheck")
        {
            BossFight.instance.isGrabbable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Web") {
            moveSpeed *= 2;
        }

        if (collision.tag == "BossSecondCheck")
        {
            BossFight.instance.isGrabbable = false;
        }

        if (collision.tag == "Boss")
        {
            BossFight.instance.isInEatRange = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "CaveWall")
        {
            if (canGetHit)
            {
                StartCoroutine(HitInvinsibility(hitInvinsiibilityTime));

                Instantiate(rayRound, collision.contacts[0].point, Quaternion.identity);

                GameManager.instance.SubtractLive();
                anim.SetTrigger("pHurt");
                Camera.main.transform.GetComponent<CameraShake>().Shake(shakeDuration, shakeX, shakeY, shakeZ, shakeRotate, shakeSpeed);
            }
        }
    }

    public IEnumerator HitInvinsibility(float time)
    {
        canGetHit = false;
        yield return new WaitForSeconds(time);
        canGetHit = true;
    }
}
