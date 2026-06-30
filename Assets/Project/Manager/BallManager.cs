using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField] private List<BallData> equippedBalls = new List<BallData>() ;
    [SerializeField] private Transform firePoint;
    [SerializeField] private PlayerShooter2D playerShooter;
    [SerializeField] private int ballCount;
    [SerializeField] private float fireInterval = 0.08f;
    private int nextBallIndex;

    private void Start()
    {
        StartCoroutine(FireInitialBalls());
    }
    private void FireOneBallImmediate() //현재 장착된 볼 데이터를 기반으로 볼 생성 및 발사
    {
        if (equippedBalls.Count <= 0) return;

        BallData ballData = equippedBalls[nextBallIndex % equippedBalls.Count];
        nextBallIndex++;

        BallController2D ball = Instantiate(ballData.ballPrefab, firePoint.position, Quaternion.identity);

        ball.Initialize(ballData);
        ball.OnRecovered += HandleBallRecovered;
        ball.Launch(playerShooter.AimDirection);
    }

    private IEnumerator FireInitialBalls()  //시작 시 여러 개의 볼을 순차적으로 발사
    {
        for (int i = 0; i < ballCount; i++)
        {
            FireOneBallImmediate();
            yield return new WaitForSeconds(fireInterval);
        }
    }

    private IEnumerator FireOneBall() //회수된 볼을 일정 시간 후 다시 발사
    {
        yield return new WaitForSeconds(fireInterval);
        FireOneBallImmediate();
    }

    private void HandleBallRecovered(BallController2D ball) //회수된 볼 제거 후 새로운 볼 발사
    {
        ball.OnRecovered -= HandleBallRecovered;
        Destroy(ball.gameObject); //임시
        StartCoroutine(FireOneBall());
    }
}
