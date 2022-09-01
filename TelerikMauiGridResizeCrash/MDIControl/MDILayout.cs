using System.Diagnostics;
using TelerikMauiGridResizeCrash.Helpers;

namespace TelerikMauiGridResizeCrash.MDIControl
{
    public class MDILayout : AtpCustomLayout
    {
        private const int MinChildSize = 100;

        private ToolContainerInteractionState _toolContainerInteractionState;
        private ToolContainer _mdiTarget;
        private Rect _mdiTargetStartedBounds;
        private PanGestureRecognizer _panGesture;
        private TapGestureRecognizer _tapGesture;

        public MDILayout()
        {
            Debug.WriteLine("Constructing MDIBase");

            _panGesture = new PanGestureRecognizer();
            _panGesture.PanUpdated += PanGesture_PanUpdated;
            _tapGesture = new TapGestureRecognizer();
            _tapGesture.Tapped += TapGesture_Tapped;
            GestureRecognizers.Add(_panGesture);
        }


        protected override void OnAdd(int index, IView view)
        {
            ProcessChild(view, true);
            base.OnAdd(index, view);
        }

        protected override void OnRemove(int index, IView view)
        {
            ProcessChild(view, false);
            base.OnRemove(index, view);
        }

        private void ProcessChild(IView child, bool onAdded)
        {
            if (child is not ToolContainer view)
                return;

            Debug.WriteLine($"[MDI] A child window [{view}] is being {(onAdded ? "added to" : "removed from")} the Mdi container");

            if (onAdded)
                view.InitializeLayout(_panGesture, _tapGesture);
            else
                view.ReleaseLayout();
        }

        private ToolContainer FindToolContainerRoot(Element target)
        {
            if (target == null)
                return null;

            if (target is ToolContainer toolContainer)
                return toolContainer;

            return FindToolContainerRoot(target.Parent);
        }

        private void PanGesture_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            //Debug.WriteLine($"[{sender}] PanGesture_PanUpdated: MDI={_mdiState} | GestureId={e.GestureId} | StatusType={e.StatusType}");
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    if (sender is Element senderElement && sender != this)
                    {
                        var atpView = FindToolContainerRoot(senderElement);
                        if (atpView != null)
                        {
                            RaiseChildInLayout(atpView);
                            _mdiTarget = atpView as ToolContainer;
                            _toolContainerInteractionState = atpView.GetInteractionStateFromTarget(senderElement);
                            _mdiTargetStartedBounds = GetLayoutBounds(_mdiTarget);
                            AtpLayoutManager.MovingMdiTarget = true;
                            AtpLayoutManager.FocusedMdiTarget = _mdiTarget;
                            Children.OfType<ToolContainer>().ForEach(child => child.OnRearrangeStarted(child == _mdiTarget, _toolContainerInteractionState));
                        }
                    }

                    break;
                case GestureStatus.Running:
                    switch (_toolContainerInteractionState)
                    {
                        case ToolContainerInteractionState.Moving:
                            HandleWindowMoveRequest(sender, e);
                            break;
                        case ToolContainerInteractionState.ResizingRight:
                        case ToolContainerInteractionState.ResizingRightBottom:
                        case ToolContainerInteractionState.ResizingBottom:
                            HandleWindowResizeRequest(sender, e);
                            break;
                    }
                    break;
                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                default:
                    Children.OfType<ToolContainer>().ForEach(child => child.OnRearrangeCompleted(child == _mdiTarget));
                    _mdiTarget = null;
                    _mdiTargetStartedBounds = Rect.Zero;
                    _toolContainerInteractionState = ToolContainerInteractionState.Idle;
                    AtpLayoutManager.MovingMdiTarget = false;
                    AtpLayoutManager.FocusedMdiTarget = null;
                    break;
            }
        }

        private void TapGesture_Tapped(object sender, System.EventArgs e)
        {
            //Debug.WriteLine($"[{sender}] PanGesture_PanUpdated");
            if (sender is Element senderElement && sender != this)
            {
                var atpView = FindToolContainerRoot(senderElement);
                if (atpView != null)
                {
                    RaiseChildInLayout(atpView);
                }
            }
        }

        private void RaiseChildInLayout(View mdiTarget)
        {
            var mdiContainer = mdiTarget.Parent as Layout;
            if (mdiContainer == null)
                return;

            var orderedChildren = mdiContainer.Children
                .OfType<View>()
                .Select(c => new { Child = c, Order = c == mdiTarget ? int.MaxValue : c.ZIndex })
                .OrderBy(c => c.Order)
                .ToArray();

            for (int i = 0; i < orderedChildren.Length; i++)
            {
                //Debug.WriteLine($"[MDI] Updating ZIndex for {orderedChildren[i]} to {i}");
                orderedChildren[i].Child.ZIndex = i;
            }
        }

        private void HandleWindowResizeRequest(object sender, PanUpdatedEventArgs e)
        {
            if (_mdiTarget == null || e.StatusType != GestureStatus.Running)
                return;

            var newWidth = _mdiTargetStartedBounds.Width;
            var newHeight = _mdiTargetStartedBounds.Height;

            switch (_toolContainerInteractionState)
            {
                case ToolContainerInteractionState.ResizingRight:
                    newWidth += e.TotalX;
                    break;
                case ToolContainerInteractionState.ResizingRightBottom:
                    newWidth += e.TotalX;
                    newHeight += e.TotalY;
                    break;
                case ToolContainerInteractionState.ResizingBottom:
                    newHeight += e.TotalY;
                    break;
            }

            if (newWidth >= MinChildSize && newHeight >= MinChildSize)
            {
                var newBounds = new Rect(_mdiTargetStartedBounds.X, _mdiTargetStartedBounds.Y, newWidth, newHeight);
                SetLayoutBounds(_mdiTarget, newBounds);
                _mdiTarget.WidthRequest = newBounds.Width;
                _mdiTarget.HeightRequest = newBounds.Height;
                //((Microsoft.Maui.ILayout)_mdiTarget.Parent).InvalidateMeasure();
                //Debug.WriteLine($"HandleWindowResizeRequest: MdiState={_mdiState} | GestureId={e.GestureId} | StatusType={e.StatusType} | XY={e.TotalX:0.0},{e.TotalY:0.0} | CurrentBounds={GetLayoutBounds(_mdiTarget)}");
            }
        }

        private void HandleWindowMoveRequest(object sender, PanUpdatedEventArgs e)
        {
            if (_mdiTarget == null || e.StatusType != GestureStatus.Running)
                return;

            var newBounds = new Rect(_mdiTargetStartedBounds.X + e.TotalX, _mdiTargetStartedBounds.Y + e.TotalY, _mdiTargetStartedBounds.Width, _mdiTargetStartedBounds.Height);
            SetLayoutBounds(_mdiTarget, newBounds);
            //((Microsoft.Maui.ILayout)_mdiTarget.Parent).InvalidateMeasure();
            //Debug.WriteLine($"HandleWindowMoveRequest: MdiState={_mdiState} | GestureId={e.GestureId} | StatusType={e.StatusType} | XY={e.TotalX:0.0},{e.TotalY:0.0} | CurrentBounds={GetLayoutBounds(_mdiTarget)}");
        }

    }
}
