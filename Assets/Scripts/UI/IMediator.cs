using System;
using UnityEngine;

namespace UI
{
    public interface IMediator
    {
        public event Action<IMediator> OnCleanUp;
        public GameObject GameObject { get; }
        public void Show();
        public void Hide();
      
    }
}