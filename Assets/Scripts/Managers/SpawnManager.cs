using UnityEngine;

public class SpawnManager : MonoBehaviour {
    [Header("Spawn Positions")]
    [Tooltip("IMPORTANT : The spawn points have to be references in a special order, First point is N then rotating Clockwise")]
    [SerializeField] Transform[] spawnPositions;

    [Header("Enemies Prefab")]
    [SerializeField] GameObject[] enemiesPrefabs;
    public static SpawnManager Instance { get; private set; }
    void Start() {
        if (!Instance) Instance = this;
        if (spawnPositions.Length == 0) Debug.LogWarning("SpawnPositions is empty");
        if (enemiesPrefabs.Length == 0) Debug.LogWarning("EnemiesPrefabs is empty");
    }

    void Update() {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.S)) {
            Spawn();
        }
#endif
    }

    public void Spawn(int positionIndex = -1, int enemiIndex = -1) {
        if (positionIndex <= -1) {
            positionIndex = Random.Range(0, spawnPositions.Length);
        }
        if (enemiIndex <= -1) {
            enemiIndex = Random.Range(0, enemiesPrefabs.Length);
        }
        Instantiate(enemiesPrefabs[enemiIndex], spawnPositions[positionIndex].position, Quaternion.identity);
    }
}
