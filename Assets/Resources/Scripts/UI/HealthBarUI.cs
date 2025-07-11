using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Transform target; 
    private Camera cam;
    public Image healthBarImage;
    private void Awake()
    {
        GetComponent<RectTransform>().pivot = new Vector2(0.5f, -2.8f);
    }
    void Start()
    {
        cam = Camera.main;
    }
    public void SetupCamera(Transform target, EntityType tag)
    {
        this.target = target;
        healthBarImage.fillAmount = 1f;
        if (tag == EntityType.Player)
        {
            healthBarImage.color = Color.red;
        }
        else if(tag == EntityType.Enemy)
        {
            healthBarImage.color = Color.green;
        }
    }
    void LateUpdate()
    {
        if (target == null || cam == null) return;

        Vector3 worldPos = target.position;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);
        GetComponent<RectTransform>().position = screenPos;
    }

    public void UpdateHealthBar(float healthPercentage)
    {

            healthBarImage.fillAmount -= healthPercentage;
            if(healthBarImage.fillAmount <= 0f)
            {
                gameObject.SetActive(false);
            }
           
    }
}
