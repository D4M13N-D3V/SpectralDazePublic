using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpectralDaze.AI;
using UnityEngine;
using SpectralDaze.Player;
using SpectralDaze.Time;

/// <summary>
/// Manages the projectile attached to.
/// </summary>
public class Bullet : MonoBehaviour
{
    /// <summary>
    /// The local time scale
    /// </summary>
    public float _localTimeScale = 1.0f;
    /// <summary>
    /// Gets or sets the local time scale.
    /// </summary>
    /// <value>
    /// The local time scale.
    /// </value>
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
    /// <summary>
    /// Gets the local delta time.
    /// </summary>
    /// <value>
    /// The local delta time.
    /// </value>
    public float localDeltaTime
    {
        get
        {
            return UnityEngine.Time.deltaTime * UnityEngine.Time.timeScale * _localTimeScale;
        }
    }

    /// <summary>
    /// The speed
    /// </summary>
    public float Speed = 4f;
    /// <summary>
    /// Is homing
    /// </summary>
    public bool Homing = false;
    /// <summary>
    /// The cached position
    /// </summary>
    private Vector3 cachedPosition;

    /// <summary>
    /// The source that fired the gameobject
    /// </summary>
    public GameObject Source;

    /// <summary>
    /// The time information for time manipulation
    /// </summary>
    public TimeInfo TimeInfo;
    /// <summary>
    /// Is the time being manipulated
    /// </summary>
    private bool _timeBeingManipulated;
    /// <summary>
    /// The manipulation type
    /// </summary>
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

    /// <summary>
    /// Starts the time manipulation.
    /// </summary>
    /// <param name="type">The type.</param>
    public void StartTimeManipulation(int type)
    {
        _timeBeingManipulated = true;
        _manipulationType = (Manipulations)type;
        Speed = TimeInfo.Data.SingleOrDefault(x => x.Type == _manipulationType).Stats.MovementModifier;
    }
    /// <summary>
    /// Stops the time manipulation.
    /// </summary>
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
