using FAS.Sounds;
using Zenject;
using FAS.UI;

namespace FAS.Players.States
{
	public class Monologue : PlayerState
	{
		[Inject] private ISoundEffectsPlayer _soundEffectsPlayer;
		[Inject] private IReadOnlyInputEvents _inputEvents;
		[Inject] private IMonologueButtonView _buttonView;
		[Inject] private IVoicePlayer _voicePlayer;
		[Inject] private IMonologueScreenGroup _screen;
		[Inject] private IMonologueText _text;
		
		private MonologueData _currentMonologue;
		
		public override PlayerStates Key => PlayerStates.Monologue;
		
		private int _currentMonologueStringIndex;
		
		public override bool IsPlayerControlledState => false;

		public void Setup(MonologueData monologueData) => _currentMonologue = monologueData;

		public override void Enter()
		{
			_inputEvents.OnNextMonologueButtonClicked += OnNextMonologueButtonClicked;
			_currentMonologueStringIndex = 0;
			PlayCurrentMonologue();
			_screen.Show();
			
			if (_currentMonologue.MonologueStrings.Count - 1 == 0)
				_buttonView.ShowEnd();
			else
				_buttonView.ShowNext();
		}

		private void PlayCurrentMonologue()
		{
			var current = _currentMonologue.MonologueStrings[_currentMonologueStringIndex];
			_text.Set(current.Text);

			if (current.Voice != null)
			{
				switch (current.AudioType)
				{
					case AudioType.SoundEffect:
						_soundEffectsPlayer.Play(current.Voice);
						break;
					case AudioType.Voice:
					default:
					_voicePlayer.Play(current.Voice);
						break;
				}
			}
		}

		private void OnNextMonologueButtonClicked()
		{
			_currentMonologueStringIndex++;
			_soundEffectsPlayer.TryStop();
			_voicePlayer.TryStop();

			if (_currentMonologueStringIndex > _currentMonologue.MonologueStrings.Count - 1)
			{
				StateReturner.TryReturnLastControlledState();
				GamePause.TryPlay();
			}
			else
			{
				PlayCurrentMonologue();
				
				if (_currentMonologueStringIndex == _currentMonologue.MonologueStrings.Count - 1)
					_buttonView.ShowEnd();
			}
		}
		
		public override void Exit()
		{
			_inputEvents.OnNextMonologueButtonClicked -= OnNextMonologueButtonClicked;
			base.Exit();
		}
	}
}