using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.World
{
    /// <summary>
    /// Spawns enemys on given posistions with given prefabs.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class EnemySpawner : MonoBehaviour
    {

        /// <summary>
        /// Enemy spawn information
        /// </summary>
        [Serializable]
        public class EnemySpawnerEntry
        {
            /// <summary>
            /// Enemy Prefab
            /// </summary>
            public GameObject Prefab;
            /// <summary>
            /// The transform the spawn the enemy on.
            /// </summary>
            public Transform Posistion;
        }

        /// <summary>
        /// The enemys spawn infomrations
        /// </summary>
        public List<EnemySpawnerEntry> Enemys = new List<EnemySpawnerEntry>();
        /// <summary>
        /// The spawned enemys cache.
        /// </summary>
        private List<GameObject> SpawnedEnemys = new List<GameObject>();

        private void Start()
        {
            foreach (var enemy in Enemys)
            {
                var obj = Instantiate(enemy.Prefab, enemy.Posistion.position, enemy.Posistion.rotation);
                obj.transform.parent = transform;
                obj.SetActive(false);
                SpawnedEnemys.Add(obj);
            }
        }

        /// <summary>
        /// Enables the spawner.
        /// </summary>
        public void Enable()
        {
            foreach (var obj in SpawnedEnemys)
            {
                obj.SetActive(true);
            }
        }
    }

}