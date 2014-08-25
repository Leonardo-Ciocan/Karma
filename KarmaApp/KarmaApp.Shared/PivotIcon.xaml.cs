using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace KarmaApp
{
    public sealed partial class PivotIcon : UserControl
    {
        public PivotIcon()
        {
            this.InitializeComponent();
        }

        public Brush Background
        {
            get
            {
                return smallCircle.Fill;
            }
            set
            {
                smallCircle.Fill = value;
                bigCircle.Fill = value;
            }
        }

        public Brush Foreground
        {
            get
            {
                return smallCircle.Stroke;
            }
            set
            {
                smallCircle.Stroke = value;
                bigCircle.Stroke = value;
                txtIcon.Foreground = value;
            }
        }

        public bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                Brush color = smallCircle.Stroke;
                Brush back = smallCircle.Fill;
                if (value)
                {
                    smallCircle.Stroke = back;
                    smallCircle.Fill = color;
                    bigCircle.Stroke = back;
                    bigCircle.Fill = color;
                    txtCount.Foreground = back;
                    txtIcon.Foreground = back;
                }
                else
                {
                    smallCircle.Stroke = color;
                    smallCircle.Fill = back;
                    bigCircle.Stroke = color;
                    bigCircle.Fill = back;
                    txtCount.Foreground = color;
                    txtIcon.Foreground = color;
                }
            }
        }

        public string Icon
        {
            get
            {
                return txtIcon.Text;
            }
            set { txtIcon.Text = value; }
        }

        public bool _hasCounter;
        public bool HasCounter
        {
            get { return _hasCounter; }
            
            set { _hasCounter = value;
            if (!value)
            {
                smallCircle.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                txtCount.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            }
        }

        public string Count
        {
            get
            {
                return (string)GetValue(CountProperty);
            }
            set
            {
                SetValue(CountProperty, value);
            }
        }

        public static DependencyProperty CountProperty = DependencyProperty.Register("Count", typeof(string), typeof(PivotIcon), new PropertyMetadata(null , CountChanged));


        public static void CountChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e){
            var pi = (PivotIcon) obj;
            pi.txtCount.Text = e.NewValue as string; 
        }
    }
}
