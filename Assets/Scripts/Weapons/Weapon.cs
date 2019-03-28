using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SphereCollider))]
public class Weapon : MonoBehaviour, IInteractable
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
    private SphereCollider sphereCollider;
    private LineRenderer line;

    void GetReferences()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        line = GetComponent<LineRenderer>();
        sphereCollider = GetComponent<SphereCollider>();
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

        sphereCollider.isTrigger = true;
        sphereCollider.center = boxCollider.center;
        sphereCollider.radius = boxCollider.size.magnitude * .5f;

        boxCollider.center = bounds.center - transform.position;
        boxCollider.size = bounds.size;
    }

    void Awake()
    {
        GetReferences();
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootRate)
        {
            canShoot = true;
        }
    }

    public void PickUp()
    {
        rigid.isKinematic = true;
        sphereCollider.enabled = false;
    }

    public void Drop()
    {
        rigid.isKinematic = false;
        sphereCollider.enabled = true;
    }

    public string GetTitle()
    {
        return "Weapon";
    }

    IEnumerator ShowLine(Ray bulletRay, float lineDelay)
    {
        line.enabled = true;
        line.SetPosition(0, bulletRay.origin);
        line.SetPosition(1, bulletRay.origin + bulletRay.direction * range);
        yield return new WaitForSeconds(lineDelay);
        line.enabled = false;
    }

    public virtual void Reload()
    {
        //Crap
        clip += ammo;
        ammo -= maxClip;
    }
    public virtual void Shoot()
    {
        if (canShoot)
        {
            Ray bulletRay = new Ray(shotOrigin.position, shotOrigin.forward);
            RaycastHit hit;
            if (Physics.Raycast(bulletRay, out hit, range))
            {
                IKillable killable = hit.collider.GetComponent<IKillable>();
                if (killable != null)
                {
                    killable.TakeDamage(damage);
                }
            }

            StartCoroutine(ShowLine(bulletRay, lineDelay));
            shootTimer = 0;
            canShoot = false;
        }
    }
}

