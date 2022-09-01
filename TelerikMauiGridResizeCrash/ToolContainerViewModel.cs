using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TelerikMauiGridResizeCrash
{
    public abstract class ToolContainerViewModel
    {
        /// <summary>
        /// Pauses ViewModel logic upon request
        /// </summary>
        public virtual void OnPauseUpdates() => IsPaused = true;

        /// <summary>
        /// Resumes ViewModel logic upon request
        /// </summary>
        public virtual void OnResumeUpdates() => IsPaused = false;

        /// <summary>
        /// Name of the monitor
        /// </summary>
        public virtual string ToolName { get; }


        private bool _isPaused;
        /// <summary>
        /// Gets a value indicating whether the ViewModel is in Paused state
        /// </summary>
        public bool IsPaused
        {
            get => _isPaused;
            set => RaiseAndUpdate(ref _isPaused, value);
        }


        /// <summary>
        /// Raises PropertyChanged after updating the backing property with the specified value.
        /// </summary>
        /// <returns><c>true</c>, if and update was raised, <c>false</c> otherwise.</returns>
        /// <param name="field">Field.</param>
        /// <param name="value">Value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected bool RaiseAndUpdate<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
#pragma warning disable CS8604 // Possible null reference argument.
            Raise(propertyName);
#pragma warning restore CS8604 // Possible null reference argument.

            return true;
        }

        readonly Dictionary<string, List<Action>> _propertyWatchers = new Dictionary<string, List<Action>>();

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises PropertyChanged for the a named property.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected void Raise(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName) && PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            if (!_propertyWatchers.ContainsKey(propertyName))
                return;

            var watchers = _propertyWatchers[propertyName];

            foreach (Action watcher in watchers)
                watcher();
        }

        public ISettingsService Settings { get; private set; }

        public ToolContainerViewModel(ISettingsService settingsService)
        {
            Settings = settingsService;
        }
    }
    public interface ISettingsService
    {
        RearrangeUpdateBehavior PauseUpdatesOnRearrange { get; set; }
    }

    public enum RearrangeUpdateBehavior
    {
        DoNothing,
        HideMine,
        HideAll,
        PauseMine,
        PauseAll,
        PauseAndHideMine,
        PauseAndHideAll
    }

    public class SettingsService : ISettingsService
    {
        public RearrangeUpdateBehavior PauseUpdatesOnRearrange
        {
            get => (RearrangeUpdateBehavior)Preferences.Get(nameof(PauseUpdatesOnRearrange), (int)RearrangeUpdateBehavior.PauseAndHideMine);
            set => Preferences.Set(nameof(PauseUpdatesOnRearrange), (int)value);
        }
    }
}