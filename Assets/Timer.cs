using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
  public float currentTime = 0.0f;
  float startingTime = 0.0f;

  [SerializeField] Text countdownText;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = startingTime;
    }

    // Update is called once per frame
    public void Update()
    {
        currentTime += 1 * (Time.deltaTime);
        countdownText.text = currentTime.ToString("0.0");
    }
}
