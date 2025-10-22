using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGem : MonoBehaviour
{
    [SerializeField] private float baseExp = 1;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float homeInSpeed = 1;
    private float maxSpeed;
    private float exp;
    private Rigidbody2D rb;
    private GameObject player;
    private bool startHomeIn = false;

    public static event Action<GameObject> OnExpDrop;
    public static event Action<GameObject> OnExpPickUp;

    private void Awake()
    {
        exp = baseExp;
        GameManager.OnGameStateChanged += HandleGameStateChange;
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = GameObject.Find("PlayerCharacter");
        maxSpeed = 2 * homeInSpeed;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= HandleGameStateChange;

        OnExpPickUp?.Invoke(gameObject);
    }

    private void HandleGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.Camp:
                Destroy(gameObject);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.GetComponent<CapsuleCollider2D>().enabled == false)
            {
                player.GetComponent<PlayerExperience>().GrantExperience(exp);
                Destroy(gameObject);
            }
            else startHomeIn = true;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (startHomeIn) HomeInOnPlayer();
    }

    private void HomeInOnPlayer()
    {
        Vector3 currentPos = gameObject.transform.position;
        Vector3 direction = (player.transform.position - currentPos).normalized;
        rb.velocity = direction * homeInSpeed;

        if(homeInSpeed < maxSpeed) homeInSpeed += 0.1f;
    }

    public void SpawnExperienceGem(Enemy enemy)
    {
        Vector3 spawnPosition = enemy.transform.position;

        ExpGem newGem = Instantiate(this, spawnPosition, Quaternion.identity);

        switch (enemy.difficulty)
        {
            case (Difficulty.normal):
                newGem.GetComponent<SpriteRenderer>().sprite = sprites[0];
                break;
            case (Difficulty.elite):
                newGem.exp *= 2;
                newGem.GetComponent<SpriteRenderer>().sprite = sprites[1];
                break;
            case (Difficulty.boss):
                newGem.exp *= 20;
                newGem.GetComponent<SpriteRenderer>().sprite = sprites[2];
                break;
        }

        OnExpDrop?.Invoke(newGem.gameObject);
    }
}
