using System;
using UnityEngine;

namespace Data
{
   [Serializable]
    public class GroundConfig 
    {
        [SerializeField] private Sprite icon;
        
        public Sprite Icon=>icon;
       
    }
    
    
}