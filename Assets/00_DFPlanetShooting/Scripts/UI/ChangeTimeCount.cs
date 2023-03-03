using UnityEngine;
using UnityEngine.UI;

namespace J8N9.PlanetShooting
{
    public class ChangeTimeCount : MonoBehaviour
    {
        public Toggle toggle;

        public void OnChangeTimeCount()
        {
            PlayTime.playTime = toggle.isOn ? 60 : 89; //チェックON,OFFでプレイタイム切り替え
        }
    }
}