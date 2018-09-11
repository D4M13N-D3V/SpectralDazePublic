using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpectralDaze.AI;
using SpectralDaze.AI.QuestNPC;
using SpectralDaze.ScriptableObjects.Conversations;
using UnityEngine;

namespace SpectralDaze.ScriptableObjects.AI
{
    [CreateAssetMenu(menuName = "Spectral Daze/AI/Shooting AI Options")]
    public class ShootingAIOptions : ScriptableObject
    {
        public ShootingAI.MovementType MovementType;
        public float WanderDistance;
        public float IdleTime;
        public int StartingPatorlPoint;
        public List<Vector3> PatrolPoints;
        public float AggroDistance;
        public float LoseAggroDistance;
        public float TimeBetweenAttacks;
        public float MovementSpeed = 3;
        public GameObject BulletPrefab;
        public float ShootDelay;
        public float AttackChargeAmount = 0.5f;
        public bool Chase = true;
        public float ChaseDistance = 20;
        public AudioClip ShootingSound;
    }
}
 