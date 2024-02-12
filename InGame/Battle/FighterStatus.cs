using UnityEngine;

namespace Program.InGame.Battle {

    [System.Serializable]
    public class FighterStatus {
        public string name = "";
        public float life=10, atk=50, def=50, spd=100;
        public string job="";
        public Sprite img;
        public int pos = 0;
        public int charge = 0;
        public bool isGurd = false;
        public bool isDamaged = false;
    }

}