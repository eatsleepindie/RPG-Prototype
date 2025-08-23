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

    [Space(7.5f)]
    [Header("Target Dummy")]
    [SerializeField] DummyMode dummyMode;
    [SerializeField] GameObject dummyPrefab;
    [SerializeField] Slider dummyHealthBar;

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
        switch(dummyMode)
        {
            default:
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
                        Instantiate(dummyPrefab, _enemy.transform);
                        dummyHealthBar = _enemy.GetComponentInChildren<CharacterCanvas>().GetComponentInChildren<Slider>();
                        dummyHealthBar.value = 100f;
                    }
                }
                break;
            case DummyMode.Running:
                //Enemy _enemy = Instantiate(dummyPrefab, Vector3.forward * 15f, Quaternion.Euler(Vector3.up * 180f));
                //_enemy.GetComponentInChildren<Animator>().Play("Run");
                break;
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

    public Slider DummyHealthBar { get { return dummyHealthBar; } }

    public enum DummyMode
    {
        Default,
        Running
    }
}
