using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject enemyPrefab;
    public GameObject bossPrefab; 
    public Transform spawnPoint; 
    public Text waveText; 
    public int currentWave = 1;
    public int enemiesPerWave = 5; 

    public GameObject pausePanel;
    public GameObject gameOverPanel;

    private bool waveInProgress = false;
    private int enemiesSpawned = 0;
    private int enemiesDestroyed = 0;
   

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }

    void Start() {
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        StartCoroutine(SpawnWave());
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }

    IEnumerator SpawnWave() {
        if (waveInProgress)
            yield break;

        waveInProgress = true;
        waveText.text = "Wave: " + currentWave;

        enemiesSpawned = 0;
        enemiesDestroyed = 0;
        
        for (int i = 0; i < enemiesPerWave; i++) {
            SpawnEnemy();
            enemiesSpawned++;
            yield return new WaitForSeconds(0.5f); 
        }

        if (currentWave % 5 == 0) {
            SpawnBoss();
        }

        yield return new WaitUntil(() => enemiesDestroyed == enemiesSpawned);

        currentWave++;
        waveInProgress = false;

        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnWave());
    }

    void SpawnEnemy() {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        EnemyShip enemyShip = enemy.GetComponent<EnemyShip>();

        if (enemyShip != null) {
            enemyShip.OnDeath += OnEnemyDestroyed;
        }
    }

    void SpawnBoss() {
        GameObject boss = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        EnemyShip bossShip = boss.GetComponent<EnemyShip>();

        if (bossShip != null) {
            bossShip.OnDeath += OnEnemyDestroyed;
        }
    }

    void OnEnemyDestroyed() {
        enemiesDestroyed++;
    }

    public void TogglePause() {
        bool isPaused = Time.timeScale == 0;
        Time.timeScale = isPaused ? 1 : 0;
        pausePanel.SetActive(!isPaused);
    }

    public void GameOver() {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void RestartGame() {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    // public void RestartGame() {

    //     Time.timeScale = 1;

    //     gameOverPanel.SetActive(false);

    //     currentWave = 1;
    //     enemiesDestroyed = 0;
    //     enemiesSpawned = 0;

    
    //     SceneManager.sceneLoaded += OnSceneLoaded;
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    // }

    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    //     SceneManager.sceneLoaded -= OnSceneLoaded;

    //     StartCoroutine(SpawnWave());

    //     pausePanel.SetActive(false);
    // }
}

