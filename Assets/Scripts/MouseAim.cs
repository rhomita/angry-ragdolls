using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAim : MonoBehaviour
{
    [SerializeField] private Canon canon;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Vector3 camOffset;
    [SerializeField] private Vector3 shotPosition;
    [SerializeField] private Quaternion shotRotation;

    private static float MAXIMUM_FORCE = 600;
    private static float MINIMUM_FORCE = 150;

    private Camera cam;
    private bool shot = false;
    private Ragdoll currentRagdoll = null;
    private Vector3 initPosition;
    private Quaternion initRotation;
    private float shotForce = 400f;

    private GameUI UI;

    
    void Start()
    {
        UI = GameManager.instance.UI;
        cam = GameManager.instance.Cam;
        initPosition = cam.transform.position;
        initRotation = cam.transform.rotation;
        shotRotation = Quaternion.identity;
    }

    void Update()
    {
        UI.HideCurrentShot();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            shot = false;
            currentRagdoll = null;
        }

        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        shotForce += scrollWheel * 20;
        shotForce = Mathf.Clamp(shotForce, MINIMUM_FORCE, MAXIMUM_FORCE);

        Vector3 position = initPosition;
        Quaternion rotation = initRotation;
        if (shot)
        {
            position = currentRagdoll.GetPosition() + camOffset;
            rotation = Quaternion.LookRotation(currentRagdoll.GetPosition() - cam.transform.position);
        }
        else if (Input.GetMouseButton(1))
        {
            position = shotPosition;
            rotation = shotRotation;
            Aim();
        }

        cam.transform.position = Vector3.Lerp(cam.transform.position, position, Time.deltaTime * 20);
        cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation,rotation, Time.deltaTime * 10);
    }

    private int GetPowerPercentage()
    {
        return (int) ((shotForce - MINIMUM_FORCE) * 100) / 700;
    }

    private void Aim()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, float.MaxValue, layerMask))
        {
            Vector3 target = hit.point + Vector3.up * shotPosition.y;
            UI.UpdateCurrentShot(canon.GetCurrentAngles(), GetPowerPercentage());
            canon.Aim(target);
        }

        if (Input.GetMouseButtonDown(0))
        {
            UI.UpdateLastShot(canon.GetCurrentAngles(), GetPowerPercentage());

            currentRagdoll = canon.Shot(shotForce);
            currentRagdoll.onDeactivate += (ragdoll) =>
            {
                if (ragdoll != currentRagdoll) return;
                shot = false;
                currentRagdoll = null;
            };
            shot = true;
        }
    }
}