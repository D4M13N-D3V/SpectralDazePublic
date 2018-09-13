using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Camera;
using SpectralDaze.Player;
using SpectralDaze.ScriptableObjects.Managers.Audio;
using UnityEngine;

public class DestructableWall : Wall
{
    public AudioClipInfo DestructionSound;
    private AudioQueue AudioQueue;

    void Start()
    {
        AudioQueue = Resources.Load<AudioQueue>("Managers/Audio/AudioQueue");
    }

    public IEnumerator SplitMesh(bool destroy)
    {
        AudioQueue.Queue.Enqueue(DestructionSound);
        UnityEngine.Camera.main.GetComponent<CameraFunctions>().Shake(0.275f,0.4f);
        if (GetComponent<MeshFilter>() == null || GetComponent<SkinnedMeshRenderer>() == null)
        {
            yield return null;
        }

        if (GetComponent<Collider>())
        {
            GetComponent<Collider>().enabled = false;
        }

        Mesh M = new Mesh();
        if (GetComponent<MeshFilter>())
        {
            M = GetComponent<MeshFilter>().mesh;
        }
        else if (GetComponent<SkinnedMeshRenderer>())
        {
            M = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }

        Material[] materials = new Material[0];
        if (GetComponent<MeshRenderer>())
        {
            materials = GetComponent<MeshRenderer>().materials;
        }
        else if (GetComponent<SkinnedMeshRenderer>())
        {
            materials = GetComponent<SkinnedMeshRenderer>().materials;
        }

        Vector3[] verts = M.vertices;
        Vector3[] normals = M.normals;
        Vector2[] uvs = M.uv;
        for (int submesh = 0; submesh < M.subMeshCount; submesh++)
        {

            int[] indices = M.GetTriangles(submesh);

            for (int i = 0; i < indices.Length; i += 3)
            {
                Vector3[] newVerts = new Vector3[3];
                Vector3[] newNormals = new Vector3[3];
                Vector2[] newUvs = new Vector2[3];
                for (int n = 0; n < 3; n++)
                {
                    int index = indices[i + n];
                    newVerts[n] = verts[index];
                    newUvs[n] = uvs[index];
                    newNormals[n] = normals[index];
                }

                Mesh mesh = new Mesh();
                mesh.vertices = newVerts;
                mesh.normals = newNormals;
                mesh.uv = newUvs;

                mesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };

                GameObject GO = new GameObject("Triangle " + (i / 3));
                //GO.layer = LayerMask.NameToLayer("Particle");
                GO.transform.position = transform.position;
                GO.transform.rotation = transform.rotation;
                GO.AddComponent<MeshRenderer>().material = materials[submesh];
                GO.AddComponent<MeshFilter>().mesh = mesh;
                GO.AddComponent<BoxCollider>();
                Vector3 explosionPos = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(0f, 0.5f), transform.position.z + Random.Range(-0.5f, 0.5f));
                GO.AddComponent<Rigidbody>().AddExplosionForce(Random.Range(300, 500), explosionPos, 5);
                Destroy(GO, 5 + Random.Range(0.0f, 5.0f));
            }
        }

        GetComponent<Renderer>().enabled = false;

        yield return new WaitForSeconds(1.0f);
        if (destroy == true)
        {
            Destroy(gameObject);
        }

    }

    private new void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && collider.gameObject.GetComponent<PlayerPowerController>().DashPower.IsDashing)
        {
            StartCoroutine(SplitMesh(true));
        }
    }

    private new void OnTriggerStay(Collider collider)
    {
    }
}

