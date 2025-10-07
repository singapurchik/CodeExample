using UnityEngine;
using FAS.Utils;

namespace FAS
{
	public abstract class Weapon<TData, TCostumePoints> : MonoBehaviour
		where TData : WeaponData
		where TCostumePoints : CostumeWeaponPoints
	{
		[SerializeField] private TData _data;
		[SerializeField] private GameObject _model;

		private TCostumePoints _costumePoints;
		private Transform _defaultParent;

		private readonly WeaponTransformTransition _transition = new ();

		public TData Data => _data;

		protected virtual void Awake()
		{
			_defaultParent = transform.parent;
			Unequip();
		}

		public void SetCostumePoints(TCostumePoints points) => _costumePoints = points;

		public virtual void Unequip()
		{
			Data.SetEquipped(false);

			if (_model.TryDisable())
				ChangeParent(_defaultParent);
		}

		public virtual void Equip()
		{
			Data.SetEquipped(true);
			_model.TryEnable();
		}

		public void SetToRightHand(float duration = 0f) => ChangeParent(_costumePoints.RightHand, duration);
		
		public void SetToLeftHand(float duration = 0f) => ChangeParent(_costumePoints.LeftHand, duration);
		
		public void SetToSpine(float duration = 0f) => ChangeParent(_costumePoints.Spine, duration);

		private void ChangeParent(Transform parent, float duration = 0f)
			=> _transition.StartTransition(transform, parent, duration);

		private void Update()
		{
			if (_transition.IsActive)
				_transition.Update();
		}
	}
}