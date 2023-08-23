using System.Collections;
using System.Collections.Generic;
using UnityEditor.TextCore.Text;
using UnityEngine;
using static UnityEngine.UI.Image;

public class SuperArmor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject superArmorCollider;
    private PlayerMovement pm;

    [Header("Super Armor Settings")]
    [SerializeField] float superArmorDuration;
    [SerializeField] float superArmorCooldown;
    [SerializeField] LayerMask superArmorLayers;

    private float superArmorCooldownTimer;
    private RaycastHit hitInfo;

    [Header("Camera Settings")]
    [SerializeField] CameraController cc;
    [SerializeField] float superArmorFov;

    [Header("Input")]
    [SerializeField] KeyCode superArmorKey = KeyCode.Mouse1;

    private void Awake()
    {
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(superArmorKey))
        {
            SuperArmorActivate();
        }

        if (superArmorCooldownTimer > 0)
        {
            superArmorCooldownTimer -= Time.deltaTime;
        }
    }

    private void SuperArmorActivate()
    {
        if (superArmorCooldownTimer > 0) return;
        else superArmorCooldownTimer = superArmorCooldown;

        pm.deflecting = true;
        superArmorCollider.SetActive(true);
        cc.DoFov(superArmorFov);

        Invoke(nameof(ResetSuperArmor), superArmorDuration);
    }

    private void ResetSuperArmor()
    {
        pm.deflecting = false;
        superArmorCollider.SetActive(false);
        cc.DoFov(85);
    }
}
