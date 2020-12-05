using System.Collections;
using System.Collections.Generic;
using Unity.MPE;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerInputHandler : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lookSpeed = 3f;

    private CharacterController characterController;
    private Transform playerCamera;

    public float cameraMaxAngle = 60f;
    public float cameraMinAngle = -60f;
    private float cameraRotation = 0f;
    public bool invertCameraRotation = false;

    public Projectile projectile;
    public Transform firePosition;
    public float fireDelay = 0.3f;
    private bool canFire = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>().transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveX = Input.GetAxis("Horizontal") * transform.right;
        Vector3 moveZ = Input.GetAxis("Vertical") * transform.forward;
        characterController.Move(moveSpeed * Time.deltaTime * (moveX + moveZ).normalized);

        // Rotate character transform around y (turn left & right).
        float yRotation = Input.GetAxis("Mouse X") * lookSpeed;
        transform.Rotate(Vector3.up, yRotation);

        // Rotate camera round x (look up & down)
        float cameraRotationDirection = invertCameraRotation ? 1 : -1;
        float xRotation = Input.GetAxis("Mouse Y") * lookSpeed * cameraRotationDirection;
        cameraRotation = Mathf.Clamp(cameraRotation + xRotation, cameraMinAngle, cameraMaxAngle);
        playerCamera.localRotation = Quaternion.Euler(cameraRotation, 0, 0);

        if (canFire && Input.GetMouseButton(0))
        {
            Fire(playerCamera.forward);
        }
    }

    int GetProjectileCount()
    {
        var projectileCount = projectile.baseProjectileCount;

        return projectileCount;
    }

    void GetProjectileDirections(Vector3 originalDirection)
    {
        var numProjectiles = GetProjectileCount();
    }

    void Fire(Vector3 fireDirection)
    {
        StartCoroutine(Cooldown(fireDelay));
        var projectileInstance = Instantiate(projectile, firePosition.position, Quaternion.identity);
        projectileInstance.GetComponent<Projectile>().projectileDirection = fireDirection;
        //projectileInstance.GetComponent<Projectile>().Fire(fireDirection);

        // for (var i = 0; i < 10; i++)
        // {
        //     Vector3 spread = Random.insideUnitCircle * 0.1f;
        //     var skewedDirection = fireDirection + (spread.x * playerCamera.right) + (spread.y * playerCamera.up);
        //     var projectileInstance = Instantiate(projectile, firePosition.position, Quaternion.identity);
        //     projectileInstance.GetComponent<Projectile>().projectileDirection = skewedDirection;
        // }
    }

    private IEnumerator Cooldown(float duration)
    {
        canFire = false;
        yield return new WaitForSeconds(duration);
        canFire = true;
    }
}
