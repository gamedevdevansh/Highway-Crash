using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarSelection : MonoBehaviour
{
    [SerializeField] GameObject[] cars;
    int currentCarIndex = 0;
    int lastIndex = -1;
    // Start is called before the first frame update
    void Start()
    {
        ShowCar(currentCarIndex);
    }

    public void NextCar()
    {
        currentCarIndex++;
        if(currentCarIndex > cars.Length - 1)
        {
            currentCarIndex = 0;
        }
        ShowCar(currentCarIndex);
    }
    public void PreviousCar()
    {
        currentCarIndex--;
        if(currentCarIndex < 0)
        {
            currentCarIndex = cars.Length-1;
        }
        ShowCar(currentCarIndex);
    }

    //void ShowCar(int index)
    //{
    //    for (int i = 0; i < cars.Length; i++)
    //    {
    //        cars[i].SetActive(i == index);
    //    }
    //}

    void ShowCar(int index)
    {
        if (lastIndex != -1)
            cars[lastIndex].SetActive(false);

        cars[index].SetActive(true);
        lastIndex = index;
    }
    public void SelectCar()
    {
        //SceneManager.LoadScene("Level01"); 
        PlayerPrefs.SetInt("CarIndexValue", currentCarIndex);

    }
}
