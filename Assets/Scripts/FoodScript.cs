using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    [SerializeField] private Transform parent;

    [SerializeField] private FoodScriptableObject currentFood;

    private bool isDragging;
    private bool hasTriggered;
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private IEnumerator WaitTriggerFood()
    {
        hasTriggered = true;
        yield return new WaitForSeconds(0.5f);
        hasTriggered = false;
    }

    public void DragFood()
    {
        if (!isDragging)
        {
            return;
        }
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -cam.transform.position.z;

        this.transform.position = cam.ScreenToWorldPoint(mousePos);
    }

    public void SelectFood()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -cam.transform.position.z;

        this.transform.position = cam.ScreenToWorldPoint(mousePos);
        isDragging = true;
    }
    public void DeselectFood()
    {
        isDragging = false;
        this.transform.position = parent.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(hasTriggered)
            return;
        
        if (other.CompareTag("FoodLocation"))
        {
            StartCoroutine(WaitTriggerFood());
            DeselectFood();
            other.GetComponent<FoodLocationScript>().ActivateFood(currentFood) ;
        }
    }
}
