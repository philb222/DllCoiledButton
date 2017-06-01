using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DllCoiledButton
{
	/// <summary>
	/// Provide a button that when clicked:
	/// 1. Does an animation that looks like the button has sprung on a coiled wire that needs fixing.
	/// 2. The user can listen for the 'CB_Click' event to handle what happens on a click after the animation has finished.
	/// 
	/// FYI: Various properties allow the user to change how the button looks.
	/// All properties start with 'CB_' as 'CB' stands for Coiled Button.
	/// </summary>
	public partial class CoiledButton : UserControl
	{
		// For initial animations where button and coiled beziers go up.
		private BounceEase _bounce1;

        // For 2nd animations where button and coiled beziers hang down a bit and bounce.
        private BounceEase _bounce2;

        #region Constructor
        public CoiledButton()
        {
            InitializeComponent();

            _bounce1 = new BounceEase();
            _bounce1.Bounces = 6;
            _bounce1.Bounciness = 4;
            _bounce1.EasingMode = EasingMode.EaseOut;

            _bounce2 = new BounceEase();
            _bounce2.Bounces = 10;
            _bounce2.Bounciness = 4;
            _bounce2.EasingMode = EasingMode.EaseOut;
        }
        #endregion

        #region CB_ClickEvent is raised when the user clicks on this button after animations complete

        private static readonly RoutedEvent CB_ClickEvent = EventManager.RegisterRoutedEvent("CB_Click", RoutingStrategy.Direct,
            typeof(RoutedEventHandler), typeof(CoiledButton));

        public event RoutedEventHandler CB_Click
        {
            add { this.AddHandler(CB_ClickEvent, value); }
            remove { this.RemoveHandler(CB_ClickEvent, value); }
        }

        public void RaiseCB_Click()
        {
            RoutedEventArgs NewEvent = new RoutedEventArgs(CoiledButton.CB_ClickEvent);
            RaiseEvent(NewEvent);
        }

        #endregion

        #region CB_Background Property (for the button)

        private static readonly DependencyProperty CB_BackgroundProperty =
            DependencyProperty.Register("CB_Background", typeof(Color), typeof(CoiledButton),
            new FrameworkPropertyMetadata(Colors.Aqua, FrameworkPropertyMetadataOptions.AffectsRender,
            new PropertyChangedCallback(CB_BackgroundPropertyChanged)));

        private static void CB_BackgroundPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as CoiledButton).OnCB_BackgroundPropertyChanged(e);
        }

        private void OnCB_BackgroundPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            btnCoiled.Background = new SolidColorBrush((Color)e.NewValue);
        }

        // Allow access via XAML.
        public Color CB_Background
        {
            get { return (Color)GetValue(CB_BackgroundProperty); }

            set { SetValue(CB_BackgroundProperty, value); }
        }
        #endregion

        #region CB_Foreground Property (for the button)

        private static readonly DependencyProperty CB_ForegroundProperty =
            DependencyProperty.Register("CB_Foreground", typeof(Color), typeof(CoiledButton),
            new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.AffectsRender,
            new PropertyChangedCallback(CB_ForegroundPropertyChanged)));

        private static void CB_ForegroundPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as CoiledButton).OnCB_ForegroundPropertyChanged(e);
        }

        private void OnCB_ForegroundPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            labCoiled.Foreground = new SolidColorBrush((Color)e.NewValue);
        }

        // Allow access via XAML.
        public Color CB_Foreground
        {
            get { return (Color)GetValue(CB_ForegroundProperty); }

            set { SetValue(CB_ForegroundProperty, value); }
        }
        #endregion

        #region CB_CoilColor Property (for the coils / bezier curves)

        private static readonly DependencyProperty CB_CoilColorProperty =
            DependencyProperty.Register("CB_CoilColor", typeof(Color), typeof(CoiledButton),
            new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.AffectsRender,
            new PropertyChangedCallback(CB_CoilColorPropertyChanged)));

        private static void CB_CoilColorPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as CoiledButton).OnCB_CoilColorPropertyChanged(e);
        }

        private void OnCB_CoilColorPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            pathCoil.Stroke = new SolidColorBrush((Color)e.NewValue);
        }

        // Allow access via XAML.
        public Color CB_CoilColor
        {
            get { return (Color)GetValue(CB_CoilColorProperty); }

            set { SetValue(CB_CoilColorProperty, value); }
        }
        #endregion

        #region CB_BorderThickness Property (for the button)

        private static Thickness _defaultThickness = new Thickness(3,3,3,3);

        private static readonly DependencyProperty CB_BorderThicknessProperty =
            DependencyProperty.Register("CB_BorderThickness", typeof(Thickness), typeof(CoiledButton),
            new FrameworkPropertyMetadata(_defaultThickness, FrameworkPropertyMetadataOptions.AffectsRender | 
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsParentMeasure,
            new PropertyChangedCallback(CB_BorderThicknessPropertyChanged)));

        private static void CB_BorderThicknessPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as CoiledButton).OnCB_BorderThicknessPropertyChanged(e);
        }

        private void OnCB_BorderThicknessPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            btnCoiled.BorderThickness = (Thickness)e.NewValue;
        }

        // Allow access via XAML.
        public Thickness CB_BorderThickness
        {
            get { return (Thickness)GetValue(CB_BorderThicknessProperty); }

            set { SetValue(CB_BorderThicknessProperty, value); }
        }
        #endregion

        #region CB_Height Property (for the button)

        private static readonly DependencyProperty CB_HeightProperty =
            DependencyProperty.Register("CB_Height", typeof(Double), typeof(CoiledButton),
            new FrameworkPropertyMetadata(32d, FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsParentMeasure |
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsParentArrange,
            new PropertyChangedCallback(CB_HeightPropertyChanged)));

        private static void CB_HeightPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as CoiledButton).OnCB_HeightPropertyChanged(e);
        }

        private void OnCB_HeightPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            btnCoiled.Height = (Double)e.NewValue;
            CoiledButtonCanvas.Height = (Double)e.NewValue;
        }

        // Allow access via XAML.
        public Double CB_Height
        {
            get { return (Double)GetValue(CB_HeightProperty); }

            set { SetValue(CB_HeightProperty, value); }
        }
        #endregion

        #region CB_Width Property (for the button)

        private static readonly DependencyProperty CB_WidthProperty =
            DependencyProperty.Register("CB_Width", typeof(Double), typeof(CoiledButton),
            new FrameworkPropertyMetadata(100d, FrameworkPropertyMetadataOptions.AffectsRender |
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsParentMeasure | 
                FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsParentArrange,
            new PropertyChangedCallback(CB_WidthPropertyChanged)));

        private static void CB_WidthPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as CoiledButton).OnCB_WidthPropertyChanged(e);
        }

        private void OnCB_WidthPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            btnCoiled.Width = (Double)e.NewValue;
            CoiledButtonCanvas.Width = (Double)e.NewValue;

            // Need to move the bezier curves that make up the coil-like shape
            // so they remain hidden directly behind the button with its newly set width.
            RecenterBezierSegments();
        }

        // Allow access via XAML.
        public Double CB_Width
        {
            get { return (Double)GetValue(CB_WidthProperty); }

            set { SetValue(CB_WidthProperty, value); }
        }
        #endregion

        #region CB_Text Property (for the button)

        private static readonly DependencyProperty CB_TextProperty =
            DependencyProperty.Register("CB_Text", typeof(String), typeof(CoiledButton),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsRender,
            new PropertyChangedCallback(CB_TextPropertyChanged)));

        private static void CB_TextPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            (obj as CoiledButton).OnCB_TextPropertyChanged(e);
        }

        private void OnCB_TextPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            labCoiled.Content = (String)e.NewValue;
        }

        // Allow access via XAML.
        public String CB_Text
        {
            get { return (String)GetValue(CB_TextProperty); }

            set { SetValue(CB_TextProperty, value); }
        }
        #endregion

        #region Private Helper to recenter the Bezier Segments
        /// <summary>
        /// For properties that change the position of the button, here we
        /// recalculate where the hidden coil goes (the bezier segments).
        /// </summary>
        private void RecenterBezierSegments()
        {
            // First calculate the center point of the button. That's where the
            // start of the path object will start for the bezier curvers.
            double buttonCenterX = btnCoiled.Margin.Left;
            buttonCenterX += btnCoiled.Width / 2;

            double buttonCenterY = btnCoiled.Margin.Top;
            buttonCenterY += btnCoiled.Height / 2;

            Point newBezierPoint = new Point(buttonCenterX, buttonCenterY);

            PathStart.StartPoint = newBezierPoint;

            // Give the same point for all Bezier points. These points will be replaced
            // when the animation begins. In the meantime the bezier's remain hidden behind the button.
            btnBezier1.Point1 = newBezierPoint;
            btnBezier1.Point2 = newBezierPoint;
            btnBezier1.Point3 = newBezierPoint;

            btnBezier2.Point1 = newBezierPoint;
            btnBezier2.Point2 = newBezierPoint;
            btnBezier2.Point3 = newBezierPoint;

            btnBezier3.Point1 = newBezierPoint;
            btnBezier3.Point2 = newBezierPoint;
            btnBezier3.Point3 = newBezierPoint;

            btnBezier4.Point1 = newBezierPoint;
            btnBezier4.Point2 = newBezierPoint;
            btnBezier4.Point3 = newBezierPoint;

            btnBezier5.Point1 = newBezierPoint;
            btnBezier5.Point2 = newBezierPoint;
            btnBezier5.Point3 = newBezierPoint;

            btnBezier6.Point1 = newBezierPoint;
            btnBezier6.Point2 = newBezierPoint;
            btnBezier6.Point3 = newBezierPoint;

            btnBezier7.Point1 = newBezierPoint;
            btnBezier7.Point2 = newBezierPoint;
            btnBezier7.Point3 = newBezierPoint;
        }
        #endregion

        #region Button Clicked - Do animation
        private void btnCoiled_Click(object sender, RoutedEventArgs e)
        {
            Storyboard _coiledSpringStoryBoard1 = new Storyboard();
            _coiledSpringStoryBoard1.Completed += _coiledSpringStoryBoard1_Completed;

            // Make final assignments of the bezier attraction points now. 
            // If the user set 'width' to a small value, the beziers would be seen, so we adjust the
            // bezier's attraction points now, just before animation.
            Point startPoint = new Point(btnBezier1.Point1.X, btnBezier1.Point1.Y);

            // Set each bezier curver relative to the button center.
            // Point1 = the left attraction point
            // Point2 = the right attraction point
            // Point3 = the end point
            btnBezier1.Point1 = new Point(startPoint.X - 80, startPoint.Y);
            btnBezier2.Point1 = new Point(startPoint.X - 80, startPoint.Y);
            btnBezier3.Point1 = new Point(startPoint.X - 80, startPoint.Y);
            btnBezier4.Point1 = new Point(startPoint.X - 80, startPoint.Y);
            btnBezier5.Point1 = new Point(startPoint.X - 80, startPoint.Y);
            btnBezier6.Point1 = new Point(startPoint.X - 80, startPoint.Y);
            btnBezier7.Point1 = new Point(startPoint.X - 80, startPoint.Y);

            btnBezier1.Point2 = new Point(startPoint.X + 80, startPoint.Y);
            btnBezier2.Point2 = new Point(startPoint.X + 80, startPoint.Y);
            btnBezier3.Point2 = new Point(startPoint.X + 80, startPoint.Y);
            btnBezier4.Point2 = new Point(startPoint.X + 80, startPoint.Y);
            btnBezier5.Point2 = new Point(startPoint.X + 80, startPoint.Y);
            btnBezier6.Point2 = new Point(startPoint.X + 80, startPoint.Y);
            btnBezier7.Point2 = new Point(startPoint.X + 80, startPoint.Y);

            // Animate btnBezier1 ===============================
            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1, 
                new Point(btnBezier1.Point1.X, btnBezier1.Point1.Y),
                new Point(btnBezier1.Point1.X, btnBezier1.Point1.Y - 10),
                "btnBezier1", new PropertyPath(BezierSegment.Point1Property));

            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier1.Point2.X, btnBezier1.Point2.Y),
                new Point(btnBezier1.Point2.X, btnBezier1.Point2.Y - 15),
                "btnBezier1", new PropertyPath(BezierSegment.Point2Property));

            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier1.Point3.X, btnBezier1.Point3.Y),
                new Point(btnBezier1.Point3.X, btnBezier1.Point3.Y - 20),
                "btnBezier1", new PropertyPath(BezierSegment.Point3Property));

            // Animate btnBezier2 ===============================
            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier2.Point1.X, btnBezier2.Point1.Y),
                new Point(btnBezier2.Point1.X -5, btnBezier2.Point1.Y - 30),
                "btnBezier2", new PropertyPath(BezierSegment.Point1Property));

            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier2.Point2.X, btnBezier2.Point2.Y),            
                new Point(btnBezier2.Point2.X - 5, btnBezier2.Point2.Y - 35),
                "btnBezier2", new PropertyPath(BezierSegment.Point2Property));

            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier2.Point3.X, btnBezier2.Point3.Y),
                new Point(btnBezier2.Point3.X - 5, btnBezier2.Point3.Y - 40),
                "btnBezier2", new PropertyPath(BezierSegment.Point3Property));

            // Animate btnBezier3 ===============================
            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier3.Point1.X, btnBezier3.Point1.Y),
                new Point(btnBezier3.Point1.X - 10, btnBezier3.Point1.Y - 50),
                "btnBezier3", new PropertyPath(BezierSegment.Point1Property));

            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier3.Point2.X, btnBezier3.Point2.Y),        
                new Point(btnBezier3.Point2.X - 10, btnBezier3.Point2.Y - 55),
                "btnBezier3", new PropertyPath(BezierSegment.Point2Property));

            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier3.Point3.X, btnBezier3.Point3.Y),        
                new Point(btnBezier3.Point3.X - 10, btnBezier3.Point3.Y - 60),
                "btnBezier3", new PropertyPath(BezierSegment.Point3Property));

            // Animate btnBezier4 ===============================
            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier4.Point1.X, btnBezier4.Point1.Y),
                new Point(btnBezier4.Point1.X - 15, btnBezier4.Point1.Y - 70),
                "btnBezier4", new PropertyPath(BezierSegment.Point1Property));

            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier4.Point2.X, btnBezier4.Point2.Y),        
                new Point(btnBezier4.Point2.X - 15, btnBezier4.Point2.Y - 75),
                "btnBezier4", new PropertyPath(BezierSegment.Point2Property));

            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier4.Point3.X, btnBezier4.Point3.Y),
                new Point(btnBezier4.Point3.X - 15, btnBezier4.Point3.Y - 80),
                "btnBezier4", new PropertyPath(BezierSegment.Point3Property));

            // Animate btnBezier5 ===============================
            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier5.Point1.X, btnBezier5.Point1.Y),
                new Point(btnBezier5.Point1.X - 20, btnBezier5.Point1.Y - 90),
                "btnBezier5", new PropertyPath(BezierSegment.Point1Property));

            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier5.Point2.X, btnBezier5.Point2.Y),        
                new Point(btnBezier5.Point2.X - 20, btnBezier5.Point2.Y - 95),
                "btnBezier5", new PropertyPath(BezierSegment.Point2Property));

            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier5.Point3.X, btnBezier5.Point3.Y),
                new Point(btnBezier5.Point3.X - 20, btnBezier5.Point3.Y - 100),
                "btnBezier5", new PropertyPath(BezierSegment.Point3Property));

            // Animate btnBezier6 ===============================
            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier6.Point1.X, btnBezier6.Point1.Y),
                new Point(btnBezier6.Point1.X - 25, btnBezier6.Point1.Y - 110),
                "btnBezier6", new PropertyPath(BezierSegment.Point1Property));

            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier6.Point2.X, btnBezier6.Point2.Y),        
                new Point(btnBezier6.Point2.X - 25, btnBezier6.Point2.Y - 115),
                "btnBezier6", new PropertyPath(BezierSegment.Point2Property));

            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier6.Point3.X, btnBezier6.Point3.Y),
                new Point(btnBezier6.Point3.X - 25, btnBezier6.Point3.Y - 120),
                "btnBezier6", new PropertyPath(BezierSegment.Point3Property));

            // Animate btnBezier7 ===============================
            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier7.Point1.X, btnBezier7.Point1.Y),
                new Point(btnBezier7.Point1.X - 30, btnBezier7.Point1.Y - 130),
                "btnBezier7", new PropertyPath(BezierSegment.Point1Property));

            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier7.Point2.X, btnBezier7.Point2.Y),        
                new Point(btnBezier7.Point2.X - 30, btnBezier7.Point2.Y - 135),
                "btnBezier7", new PropertyPath(BezierSegment.Point2Property));

            Setup_KeyFramePointAnimation1(_coiledSpringStoryBoard1,
                new Point(btnBezier7.Point3.X, btnBezier7.Point3.Y),
                new Point(btnBezier7.Point3.X - 30, btnBezier7.Point3.Y - 140),
                "btnBezier7", new PropertyPath(BezierSegment.Point3Property));

            // Start a button move animation
            Storyboard _moveCoiledButtonStoryBoard1 = new Storyboard();
            _moveCoiledButtonStoryBoard1.Completed += _moveCoiledButtonStoryBoard1_Completed;

            //double _finalTop = 0 - 250d - (CB_Height / 5d);
            double _finalTop = 0 - 250d;

            Setup_KeyFrameThicknessAnimation1(_moveCoiledButtonStoryBoard1,
                new Thickness(btnCoiled.Margin.Left, btnCoiled.Margin.Top, 0, 0),
                new Thickness(btnCoiled.Margin.Left -30, _finalTop, 0, 0),
                "btnCoiled", new PropertyPath(Button.MarginProperty));

            // Start the initial animations ==========================================================
            _coiledSpringStoryBoard1.Begin(CoiledButtonCanvas);
            _moveCoiledButtonStoryBoard1.Begin(CoiledButtonCanvas);
        }

        /// <summary>
        /// Used to move the bezier points.
        /// </summary>
        private void Setup_KeyFramePointAnimation1(Storyboard inStoryboard, Point inPoint1, Point inPoint2,
            string inElementName, PropertyPath inPropertyPath)
        {
            PointAnimationUsingKeyFrames animation = new PointAnimationUsingKeyFrames();

            animation.KeyFrames.Add(new EasingPointKeyFrame(new Point(inPoint1.X, inPoint1.Y), 
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0d)), _bounce1));
            animation.KeyFrames.Add(new EasingPointKeyFrame(new Point(inPoint2.X, inPoint2.Y), 
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2d)), _bounce1));

            animation.AutoReverse = false;
            animation.FillBehavior = FillBehavior.HoldEnd;
            animation.SpeedRatio = 3;

            Storyboard.SetTargetName(animation, inElementName);
            Storyboard.SetTargetProperty(animation, inPropertyPath);

            inStoryboard.Children.Add(animation);
        }

        /// <summary>
        /// Used to move the button.
        /// </summary>
        private void Setup_KeyFrameThicknessAnimation1(Storyboard inStoryboard, Thickness inMargin1, Thickness inMargin2,
            string inElementName, PropertyPath inPropertyPath)
        {
            ThicknessAnimationUsingKeyFrames animation = new ThicknessAnimationUsingKeyFrames();

            animation.KeyFrames.Add(new EasingThicknessKeyFrame(new Thickness(inMargin1.Left, inMargin1.Top, 0, 0),
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0d)), _bounce1));
            animation.KeyFrames.Add(new EasingThicknessKeyFrame(new Thickness(inMargin2.Left, inMargin2.Top, 0, 0),
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2d)), _bounce1));

            animation.AutoReverse = false;
            animation.FillBehavior = FillBehavior.HoldEnd;
            animation.SpeedRatio = 3;

            Storyboard.SetTargetName(animation, inElementName);
            Storyboard.SetTargetProperty(animation, inPropertyPath);

            inStoryboard.Children.Add(animation);
        }

        /// <summary>
        /// Bezier's (coil) has extended up all the way. Don't need this after all.
        /// The _moveCoiledButtonStoryBoard1_Completed will continue things instead of this event.
        /// </summary>
        private void _coiledSpringStoryBoard1_Completed(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The button has moved all the way. Now rotate the button a bit so it looks crooked.
        /// </summary>
        private void _moveCoiledButtonStoryBoard1_Completed(object sender, EventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 25;
            animation.SpeedRatio = 6;
            animation.Duration = new Duration(new TimeSpan(0, 0, 1));
            animation.FillBehavior = FillBehavior.HoldEnd;
            animation.Completed += ButtonRotateAnimation_Completed;

            btnRotate.BeginAnimation(RotateTransform.AngleProperty, animation);
        }

        /// <summary>
        /// Button has rotated and looks crooked. Start the final animations.
        /// </summary>
        private void ButtonRotateAnimation_Completed(object sender, EventArgs e)
        {
            // For animating the coils / bezier curves to dangle the button.
            Storyboard _coiledSpringStoryBoard2 = new Storyboard();
            _coiledSpringStoryBoard2.Completed += _coiledSpringStoryBoard2_Completed;

            // Leave Bezier1 alone at the bottom
            // Animate btnBezier1 ===============================
            //Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
            //    new Point(btnBezier1.Point1.X, btnBezier1.Point1.Y),
            //    new Point(btnBezier1.Point1.X, btnBezier1.Point1.Y - 10),
            //    "btnBezier1", new PropertyPath(BezierSegment.Point1Property));

            //Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
            //    new Point(btnBezier1.Point2.X, btnBezier1.Point2.Y),
            //    new Point(btnBezier1.Point2.X, btnBezier1.Point2.Y - 15),
            //    "btnBezier1", new PropertyPath(BezierSegment.Point2Property));

            //Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
            //    new Point(btnBezier1.Point3.X, btnBezier1.Point3.Y),
            //    new Point(btnBezier1.Point3.X, btnBezier1.Point3.Y - 20),
            //    "btnBezier1", new PropertyPath(BezierSegment.Point3Property));

            // Animate btnBezier2 ===============================
            Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
                new Point(btnBezier2.Point1.X, btnBezier2.Point1.Y),
                new Point(btnBezier2.Point1.X - 5, btnBezier2.Point1.Y),
                "btnBezier2", new PropertyPath(BezierSegment.Point1Property));

            Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
                new Point(btnBezier2.Point2.X, btnBezier2.Point2.Y),
                new Point(btnBezier2.Point2.X - 5, btnBezier2.Point2.Y),
                "btnBezier2", new PropertyPath(BezierSegment.Point2Property));

            Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
                new Point(btnBezier2.Point3.X, btnBezier2.Point3.Y),
                new Point(btnBezier2.Point3.X - 5, btnBezier2.Point3.Y),
                "btnBezier2", new PropertyPath(BezierSegment.Point3Property));

            // Animate btnBezier3 ===============================
            Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
                new Point(btnBezier3.Point1.X, btnBezier3.Point1.Y),
                new Point(btnBezier3.Point1.X - 10, btnBezier3.Point1.Y + 10),
                "btnBezier3", new PropertyPath(BezierSegment.Point1Property));

            Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
                new Point(btnBezier3.Point2.X, btnBezier3.Point2.Y),
                new Point(btnBezier3.Point2.X - 10, btnBezier3.Point2.Y + 10),
                "btnBezier3", new PropertyPath(BezierSegment.Point2Property));

            Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
                new Point(btnBezier3.Point3.X, btnBezier3.Point3.Y),
                new Point(btnBezier3.Point3.X - 10, btnBezier3.Point3.Y + 10),
                "btnBezier3", new PropertyPath(BezierSegment.Point3Property));

            // Animate btnBezier4 ===============================
            Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
                new Point(btnBezier4.Point1.X, btnBezier4.Point1.Y),
                new Point(btnBezier4.Point1.X - 20, btnBezier4.Point1.Y + 20),
                "btnBezier4", new PropertyPath(BezierSegment.Point1Property));

            Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
                new Point(btnBezier4.Point2.X, btnBezier4.Point2.Y),
                new Point(btnBezier4.Point2.X - 20, btnBezier4.Point2.Y + 20),
                "btnBezier4", new PropertyPath(BezierSegment.Point2Property));

            Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
                new Point(btnBezier4.Point3.X, btnBezier4.Point3.Y),
                new Point(btnBezier4.Point3.X - 20, btnBezier4.Point3.Y + 20),
                "btnBezier4", new PropertyPath(BezierSegment.Point3Property));

            // Animate btnBezier5 ===============================
            Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
                new Point(btnBezier5.Point1.X, btnBezier5.Point1.Y),
                new Point(btnBezier5.Point1.X - 30, btnBezier5.Point1.Y + 30),
                "btnBezier5", new PropertyPath(BezierSegment.Point1Property));

            Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
                new Point(btnBezier5.Point2.X, btnBezier5.Point2.Y),
                new Point(btnBezier5.Point2.X - 30, btnBezier5.Point2.Y + 30),
                "btnBezier5", new PropertyPath(BezierSegment.Point2Property));

            Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
                new Point(btnBezier5.Point3.X, btnBezier5.Point3.Y),
                new Point(btnBezier5.Point3.X - 30, btnBezier5.Point3.Y + 30),
                "btnBezier5", new PropertyPath(BezierSegment.Point3Property));

            // Animate btnBezier6 ===============================
            Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
                new Point(btnBezier6.Point1.X, btnBezier6.Point1.Y),
                new Point(btnBezier6.Point1.X - 60, btnBezier6.Point1.Y + 40),
                "btnBezier6", new PropertyPath(BezierSegment.Point1Property));

            Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
                new Point(btnBezier6.Point2.X, btnBezier6.Point2.Y),
                new Point(btnBezier6.Point2.X - 20, btnBezier6.Point2.Y + 40),
                "btnBezier6", new PropertyPath(BezierSegment.Point2Property));

            Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
                new Point(btnBezier6.Point3.X, btnBezier6.Point3.Y),
                new Point(btnBezier6.Point3.X - 40, btnBezier6.Point3.Y + 40),
                "btnBezier6", new PropertyPath(BezierSegment.Point3Property));

            // Animate btnBezier7 ===============================
            Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
                new Point(btnBezier7.Point1.X, btnBezier7.Point1.Y),
                new Point(btnBezier7.Point1.X - 60, btnBezier7.Point1.Y + 80),
                "btnBezier7", new PropertyPath(BezierSegment.Point1Property));

            Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
                new Point(btnBezier7.Point2.X, btnBezier7.Point2.Y),
                new Point(btnBezier7.Point2.X - 10, btnBezier7.Point2.Y + 80),
                "btnBezier7", new PropertyPath(BezierSegment.Point2Property));

            Setup_KeyFramePointAnimation2(_coiledSpringStoryBoard2,
                new Point(btnBezier7.Point3.X, btnBezier7.Point3.Y),
                new Point(btnBezier7.Point3.X -50, btnBezier7.Point3.Y + 100),
                "btnBezier7", new PropertyPath(BezierSegment.Point3Property));

            // Start a button move animation to follow the dangling bezier curves ================================================
            Storyboard _moveCoiledButtonStoryBoard2 = new Storyboard();
            _moveCoiledButtonStoryBoard2.Completed += _moveCoiledButtonStoryBoard2_Completed;

            double _finalTop = btnCoiled.Margin.Top + 180d + (CB_Height / 2);

            Setup_KeyFrameThicknessAnimation2(_moveCoiledButtonStoryBoard2,
                new Thickness(btnCoiled.Margin.Left, btnCoiled.Margin.Top, 0, 0),
                new Thickness(btnCoiled.Margin.Left - 55, _finalTop, 0, 0),
                "btnCoiled", new PropertyPath(Button.MarginProperty));

            _coiledSpringStoryBoard2.Begin(CoiledButtonCanvas);
            _moveCoiledButtonStoryBoard2.Begin(CoiledButtonCanvas);
        }

        /// <summary>
        /// Used to move the button.
        /// </summary>
        private void Setup_KeyFrameThicknessAnimation2(Storyboard inStoryboard, Thickness inMargin1, Thickness inMargin2,
            string inElementName, PropertyPath inPropertyPath)
        {
            ThicknessAnimationUsingKeyFrames animation = new ThicknessAnimationUsingKeyFrames();

            animation.KeyFrames.Add(new EasingThicknessKeyFrame(new Thickness(inMargin1.Left, inMargin1.Top, 0, 0),
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0d)), _bounce2));
            animation.KeyFrames.Add(new EasingThicknessKeyFrame(new Thickness(inMargin2.Left, inMargin2.Top, 0, 0),
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2d)), _bounce2));

            animation.AutoReverse = false;
            animation.FillBehavior = FillBehavior.HoldEnd;
            animation.SpeedRatio = 1;

            Storyboard.SetTargetName(animation, inElementName);
            Storyboard.SetTargetProperty(animation, inPropertyPath);

            inStoryboard.Children.Add(animation);
        }

        private void _moveCoiledButtonStoryBoard2_Completed(object sender, EventArgs e)
        {
            return;
        }

        /// <summary>
        /// Used to move the coiled bezier segments.
        /// </summary>
        private void Setup_KeyFramePointAnimation2(Storyboard inStoryboard, Point inPoint1, Point inPoint2,
    string inElementName, PropertyPath inPropertyPath)
        {
            PointAnimationUsingKeyFrames animation = new PointAnimationUsingKeyFrames();

            animation.KeyFrames.Add(new EasingPointKeyFrame(new Point(inPoint1.X, inPoint1.Y),
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0d)), _bounce2));
            animation.KeyFrames.Add(new EasingPointKeyFrame(new Point(inPoint2.X, inPoint2.Y),
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(2d)), _bounce2));

            animation.AutoReverse = false;
            animation.FillBehavior = FillBehavior.HoldEnd;
            animation.SpeedRatio = 1;

            Storyboard.SetTargetName(animation, inElementName);
            Storyboard.SetTargetProperty(animation, inPropertyPath);

            inStoryboard.Children.Add(animation);
        }

        private void _coiledSpringStoryBoard2_Completed(object sender, EventArgs e)
        {
            RaiseCB_Click();
        }
        #endregion


    }
}
