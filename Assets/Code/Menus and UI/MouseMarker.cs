using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMarker : MonoBehaviour
{

    public Material ValidPoint;
    public Material NotValidPoint;

    private Renderer renderer;
    private ParticleSystem particleSys;

    private void Start()
    {
        renderer = this.gameObject.GetComponentInChildren<Renderer>();
        particleSys = this.gameObject.GetComponentInChildren<ParticleSystem>();
        UnityEngine.Cursor.visible = false;
    }

    private void Update()
    {
		if (!Input.GetMouseButton(0))
		{
			renderer.material = NotValidPoint;
			particleSys.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
		else
		{
			renderer.material = ValidPoint;
			if (particleSys.isStopped)
				particleSys.Play();
		}

		Cursor.visible = false;

        RaycastHit mouseHit;
        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouseHit))
            return;

        this.transform.position = mouseHit.point;

        if (mouseHit.collider.gameObject.tag != "Walkable")
        {
            renderer.material = NotValidPoint;
            if (!particleSys.isStopped)
            particleSys.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }
        else
        {
            renderer.material = ValidPoint;
            if (particleSys.isStopped)
                particleSys.Play();
        }
    }
}
