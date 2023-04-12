using UnityEngine ;

namespace PickerWheelUI {
   [System.Serializable]
   public class WheelPiece {
        public UnityEngine.Sprite Icon ;
        public string Label ;

        [Tooltip ("Probability in %")] 
        [Range (0f, 100f)] 
        public float Chance = 100f ;

        [HideInInspector] public int Index ;
        [HideInInspector] public double _weight = 0f ;
        public string Question;
        public string[] Toggles;
      

   }
}
