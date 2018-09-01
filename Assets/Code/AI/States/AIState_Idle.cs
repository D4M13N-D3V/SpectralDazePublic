using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace gmtk.AI
{
	public class AIState_Idle : UState<AIStateParams>
	{
		private const float SPAWN_DIST = 6f;

		private Transform player;
		private RaycastHit[] raycastResults;

		public override void Enter(AIStateParams ps)
		{
			player = GameObject.FindWithTag("Player").transform;
			raycastResults = new RaycastHit[16];
		}

		public override void Update(AIStateParams ps)
		{
			// Initially check within a radius around our AI, this way we don't waste any unnecessary resources if they aren't in range.
			float dist = Vector3.Distance(ps.Transform.position, player.transform.position);

			if (dist > SPAWN_DIST)
				return;

			if (!CanSeePlayer(ps))
				return;

			ps.Controller.CachedTarget = player;
			// TODO - Add a check here, we could possibly jump directly into attacking.
			Parent.SetState(typeof(AIState_Chase), ps);
		}

		private bool CanSeePlayer(AIStateParams ps)
		{
			Vector3 dir = (player.position - ps.Transform.position).normalized;
			Ray ray = new Ray(ps.Transform.position, dir);
			RaycastHit hit;

			if (!Physics.Raycast(ray, out hit)) return false;

			if (hit.distance > 10f)
				return false;

			return hit.transform == ps.Controller.CachedTarget || hit.transform.IsChildOf(ps.Controller.CachedTarget);
		}
	}
}