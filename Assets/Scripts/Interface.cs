using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour
{
    GameObject player;
    SpaceshipController playerController;
    GameObject wreck;
    GameObject station;
    Rigidbody2D rb;

    TextMeshProUGUI velocityText;
    TextMeshProUGUI survivorText;
    TextMeshProUGUI objectiveText;
    RectTransform velocityTransform;
    RectTransform objectiveTransform;
    RectTransform fuelTransform;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<SpaceshipController>();
        station = GameObject.FindGameObjectWithTag("Station");
        rb = player.GetComponent<Rigidbody2D>();

        survivorText = transform.Find("Astronauts/Saved").GetComponent<TextMeshProUGUI>();
        objectiveText = transform.Find("Compass/ObjectiveText").GetComponent<TextMeshProUGUI>();
        velocityText = transform.Find("Compass/VelocityText").GetComponent<TextMeshProUGUI>();
        velocityTransform = transform.Find("Compass/Velocity").GetComponent<RectTransform>();
        objectiveTransform = transform.Find("Compass/Objective").GetComponent<RectTransform>();
        fuelTransform = transform.Find("Fuel/Bar").GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (player == null || rb == null) { return; }

        wreck = GameObject.FindGameObjectWithTag("Wreck");

        fuelTransform.sizeDelta = new Vector2((playerController.GetFuel() - 100.0f) * 2, 20);
        Debug.Log(playerController.GetFuel());

        survivorText.SetText(playerController.GetSurvivors().ToString());

        velocityText.SetText($"{Mathf.Round(rb.velocity.magnitude)} u/s");
        Quaternion velocityRotation = Quaternion.LookRotation(rb.velocity, -Vector3.forward); ;
        velocityTransform.rotation = new Quaternion(0,0,-velocityRotation.y,velocityRotation.w);
        velocityTransform.sizeDelta = new Vector2(3, Mathf.Clamp(rb.velocity.magnitude, 0, 40));

        if (wreck == null) 
        {
            Vector2 stationDistance = station.transform.position - player.transform.position;
            objectiveText.SetText($"{Mathf.Round(stationDistance.magnitude)} u");
            Quaternion stationRotation = Quaternion.LookRotation(stationDistance, -Vector3.forward); ;
            objectiveTransform.rotation = new Quaternion(0, 0, -stationRotation.y, stationRotation.w);
            objectiveTransform.sizeDelta = new Vector2(3, Mathf.Clamp(stationDistance.magnitude, 0, 40));

            return;
        }

        Vector2 distance = wreck.transform.position - player.transform.position;
        objectiveText.SetText($"{Mathf.Round(distance.magnitude)} u");
        Quaternion objectiveRotation = Quaternion.LookRotation(distance, -Vector3.forward); ;
        objectiveTransform.rotation = new Quaternion(0, 0, -objectiveRotation.y, objectiveRotation.w);
        objectiveTransform.sizeDelta = new Vector2(3, Mathf.Clamp(distance.magnitude, 0, 40));

    }
}
