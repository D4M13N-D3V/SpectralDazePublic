using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [Serializable]
    public class EnemySpawnerEntry
    {
        public GameObject Prefab;
        public Transform Posistion;
    }

    //public Transform LookAtLocation;
    public List<EnemySpawnerEntry> Enemys = new List<EnemySpawnerEntry>();
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

    public void Enable()
    {
        foreach (var obj in SpawnedEnemys)
        {
            obj.SetActive(true);
        }
    }
}
