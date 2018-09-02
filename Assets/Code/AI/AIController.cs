using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace gmtk.AI
{
	public class AIController : MonoBehaviour
	{
		public Transform CachedTarget;
		public Vector3? LastKnownPosition;

		private UStateMachine<AIStateParams> stateMachine;

		private AIStateParams paramsInstance;

		private void Start()
		{
			paramsInstance = new AIStateParams()
			{
				Controller = this,
				NavMeshAgent = GetComponent<NavMeshAgent>(),
				Transform = transform
			};
			stateMachine = new UStateMachine<AIStateParams>(paramsInstance,
				new AIState_Idle(),
				new AIState_Chase()
				);
			stateMachine.SetState(typeof(AIState_Idle), paramsInstance);
		}

		private void Update()
		{
			stateMachine.Update(paramsInstance);
		}

		private void OnGUI()
		{
			stateMachine.OnGUI(paramsInstance);
			GUI.Label(new Rect(0, 0, 100, 50), stateMachine.ActiveState.GetType()+"");
		}
	}
}