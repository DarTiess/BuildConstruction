using System;
using UnityEngine;

namespace Card
{
   [Serializable]
    public class GroundConfig 
    {
        [SerializeField] private Sprite icon;
        
        public Sprite Icon=>icon;
       
    }
    
    
}