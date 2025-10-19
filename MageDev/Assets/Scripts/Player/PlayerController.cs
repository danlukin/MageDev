using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPausable
{
    public Rigidbody2D body;
    public float baseSpeed;
    private float speed;

    void OnEnable()
    {
        GameManager.OnGameStateChanged += HandleStateChange;
        GameManager.OnGamePause += HandlePause;
    }

    void OnDisable()
    {
        GameManager.OnGameStateChanged -= HandleStateChange;
        GameManager.OnGamePause -= HandlePause;
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
                body.velocity = new Vector2(0, 0);
                body.gameObject.transform.position = new Vector3(0, 0, 0);
                break;
            case GameState.Stage:
                AllowMovement(true);
                break;
        }
    }

    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(xInput, yInput).normalized;
        body.velocity = direction * speed;
    }

}
