using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPausable
{
    public Rigidbody2D rb;
    public float baseSpeed;
    private float speed;

    private GameObject[] allTargets;
    private GameObject target;

    private Vector2 playerPos;
    private Vector2 targetPos;
    private Vector2 direction;

    private bool inCollision = false;
    private bool canRetarget = true;

    void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleStateChange;
        GameManager.OnGamePause += HandlePause;
        StageManager.OnWaveStateChanged += HandleWaveStateChange;
    }

    void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleStateChange;
        GameManager.OnGamePause -= HandlePause;
        StageManager.OnWaveStateChanged -= HandleWaveStateChange;
    }

    private void HandleWaveStateChange(WaveState state)
    {
        if (state == WaveState.Dead)
        {
            speed = 0;
        }
        else
        {
            speed = baseSpeed;
        }
    }

    private void AllowMovement(bool allowed)
    {
        if (allowed)
        {
            speed = baseSpeed;
        }
        else
        {
            speed = 0;
        }
    }

    public void HandlePause(bool isPaused)
    {
        AllowMovement(!isPaused);
    }

    private void HandleStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Camp:
                AllowMovement(false);
                rb.velocity = new Vector2(0, 0);
                rb.gameObject.transform.position = new Vector3(0, 0, 0);
                break;
            case GameState.Stage:
                AllowMovement(true);
                break;
        }
    }

    void FixedUpdate()
    {
        HandleAutoMove();

        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        if (xInput != 0 || yInput != 0) direction = new Vector2(xInput, yInput).normalized;

        rb.velocity = direction * speed;
    }

    private void HandleTargeting()
    {
        allTargets = GameObject.FindGameObjectsWithTag("Enemy");
        if (allTargets.Length > 0)
        {
            target = allTargets[0];
            foreach (GameObject tempTarget in allTargets)
            {
                if (Vector2.Distance(transform.position, tempTarget.transform.position) < Vector2.Distance(transform.position, target.transform.position))
                {
                    target = tempTarget;
                    targetPos = target.transform.position;

                    canRetarget = false;
                    StartCoroutine(EnableRetarget());
                }
            }
        }
    }

    IEnumerator EnableRetarget()
    {
        yield return new WaitForSeconds(0.5f);

        canRetarget = true;
    }

    private void HandleAutoDirection()
    {
        playerPos = rb.transform.position;
        direction = (playerPos - targetPos).normalized;
    }

    private void HandleAutoMove()
    {
        if (!inCollision & canRetarget) HandleTargeting();
        HandleAutoDirection();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        inCollision = true;
        targetPos = collision.GetContact(0).point;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        StartCoroutine(DisableCollisionDetection());
    }

    IEnumerator DisableCollisionDetection()
    {
        yield return new WaitForSeconds(0.75f);

        inCollision = false;
    }

}
