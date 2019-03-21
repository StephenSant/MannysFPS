using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class Weapon : MonoBehaviour
{
    public int damage = 10;
    public int maxAmmo = 500;
    public int maxClip = 30;
    public float range = 10f;
    public float shootRate = .2f;
    public float lineDelay = .1f;
    public Transform shotOrigin;

    private int ammo;
    private int clip;
    private float shootTimer;
    private bool canShoot = false;

    private Rigidbody rigid;
    private BoxCollider boxCollider;
    private LineRenderer lineRenderer;

    void GetReferences()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Reset()
    {
        GetReferences();
        Transform[] children = GetComponentsInChildren<Transform>();
        Bounds bounds = new Bounds(transform.position, Vector3.zero);
        foreach (Transform child in children)
        {
            Renderer rend = child.GetComponent<MeshRenderer>();
            if (rend)
            {
                bounds.Encapsulate(rend.bounds);
            }
        }

        rigid.isKinematic = false;

        boxCollider.center = bounds.center - transform.position;
        boxCollider.size = bounds.size;
    }

    void Awake()
    {
        GetReferences();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
