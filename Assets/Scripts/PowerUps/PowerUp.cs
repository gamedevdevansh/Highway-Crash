using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type { Speed, Shield, Magnet }

    public Type type;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        CarController car = other.GetComponent<CarController>();

        switch (type)
        {
            case Type.Speed:
                car.BoostSpeed();

                break;

            case Type.Shield:
                car.ActivateShield();
                break;

            case Type.Magnet:
                car.ActivateMagnet();
                break;
        }

        gameObject.SetActive(false); // RETURN TO POOL
    }
}