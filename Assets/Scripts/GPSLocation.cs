using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPSLocation : MonoBehaviour
{
    public Text latitude;
    public Text longitude;
    public Text altitude;
    public Text savedLatitude;
    public Text savedLongitude;
    public Text savedAltitude;
    public Text savedUnityLocalPosition;
    [SerializeField] private GameObject marker;
    private GameObject spawnedMarker;
    private string markerName;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GPSLoc());
    }

    IEnumerator GPSLoc()
    {
        // check if user has location services enabled on device
        if(!Input.location.isEnabledByUser)
            yield break;

        // starts service
        Input.location.Start();

        // waits for service to initialize
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // service did not initialize in wait time
        if (maxWait < 1)
            yield break;

        // yield break if connection failed
        if (Input.location.status == LocationServiceStatus.Failed)
            yield break;
        else
        {
            // access granted and GPS updates every second here
            InvokeRepeating("UpdateGPSData", 0.5f, 1f);
            
        }
    }// end of GPSLoc ---------------------------

    private void UpdateGPSData()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            // values set here, based on device's location
            latitude.text = Input.location.lastData.latitude.ToString();
            longitude.text = Input.location.lastData.longitude.ToString();
            altitude.text = Input.location.lastData.altitude.ToString();
        }
        else
        {
            // service stopped
        }
    }// end of GPSData --------------------------

    public void RefreshData()
    {
        UpdateGPSData();
    }

    public void SaveCoords()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            // values set here, based on device's location
            savedLatitude.text = Input.location.lastData.latitude.ToString();
            savedLongitude.text = Input.location.lastData.longitude.ToString();
            savedAltitude.text = Input.location.lastData.altitude.ToString();
            
        }
    }
    public void createUnityLocalPosition()
    {
        savedUnityLocalPosition.text = GPSEncoder.GPSToUCS(Input.location.lastData.latitude, Input.location.lastData.longitude).ToString();
    }
    public void markLocation()
    {
        markerName = FindObjectOfType<InputField>().text;
        Vector3 markerPOS = Camera.main.transform.position;
        spawnedMarker = Instantiate(marker, markerPOS, Quaternion.identity);
        spawnedMarker.GetComponentInChildren<TextMesh>().text = markerName;
    }
}
