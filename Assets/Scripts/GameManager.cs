using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public LayerMask EnemyMask;

    [SerializeField] LocalPlayer[] players;
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] Transform[] spawnPoints;

    [Range(0f, 1f)]
    [SerializeField] float slowedTime = 0.33f;

    float cachedFixedDelta;

    private void Awake()
    {
        Instance = this;
        cachedFixedDelta = Time.fixedDeltaTime;

        Enemy[] _enemies = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        if (_enemies.Length == 0)
            SpawnTargets();
    }

    public void SpawnTargets()
    {
        List<GameObject> _targets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Target"));
        for (int _i = _targets.Count - 1; _i >= 0; _i--)
            Destroy(_targets[_i]);
        foreach(Transform _spawnPoint in spawnPoints)
        {
            Ray _ray = new Ray(_spawnPoint.TransformPoint(Vector3.up * 5), Vector3.down);
            if(!Physics.Raycast(
                _ray.origin,
                _ray.direction,
                3f,
                EnemyMask
                ))
            {
                Enemy _enemy = Instantiate(enemyPrefab, _spawnPoint.position, _spawnPoint.rotation);
            }
        }
    }

    public void ClearBodies()
    {
        Enemy[] _enemies = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach(Enemy _enemy in _enemies)
        {
            if (_enemy.Anim.enabled) continue;
            Destroy(_enemy.gameObject);
        }
    }

    public void ToggleTime()
    {
        Time.timeScale = Time.timeScale > 0.6f ? slowedTime : 1f;
        Time.fixedDeltaTime = Time.timeScale * cachedFixedDelta;
    }

    public LocalPlayer[] Players { get { return players; } }

    public enum DummyMode
    {
        Default,
        Running
    }
}
