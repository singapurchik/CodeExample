using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace FAS
{
	public class Zones : MonoBehaviour, IZonesHolder
	{
		[SerializeField] private ZoneBase _startZone;

		[Inject] private ICameraBlendsChanger _cameraBlendsChanger;
		[Inject] private List<ZoneBase> _zonesList;
		
		private Dictionary<ZoneType, ZoneBase> _zones;
		
		public ZoneType CurrentZoneType => CurrentZone.Type;
		public IZone CurrentZone { get; private set; }

		public event Action<ZoneType> OnZoneChanged;
		
		private void Awake()
		{
			CurrentZone = _startZone;
			
			_zones = new Dictionary<ZoneType, ZoneBase>(_zonesList.Count);
			
			foreach (var zone in _zonesList)
				_zones.Add(zone.Type, zone);
		}

		private void OnEnable()
		{
			foreach (var zone in _zonesList)
				zone.OnEnter.AddListener(OnEnterZone);
		}

		private void OnDisable()
		{
			foreach (var zone in _zonesList)
				zone.OnEnter.RemoveListener(OnEnterZone);
		}

		private void Start()
		{
			foreach (var zone in _zonesList)
				if (zone.Type != CurrentZone.Type)
					zone.Hide();

			StartCoroutine(SetCurrentZonesCameraBlends());
		}

		private IEnumerator SetCurrentZonesCameraBlends()
		{
			yield return new WaitForEndOfFrame();
			_cameraBlendsChanger.TryChangeCameraBlends(CurrentZone.CameraBlends);
		}

		private void OnEnterZone(ZoneType zoneType)
		{
			if (CurrentZone.Type != zoneType)
				OnZoneChanged?.Invoke(zoneType);
			
			CurrentZone = GetZoneByType(zoneType);
		}
		
		public IZone GetZoneByType(ZoneType zoneType) => _zones[zoneType];
	}
}