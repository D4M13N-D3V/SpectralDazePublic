using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace gmtk.AI
{
	public class AIState_Idle : UState<AIStateParams>
	{
		private const float CHASE_DIST = 6f;

		private Transform player;

		public override void Enter(AIStateParams ps)
		{
			player = GameObject.FindWithTag("Player").transform;
		}

		public override void Update(AIStateParams ps)
		{
			// Initially check within a radius around our AI, this way we don't waste any unnecessary resources if they aren't in range.
			float dist = Vector3.Distance(ps.Transform.position, player.transform.position);

			if (dist > CHASE_DIST)
				return;

			ps.Controller.CachedTarget = player;

			if (!ps.CanSeePlayer())
			{
				Debug.Log("Can't see player, exiting");
				ps.Controller.CachedTarget = null;
				return;
			}

			// TODO - Add a check here, we could possibly jump directly into attacking.
			Parent.SetState(typeof(AIState_Chase), ps);
		}

		public override void OnDrawGizmos(AIStateParams ps)
		{
			using(new GizmoColorScope(Color.yellow))
			Gizmos.DrawWireSphere(ps.Transform.position, CHASE_DIST);
		}
	}
}