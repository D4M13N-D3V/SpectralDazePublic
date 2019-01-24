using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SpectralDaze.Managers;
using SpectralDaze.AI;
using SpectralDaze.DialogueSystem;
using SpectralDaze.Player;
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

        public GameObject StoreExit;

        private GameObject gameCam;
        private GameObject player;
        private GameObject inputManager;
        private GameObject shopKeeper;
        private DialogueManager dialogueManager;

        [HideInInspector]
        public StoreEntrance UsedEntrance;

        public TextAsset WelcomeDialogue;

        void Start()
        {
            this.StoreExit.SetActive(false);
            gameCam = GameObject.FindGameObjectWithTag("GameCam");
            inputManager = GameObject.FindGameObjectWithTag("InputManager");
            player = GameObject.FindGameObjectWithTag("Player");
            shopKeeper = GameObject.FindGameObjectWithTag("Shopkeeper");
            dialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();

            gameCam.SetActive(false);
            player.GetComponent<PowerManager>().SetPowersEnabled(false);
            player.GetComponent<Player.PlayerController>().PlayerInfo.CanMove = false;
    
            player.transform.LookAt(shopKeeper.transform.position);
            player.GetComponent<NavMeshAgent>().isStopped = true;
            player.GetComponent<NavMeshAgent>().ResetPath();
            player.GetComponent<NavMeshAgent>().velocity = Vector3.zero;

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
                yield return new WaitForSeconds(0.1f);
            }
            player.GetComponent<NavMeshAgent>().isStopped = true;
            player.GetComponent<NavMeshAgent>().ResetPath();
            shopKeeper.GetComponent<NavMeshAgent>().isStopped = true;
            shopKeeper.GetComponent<NavMeshAgent>().ResetPath();
            shopKeeper.GetComponent<FollowerScript>().enabled = true;
            dialogueManager.StartDialogue(WelcomeDialogue);
            inputManager.GetComponent<Managers.InputManager.InputManager>().SetInputEnabled(true);
            this.StoreExit.SetActive(true);
        }

        public void ExitShop()
        {
            player.GetComponent<NavMeshAgent>().isStopped = true;
            player.GetComponent<NavMeshAgent>().ResetPath();
            shopKeeper.GetComponent<NavMeshAgent>().isStopped = true;
            shopKeeper.GetComponent<NavMeshAgent>().ResetPath();
            shopKeeper.GetComponent<FollowerScript>().enabled = false;
            gameCam.SetActive(true);
            player.GetComponent<PowerManager>().SetPowersEnabled(true);
            player.GetComponent<Player.PlayerController>().PlayerInfo.CanMove = true;
            NavMeshHit entranceSample;
            NavMesh.SamplePosition(UsedEntrance.transform.position+UsedEntrance.transform.forward*2, out entranceSample, 1, NavMesh.AllAreas);
            player.GetComponent<NavMeshAgent>().Warp(entranceSample.position);
            UsedEntrance.SendMessage("UnloadShop");
        }
    }
}