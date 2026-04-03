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
        rb.WakeUp();

        carController.SetUiManager(uiManager);
        cameraMovement.SetTransform(carController.transform);
        uiManager.SetCarController(carController);
        cityArray[0].SetTransform(carController.transform);
        cityArray[1].SetTransform(carController.transform);
        trafficManager.SetCarController(carController);
        laneMovement.SetTransform(carController.transform);
        powerUpSpawner.SetCarController(carController);
        coinSpawner.SetCarController(carController);


    }
}