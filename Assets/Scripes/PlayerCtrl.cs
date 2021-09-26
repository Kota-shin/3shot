using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{

    public Transform[] RoutePoints;
    [Range(0, 200)]
    public float Speed = 10.0f;

    [Range(0, 200)]
    public float MoveSpeed = 10.0f;
    public float MoveRange = 40.0f;

    bool _isHitRoutePoint;

    IEnumerator Move()
    {
        var prevPointPos = transform.position;
        var basePosition = transform.position;
        var movedPos = Vector2.zero;

        foreach (var nextPoint in RoutePoints)
        {
            _isHitRoutePoint = false;

            while (!_isHitRoutePoint)
            {
                //進行方向の計算
                var vec = nextPoint.position - prevPointPos;
                vec.Normalize();

                //プレイヤーの移動
                //transform.position += vec * Speed * Time.deltaTime;
                basePosition += vec * Speed * Time.deltaTime;

                //上下左右に移動する処理
                movedPos.x += Input.GetAxis("Horizontal") * MoveSpeed * Time.deltaTime;
                movedPos.y += Input.GetAxis("Vertical") * MoveSpeed * Time.deltaTime;
                movedPos = Vector2.ClampMagnitude(movedPos, MoveRange);
                var worldMovedPos = Matrix4x4.Rotate(transform.rotation).MultiplyVector(movedPos);

                //ルート上に位置に上下殺宇野移動量を加える
                transform.position = basePosition + worldMovedPos;


                //進行方向を向くように計算する
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vec, Vector3.up), 0.5f);

                yield return null;
            }

            prevPointPos = nextPoint.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "RoutePoint")
        {
            other.gameObject.SetActive(false);
            _isHitRoutePoint = true;
        }
    }

    void Start()
    {
        StartCoroutine(Move());
    }

    
    void Update()
    {
        
    }
}
