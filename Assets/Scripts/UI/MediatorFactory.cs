using System.Collections.Generic;
using UI;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Infrastructure.Services
{
    public class MediatorFactory 
	{
		private const string AdditionalPath = "GameData/UI/";
		private readonly Dictionary<Type, IMediator> _mediators;
		private Transform _parent;
		private readonly Dictionary<string, Object> _completedCashe = new();
		private Messenger _messenger;
		private readonly BuildSettings _buildSettings;


		public MediatorFactory(BuildSettings buildSettings)
		{
			_buildSettings = buildSettings;
			_mediators = new Dictionary<Type, IMediator>();
		}

		public async Task<TMediator> Get<TMediator>() where TMediator : MonoBehaviour, IMediator
		{
			if (_mediators.TryGetValue(typeof(TMediator), out var mediator))
				return mediator as TMediator;
			

			TMediator mediatorGo = await Load<TMediator>(AdditionalPath + typeof(TMediator).Name);
			TMediator instance = Object.Instantiate(mediatorGo, _parent);
			InitMediator(instance);
			_mediators.Add(instance.GetType(), instance);
			instance.OnCleanUp += CleanUp;
			return instance;
		}

		public async Task<T> Load<T>(string address) where T : Object
		{
			if (_completedCashe.TryGetValue(address, out Object loadedObject))
				return loadedObject as T;

			var resourceRequest = Resources.LoadAsync<T>(address);
			await TaskCompletionSourceFromAsync(resourceRequest);
    
			Object result = resourceRequest.asset;
			_completedCashe.TryAdd(address, result);
			return result as T;
		}

		private Task TaskCompletionSourceFromAsync(ResourceRequest request)
		{
			var tcs = new TaskCompletionSource<bool>();

			request.completed += _ => tcs.SetResult(true);

			return tcs.Task;
		}
		private void InitMediator<TMediator>(TMediator instance) where TMediator : MonoBehaviour, IMediator
		{
			switch (instance)
			{
				case StartMediator startMediator:
					startMediator.Construct();
					break;
				case BuildMediator buildMediator:
					buildMediator.Construct(_buildSettings.BuildConfigs,_messenger);
					break;
			}
		}

		public async Task Show<TMediator>(bool suppressAllWindows = false) where TMediator : MonoBehaviour, IMediator
		{
			var result = await Get<TMediator>();
			foreach (var mediatorPair in _mediators)
			{
				mediatorPair.Value.Hide();
			}
			
			result.Show();
		}
		

		private void CleanUp(IMediator mediator)
		{
			mediator.OnCleanUp -= CleanUp;
			if (_mediators.ContainsKey(mediator.GetType()))
				_mediators.Remove(mediator.GetType());
		}

		public void Init(Transform canvas, Messenger messenger )
		{
			_parent = canvas;
			_messenger = messenger;
		}
	}
}