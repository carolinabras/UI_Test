using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class pressUIController : MonoBehaviour
{
    public float jumpHeight = 0.3f;  
    public float jumpSpeed = 0.1f;   
    public float fallSpeed = 0.2f;   

    private Vector3 originalLocalPosition; 
    private Coroutine movementCoroutine;

    void Start()
    {
        originalLocalPosition = transform.localPosition; 
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            StartJump();
        }
        if (Input.GetKeyUp(KeyCode.P)) 
        {
            StartFall();
        }
    }

    private void StartJump()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        movementCoroutine = StartCoroutine(AnimateMove(originalLocalPosition.y + jumpHeight, jumpSpeed));
        
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(0xAD, 0xBC, 0xB1, 0xFF);
    }

    private void StartFall()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        movementCoroutine = StartCoroutine(AnimateMove(originalLocalPosition.y, fallSpeed));
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);

    }

    // ---------------animation coroutine -------------- //
    private IEnumerator AnimateMove(float targetY, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startPos = transform.localPosition;

        while (elapsedTime < duration)
        {
            transform.localPosition = new Vector3(
                startPos.x,
                Mathf.Lerp(startPos.y, targetY, elapsedTime / duration),
                startPos.z
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = new Vector3(startPos.x, targetY, startPos.z);
    }

    public void deactivateUI()
    {
        if (!this.gameObject)
        {
            return;
        }
        this.gameObject.SetActive(false);
    }

    public void activateUI()
    {
        if (!this.gameObject)
        {
            return;
        }
        this.gameObject.SetActive(true);
    }
}