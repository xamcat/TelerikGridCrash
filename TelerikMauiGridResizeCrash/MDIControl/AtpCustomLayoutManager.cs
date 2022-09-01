using Microsoft.Maui.Layouts;

namespace TelerikMauiGridResizeCrash.MDIControl
{
    public class AtpCustomLayoutManager : LayoutManager
    {
        public AtpCustomLayout AtpCustomLayout { get; }

        public bool MovingMdiTarget { get; set; } = false;

        public Element FocusedMdiTarget { get; set; }

        public AtpCustomLayoutManager(AtpCustomLayout layout)
            : base(layout)
        {
            AtpCustomLayout = layout;
        }

        public override Size ArrangeChildren(Rect bounds)
        {
            var padding = AtpCustomLayout.Padding;
            var top = padding.Top + bounds.Y;
            var left = padding.Left + bounds.X;
            var availableWidth = bounds.Width - padding.HorizontalThickness;
            var availableHeight = bounds.Height - padding.VerticalThickness;

            for (int n = 0; n < AtpCustomLayout.Count; n++)
            {
                var child = AtpCustomLayout[n];
                if (child.Visibility == Visibility.Collapsed)
                    continue;

                if (MovingMdiTarget && child is Element elem && elem.Id != FocusedMdiTarget.Id)
                    continue;

                var destination = AtpCustomLayout.GetLayoutBounds((BindableObject)child);
                destination.X += left;
                destination.Y += top;
                child.Arrange(destination);
            }

            return new Size(availableWidth, availableHeight);
        }

        public override Size Measure(double widthConstraint, double heightConstraint)
        {
            for (int n = 0; n < AtpCustomLayout.Count; n++)
            {
                var child = AtpCustomLayout[n];
                if (child.Visibility == Visibility.Collapsed)
                    continue;

                if (MovingMdiTarget && child is Element elem && elem.Id != FocusedMdiTarget.Id)
                    continue;

                var measure = child.Measure(widthConstraint, heightConstraint);
            }

            return new Size(widthConstraint, heightConstraint);
        }
    }
}

