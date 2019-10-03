﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{ 

    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _laserPrefabs;
    [SerializeField]
    private GameObject[] _powerupPrefabs;
    [SerializeField]
    private GameObject[] _containerObjects = null;
    [SerializeField]
    private Transform _laserSpawnPosition;
    private Enemy _enemy;
    private bool _stopSpawning = false;
    private bool isTripleShotActive = false;

    void Start()
    {
        _enemy = _enemyPrefab.GetComponent<Enemy>();


        if (_enemy == null)
            Debug.LogError("Enemy prefab is null");

        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    public Vector3 RandomizeSpawn()
    {
        float randomX = Random.Range(Boundaries.spawnXMin, Boundaries.spawnXMax);
        Vector3 randomSpawn = new Vector3(randomX, Boundaries.spawnYMax, 0);
        return transform.position = randomSpawn;
    }

    #region LaserSpawns
    public void SpawnLaser()
    {
        
        Vector3 spawnPosition = new Vector3 
        (
            _laserSpawnPosition.transform.position.x, 
            _laserSpawnPosition.transform.position.y, 
            0
        );

        GameObject newLaser;

        if (isTripleShotActive == true)
            newLaser = Instantiate(_laserPrefabs[1], spawnPosition, Quaternion.identity);
        else
            newLaser = Instantiate(_laserPrefabs[0], spawnPosition, Quaternion.identity);

        newLaser.transform.parent = _containerObjects[0].transform;
    }

    IEnumerator PowerDownTripleShot()
    {
        yield return new WaitForSeconds(3.5f);
        isTripleShotActive = false;
    }

    public void TripleShotActive()
    {
        isTripleShotActive = true;
        StartCoroutine(PowerDownTripleShot());
    }
    #endregion

    IEnumerator SpawnEnemyRoutine()
    {
        while (!_stopSpawning)
        {
            GameObject newEnemy = 
                Instantiate(_enemyPrefab, RandomizeSpawn(), Quaternion.identity);

            newEnemy.transform.parent = _containerObjects[1].transform;
            
            yield return new WaitForSeconds(1.5f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (!_stopSpawning)
        {
            yield return new WaitForSeconds(Random.Range(2, 8));

            int randomPowerup = Random.Range(0, 3);

            GameObject newPowerUp = 
                Instantiate(_powerupPrefabs[randomPowerup], RandomizeSpawn(), Quaternion.identity);

            newPowerUp.transform.parent = _containerObjects[2].transform;
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
