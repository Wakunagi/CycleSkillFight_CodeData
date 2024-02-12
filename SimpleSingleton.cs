using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Program {
    public class SimpleSingleton<T> : MonoBehaviour {
        public static T instance;

        private void Awake() {
            if (instance == null) {
                instance = gameObject.GetComponent<T>();
            }
            else {
                Destroy(this);
            }
        }
    }
}