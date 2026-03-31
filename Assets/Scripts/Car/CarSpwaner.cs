//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CarSpwaner : MonoBehaviour
//{
//    [SerializeField] GameObject[] carsPrefab;
//    [SerializeField] CameraMovement cameraMovement; 
//    [SerializeField] UIManager uiManager; 
//    [SerializeField] EndlessCity[] cityArray; 
//    [SerializeField] LaneMovement laneMovement;
//    [SerializeField] TrafficManager trafficManager;

//    // Start is called before the first frame update
//    void Start()
//    {
//        SpwanCar();
//    }

//    void SpwanCar()
//    {
//        int currentCarIndex = PlayerPrefs.GetInt("CarIndexValue",0);
//        GameObject newCar = Instantiate(carsPrefab[currentCarIndex], transform.position, transform.rotation);
//        CarController carController = newCar.GetComponent<CarController>();

//        Rigidbody rb = newCar.GetComponent<Rigidbody>();
//        rb.WakeUp();

//        carController.SetUiManager(uiManager);
//        cameraMovement.SetTransform(carController.transform);
//        uiManager.SetCarController(carController);
//        cityArray[0].SetTransform(carController.transform);
//        cityArray[1].SetTransform(carController.transform);
//        trafficManager.SetCarController(carController);
//        laneMovement.SetTransform(carController.transform);

//    }
//}


using UnityEngine;

/// <summary>
/// Spawns the player car and connects it to camera, UI, traffic, and lanes.
/// </summary>
public class CarSpawner : MonoBehaviour
{
    [Tooltip("Player car prefabs (one per selection)")]
    public GameObject[] carsPrefab;
    [Tooltip("Camera movement script to follow car")]
    public CameraMovement cameraMovement;
    [Tooltip("UI manager to update speedometer, etc.")]
    public UIManager uiManager;
    [Tooltip("Lane movement scripts")]
    public LaneMovement laneMovement;
    [Tooltip("Traffic manager to pass the player car reference")]
    public TrafficManager trafficManager;
    [Tooltip("City/Environment scripts to follow the car (optional)")]
    public EndlessCity[] cityArray;

    void Start()
    {
        SpawnCar();
    }

    void SpawnCar()
    {
        int index = PlayerPrefs.GetInt("CarIndexValue", 0);
        GameObject newCar = Instantiate(carsPrefab[index], transform.position, transform.rotation);
        CarController carCtrl = newCar.GetComponent<CarController>();
        Rigidbody rb = newCar.GetComponent<Rigidbody>();
        if (rb != null) rb.WakeUp();

        // Wire up components
        carCtrl.SetUiManager(uiManager);
        cameraMovement.SetTransform(carCtrl.transform);
        uiManager.SetCarController(carCtrl);
        laneMovement.SetTransform(carCtrl.transform);
        trafficManager.SetPlayerCar(carCtrl);

        // If there are city scripts (for endless scenery), assign them
        foreach (var city in cityArray)
            city.SetTransform(carCtrl.transform);
    }
}
