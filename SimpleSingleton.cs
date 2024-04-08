using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Program {

    //シングルトン用の親クラス
    public class SimpleSingleton<T> : MonoBehaviour {
        public static T instance;

        private void Awake() {

            //instanceに何も設定されてなければ設定する
            if (instance == null) {
                instance = gameObject.GetComponent<T>();
            }

            //他で設定されていれば破棄する
            else { Destroy(this); }
        }
    }
}