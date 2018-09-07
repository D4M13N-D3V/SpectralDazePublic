using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SpectralDaze.Player;
using SpectralDaze.ScriptableObjects.Time;
using SpectralDaze.Time;

public class Bullet : MonoBehaviour
{
	public float Speed = 4f;

	private Vector3 cachedPosition;

    public Information TimeInfo;
    private bool _timeBeingManipulated;
    private Manipulations _manipulationType;

    private void Start()
    {
        TimeInfo.Data.SingleOrDefault(x => x.Type == Manipulations.Normal).Stats.MovementModifier = Speed;
    }

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

    /*
     * Time Bubble/Manipulation Code
     */
    public void StartTimeManipulation(int type)
    {
        _timeBeingManipulated = true;
        _manipulationType = (Manipulations)type;
        Speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
    }

    public void StopTimeManipulation()
    {
        _timeBeingManipulated = true;
        _manipulationType = Manipulations.Normal;
        Speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
    }
}