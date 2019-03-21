using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController)/*, typeof(Animator)*/)]
public class Player : MonoBehaviour
{
    #region Variables
    [Header("Health")]
    public int maxHealth = 100;
    public int curHealth;
    [Header("Movement")]
    public float runSpeed = 7.5f;
    public float walkSpeed = 6f;
    public float crouchSpeed = 4f;
    public float jumpHeight = 20f;
    private Vector3 movement;
    [Header("Other stuff")]
    public float gravity = 10f;
    public float groundRayDistance = 1.1f;
    public float interactRange = 10f;

    [Header("References")]
    public Camera attachedCamera;
    public Transform hand;
    //private Animator anim;
    private CharacterController controller;

    public Weapon currentWeapon;
    private List<Weapon> weapons;
    private int currentWeaponIndex = 0;
    #endregion

    void OnDrawGizmos()
    {
        Ray groundRay = new Ray(transform.position, -transform.up);
        Gizmos.DrawLine(groundRay.origin, groundRay.origin + groundRay.direction * groundRayDistance);
    }

    #region Initialisation
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Start()
    {
        curHealth = maxHealth;
        SelectWeapon(0);
    }
    void CreateUI()
    {

    }
    void RegisterWeapons()
    {

    }
    #endregion

    #region Controls
    private void Move(float inputH, float inputV)
    {
        Vector3 input = new Vector3(inputH, 0, inputV);
        input = transform.TransformDirection(input);
        float moveSpeed = walkSpeed;
        movement.x = input.x * moveSpeed;
        movement.z = input.z * moveSpeed;
    }
    #endregion

    #region Combat
    /// <summary>
    /// Switches between weapons in the list with a given direction
    /// </summary>
    /// <param name="direction">-1 to 1 number for list selection</param>
    void SwitchWeapon(int direction)
    {

    }
    void DisableAllWeapons()
    {

    }
    void Pickup(Weapon weaponToPickup)
    {

    }
    void Drop(Weapon weaponToDrop)
    {

    }
    void SelectWeapon(int index)
    {

    }
    #endregion

    #region Actions
    void Movement()
    {
        float inputV = Input.GetAxis("Vertical");
        float inputH = Input.GetAxis("Horizontal");
        Move(inputH, inputV);
        Ray groundRay = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(groundRay, out hit, groundRayDistance))
        {
            if (Input.GetButton("Jump"))
            {
                movement.y = jumpHeight;
            }
            else
            {
                movement.y = 0;
            }
        }
        else
        {
            if (movement.y < -gravity)
            {
                movement.y = -gravity;
            }
        }
        movement.y -= gravity * Time.deltaTime;
        controller.Move(movement * Time.deltaTime);

    }
    void Interact()
    {

    }
    void Shooting()
    {

    }
    void Switching()
    {

    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        Movement();
        Interact();
        Shooting();
        Switching();
    }
}
