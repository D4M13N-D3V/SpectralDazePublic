using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpectralDaze.AI.QuestNPC;
using UnityEngine;

namespace SpectralDaze.AI
{
    [CreateAssetMenu(menuName = "Spectral Daze/AI/QuestNPCSettings")]
    public class QuestNPCOptions : ScriptableObject
    {
        /// <summary>
        /// The movement type
        /// </summary>
        public QuestNpc.MovementType MovementType;
        /// <summary>
        /// The wander distance
        /// </summary>
        public float WanderDistance;
        /// <summary>
        /// The idle time
        /// </summary>
        public float IdleTime;
        /// <summary>
        /// The starting patrol point
        /// </summary>
        public int StartingPatrolPoint;
        /// <summary>
        /// The patrol points
        /// </summary>
        public List<Vector3> PatrolPoints;
        /// <summary>
        /// The movement speed
        /// </summary>
        public float MovementSpeed=3;
        /// <summary>
        /// The dialogue connected to this quest npc.
        /// </summary>
        public TextAsset Dialogue;
    }
}
