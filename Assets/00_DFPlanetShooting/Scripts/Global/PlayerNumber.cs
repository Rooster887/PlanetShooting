using UnityEngine;

namespace J8N9.PlanetShooting
{
    public class PlayerNumber : MonoBehaviour
    {
        [SerializeField]
        private int Num;

        // プレイヤー判別
        public int PlayerNum()
        {
            return Num;
        }
    }
}