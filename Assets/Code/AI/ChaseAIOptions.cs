using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using SpectralDaze.AI;
using SpectralDaze.AI.QuestNPC;
using SpectralDaze.Managers.AudioManager;
using UnityEngine;

namespace SpectralDaze.AI
{
    [CreateAssetMenu(menuName = "Spectral Daze/AI/Chase AI Settings")]
    public class ChaseAIOptions : ScriptableObject
    {
        [BoxGroup("Movement",true,true,1)]
        public ChaseAI.MovementType MovementType;
        [BoxGroup("Movement", true, true, 1)]
        public float WanderDistance;
        [BoxGroup("Movement", true, true, 1)]
        public float IdleTime;
        [BoxGroup("Movement", true, true, 1)]
        public int StartingPatorlPoint;
        [BoxGroup("Movement", true, true, 1)]
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
