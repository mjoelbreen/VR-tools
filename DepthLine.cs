using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


//A tool for measuring distances 

public class DepthLine : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    [SerializeField] GameObject lineStartHandle;
    [SerializeField] GameObject lineEndHandle;
    [SerializeField] TextMeshPro heightLabel;

    private float METERS_TO_FEET = 3.2808f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
		Vector3 pointerStart = transform.position;
        Vector3 pointerDir = Vector3.down;
		line.SetPosition(0, pointerStart);
        lineStartHandle.transform.position = pointerStart;
        lineEndHandle.transform.position = pointerStart;


        RaycastHit hitInfo;

		if (Physics.Raycast(pointerStart, pointerDir, out hitInfo, Mathf.Infinity))
		{
            var pointerEnd = hitInfo.point;
            var depth = transform.position.y - pointerEnd.y;
            var depthInFeet = depth * METERS_TO_FEET;

            line.SetPosition(1, pointerEnd);
                        lineEndHandle.transform.position = hitInfo.point;
            heightLabel.text = $"{Mathf.Round(depthInFeet)}'";


        } else
        {
			line.SetPosition(1, pointerStart + pointerDir * 1000);
			
		}

		
        
    }
}
