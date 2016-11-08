using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// ================================
//* 功能描述：CharactorCameraCtrl  
//* 创 建 者：chenghaixiao
//* 创建日期：2016/11/7 10:49:01
// ================================
namespace Assets.CharactorCameraCtrl
{
    [ExecuteInEditMode]
    public class CharactorCameraCtrl : MonoBehaviour
    {
        //摄像机朝向的目标模型
        public Transform target;
        //摄像机与模型保持的距离
        public float distance = 10.0f;
        //射线机与模型保持的高度
        public float height = 5.0f;
        //高度阻尼
        public float heightDamping = 2.0f;
        //旋转阻尼
        public float rotationDamping = 3.0f;


        void Update()
        {
            if (InputCommon.TouchIng())
            {
                CheckObstacles();
            }
        }


        private void CheckObstacles()
        {
            bool follow = true;
            //计算相机与主角Y轴旋转角度的差。
            float abs = Mathf.Abs(transform.rotation.eulerAngles.y - target.transform.rotation.eulerAngles.y);
            //abs等于180的时候标示摄像机完全面对这主角， 》130 《 230 表示让面对的角度左右偏移50度
            //这样做是不希望摄像机跟随主角，具体效果大家把代码下载下来看看，这样的摄像机效果很好。
            if (abs > 130 && abs < 230)
            {
                follow = false;
            }
            else
            {
                follow = true;
            }

            float wantedRotationAngle = target.eulerAngles.y;
            float wantedHeight = target.position.y + height;

            float currentRotationAngle = transform.eulerAngles.y;
            float currentHeight = transform.position.y;

            //主角面朝射线机 和背对射线机 计算正确的位置
            if (follow)
            {
                currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
                currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);
                Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);
                Vector3 positon = target.position;
                positon -= currentRotation * Vector3.forward * distance;
                positon = new Vector3(positon.x, currentHeight, positon.z);
                transform.position = Vector3.Lerp(transform.position, positon, Time.time);

            }
            else
            {
                Vector3 positon = target.position;
                Quaternion cr = Quaternion.Euler(0, currentRotationAngle, 0);

                positon += cr * Vector3.back * distance;
                positon = new Vector3(positon.x, target.position.y + height, positon.z);
                transform.position = Vector3.Lerp(transform.position, positon, Time.time);
            }


            //这里是计算射线的方向，从主角发射方向是射线机方向
            Vector3 aim = target.position;
            //得到方向
            Vector3 ve = (target.position - transform.position).normalized;
            float an = transform.eulerAngles.y;
            aim -= an * ve;
            //在场景视图中可以看到这条射线
            Debug.DrawLine(target.position, aim, Color.red);
            //主角朝着这个方向发射射线
            RaycastHit hit;
            if (Physics.Linecast(target.position, aim, out hit))
            {
                if (hit.collider.gameObject.CompareTag("Wall"))
                {
                    //当碰撞的不是摄像机也不是地形 那么直接移动摄像机的坐标
                    transform.position = hit.point;

                }
            }

            // 让射线机永远看着主角
            transform.LookAt(target);

        }

        void OnDrawGizmos()
        {
            Debug.DrawLine(transform.position, target.position, Color.green);
        }
    }
}
