using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

//Measure distance between spherical "constellations" 

[System.Serializable]
public enum HandleType { EDGE, BASE, HEIGHT, SHAPE};
public class ConstellationController : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private GameObject EdgeHandle;
    [SerializeField] private GameObject BaseHandle;
    [SerializeField] private GameObject heightHandle;
    [SerializeField] private GameObject heightContainer;
    [SerializeField] private GameObject angleContainer;

    [SerializeField] private GameObject lengthLabelPrefab;

    [SerializeField] private float height;
    [SerializeField] private float angle;

    private List<ConstellationHandle> nodeList = new List<ConstellationHandle>();
    private List<GameObject> spawnedLabels = new List<GameObject>();


    public bool b_baseHandleActive = false;
    public bool b_edgeHandleActive = false;
    public bool b_heightHandleActive = false;
    public bool b_DrawShape = false;

    private float METERS_TO_FEET = 3.2808f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnDisable()
    {
        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateLine();

        heightContainer.SetActive(ShouldShowHeight());
        angleContainer.SetActive(ShouldShowAngle());
    }

    public void ToggleHandle(HandleType thisHandle)
    {
        switch (thisHandle)
        {
            case HandleType.BASE:
                b_baseHandleActive = !b_baseHandleActive;
                break;
            case HandleType.EDGE:
                b_edgeHandleActive = !b_edgeHandleActive;
                break;
            case HandleType.HEIGHT:
                b_heightHandleActive = !b_heightHandleActive;
                break;
        }

        UpdateLine();
    }

    public void ToggleHandle(ConstellationHandle handle)
    {
        if (nodeList.Contains(handle))
        {
            nodeList.Remove(handle);
        }
        else
        {
            nodeList.Add(handle);
        }

        UpdateLine();
    }

    public void Deactivate()
    {
        b_baseHandleActive = false;
        b_edgeHandleActive = false;
        b_heightHandleActive = false;

        line.positionCount = 0;
        line.SetPositions(new Vector3[0]);
        nodeList.Clear();
        foreach (var label in spawnedLabels)
        {
            Destroy(label);
        }

        spawnedLabels.Clear();
    }

    public void ShowAll()
    {
        b_baseHandleActive = true;
        b_edgeHandleActive = true;
        b_heightHandleActive = true;
    }

    private bool ShouldShowHeight()
    {
        return b_baseHandleActive && b_heightHandleActive;
    }

    private bool ShouldShowAngle()
    {
        return b_edgeHandleActive && ShouldShowHeight();
    }

    private void UpdateLine()
    {
        var points = b_DrawShape ? DrawConstellation() : GetLinePoints();
        line.positionCount = points.Length;
        line.SetPositions(points);
    }

    private Vector3[] GetLinePoints()
    {
        if (ShouldShowAngle())
        {
            return new Vector3[3] { EdgeHandle.transform.position, heightHandle.transform.position, BaseHandle.transform.position };
        } else
        {
            if (ShouldShowHeight())
            {
                return new Vector3[3] { heightHandle.transform.position, heightHandle.transform.position, BaseHandle.transform.position };
            }
            else
            {
                return new Vector3[3] { Vector3.zero, Vector3.zero, Vector3.zero };
            }
        }
    }

    private Vector3[] DrawConstellation()
    {
        int length = nodeList.Count;
        Vector3[] shapeVertices = new Vector3[length];

        foreach (var label in spawnedLabels)
        {
            Destroy(label);
        }

        spawnedLabels.Clear();

        for (var i = 0; i < nodeList.Count; i++)
        {
            shapeVertices[i] = nodeList[i].transform.position;
            if (i > 0)
            {
                var distance = Vector3.Distance(shapeVertices[i - 1], shapeVertices[i]);
                var distanceInFeet = distance * METERS_TO_FEET;
                var feet = Mathf.FloorToInt(distanceInFeet);
                var inches = Mathf.FloorToInt((distanceInFeet - feet) * 12);
                var label = Instantiate(lengthLabelPrefab, Vector3.Lerp(shapeVertices[i - 1], shapeVertices[i], 0.5f), Quaternion.identity);

                label.GetComponentInChildren<TextMeshPro>().text = $"{feet}'{inches}\"";
                spawnedLabels.Add(label);
            }

            if (i == nodeList.Count - 1)
            {
                //shapeVertices[i + 1] = nodeList[0].transform.position;
            }
        }

        return shapeVertices;
    }
}
