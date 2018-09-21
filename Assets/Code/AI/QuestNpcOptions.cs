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
        public QuestNpc.MovementType MovementType;
        public float WanderDistance;
        public float IdleTime;
        public int StartingPatorlPoint;
        public List<Vector3> PatrolPoints;
        //public Conversation Conversation;
        public float MovementModifier;
        public float MovementSpeed=3;
    }
}
