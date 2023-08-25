using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCooldownUI : MonoBehaviour
{
    [SerializeField] Dashing dash;
    [SerializeField] RectTransform progressBar;

    // Update is called once per frame
    void Update()
    {
        float updatedProgressValue = 1 - dash.GetDashCooldown() / dash.dashCooldown;
        if (updatedProgressValue > 0.99) progressBar.gameObject.SetActive(false);
        else progressBar.gameObject.SetActive(true);

        progressBar.localScale = new Vector3(updatedProgressValue, 1, 1);
    }
}
