using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelerikMauiGridResizeCrash.MDIControl
{
    public class AtpCustomLayout : Layout
    {
        #region attached properties

        public static readonly BindableProperty LayoutBoundsProperty = BindableProperty.CreateAttached("LayoutBounds",
            typeof(Rect), typeof(AtpCustomLayout), new Rect(0, 0, -1, -1), propertyChanged: LayoutBoundsPropertyChanged);

        static void LayoutBoundsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is View view && view.Parent is Microsoft.Maui.ILayout layout)
            {
                layout.InvalidateMeasure();
            }
        }

        [System.ComponentModel.TypeConverter(typeof(BoundsTypeConverter))]
        public static Rect GetLayoutBounds(BindableObject bindable)
        {
            return (Rect)bindable.GetValue(LayoutBoundsProperty);
        }

        public static void SetLayoutBounds(BindableObject bindable, Rect bounds)
        {
            bindable.SetValue(LayoutBoundsProperty, bounds);
        }

        #endregion

        protected AtpCustomLayoutManager AtpLayoutManager { get; private set; }

        public AtpCustomLayout()
        {
            AtpLayoutManager = new AtpCustomLayoutManager(this);
        }

        protected override ILayoutManager CreateLayoutManager()
        {
            return AtpLayoutManager;
        }
    }
}


