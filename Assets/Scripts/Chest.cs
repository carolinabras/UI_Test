using System;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{

    public bool isOpen;
    public Animator animator;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }

    public bool CanInteract()
    {
        return !isOpen;
    }

    public void Interact()
    {
        if (!CanInteract())
        {
            return;
        }
        OpenChest();
    }

    public void OpenChest()
    {
       isOpen = true;
       if (animator)
       {
           animator.SetTrigger("isOpen");
       }
        
    }
    
}
