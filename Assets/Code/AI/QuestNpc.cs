using System.Collections.Generic;
using System.Linq;
using Assets.Code.AI;
using SpectralDaze.DialogueSystem;
using SpectralDaze.Managers.InputManager;
using SpectralDaze.Player;
using SpectralDaze.Time;
using UnityEngine;
using UnityEngine.AI;

namespace SpectralDaze.AI.QuestNPC
{
    /// <summary>
    /// The Quest NPC mono behaviour
    /// </summary>
    /// <seealso cref="Assets.Code.AI.BaseAI" />
    public class QuestNpc : BaseAI
    {
        /// <summary>
        /// The options for the Quest NPC
        /// </summary>
        public QuestNPCOptions Options;
        /// <summary>
        /// The state machine
        /// </summary>
        private UStateMachine<QuestNpcParams> stateMachine;
        /// <summary>
        /// The parameters instance for the state machine
        /// </summary>
        private QuestNpcParams paramsInstance;

        /// <summary>
        /// The current dialogue scriptable objcet reference
        /// </summary>
        public CurrentDialogue CurrentDialogueReference;
        /// <summary>
        /// The interaction control
        /// </summary>
        public Control InteractControl;

        /// <summary>
        /// Information representing values to slow things down with based on waht kind of time manipulation
        /// </summary>
        public TimeInfo TimeInfo;
        /// <summary>
        /// The time being manipulated
        /// </summary>
        private bool _timeBeingManipulated;
        /// <summary>
        /// The time manipulation type
        /// </summary>
        private Manipulations _manipulationType;

        private void Start()
        {
            InteractControl = Resources.Load<Control>("Managers/InputManager/Interact");
            CurrentDialogueReference = Resources.Load<CurrentDialogue>("DialogueSystem/CurrentDialogue");
            paramsInstance = new QuestNpcParams()
            {
                NpcTransform = transform,
                Npc = this,
                NavAgent = GetComponent<NavMeshAgent>(),
                Animator = GetComponent<Animator>(),
                OriginPosistion = transform.position,
                MovementType = Options.MovementType,
                WanderDistance = Options.WanderDistance,
                IdleTime = Options.IdleTime,
                TimeLeftIdle = Options.IdleTime,
                PatrolPoints = Options.PatrolPoints,
                CurrentPatrolPoint = Options.StartingPatrolPoint,
                Player = FindObjectOfType<PlayerController>(),
                Dialogue = Options.Dialogue
        };
            stateMachine = new UStateMachine<QuestNpcParams>(paramsInstance, new Conversing(), new Idle(), new Move());
            stateMachine.SetState(typeof(Idle), paramsInstance);
            _manipulationType = Manipulations.Normal;
            paramsInstance.NavAgent.speed = paramsInstance.MovementSpeed*TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            paramsInstance.Animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
        }

        private void Update()
        {
            stateMachine.Update(paramsInstance);
        }

        private void FixedUpdate()
        {
            stateMachine.FixedUpdate(paramsInstance);
        }


        /// <summary>
        /// Starts time manipulation on the gameobject.
        /// </summary>
        /// <param name="type">The integer index of the Manipulations enum that represents the type of manipulation.</param>
        public void StartTimeManipulation(int type)
        {
            _timeBeingManipulated = true;
            _manipulationType = (Manipulations)type;
            paramsInstance.NavAgent.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            paramsInstance.Animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
            //_animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
        }

        /// <summary>
        /// Stops time manipulation on the gameobject.
        /// </summary>
        public void StopTimeManipulation()
        {
            _timeBeingManipulated = false;
            _manipulationType = Manipulations.Normal;
            paramsInstance.NavAgent.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
            paramsInstance.Animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
            //_animator.speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.AnimationModifier;
        }

        /// <summary>
        /// The conversing state in the state machine   
        /// </summary>
        /// <seealso cref="UState{SpectralDaze.AI.QuestNPC.QuestNpc.QuestNpcParams}" />
        private class Conversing : UState<QuestNpcParams>
        {
            /// <inheritdoc />
            public override void Enter(QuestNpcParams p)
            {
                //_dialogueManager = GameManager.Instance.DialogueManager;
            }

            /// <inheritdoc />
            public override void Update(QuestNpcParams p)
            {
                p.NpcTransform.rotation = Quaternion.LookRotation(p.Player.transform.position - p.NpcTransform.position);
                p.NpcTransform.eulerAngles = new Vector3(0, p.NpcTransform.eulerAngles.y, 0);
                if (p.Npc.InteractControl.JustPressed && p.Npc.CurrentDialogueReference.Dialogue==null)
                {
                    p.Npc.CurrentDialogueReference.Dialogue = p.Dialogue;
                }
            }

            /// <inheritdoc />
            public override void CheckForTransitions(QuestNpcParams p)
            {
                if (Vector3.Distance(p.NpcTransform.position, p.Player.transform.position) >= 4)
                {
                    Parent.SetState(typeof(Move), p);
                }
            }
        }

        /// <summary>
        /// The idle state in the state machine.
        /// </summary>
        /// <seealso cref="UState{SpectralDaze.AI.QuestNPC.QuestNpc.QuestNpcParams}" />
        private class Idle : UState<QuestNpcParams>
        {
            /// <summary>
            /// The time left in idle step.
            /// </summary>
            private float _timeLeftIdle = 0;

            /// <inheritdoc />
            public override void Enter(QuestNpcParams p)
            {
                _timeLeftIdle = p.IdleTime;
            }

