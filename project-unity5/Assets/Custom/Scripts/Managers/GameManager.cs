using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
	public EnemyManager enemyManager;
	public int enemiesPerWave = 1;
	public float timeBetweenWaves = 3.0f;

	public Text countdownText;

	public int perWaveDeltaHealth = 0;
	public int perWaveDeltaDamage = 0;

	private int waveCurrent = 0;
	private int enemiesSpawnedPreviously = 0;
	private bool waveOngoing = false;
	private float timeSinceLastWave;

	private bool gameIsStarted = false;

	public void StartGame() {
		gameIsStarted = true;
		StartWave (waveCurrent);
	}

	public void StartWave(int waveNumber) {
		Debug.Log ("Starting wave " + waveNumber);
		countdownText.text = "";
		waveOngoing = true;
		enemyManager.deltaHealth = waveNumber * perWaveDeltaHealth;
		enemyManager.deltaDamage = waveNumber * perWaveDeltaDamage;
		enemyManager.StartRepeatSpawn ();
		timeSinceLastWave = 0;
	}

	public void StopWave() {
		Debug.Log ("Stopping wave " + waveCurrent);
		enemyManager.StopRepeatSpawn ();
	}

	public void EndWave() {
		countdownText.text = timeBetweenWaves.ToString ();
		waveOngoing = false;
		++waveCurrent;
		enemiesSpawnedPreviously = enemyManager.enemiesSpawned;
	}

	public void Update() {
		if (((enemyManager.enemiesSpawned - enemiesSpawnedPreviously) >= enemiesPerWave) && enemyManager.isSpawning) {
			StopWave ();
		} else if (!enemyManager.isSpawning && enemyManager.enemiesAlive == 0 && waveOngoing) {
			EndWave ();
		} else if (timeSinceLastWave >= timeBetweenWaves) {
			StartWave (waveCurrent);
		} else if(gameIsStarted && !waveOngoing) {
			timeSinceLastWave += Time.deltaTime;
			countdownText.text = (Mathf.MoveTowards(timeBetweenWaves, timeBetweenWaves - timeSinceLastWave, timeBetweenWaves)).ToString ("F1");
		}
	}
}

