using System.Collections;
using System.Collections.Generic;
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
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveX = Input.GetAxis("Horizontal") * transform.right;
        Vector3 moveZ = Input.GetAxis("Vertical") * transform.forward;
        characterController.Move((moveX + moveZ).normalized * moveSpeed * Time.deltaTime);

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

    void Fire(Vector3 fireDirection)
    {
        StartCoroutine(Cooldown(fireDelay));
        var projectileObject = Instantiate(projectile, firePosition.position, Quaternion.identity);
        projectileObject.GetComponent<Projectile>().projectileDirection = fireDirection;
    }

    private IEnumerator Cooldown(float duration)
    {
        canFire = false;
        yield return new WaitForSeconds(duration);
        canFire = true;
    }
}
