using UnityEngine;

public class SpawnManager : MonoBehaviour {
    [Header("Spawn Positions")]
    [Tooltip("IMPORTANT : The spawn points have to be references in a special order : \nFirst point is North then rotating Clockwise")] //L'ordre sera utilisé pour la depth des Sprites Renderers
    [SerializeField] Transform[] spawnPositions;

    [Header("Enemies Prefab")]
    [SerializeField] EnemyBehavior[] enemiesPrefabs;

    public bool RoundDone {
        get {
            return lastEnemy == null;
        }
    }
    EnemyBehavior lastEnemy;
    public static SpawnManager Instance { get; private set; }
    void Start() {
        if (!Instance) Instance = this;
        if (spawnPositions.Length == 0) Debug.LogWarning("SpawnPositions is empty");
        if (enemiesPrefabs.Length == 0) Debug.LogWarning("EnemiesPrefabs is empty");
    }

    void Update() {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(1)) {
            Spawn();
        }
#endif
    }

    public void Spawn(int positionIndex = -1, int enemyIndex = -1, int enemyPattern = -1) {
        if (positionIndex <= -1) {
            positionIndex = Random.Range(0, spawnPositions.Length);
        }
        if (enemyIndex <= -1) {
            enemyIndex = Random.Range(0, enemiesPrefabs.Length);
        }

        lastEnemy = Instantiate(enemiesPrefabs[enemyIndex], spawnPositions[positionIndex].position, Quaternion.identity);
        lastEnemy.InitInputsPattern(enemyPattern);
    }
}
