using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterController)/*, typeof(Animator)*/)]
public class Player : MonoBehaviour, IKillable
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
    [Header("Physics")]
    public float gravity = 10f;
    public float groundRayDistance = 1.1f;
    public float interactRange = 10f;
    [Header("UI")]
    public GameObject interactUIPrefab;
    public Transform interactUIParent;

    [Header("References")]
    public Camera attachedCamera;
    public Transform hand;
    //private Animator anim;
    private CharacterController controller;

    public Weapon currentWeapon;
    private List<Weapon> weapons;
    private int currentWeaponIndex = 0;

    private GameObject interactUI;
    private TextMeshProUGUI interactText;

    #endregion

    void OnDrawGizmosSelected()
    {
        //Interact ray
        Ray interactRay = attachedCamera.ViewportPointToRay(new Vector2(.5f, .5f));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(interactRay.origin, interactRay.origin + interactRay.direction * interactRange);

        //isGrounded
        Ray groundRay = new Ray(transform.position, -transform.up);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(groundRay.origin, groundRay.origin + groundRay.direction * groundRayDistance);
    }

    #region Initialisation
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        CreateUI();
        RegisterWeapons();
    }
    private void Start()
    {
        curHealth = maxHealth;
        SelectWeapon(0);
    }
    void CreateUI()
    {
        interactUI = Instantiate(interactUIPrefab, interactUIParent);
        interactText = interactUI.GetComponentInChildren<TextMeshProUGUI>();
    }
    void RegisterWeapons()
    {
        weapons = new List<Weapon>(GetComponentsInChildren<Weapon>());
        foreach (var weapon in weapons)
        {
            Pickup(weapon);
        }
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
        currentWeaponIndex += direction;
        if (currentWeaponIndex < 0)
        {
            currentWeaponIndex = weapons.Count - 1;
        }
        if (currentWeaponIndex >= weapons.Count)
        {
            currentWeaponIndex = 0;
        }
        SelectWeapon(currentWeaponIndex);
    }
    void DisableAllWeapons()
    {

    }
    void Pickup(Weapon weaponToPickup)
    {
        weaponToPickup.PickUp();
        Transform weaponTransform = weaponToPickup.transform;
        weaponTransform.SetParent(hand);
        weaponTransform.localRotation = Quaternion.identity;
        weaponTransform.localPosition = Vector3.zero;
        weapons.Add(weaponToPickup);
        SelectWeapon(weapons.Count - 1);
    }
    void Drop(Weapon weaponToDrop)
    {
        weaponToDrop.Drop();
        Transform weaponTrasform = weaponToDrop.transform;
        weaponTrasform.SetParent(null);
        weapons.Remove(weaponToDrop);
    }
    void SelectWeapon(int index)
    {
        if (index >= 0 && index < weapons.Count)
        {
            DisableAllWeapons();
            currentWeapon = weapons[index];
            currentWeapon.gameObject.SetActive(true);
            currentWeaponIndex = index;
        }
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
        interactUI.SetActive(false);
        Ray interactRay = attachedCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        RaycastHit hit;
        if (Physics.Raycast(interactRay, out hit, interactRange))
        {
            IInteractable interact = hit.collider.GetComponent<IInteractable>();
            if (interact != null)
            {
                interactUI.SetActive(true);
                interactText.text = interact.GetTitle();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Weapon weapon = hit.collider.GetComponent<Weapon>();
                    if (weapon)
                    {
                        Pickup(weapon);
                    }
                }
            }
        }
    }
    void Shooting()
    {
        if (currentWeapon)
        {
            if (Input.GetButton("Fire1"))
            {
                currentWeapon.Shoot();
            }
        }
    }
    void Switching()
    {
        if (weapons.Count > 1)
        {
            float inputScroll = Input.GetAxis("Mouse ScrollWheel");
            if (inputScroll != 0)
            {
                int direction = inputScroll > 0 ? Mathf.CeilToInt(inputScroll) : Mathf.FloorToInt(inputScroll);
                SelectWeapon(direction);
            }
        }
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

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int damage)
    {
        throw new System.NotImplementedException();
    }
    private void OnGUI()
    {
        GUI.Box(new Rect(Screen.width * .5f, Screen.height * .5f, 16 / Screen.width, 9 / Screen.height), "");
    }
}
