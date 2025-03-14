using System;
using Infrastructure.Services;
using UnityEngine;


namespace UI
{
    public class UIRoot: IDisposable
    {
	    private MediatorFactory _mediatorFactory;
	    private StartMediator _startMediator;
	    private BuildMediator _buildMediator;

	    public UIRoot(MediatorFactory mediatorFactory)
		{
			_mediatorFactory = mediatorFactory;
		}

		public async void Init()
		{
			_startMediator = await _mediatorFactory.Get<StartMediator>();
			_buildMediator = await _mediatorFactory.Get<BuildMediator>();
			await _mediatorFactory.Show<StartMediator>();

			_startMediator.OnStartGame += ClickedStartGame;

			ShowStartMediator();
		}

		
		private void ClickedStartGame()
		{
			ShowBuildMediator();
		}


		private async void ShowStartMediator() => await _mediatorFactory.Show<StartMediator>();
		private async void ShowBuildMediator() => await _mediatorFactory.Show<BuildMediator>();

		public void Dispose()
		{
			_startMediator.OnStartGame -= ClickedStartGame;
			_startMediator?.Dispose();
			_buildMediator?.Dispose();
		}
    }
}