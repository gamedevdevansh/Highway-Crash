using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] TextMeshProUGUI scoreText;
    
    [SerializeField] TextMeshProUGUI totalScoreText;
    [SerializeField] TextMeshProUGUI totalDistanceText;
    [SerializeField] TextMeshProUGUI maxSpeedText;

    [SerializeField] CarController carController;
    //[SerializeField] Transform carTransform;
    [SerializeField] GameObject gameOverPanel;
    
    [SerializeField] GameObject speedIcon;
    [SerializeField] GameObject distanceIcon;
    [SerializeField] GameObject scoreIcon;

    [SerializeField] GameObject gas;
    [SerializeField] GameObject brake;
    [SerializeField] GameObject steering;

    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI powerUpText;
    private int coinCount = 0;
    private float speed = 1f;
    private float distance = 1f;
    private float score = 1f;
    private float maxSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        gameOverPanel.SetActive(false);
        speedIcon.SetActive(true);
        distanceIcon.SetActive(true);
        scoreIcon.SetActive(true);
        gas.SetActive(true);
        brake.SetActive(true);
        steering.SetActive(true);
    }

    public void SetCarController(CarController controller)
    {
        carController = controller;
    }

    // Update is called once per frame
    void Update()
    {
        SpeedUI();
        DistanceUI();
        ScoreUI();
        MaximumSpeed(); 
    }
    public void AddCoin(int amount)
    {
        coinCount += amount;
        coinText.text = "Coins: " + coinCount;
    }
    void SpeedUI()
    {
        speed = carController.CarSpeed();
        speedText.text = speed.ToString("0") + "km/h";
    }
    void DistanceUI()
    {
        distance = carController.transform.position.z/1000;
        distanceText.text = distance.ToString("0.00") + "km";
    }
    void ScoreUI()
    {
        score = carController.transform.position.z * 6;
        scoreText.text = score.ToString("0");
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        speedIcon.SetActive(false);
        distanceIcon.SetActive(false);
        scoreIcon.SetActive(false);
        gas.SetActive(false);
        brake.SetActive(false);
        steering.SetActive(false);
        totalScoreText.text = score.ToString("0");
        totalDistanceText.text = distance.ToString("0.00" + "Km");

    }
    void MaximumSpeed()
    {
        float currentSpeed = carController.CarSpeed();
        if (currentSpeed > maxSpeed)
        {
            maxSpeed = currentSpeed;
        }
        maxSpeedText.text = maxSpeed.ToString("0"+ "km/h");
    }
    public void TryAgain()
    {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void Garage()
    {
        SceneManager.LoadScene("Garage");
    }

    // Inside UIManager.cs

    public void OnGasPressed()
    {
        if (carController != null) carController.GasPressed();
    }

    public void OnGasReleased()
    {
        if (carController != null) carController.GasReleased();
    }

    public void OnBrakePressed()
    {
        if (carController != null) carController.BrakePressed();
    }

    public void OnBrakeReleased()
    {
        if (carController != null) carController.BrakeReleased();
    }
}