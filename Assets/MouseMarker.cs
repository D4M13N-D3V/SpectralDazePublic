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

    // Update is called once per frame
    void Update()
    {

        if (UnityEngine.Cursor.visible == true)
            UnityEngine.Cursor.visible = false;


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
