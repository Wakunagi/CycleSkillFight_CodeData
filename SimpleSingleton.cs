using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Program {

    //�V���O���g���p�̐e�N���X
    public class SimpleSingleton<T> : MonoBehaviour {
        public static T instance;

        private void Awake() {

            //instance�ɉ����ݒ肳��ĂȂ���ΐݒ肷��
            if (instance == null) {
                instance = gameObject.GetComponent<T>();
            }

            //���Őݒ肳��Ă���Δj������
            else { Destroy(this); }
        }
    }
}