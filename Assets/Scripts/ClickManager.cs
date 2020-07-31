using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    private GameManager gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f)) {
                if (hit.collider.gameObject.CompareTag("Tile")) {
                    if (gameManager.startPoint == null && hit.collider.gameObject.GetComponent<Tile>().cost >= 0) {
                        hit.collider.gameObject.SendMessage("Click");
                        gameManager.startPoint = hit.collider.gameObject;
                    }
                    else if (gameManager.endPoint == null && hit.collider.gameObject.GetComponent<Tile>().cost >= 0) {
                        hit.collider.gameObject.SendMessage("Click");
                        gameManager.endPoint = hit.collider.gameObject;
                    }
                }
            }
        }
    }
}
