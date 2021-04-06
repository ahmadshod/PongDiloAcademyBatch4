﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
   public BallControl ball;
    CircleCollider2D ballCollider;
    Rigidbody2D ballRigidbody;
    public GameObject ballAtCollision;
    RaycastHit2D[] circleCastHit2DArray;
    void Start()
    {
        ballCollider = ball.GetComponent<CircleCollider2D>();
        ballRigidbody = ball.GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        bool drawBallAtCollision = true;
        Vector2 offsetHitPoint = new Vector2();
        RaycastHit2D[] circleCastHit2DArray =
        Physics2D.CircleCastAll(ballRigidbody.position, ballCollider.radius,
        ballRigidbody.velocity.normalized);


        foreach (RaycastHit2D circleCastHit2D in circleCastHit2DArray)
        {
           
            if (circleCastHit2D.collider != null &&
                circleCastHit2D.collider.GetComponent<BallControl>() == null)
            {
                // Tentukan titik tumbukan
                Vector2 hitPoint = circleCastHit2D.point;

                // Tentukan normal di titik tumbukan
                Vector2 hitNormal = circleCastHit2D.normal;

                offsetHitPoint = hitPoint + hitNormal * ballCollider.radius;

                //DottedLine.DottedLine.Instance.DrawDottedLine(ball.transform.position, offsetHitPoint);

                // Kalau bukan sidewall, gambar pantulannya
                if (circleCastHit2D.collider.GetComponent<SideWall>() == null)
                {
                    // Hitung vektor datang
                    Vector2 inVector = (offsetHitPoint - ball.TrajectoryOrigin).normalized;

                    // Hitung vektor keluar
                    Vector2 outVector = Vector2.Reflect(inVector, hitNormal);

                    // Hitung dot product dari outVector dan hitNormal. Digunakan supaya garis lintasan ketika 
                    // terjadi tumbukan tidak digambar.
                    float outDot = Vector2.Dot(outVector, hitNormal);
                    if (outDot > -1.0f && outDot < 1.0)
                    {
                        // Gambar lintasan pantulannya
                        DottedLine.DottedLine.Instance.DrawDottedLine(ball.transform.position, offsetHitPoint);
                         
                        // Untuk menggambar bola "bayangan" di prediksi titik tumbukan
                        drawBallAtCollision = true;
                    }
                }

                // Hanya gambar lintasan untuk satu titik tumbukan, jadi keluar dari loop
                break;

            }

           
        }

        if (drawBallAtCollision)
        {
            // Gambar bola "bayangan" di prediksi titik tumbukan
            ballAtCollision.transform.position = offsetHitPoint;
            ballAtCollision.SetActive(true);
        }
        else
        {
            // Sembunyikan bola "bayangan"
            ballAtCollision.SetActive(false);
        }
    }
}