using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace gmtk.Player
{
	[CreateAssetMenu(fileName = "Power_Warp", menuName = "SO/PlayerPower/Warp")]
	public class PlayerPower_Warp : PlayerPower
	{
		public override void OnUpdate(PlayerController pc)
		{
			RaycastHit mouseHit;
			if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouseHit))
				return;

            // Switch this out with something better.
            if (mouseHit.collider.gameObject.tag != "Walkable")
                return;

			Vector3 adjustedHitPoint = mouseHit.point + Vector3.up;
			if (Vector3.Distance(adjustedHitPoint, pc.transform.position) > 12f)
				return;

			if (Input.GetMouseButton(0))
			{
				// If they're holding right click
				mouseHit.point.DrawDebug(Color.red, 0.1f, 0.25f);
				Time.timeScale = 0.10f;
			}
			if (Input.GetMouseButtonUp(0))
			{
				// Move the player
				Time.timeScale = 1f;
				pc.transform.position = mouseHit.point + Vector3.up;
			}
		}
	}
}