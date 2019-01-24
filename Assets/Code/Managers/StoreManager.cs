using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SpectralDaze.Managers;
using SpectralDaze.AI;
namespace SpectralDaze.Store
{
    public class StoreManager : MonoBehaviour
    {
        [SerializeField]
        private Transform _shopKeeperGreetingPoint;
        [SerializeField]
        private Transform _playerGreetingPoint;
        [SerializeField]
        private Transform _shopKeeperStartingPoint;
        [SerializeField]
        private Transform _playerStartingPoint;

        private GameObject gameCam;
        private GameObject player;
        public GameObject inputManager;
        public GameObject shopKeeper;

        void Start()
        {
            gameCam = GameObject.FindGameObjectWithTag("GameCam");
            inputManager = GameObject.FindGameObjectWithTag("InputManager");
            player = GameObject.FindGameObjectWithTag("Player");
            shopKeeper = GameObject.FindGameObjectWithTag("Shopkeeper");

            gameCam.SetActive(false);
            player.GetComponent<PowerManager>().SetPowersEnabled(false);
            inputManager.GetComponent<Managers.InputManager.InputManager>().SetInputEnabled(false);

            NavMeshHit playerStartingPointNavHit;
            NavMeshHit shopKeeperStartingPointNavHit;
            NavMeshHit playerGreetingPointNavHit;
            NavMeshHit shopKeeperGreetingPointNavHit;

            NavMesh.SamplePosition(_playerGreetingPoint.position, out playerGreetingPointNavHit, 1f, NavMesh.AllAreas);
            NavMesh.SamplePosition(_shopKeeperGreetingPoint.position, out shopKeeperGreetingPointNavHit, 1f, NavMesh.AllAreas);
            NavMesh.SamplePosition(_playerStartingPoint.position, out playerStartingPointNavHit, 1f, NavMesh.AllAreas);
            NavMesh.SamplePosition(_shopKeeperStartingPoint.position, out shopKeeperStartingPointNavHit, 1f, NavMesh.AllAreas);

            player.GetComponent<NavMeshAgent>().Warp(playerStartingPointNavHit.position);
            shopKeeper.GetComponent<NavMeshAgent>().Warp(shopKeeperStartingPointNavHit.position);
            player.GetComponent<NavMeshAgent>().SetDestination(playerGreetingPointNavHit.position);
            shopKeeper.GetComponent<NavMeshAgent>().SetDestination(shopKeeperGreetingPointNavHit.position);

            StartCoroutine(WaitForPlayerToGetToGreetPoint());
        }

        IEnumerator WaitForPlayerToGetToGreetPoint()
        {
            yield return new WaitForSeconds(1);
            while (player.GetComponent<NavMeshAgent>().isStopped)
            {
                yield return new WaitForSeconds(1);
            }
            player.GetComponent<NavMeshAgent>().isStopped = true;
            player.GetComponent<NavMeshAgent>().ResetPath();
            shopKeeper.GetComponent<NavMeshAgent>().isStopped = true;
            shopKeeper.GetComponent<NavMeshAgent>().ResetPath();
            shopKeeper.GetComponent<FollowerScript>().enabled = true;
            inputManager.GetComponent<Managers.InputManager.InputManager>().SetInputEnabled(true);
        }
    }
}