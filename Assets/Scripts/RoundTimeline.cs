using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Round Timeline", menuName = "Round Timeline")]
public class RoundTimeline : ScriptableObject {

    public float TotalTime {
        get {
            float totalTime = 0;
            foreach (var item in events) {
                totalTime += item.spawnTime;
            }
            return totalTime;
        }
    }
    public List<Event> events;

    [Serializable]
    public struct Event : IComparable {
        public enum EnemyName {
            Random = -1,
            Heavy = 0,
            Light = 1,
            Lion = 2,
            Taureau = 3,
        }
        public enum SpawnPosition {
            Random = -1,
            pos0 = 0,
            pos1 = 1,
            pos2 = 2,
            pos3 = 3,
            pos4 = 4,
            pos5 = 5,
            pos6 = 6,
            pos7 = 7,
            pos8 = 8,
        }
        public EnemyName enemyName;
        public int enemy => (int)enemyName;
        public float spawnTime;
        public SpawnPosition positionIndex;
        public int position => (int)positionIndex;
        public int patternIndex;

        public int CompareTo(object obj) {
            if (obj == null) return 1;
            if (obj is Event otherCheckpoint) {
                return this.spawnTime.CompareTo(otherCheckpoint.spawnTime);
            } else {
                throw new ArgumentException("Object is not a Checkpoint");
            }
        }
    }

    private void OnValidate() {
        events.Sort();
    }
}