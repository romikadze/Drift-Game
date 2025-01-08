using System;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Source.Scripts
{
    public class AdManager : IInitializable
    {
		public event Action<bool> OnRewarded;
	  
		private const string APP_ID = "20a43410d";

		public void Initialize()
		{
			if(!IsRewardedVideoReady())
				LoadRewardedVideo();
		}

		public bool IsRewardedVideoReady()
		{
			if (!IsInit())
				return false;

			var isReady = IronSource.Agent.isRewardedVideoAvailable();

			return isReady;
		}

		public async UniTask<bool> ShowRewardedVideoAsync()
		{
			var isClosed = false;
			var success = false;
			IronSourceRewardedVideoEvents.onAdRewardedEvent += Success;
			IronSourceRewardedVideoEvents.onAdClosedEvent += VideoClosed;
			IronSource.Agent.showRewardedVideo();
        
			await UniTask.WaitUntil(() => isClosed);
			LoadRewardedVideo();
			OnRewarded?.Invoke(success);

			return success;

			void Success(IronSourcePlacement placement, IronSourceAdInfo ironSourceAdInfo)
			{
				IronSourceRewardedVideoEvents.onAdRewardedEvent -= Success;
        				
				success = true;
			}

			void VideoClosed(IronSourceAdInfo ironSourceAdInfo)
			{
				IronSourceRewardedVideoEvents.onAdClosedEvent -= VideoClosed;
				isClosed = true;
			}
		}

		private bool IsInit()
		{
			if (IronSource.Agent != null)
				return true;
        
			return false;
		}

		private void LoadRewardedVideo()
		{
			IronSource.Agent.loadRewardedVideo();
		}
    }
}