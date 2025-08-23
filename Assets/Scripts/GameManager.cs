using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public LayerMask EnemyMask;

    [SerializeField] LocalPlayer[] players;
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] Transform spawnPoint;

    [Range(0f, 1f)]
    [SerializeField] float slowedTime = 0.33f;

    [Space(7.5f)]
    [Header("Target Dummy")]
    [SerializeField] DummyMode dummyMode;
    [SerializeField] GameObject dummyPrefab;
    [SerializeField] Slider dummyHealthBar;

    Enemy enemy;

    float cachedFixedDelta;

    private void Awake()
    {
        Instance = this;
        cachedFixedDelta = Time.fixedDeltaTime;

        Enemy[] _enemies = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        if (_enemies.Length > 0)
            enemy = _enemies[0];
        else
            SpawnTarget();
    }

    public void SpawnTarget()
    {
        List<GameObject> _targets = new List<GameObject>(GameObject.FindGameObjectsWithTag("Target"));
        for (int _i = _targets.Count - 1; _i >= 0; _i--)
            Destroy(_targets[_i]);
        switch(dummyMode)
        {
            default:
                enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                Instantiate(dummyPrefab, enemy.transform);
                dummyHealthBar = enemy.GetComponentInChildren<CharacterCanvas>().GetComponentInChildren<Slider>();
                dummyHealthBar.value = 100f;
                break;
            case DummyMode.Running:
                //Enemy _enemy = Instantiate(dummyPrefab, Vector3.forward * 15f, Quaternion.Euler(Vector3.up * 180f));
                //_enemy.GetComponentInChildren<Animator>().Play("Run");
                break;
        }
    }

    public void ToggleTime()
    {
        Time.timeScale = Time.timeScale > 0.6f ? slowedTime : 1f;
        Time.fixedDeltaTime = Time.timeScale * cachedFixedDelta;
    }

    public LocalPlayer[] Players { get { return players; } }

    public Slider DummyHealthBar { get { return dummyHealthBar; } }

    public Enemy Enemy { get { return enemy; } }

    public enum DummyMode
    {
        Default,
        Running
    }
}
