using UnityEngine;

public class TimeProgressor : MonoBehaviour
{
    [Range(0, 24)] //Sets slider between 0-24
    public float timeOfDay; //Time in 24.0 hours
    public float orbitSpeed; //Directional Light rotation speed;

    public float axisOffset; //optional offset so the sun doesn't rotate directly on the cardinal axis. Earth is 23.5f for example at the time of writing. 
    public Light sun; //The actual light
    public Gradient nightLight;


    public int hour; //display hours in 24 hour clock
    public int minute; //displays current minute

    public int hourPM; //displays time in 12 hour clock

    public AnimationCurve sunCurve; //the rules for the suns intensity. set in inspector

    //Updates scene when changes are made in inspector
    private void OnValidate()
    {
        ProgressTime();
    }

    //Called every frame in play mode

    void Update()
    {
        timeOfDay += Time.deltaTime * orbitSpeed; //increases timeOfDay by the desired amount every frame
        ProgressTime();
    }


    private void ProgressTime()
    {

        float currentTime = timeOfDay / 24; //get how much time has passed
        float sunRotation = Mathf.Lerp(-90, 270, currentTime); //rotate the sun somewhere between -90 and 270 degrees by how much time has passed

        sun.transform.rotation = Quaternion.Euler(sunRotation, axisOffset, 0); //rotate the sun
   
        hour = Mathf.FloorToInt(timeOfDay); //round timeOfDay down to the nearest int
        minute = Mathf.FloorToInt((timeOfDay / (24f / 1440f ) % 60)); //round minute down to the nearest int. Thanks Missie!

        RenderSettings.ambientLight = nightLight.Evaluate(currentTime); //set night ambience light. Without this the scene would be pitch black
        sun.intensity = sunCurve.Evaluate(currentTime); //disable sun under horizon

        if (hour > 12)
        {
            hourPM = hour - 12;
        }
        if(hour <= 12)
        {
            hourPM = hour;
        }
        if(hour == 0)
        {
            hourPM = 12;
        }

        timeOfDay %= 24; //resets day after 24 hours
    }
}