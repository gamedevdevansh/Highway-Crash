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
//    [SerializeField] PowerUpSpawner powerUpSpawner;
//    [SerializeField] CoinSpawner coinSpawner;


//    // Start is called before the first frame update
//    void Start()
//    {
//        SpwanCar();
//    }

//    void SpwanCar()
//    {
//        int currentCarIndex = PlayerPrefs.GetInt("CarIndexValue", 0);
//        GameObject newCar = Instantiate(carsPrefab[currentCarIndex], transform.position, transform.rotation);

//        var carController = newCar.GetComponent<CarController>();
//        var rb = newCar.GetComponent<Rigidbody>();
//        rb.velocity = Vector3.zero;
//        rb.angularVelocity = Vector3.zero;
//        rb.WakeUp();

//        // 🔥 IMPORTANT: assign AFTER reset
//        cameraMovement.SetTransform(newCar.transform);
//        cameraMovement.SetCarController(carController);

//        carController.SetUiManager(uiManager);
//        uiManager.SetCarController(carController);
//        cityArray[0].SetTransform(carController.transform);
//        cityArray[1].SetTransform(carController.transform);
//        powerUpSpawner.SetCarController(carController);
//        GameManager.Instance.RegisterCar(carController);
//    }
//}

using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject[] carsPrefab;

    [Header("Dependencies")]
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private EndlessCity[] cityArray;
    [SerializeField] private LaneMovement laneMovement;
    [SerializeField] private TrafficManager trafficManager;
    [SerializeField] private PowerUpSpawner powerUpSpawner;
    [SerializeField] private CoinSpawner coinSpawner;

    private void Start()
    {
        SpawnCar();
    }

    private void SpawnCar()
    {
        // ---------- VALIDATION LAYER ----------

        if (carsPrefab == null || carsPrefab.Length == 0)
        {
            Debug.LogError("CarSpawner: carsPrefab is empty");
            return;
        }

        int index = PlayerPrefs.GetInt("CarIndexValue", 0);

        if (index < 0 || index >= carsPrefab.Length)
        {
            Debug.LogError($"CarSpawner: Invalid index {index}");
            return;
        }

        GameObject prefab = carsPrefab[index];

        if (prefab == null)
        {
            Debug.LogError("CarSpawner: Selected prefab is NULL");
            return;
        }

        // ---------- INSTANTIATION ----------

        GameObject newCar = Instantiate(prefab, transform.position, transform.rotation);

        if (newCar == null)
        {
            Debug.LogError("CarSpawner: Instantiation failed");
            return;
        }

        // ---------- COMPONENT FETCH ----------

        CarController carController = newCar.GetComponent<CarController>();
        Rigidbody rb = newCar.GetComponent<Rigidbody>();

        if (carController == null)
        {
            Debug.LogError("CarSpawner: CarController missing on prefab");
            return;
        }

        if (rb == null)
        {
            Debug.LogError("CarSpawner: Rigidbody missing on prefab");
            return;
        }

        // ---------- PHYSICS RESET ----------

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.WakeUp();

        // ---------- DEPENDENCY INJECTION ----------

        if (cameraMovement != null)
        {
            cameraMovement.SetTransform(newCar.transform);
            cameraMovement.SetCarController(carController);
        }

        if (uiManager != null)
        {
            carController.SetUiManager(uiManager);
            uiManager.SetCarController(carController);
        }

        if (cityArray != null && cityArray.Length >= 2)
        {
            cityArray[0].SetTransform(carController.transform);
            cityArray[1].SetTransform(carController.transform);
        }

        if (powerUpSpawner != null)
        {
            powerUpSpawner.SetCarController(carController);
        }

        if (laneMovement != null)
        {
            laneMovement.SetTransform(carController.transform);
        }

        if (trafficManager != null)
        {
            trafficManager.SetCarController(carController);
        }

        if (coinSpawner != null)
        {
            coinSpawner.SetCarController(carController);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterCar(carController);
        }
        else
        {
            Debug.LogError("CarSpawner: GameManager.Instance is NULL");
        }
    }
}