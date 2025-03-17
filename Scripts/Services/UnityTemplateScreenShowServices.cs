namespace HyperGames.UnityTemplate.UnityTemplate.Scripts.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using GameFoundation.DI;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Signals;
    using GameFoundation.Signals;
    using UnityEngine.Scripting;

    public class UnityTemplateScreenShowServices : IInitializable
    {
        private readonly IReadOnlyList<IUnityTemplateScreenShow> screenShows;
        private readonly SignalBus                            signalBus;

        [Preserve]
        public UnityTemplateScreenShowServices(IEnumerable<IUnityTemplateScreenShow> screenShows, SignalBus signalBus)
        {
            this.screenShows = screenShows.ToArray();
            this.signalBus   = signalBus;
        }

        public void Initialize()
        {
            this.signalBus.Subscribe<ScreenShowSignal>(this.OnScreenShow);
        }

        private void OnScreenShow(ScreenShowSignal obj)
        {
            foreach (var s in this.screenShows)
                if (obj.ScreenPresenter.GetType().Name.Equals(s.ScreenPresenter.Name))
                    s.OnProcessScreenShow();
        }
    }
}