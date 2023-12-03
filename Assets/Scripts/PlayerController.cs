using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //������
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    float applySpeed;
    [SerializeField] float jumpForce;
    //����
    bool isGround = true;
    //ī�޶� ȸ��
    [SerializeField] float lookSensitivity;
    [SerializeField] float camRotLimit;
    float currentCamRotX;
    [SerializeField] Camera cam;
    //������Ʈ
    Rigidbody rb;
    CapsuleCollider col;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        applySpeed = walkSpeed;
    }

    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        Move();
        PlayerRotation();
        CameraRotation();
    }

    void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + 0.1f);
    }

    void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.velocity = transform.up * jumpForce;
    }

    void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Walk();
        }
    }

    void Running()
    {
        applySpeed = runSpeed;
    }

    void Walk()
    {
        applySpeed = walkSpeed;
    }

    void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveVecH = transform.right * moveDirX;
        Vector3 moveVecV = transform.forward * moveDirZ;

        Vector3 velocity = (moveVecH + moveVecV).normalized * applySpeed;

        rb.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    void PlayerRotation()
    {
        //�¿� ĳ���� ȸ��
        float yRot = Input.GetAxisRaw("Mouse X");
        Vector3 playerRotY = new Vector3(0f, yRot, 0) * lookSensitivity;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(playerRotY));
    }

    void CameraRotation()
    {
        //���� ī�޶� ȸ��
        float xRot = Input.GetAxisRaw("Mouse Y");
        float camRotX = xRot * lookSensitivity;
        currentCamRotX -= camRotX;
        currentCamRotX = Mathf.Clamp(currentCamRotX, -camRotLimit, camRotLimit);

        cam.transform.localEulerAngles = new Vector3(currentCamRotX, 0, 0);
    }
}
