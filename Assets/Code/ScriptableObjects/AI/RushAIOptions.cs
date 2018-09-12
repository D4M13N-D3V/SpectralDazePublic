using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpectralDaze.AI;
using SpectralDaze.AI.QuestNPC;
using SpectralDaze.ScriptableObjects.Conversations;
using SpectralDaze.ScriptableObjects.Managers.Audio;
using UnityEngine;

namespace SpectralDaze.ScriptableObjects.AI
{
    [CreateAssetMenu(menuName = "Spectral Daze/AI/Chase AI Settings")]
    public class RushAIOptions : ScriptableObject
    {
        public RushAI.MovementType MovementType;
        public float WanderDistance;
        public float IdleTime;
        public int StartingPatorlPoint;
        public List<Vector3> PatrolPoints;
        public float AggroDistance;
        public float LoseAggroDistance;
        public float TimeBetweenCharges;
        public float LaunchVelocity = 10;
        public float MovementSpeed = 3;
        public bool Chase;
        public float ChaseDistance = 20;
        public AudioClipInfo DeathSound;
    }
}
