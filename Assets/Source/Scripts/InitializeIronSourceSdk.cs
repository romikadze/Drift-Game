using com.unity3d.mediation;
using UnityEngine;

namespace Source.Scripts
{
	public class InitializeIronSourceSdk : MonoBehaviour
	{
		public static readonly string UniqueUserId = "demoUserUnity";
		
		private const string APP_KEY = "20a43410d";

		private void Awake()
		{
			LevelPlay.OnInitSuccess += OnInitializationCompleted;
			LevelPlay.OnInitFailed += (error => Debug.Log("Initialization error: " + error));
			IronSourceEvents.onSdkInitializationCompletedEvent += LaunchTestSuite;
			
			Debug.Log("unity-script: Awake called");

			IronSourceConfig.Instance.setClientSideCallbacks(true);

			string id = IronSource.Agent.getAdvertiserId();
			Debug.Log("unity-script: IronSource.Agent.getAdvertiserId : " + id);

			Debug.Log("unity-script: IronSource.Agent.validateIntegration");
			IronSource.Agent.validateIntegration();

			Debug.Log("unity-script: unity version" + IronSource.unityVersion());

			Debug.Log("unity-script: LevelPlay Init");
			LevelPlay.Init(APP_KEY, UniqueUserId, new[] { LevelPlayAdFormat.REWARDED });
			
			IronSource.Agent.setMetaData("is_test_suite", "enable");
			IronSource.Agent.init(APP_KEY);
		}

		private void LaunchTestSuite()
		{
			Debug.Log("IronSource Initialization completed");
			IronSource.Agent.launchTestSuite();
			IronSource.Agent.loadRewardedVideo();
		}

		void OnInitializationCompleted(LevelPlayConfiguration configuration)
		{
			Debug.Log("Initialization completed");
		}

	}
}