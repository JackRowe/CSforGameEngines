using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField] GameObject uiShopPrefab;
    GameObject uiShop;
    GameObject player;
    SpaceshipController playerController;
    GameObject wreck;
    GameObject station;
    Rigidbody2D rb;

    TextMeshProUGUI velocityText;
    TextMeshProUGUI survivorText;
    TextMeshProUGUI moneyText;
    TextMeshProUGUI objectiveText;
    RectTransform velocityTransform;
    RectTransform objectiveTransform;
    RectTransform fuelTransform;

    private void Awake()
    {
        // Gets the player, player controller, station and the player's rigidbody
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<SpaceshipController>();
        station = GameObject.FindGameObjectWithTag("Station");
        rb = player.GetComponent<Rigidbody2D>();

        // Gets all the UI components that need updating
        survivorText = transform.Find("Astronauts/Saved").GetComponent<TextMeshProUGUI>();
        objectiveText = transform.Find("Compass/ObjectiveText").GetComponent<TextMeshProUGUI>();
        velocityText = transform.Find("Compass/VelocityText").GetComponent<TextMeshProUGUI>();
        moneyText = transform.Find("Money/Amount").GetComponent<TextMeshProUGUI>();
        velocityTransform = transform.Find("Compass/Velocity").GetComponent<RectTransform>();
        objectiveTransform = transform.Find("Compass/Objective").GetComponent<RectTransform>();
        fuelTransform = transform.Find("Fuel/Bar").GetComponent<RectTransform>();
    }
    
    /// <summary>
    /// Opens the shop UI
    /// </summary>
    public void OpenShop()
    {
        uiShop = Instantiate(uiShopPrefab, transform);
    }

    /// <summary>
    /// Closes the shop UI
    /// </summary>
    public void CloseShop()
    {
        Destroy(uiShop);
    }

    private void Update()
    {
        // Makes sure the player and rigidbody both exist
        if (player == null || rb == null) { return; }

        wreck = GameObject.FindGameObjectWithTag("Wreck");

        // Sets the size of the fuel bar to be that of how much fuel remains (out of 100)
        fuelTransform.sizeDelta = new Vector2((playerController.GetFuel() - 100.0f) * 2, 20);

        // Updates the money and survivors text
        survivorText.SetText(playerController.GetSurvivors().ToString());
        moneyText.SetText(playerController.GetMoney().ToString());

        // Updates the velocity text before getting the direction of the player's velocity
        // then updates the velocity arrow to point towards this direction
        velocityText.SetText($"{Mathf.Round(rb.velocity.magnitude)} u/s");
        Quaternion velocityRotation = Quaternion.LookRotation(rb.velocity, -Vector3.forward); ;
        velocityTransform.rotation = new Quaternion(0,0,-velocityRotation.y,velocityRotation.w);
        velocityTransform.sizeDelta = new Vector2(3, Mathf.Clamp(rb.velocity.magnitude, 0, 40));

        // If there isn't a wreck, get the direction and distance of the station
        // update the objective arrow to point towards the direction and change it's size to represent how far away it is
        // also update the distance text
        if (wreck == null) 
        {
            Vector2 stationDistance = station.transform.position - player.transform.position;
            objectiveText.SetText($"{Mathf.Round(stationDistance.magnitude)} u");
            Quaternion stationRotation = Quaternion.LookRotation(stationDistance, -Vector3.forward); ;
            objectiveTransform.rotation = new Quaternion(0, 0, -stationRotation.y, stationRotation.w);
            objectiveTransform.sizeDelta = new Vector2(3, Mathf.Clamp(stationDistance.magnitude, 0, 40));

            return;
        }

        // Finally if the wreck does exist, do the same thing as with the station but with the wreck instead
        Vector2 distance = wreck.transform.position - player.transform.position;
        objectiveText.SetText($"{Mathf.Round(distance.magnitude)} u");
        Quaternion objectiveRotation = Quaternion.LookRotation(distance, -Vector3.forward); ;
        objectiveTransform.rotation = new Quaternion(0, 0, -objectiveRotation.y, objectiveRotation.w);
        objectiveTransform.sizeDelta = new Vector2(3, Mathf.Clamp(distance.magnitude, 0, 40));

    }
}
