using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpectralDaze.Player;

public class Bullet : MonoBehaviour
{
	public float Speed = 4f;

	private Vector3 cachedPosition;

	private void Update()
	{
		Vector3 velocity = transform.position - cachedPosition;

		Ray r = new Ray(transform.position, transform.forward);
		RaycastHit hit;
		if (Physics.Raycast(r, out hit))
		{
			if (hit.distance > velocity.magnitude)
				goto End;
			if (hit.collider.tag == "Player")
				hit.collider.GetComponent<PlayerController>().EndGame();
		}

		End:
		transform.position += transform.forward * Time.deltaTime * Speed;
	}
}