using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class End : MonoBehaviour
{
  [SerializeField] private string sceneName;

  public CoinPicker cp;
  public Timer timer;
  // void Start()
  // {
  //   cp = GameObject.FindGameObjectWithTag("CoinP");
  // }
  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.transform.tag == "Player")
    {
      SaveScore();
      SceneManager.LoadScene(sceneName);

    }
  }

  public void SaveScore()
  {
    PlayerPrefs.SetInt("Score", cp.coin);
    PlayerPrefs.SetFloat("Time", timer.currentTime);
  }
}
