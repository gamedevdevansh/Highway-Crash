using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpwaner : MonoBehaviour
{
    [SerializeField] GameObject[] carsPrefab;
    [SerializeField] CameraMovement cameraMovement;
    [SerializeField] UIManager uiManager;
    [SerializeField] EndlessCity[] cityArray;
    [SerializeField] LaneMovement laneMovement;
    [SerializeField] TrafficManager trafficManager;
    [SerializeField] PowerUpSpawner powerUpSpawner;
    [SerializeField] CoinSpawner coinSpawner;


    // Start is called before the first frame update
    void Start()
    {
        SpwanCar();
    }

    void SpwanCar()
    {
        int currentCarIndex = PlayerPrefs.GetInt("CarIndexValue", 0);
        GameObject newCar = Instantiate(carsPrefab[currentCarIndex], transform.position, transform.rotation);
        CarController carController = newCar.GetComponent<CarController>();

        Rigidbody rb = newCar.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.WakeUp();

        // 🔥 IMPORTANT: assign AFTER reset
        cameraMovement.SetTransform(newCar.transform);
        cameraMovement.SetCarController(carController);

        carController.SetUiManager(uiManager);
        cameraMovement.SetTransform(carController.transform);
        //cameraMovement.SetCarController(carController);
        uiManager.SetCarController(carController);
        cityArray[0].SetTransform(carController.transform);
        cityArray[1].SetTransform(carController.transform);
        trafficManager.SetCarController(carController);
        laneMovement.SetTransform(carController.transform);
        powerUpSpawner.SetCarController(carController);
        coinSpawner.SetCarController(carController);


    }
}