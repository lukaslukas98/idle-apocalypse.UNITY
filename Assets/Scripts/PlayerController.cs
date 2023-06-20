using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float horizontalInput, verticalInput;
    [SerializeField]
    float speed;

    [SerializeField]
    Sprite top, right, down, left;

    SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput > 0)
        {
            spriteRenderer.sprite = right;
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.sprite = left;
        }
        else if (verticalInput > 0)
        {
            spriteRenderer.sprite = top;
        }
        else if (verticalInput < 0)
        {
            spriteRenderer.sprite = down;
        }

        transform.Translate(horizontalInput * speed * Time.deltaTime, verticalInput * speed * Time.deltaTime, 0);
    }
}
