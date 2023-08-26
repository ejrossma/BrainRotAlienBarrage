using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCooldownUI : MonoBehaviour
{
    [SerializeField] Dashing dash;
    [SerializeField] RectTransform progressBar;
    [SerializeField] RectTransform progressBorder;

    // Update is called once per frame
    void Update()
    {
        float updatedProgressValue = 1 - dash.GetDashCooldown() / dash.dashCooldown;
        if (updatedProgressValue > 0.99) HideBar();
        else ShowBar();

        progressBar.localScale = new Vector3(updatedProgressValue * 5, 1, 1);
    }

    private void HideBar()
    {
        progressBar.gameObject.SetActive(false);
        progressBorder.gameObject.SetActive(false);
    }

    private void ShowBar()
    {
        progressBar.gameObject.SetActive(true);
        progressBorder.gameObject.SetActive(true);
    }
}
