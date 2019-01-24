using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace SpectralDaze.AI
{
    public class FollowerScript : MonoBehaviour
    {
        public GameObject Target;
        private NavMeshAgent _agent;

        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            Target = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            NavMeshHit targetSample;
            NavMesh.SamplePosition(Target.transform.position-Target.transform.forward, out targetSample, 2, NavMesh.AllAreas);
            _agent.SetDestination(targetSample.position);
        }
    }

}