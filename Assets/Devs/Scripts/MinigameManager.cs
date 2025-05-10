using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;
using Random = UnityEngine.Random;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }
    public List<GameObject> spawnedRocks = new List<GameObject>();

    [Header("Orpheus Setup")]
    [SerializeField] private int _gameLength = 10;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject[] _rockPrefabs;

    List<PlayerInput> _players = new List<PlayerInput>();
    private void Awake()
    {
        Instance = this;

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // _players = PlayerInput.all;
        //
        // foreach (PlayerInput user in _players)
        // {
        //     user.onControlsChanged += OnControlChange;
        // }
    }

    // Update is called once per frame
    void Update()
    {

    }
    #region Orpheus

    public IEnumerator OrpheusSetup()
    {
        yield return new WaitForSeconds(0.1f);
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
            GameObject spawnedRock = Instantiate(rockPrefab, position, Quaternion.identity);
            spawnedRock.transform.GetChild(0).rotation = Random.rotation;

            spawnedRocks.Add(spawnedRock);
        }
        foreach (PlayerInput user in _players)
        {
            OnControlChange(user);
        }
    }

    #endregion
    private void OnControlChange(PlayerInput obj)
    {
        foreach (GameObject rock in spawnedRocks)
        {
            print("CHANGE BITCH!");
            Rock rockScript = rock.GetComponent<Rock>();

            rockScript.iconPreset = ButtonIconManager.Instance.GetPreset(obj);
        }
    }

    public void AddPlayer(PlayerInput player)
    {
        _players.Add(player);
        player.onControlsChanged += OnControlChange;
        StartCoroutine(OrpheusSetup());
    }

}