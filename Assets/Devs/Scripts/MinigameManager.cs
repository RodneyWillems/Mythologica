using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MinigameManager : MonoBehaviour
{
    [Header("Orpheus Setup")]
    [SerializeField] private int _gameLength = 10;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject[] _rockPrefabs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #region Orpheus

    public void OrpheusSetup()
    {
        SpawnRocks();
    }

    private void SpawnRocks()
    {
        for (int i = 0; i < _gameLength; i++)
        {
            //Randomly spawn rocks in a circle around the spawn point
            Vector3 position = _spawnPoint.position + (Vector3.forward * (i + 0.5f));
            position += (Vector3)Random.insideUnitCircle * 1f;
            
            GameObject rockPrefab = _rockPrefabs[Random.Range(0, _rockPrefabs.Length)];
            Instantiate(rockPrefab, position, Random.rotation);
        }
    }   

    #endregion

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 200, 50), "Orpheus Setup"))
        {
            OrpheusSetup();
        }
    }
}
