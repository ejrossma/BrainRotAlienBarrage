using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCooldownUI : MonoBehaviour
{
    [SerializeField] Dashing dash;
    [SerializeField] RectTransform progressBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float updatedProgressValue = 1 - dash.GetDashCooldown() / dash.dashCooldown;
        progressBar.localScale = new Vector3(updatedProgressValue, 1, 1);
    }
}
