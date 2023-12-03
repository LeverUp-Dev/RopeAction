using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeAction : MonoBehaviour
{
    public Transform player;
    public Camera cam;
    RaycastHit hit;
    public LayerMask grapplingObj;
    LineRenderer lr;
    bool isGrappling;
    public Transform gunTip;
    Vector3 spot;
    SpringJoint sj;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        TryGrappling();
    }

    void TryGrappling()
    {
        if (Input.GetMouseButtonDown(0))
            RopeShoot();
        else if (Input.GetMouseButtonUp(0))
            EndShoot();

        DrawRope();
    }

    void RopeShoot()
    {
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100f, grapplingObj))
        {
            isGrappling = true;

            spot = hit.point;
            lr.positionCount = 2;
            lr.SetPosition(0, gunTip.position);
            lr.SetPosition(1, hit.point);

            sj = player.gameObject.AddComponent<SpringJoint>();
            sj.autoConfigureConnectedAnchor = false;
            sj.connectedAnchor = spot;

            float dis = Vector3.Distance(transform.position, spot);

            sj.maxDistance = dis;
            sj.minDistance = dis * 0.5f;
            sj.spring = 5f;
            sj.damper = 5f;
            sj.massScale = 5f;
        }
    }

    void EndShoot()
    {
        isGrappling = false;
        lr.positionCount = 0;
        Destroy(sj);
    }

    void DrawRope()
    {
        if (isGrappling)
        {
            lr.SetPosition(0, gunTip.position);
            transform.LookAt(spot);
        }
    }


}
