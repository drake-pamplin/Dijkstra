using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    [Header ("Tile Connections")]
    public GameObject top;
    public GameObject right;
    public GameObject bottom;
    public GameObject left;
    
    [Header ("Tile Navigation")]
    public int index;
    public int cost = 0;

    [Header ("Tile Pieces")]
    public TextMeshPro stepText;

    [Header ("Tile Materials")]
    public Material baseMat;
    public Material pathMat;
    public Material pointMat;
    public Material visitedMat;
    public Material obstacleMat;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).GetComponent<TextMeshPro>().SetText("{0}", cost);
        if (cost == -1) {
            GetComponent<MeshRenderer>().material = obstacleMat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Click() {
        GetComponent<MeshRenderer>().material = pointMat;
    }

    void Visit() {
        GetComponent<MeshRenderer>().material = visitedMat;
    }

    void FindPath() {
        GetComponent<MeshRenderer>().material = pathMat;
    }
}
