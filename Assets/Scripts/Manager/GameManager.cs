using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CarController CarController { get; private set; }
    public Transform PlayerTransform { get; private set; }

    //private void Awake()
    //{
    //    Instance = this;
    //}
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterCar(CarController car)
    {
        CarController = car;
        PlayerTransform = car.transform;
    }
}