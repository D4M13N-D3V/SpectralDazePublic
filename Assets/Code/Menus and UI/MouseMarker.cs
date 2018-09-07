using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMarker : MonoBehaviour
{

    public Material ValidPoint;
    public Material NotValidPoint;

    private Renderer _renderer;
    private ParticleSystem particleSys;

    private void Start()
    {
        _renderer = this.gameObject.GetComponentInChildren<Renderer>();
        particleSys = this.gameObject.GetComponentInChildren<ParticleSystem>();
        UnityEngine.Cursor.visible = false;
    }

    private void Update()
    {
		if (!Input.GetMouseButton(0))
		{
			_renderer.material = NotValidPoint;
			particleSys.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
		}
		else
		{
			_renderer.material = ValidPoint;
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
            _renderer.material = NotValidPoint;
            if (!particleSys.isStopped)
            particleSys.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        }
        else
        {
            _renderer.material = ValidPoint;
            if (particleSys.isStopped)
                particleSys.Play();
        }
    }
}
