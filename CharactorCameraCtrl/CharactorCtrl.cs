using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// ================================
//* 功能描述：CharactorCtrl  
//* 创 建 者：chenghaixiao
//* 创建日期：2016/11/7 10:51:14
// ================================
namespace Assets.CharactorCameraCtrl
{
    public class CharactorCtrl : MonoBehaviour
    {
        [SerializeField]
        private float fDistance = 10;
        [SerializeField]
        private Transform transHero;
        [SerializeField]
        private float fSpeed = 5;
        [SerializeField]
        private float fRotate = 20;

        private float fHorValue;

        private float fVerValue;


        void Start()
        {
            if (transHero == null)
                transHero = this.transform;
        }

        void Update()
        {
            fHorValue = Input.GetAxis("Horizontal");
            fVerValue = Input.GetAxis("Vertical");
            if (fHorValue != 0 && fVerValue != 0)
                SetDir();

            transHero.transform.Translate(transHero.transform.rotation * new Vector3(fHorValue * fSpeed * Time.deltaTime, 0, fVerValue * fSpeed * Time.deltaTime), Space.World);

        }

        void SetDir()
        {
            Vector3 dir = new Vector3(fHorValue, 0, fVerValue).normalized;
            Vector3 mDir = transHero.transform.rotation * Vector3.forward;
            float angle = Mathf.Acos(Vector3.Dot(dir, mDir)) * Mathf.Rad2Deg;
            if (angle > 0)
            {
                Vector3 tempAngles = transHero.transform.eulerAngles;
                if (fHorValue > 0)
                    transHero.transform.rotation = Quaternion.Euler(tempAngles.x, tempAngles.y + Time.deltaTime * fRotate, tempAngles.z);
                else
                    transHero.transform.rotation = Quaternion.Euler(tempAngles.x, tempAngles.y - Time.deltaTime * fRotate, tempAngles.z);
            }

            //Debug.Log("dir:" + dir + "myDir:" + mDir + "angle:" + angle);
        }

        void OnDrawGizmos()
        {
            Debug.DrawLine(transHero.position, transHero.position + transHero.transform.rotation * Vector3.forward * fDistance, Color.red);
        }

    }
}
