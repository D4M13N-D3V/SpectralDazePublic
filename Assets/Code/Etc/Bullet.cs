using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.AI;
using UnityEngine;
using SpectralDaze.Player;
using SpectralDaze.ScriptableObjects.Time;
using SpectralDaze.Time;

public class Bullet : MonoBehaviour
{
    public float _localTimeScale = 1.0f;
    public float localTimeScale
    {
        get
        {
            return _localTimeScale;
        }
        set
        {
            float multiplier = value / _localTimeScale;

            _localTimeScale = value;
        }
    }
    public float localDeltaTime
    {
        get
        {
            return UnityEngine.Time.deltaTime * UnityEngine.Time.timeScale * _localTimeScale;
        }
    }

    public float Speed = 4f;
    public bool Homing = false;
    private Vector3 cachedPosition;

    public GameObject Source;

    public Information TimeInfo;
    private bool _timeBeingManipulated;
    private Manipulations _manipulationType;

    private void Start()
    {
        transform.rotation = Quaternion.LookRotation(GameObject.FindObjectOfType<PlayerController>().gameObject.transform.position - transform.position);
    }

    private void Update()
    {
        if (Homing)
            transform.rotation = Quaternion.LookRotation(GameObject.FindObjectOfType<PlayerController>().gameObject.transform.position - transform.position);
        Vector3 velocity = transform.position - cachedPosition;

        Ray r = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit))
        {
            if (hit.collider.tag == "Player" && hit.distance < 0.1f && hit.collider.gameObject != Source)
            {
                hit.collider.GetComponent<PlayerController>().EndGame();
                Destroy(gameObject);
            }
            else if (hit.collider.tag == "Enemy" && hit.distance < 0.6f && hit.collider.gameObject != Source)
            {
                foreach (var comp in hit.collider.gameObject.GetComponents(typeof(Component)))
                {
                    //SHouldnt have to use refelect GetComponent should work but isnt.
                    if (comp.GetType().IsSubclassOf(typeof(BaseAI))) { var enemy = (BaseAI)comp; enemy.Die(); }
                }
                Destroy(gameObject);
            }
            else if (hit.collider.gameObject != Source && hit.distance < 0.1f)
            {
                Debug.Log(hit.collider.gameObject);
                Destroy(gameObject);
            }
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