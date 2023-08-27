using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject dialogueInterface;
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerHUD;
    [SerializeField] GameObject loseScreen;
    [SerializeField] bool skipDialogue;

    [Header("Spawning")]
    [SerializeField] GameObject walker;
    [SerializeField] GameObject turret;
    [SerializeField] GameObject jumper;

    [SerializeField] GameObject overworldWalkerSpawnPoints;
    [SerializeField] GameObject overworldTurretSpawnPoints;

    [SerializeField] GameObject firstLevelWalkerSpawnPoints;
    [SerializeField] GameObject firstLevelTurretSpawnPoints;
    [SerializeField] GameObject firstLevelJumperSpawnPoints;

    [SerializeField] GameObject secondLevelWalkerSpawnPoints;
    [SerializeField] GameObject secondLevelTurretSpawnPoints;
    [SerializeField] GameObject secondLevelJumperSpawnPoints;

    [SerializeField] GameObject thirdLevelWalkerSpawnPoints;
    [SerializeField] GameObject thirdLevelTurretSpawnPoints;
    [SerializeField] GameObject thirdLevelJumperSpawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        if (!skipDialogue)
        {
            StartDialogue();
            player.GetComponent<Player>().hasControl = false;
        }
        else
        {
            FinishIntro();
        }
    }

    public void TriggerSpawn(EnemySpawnTriggerBox.level levelToSpawn)
    {
        if (levelToSpawn == EnemySpawnTriggerBox.level.first)
        {
            SpawnEnemiesOnLevel("First");
        }
        if (levelToSpawn == EnemySpawnTriggerBox.level.second)
        {
            SpawnEnemiesOnLevel("Second");
        }
        if (levelToSpawn == EnemySpawnTriggerBox.level.third)
        {
            SpawnEnemiesOnLevel("Third");
        }
    }

    private void SpawnEnemiesOnLevel(string nameOfLevel)
    {
        if (nameOfLevel == "Overworld")
        {
            foreach (Transform walkerToSpawn in overworldWalkerSpawnPoints.transform)
            {
                SpawnEnemy(walker, walkerToSpawn.position, false);
            }

            foreach (Transform turretToSpawn in overworldTurretSpawnPoints.transform)
            {
                SpawnEnemy(turret, turretToSpawn.position, false);
            }
        }
        if (nameOfLevel == "First")
        {
            foreach (Transform walkerToSpawn in firstLevelWalkerSpawnPoints.transform)
            {
                SpawnEnemy(walker, walkerToSpawn.position, false);
            }

            foreach (Transform turretToSpawn in firstLevelTurretSpawnPoints.transform)
            {
                SpawnEnemy(turret, turretToSpawn.position, false);
            }

            foreach (Transform jumperToSpawn in firstLevelJumperSpawnPoints.transform)
            {
                SpawnEnemy(jumper, jumperToSpawn.position, false);
            }
        }
        if (nameOfLevel == "Second")
        {
            foreach (Transform walkerToSpawn in secondLevelWalkerSpawnPoints.transform)
            {
                SpawnEnemy(walker, walkerToSpawn.position, false);
            }

            foreach (Transform turretToSpawn in secondLevelTurretSpawnPoints.transform)
            {
                SpawnEnemy(turret, turretToSpawn.position, false);
            }

            foreach (Transform jumperToSpawn in secondLevelJumperSpawnPoints.transform)
            {
                SpawnEnemy(jumper, jumperToSpawn.position, false);
            }
        }
        if (nameOfLevel == "Third")
        {
            foreach (Transform walkerToSpawn in thirdLevelWalkerSpawnPoints.transform)
            {
                SpawnEnemy(walker, walkerToSpawn.position, false);
            }

            foreach (Transform turretToSpawn in thirdLevelTurretSpawnPoints.transform)
            {
                SpawnEnemy(turret, turretToSpawn.position, false);
            }

            foreach (Transform jumperToSpawn in thirdLevelJumperSpawnPoints.transform)
            {
                SpawnEnemy(jumper, jumperToSpawn.position, false);
            }
        }
    }

    private void SpawnEnemy(GameObject enemyType, Vector3 spawnPosition, bool stationary)
    {
        GameObject temp = Instantiate(enemyType, spawnPosition, Quaternion.identity);
        if (stationary && enemyType.transform.name == "Walker")
            temp.GetComponent<Walker>().SetStationary();
    }

    private void StartDialogue()
    {
        dialogueInterface.SetActive(true);
        dialogueInterface.GetComponent<DialogueWriter>().startDialogue = true;
    }

    //intro has finished give player the reins
    public void FinishIntro()
    {
        dialogueInterface.SetActive(false);
        player.GetComponent<Player>().hasControl = true;
        SpawnEnemiesOnLevel("Overworld");
    }

    public void Lose()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        loseScreen.SetActive(true);
        playerHUD.SetActive(false);
    }
}
