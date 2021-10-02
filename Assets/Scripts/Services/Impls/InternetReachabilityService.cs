using System;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Services.Impls
{
    public class InternetReachabilityService: IInternetReachabilityService, IInitializable, ITickable, IDisposable
	{
		private readonly BoolReactiveProperty _internetReachableProperty = new BoolReactiveProperty(true);

		private IDisposable _intervalDisposable;
		private bool _isAlertOpened;
		public IReadOnlyReactiveProperty<bool> IsInternetReachable => _internetReachableProperty;

		public void Initialize()
		{
			_intervalDisposable = Observable.Interval(TimeSpan.FromSeconds(10)).Subscribe(OnInterval);
		}

		public void ReachabilityRequest() => OnInterval(0);

		public bool TryCloseInternetAlert()
		{
			if (!IsInternetReachable.Value)
				return false;

			CloseInternetAlert();
			return true;
		}

		public void Tick()
		{
			if (IsInternetReachable.Value && _isAlertOpened)
				CloseInternetAlert();

			if (IsInternetReachable.Value || _isAlertOpened)
				return;

			_isAlertOpened = true;
			
			//Тут должен быть код открытия окна отсутствия интернета
			Debug.Log("Интернет отсутствует");
		}

		private void CloseInternetAlert()
		{
			//Тут должен быть код закрытия окна об отсутствия интернета
			Debug.Log("Интернет появился");
			
			_isAlertOpened = false;
		}

		private void OnInterval(long obj)
		{
			var unityWebRequest =
				new UnityWebRequest("https://www.google.ru/");
			var asyncOperation = unityWebRequest.SendWebRequest();
			asyncOperation.completed += OnRequestComplete;
		}

		private void OnRequestComplete(AsyncOperation operation)
		{
			var webRequestAsyncOperation = operation as UnityWebRequestAsyncOperation;
			var webRequest = webRequestAsyncOperation.webRequest;
			_internetReachableProperty.Value = Application.internetReachability != NetworkReachability.NotReachable
			                                   && !webRequest.isHttpError
			                                   && !webRequest.isNetworkError;
		}
		
		public void Dispose() => _intervalDisposable.Dispose();
	}
}