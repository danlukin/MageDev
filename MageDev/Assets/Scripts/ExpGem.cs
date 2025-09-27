using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGem : MonoBehaviour
{
    [SerializeField] private float baseExp = 1;
    [SerializeField] private Sprite[] sprites;
    private float exp;

    private void Awake()
    {
        exp = baseExp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerExperience player = collision.gameObject.GetComponent<PlayerExperience>();
            player.GrantExperience(exp);
            Destroy(gameObject);
        }
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
        
    }
}
