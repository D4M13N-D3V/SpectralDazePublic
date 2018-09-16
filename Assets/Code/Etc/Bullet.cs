using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.AI;
using UnityEngine;
using SpectralDaze.Player;
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

    public TimeInfo TimeInfo;
    private bool _timeBeingManipulated;
    private Manipulations _manipulationType;


    private void Update()
    {
        if (Homing)
        {
            transform.rotation = Quaternion.LookRotation(GameObject.FindObjectOfType<PlayerController>().gameObject.transform.position - transform.position);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
        Vector3 velocity = transform.position - cachedPosition;
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().EndGame();
        }
        else if (collision.gameObject.tag == "Enemy" && collision.gameObject != Source)
        {
            foreach (var comp in collision.gameObject.GetComponents(typeof(Component)))
            {
                if (comp.GetType().IsSubclassOf(typeof(BaseAI))) { var enemy = (BaseAI)comp; enemy.Die(); }
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag=="Wall" || collision.gameObject.tag=="Obstacle" && collision.gameObject != Source && !collision.collider.isTrigger)
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Reflective"))
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,-transform.eulerAngles.y, transform.eulerAngles.z);
        }

    }
}
