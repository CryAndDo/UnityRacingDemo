using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarLapCounter : MonoBehaviour
{
    public Text carPositionText;
    int passedCheckPointNumber = 0;
    float timeAtLastPassedCheckPoint = 0;
    int numberOfPassedCheckPoints = 0;

    int lapsCompleted = 0;
    const int lapsToComplete = 2;
    bool isRaceCompleted = false;
    int carPosition = 0;
    bool isHideRoutineRunning = false;
    float hideUIDelayTime;

    public event Action<CarLapCounter> OnPassCheckpoint;
    public void SetCarPosition(int position)
    {
        carPosition = position;
    }
    public int GetNumberOfCheckpointsPassed()
    {
        return numberOfPassedCheckPoints;
    }
    public float GetTimeAtLastCheckPoint()
    {
        return timeAtLastPassedCheckPoint;
    }
    IEnumerator ShowPositionCO(float delayUntilHidePosition)
    {
        hideUIDelayTime += delayUntilHidePosition;
        carPositionText.text = carPosition.ToString();
        carPositionText.gameObject.SetActive(true);
        if (!isHideRoutineRunning)
        {
            isHideRoutineRunning = true;

            yield return new WaitForSeconds(hideUIDelayTime);
            carPositionText.gameObject.SetActive(false);

            isHideRoutineRunning = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("CheckPoint"))
        {
            if (isRaceCompleted)
                return;

            CheckPoint checkPoint = collider2D.GetComponent<CheckPoint>();
            if (passedCheckPointNumber + 1 == checkPoint.checkPointNumber)
            {
                passedCheckPointNumber = checkPoint.checkPointNumber;
                numberOfPassedCheckPoints++;
                timeAtLastPassedCheckPoint = Time.time;

                if (checkPoint.isFinishLine)
                {
                    passedCheckPointNumber = 0;
                    lapsCompleted++;

                    if (lapsCompleted >= lapsToComplete)
                    {
                        isRaceCompleted = true;
                        GameManager.instance.OnRaceCompleted(); // Убедитесь, что этот вызов происходит
                        GetComponent<CarInputHandler>().enabled = false;
                    }
                }

                OnPassCheckpoint?.Invoke(this);

                if (isRaceCompleted)
                {
                    StartCoroutine(ShowPositionCO(100));
                }
                else if (checkPoint.isFinishLine)
                {
                    StartCoroutine(ShowPositionCO(1.5f));
                }
            }
        }
    }
}