            /// <inheritdoc />
            public override void FixedUpdate(QuestNpcParams p)
            {
                if (!p.NavAgent.pathPending && p.NavAgent.remainingDistance <= p.NavAgent.stoppingDistance &&
                    !p.NavAgent.hasPath || p.NavAgent.velocity.sqrMagnitude == 0f){
                    if (_timeLeftIdle > 0)
                    {
                        _timeLeftIdle = _timeLeftIdle - UnityEngine.Time.deltaTime;
                    }
                    else
                    {
                        Parent.SetState(typeof(Move), p);
                    }
                }
            }

            /// <inheritdoc />
            public override void CheckForTransitions(QuestNpcParams p)
            {
                if (Vector3.Distance(p.NpcTransform.position, p.Player.transform.position)<4)
                {
                    p.NavAgent.SetDestination(p.NpcTransform.position);
                    p.NavAgent.isStopped = true;
                    p.NpcTransform.LookAt(p.Player.transform.position);
                    Parent.SetState(typeof(Conversing), p);
                }
            }
        }

        /// <summary>
        /// The movement state in the state machine.
        /// </summary>
        /// <seealso cref="UState{SpectralDaze.AI.QuestNPC.QuestNpc.QuestNpcParams}" />
        private class Move : UState<QuestNpcParams>
        {
            /// <inheritdoc />
            public override void Enter(QuestNpcParams p)
            {
                p.NavAgent.isStopped = false;
                if (p.MovementType == MovementType.Wander || p.MovementType == MovementType.NoLimitsWander)
                {
                    Vector3 randomOffset = Random.insideUnitSphere * p.WanderDistance;
                    Vector3 randomWanderPosistion = randomOffset += p.NpcTransform.position;


                    NavMeshHit hit;
                    while (NavMesh.SamplePosition(randomWanderPosistion, out hit, 1, NavMesh.AllAreas) == false && Vector3.Distance(randomWanderPosistion, p.OriginPosistion) > p.WanderDistance || NavMesh.SamplePosition(randomWanderPosistion, out hit, 1, NavMesh.AllAreas) == false)
                    {
                        randomOffset = Random.insideUnitSphere * p.WanderDistance;
                        randomWanderPosistion = randomOffset += p.NpcTransform.position;
                    }
                    if (p.NavAgent.SetDestination(randomWanderPosistion))
                    {
                        Parent.SetState(typeof(Idle), p);
                    }
                    else
                    {
                        Debug.LogError("Error occured when setting destination of navmesh agent.");
                    }
                }
                else if (p.MovementType == MovementType.Patrol)
                {
                    p.CurrentPatrolPoint++;
                    if (p.CurrentPatrolPoint > p.PatrolPoints.Count-1)
                    {
                        p.CurrentPatrolPoint = 0;
                    }
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(p.PatrolPoints[p.CurrentPatrolPoint], out hit, p.WanderDistance, NavMesh.AllAreas))
                    {
                        if (p.NavAgent.SetDestination(p.PatrolPoints[p.CurrentPatrolPoint]))
                        {
                            Parent.SetState(typeof(Idle), p);
                        }
                        else
                        {
                            Debug.LogError("Error occured when setting destination of navmesh agent.");
                        }
                    }
                    else
                    {
                        Debug.LogError("Error occured when sampling posistion to get nearest navmesh point.");
                    }
                }
            }

            /// <inheritdoc />
            public override void CheckForTransitions(QuestNpcParams p)
            {
                if (Vector3.Distance(p.NpcTransform.position, p.Player.transform.position) < 4)
                {
                    p.NavAgent.SetDestination(p.NpcTransform.position);
                    p.NavAgent.isStopped = true;
                    p.NpcTransform.LookAt(p.Player.transform.position);
                    Parent.SetState(typeof(Conversing), p);
                }
            }
        }

        /// <summary>
        /// The different types of movement
        /// </summary>
        public enum MovementType
        {
            Patrol,
            Wander,
            NoLimitsWander
        }

        /// <summary>
        /// Struct of parameters being passed to the state machine
        /// </summary>
        private struct QuestNpcParams
        {
            /// <summary>
            /// The NPC transform
            /// </summary>
            public Transform NpcTransform;
            /// <summary>
            /// The NPC
            /// </summary>
            public QuestNpc Npc;
            /// <summary>
            /// The nav agent
            /// </summary>
            public NavMeshAgent NavAgent;
            /// <summary>
            /// The animator
            /// </summary>
            public Animator Animator;
            /// <summary>
            /// The origin posistion
            /// </summary>
            public Vector3 OriginPosistion;
            /// <summary>
            /// The movement type
            /// </summary>
            public MovementType MovementType;
            /// <summary>
            /// The distance that the AI can wander.
            /// </summary>
            public float WanderDistance;
            /// <summary>
            /// The idle time
            /// </summary>
            public float IdleTime;
            /// <summary>
            /// The time left in the idle step
            /// </summary>
            public float TimeLeftIdle;
            /// <summary>
            /// The current patrol point
            /// </summary>
            public int CurrentPatrolPoint;
            /// <summary>
            /// The patrol points
            /// </summary>
            public List<Vector3> PatrolPoints;
            /// <summary>
            /// The player controller
            /// </summary>
            public PlayerController Player;
            /// <summary>
            /// The movement speed
            /// </summary>
            public float MovementSpeed;
            /// <summary>
            /// The dialogue that is attached to this npc
            /// </summary>
            public TextAsset Dialogue;
        }
    }

}