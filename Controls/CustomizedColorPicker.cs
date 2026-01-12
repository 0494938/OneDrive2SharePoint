using BaseUtil;
using GcjUtil;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using static GcjUtil.DepUtil;
using Color = System.Windows.Media.Color;
using HorizontalAlignment = System.Windows.HorizontalAlignment;

namespace GcjUiCtrl.Control
{
    public partial class CustomizedColorPicker : Xceed.Wpf.Toolkit.ColorPicker, IColorPickerIdentfier
    {
        ToggleButton? PART_ColorPickerToggleButton = null;
        Xceed.Wpf.Toolkit.Chromes.ButtonChrome? btnDropDownArraow = null;
        ContentControl? contentCtrlColorOnly = null;
        Border? borderColorAndName = null;
        Grid? gridColorOnlyColorAndNameParent = null;
        TextBlock? lblColorText = null;

        public CustomizedColorPicker() : base()
        {
            //DefaultStyleKey = typeof(CustomizedColorPicker);
            this.Loaded += CustomizedInHeridColorPicker_Loaded;
            this.PreviewMouseLeftButtonDown += CustomizedInHeridColorPicker_PreviewMouseLeftButtonDown;
            this.SelectedColorChanged += CustomizedInHeridColorPicker_SelectedColorChanged;
        }

        private void CustomizedInHeridColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Window? topWnd = DepUtil.FindParent<Window>(this);
            Color newSelectedColor = (Color)e.NewValue;
     
            Color newForeGroundColor = CtrlUtil.GetWhiteBlackForeGroundColor(newSelectedColor);
            if (topWnd != null)
            {
                System.Collections.Generic.List<System.Windows.Controls.Control> ctrls = DepUtil.GetAllChildrenOfTypeInterface<System.Windows.Controls.Control, IColorPickerIdentfier>(topWnd);
                foreach (System.Windows.Controls.Control ctl in ctrls)
                {
                    if (ctl is IColorPickerIdentfier )
                    {
                        if(sender != ctl)
                            (ctl as IColorPickerIdentfier)?.ColorBindingExpression()?.UpdateTarget();

                        if (ctl is CustomizedColorPicker)
                            this.TextForeground = new SolidColorBrush(newForeGroundColor);
                        else
                            ((System.Windows.Controls.Control)ctl).Foreground = new SolidColorBrush(newForeGroundColor);
                    }
                }
                if (lblColorText != null)
                {
                    lblColorText.Foreground = new SolidColorBrush(newForeGroundColor);
                    lblColorText.GetBindingExpression(TextBlock.ForegroundProperty)?.UpdateTarget();
                }
            }
        }

        private void CustomizedInHeridColorPicker_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            {
                Debug.WriteLine("$$$$ OnDebugPreviewMouseLeftButtonDown(" + sender?.GetType()?.Name + ":" + (sender as FrameworkElement)?.Name + ", " + e.OriginalSource?.GetType()?.Name + ":" + (e.OriginalSource as FrameworkElement)?.Name + ") $$$$");
                DepUtil.DumpObjectStructure(this, 0, (int)DUMP_LEVEL.DUMP_ALIGNMENT_SIZE_POSITION_DUMP_MARGIN);
            }
        }

        System.Windows.Shapes.Path? arrowShap = null;
        const int WidthDiffHalf = 2;
        private void CustomizedInHeridColorPicker_Loaded(object sender, RoutedEventArgs e)
        {
            //ControlUtil.GetControlTemplateAsPrettyXaml(this);  //test for ControlTemplate Output.
            // 使用 Template.FindName 从模板中获取子控件
            PART_ColorPickerToggleButton = (ToggleButton)this.Template.FindName("PART_ColorPickerToggleButton", this);
            btnDropDownArraow = (Xceed.Wpf.Toolkit.Chromes.ButtonChrome)this.Template.FindName("ToggleButtonChrome", this);
            contentCtrlColorOnly = (ContentControl)this.Template.FindName("ColorOnly", this);
            borderColorAndName = (Border)this.Template.FindName("ColorAndName", this);
            if (PART_ColorPickerToggleButton != null)
            {
                if (this.Height != double.NaN && this.Height < 22) {
                    PART_ColorPickerToggleButton.MinHeight= this.Height;
                    PART_ColorPickerToggleButton.Height = this.Height;
                    //PART_ColorPickerToggleButton.Width = this.Height;
                    arrowShap = DepUtil.GetFirstChildrenOfType<System.Windows.Shapes.Path>(PART_ColorPickerToggleButton);
                    if(arrowShap != null)
                    {
                        arrowShap.Width = arrowShap.Height+ WidthDiffHalf+ WidthDiffHalf;
                        arrowShap.Margin = new Thickness(arrowShap.Margin.Left - (22 - this.Height) / 2+ WidthDiffHalf, arrowShap.Margin.Top - (22- this.Height)/2, arrowShap.Margin.Right - (22 - this.Height) / 2+ WidthDiffHalf, arrowShap.Margin.Bottom-(22 - this.Height) / 2);
                    }
                }
                if (gridColorOnlyColorAndNameParent == null && borderColorAndName != null)
                    gridColorOnlyColorAndNameParent = borderColorAndName.Parent as Grid;
                if (gridColorOnlyColorAndNameParent == null && contentCtrlColorOnly != null)
                    gridColorOnlyColorAndNameParent = contentCtrlColorOnly?.Parent as Grid;
                if (gridColorOnlyColorAndNameParent != null)
                {
                    gridColorOnlyColorAndNameParent.Margin = new Thickness(0, 0, 0, 0);
                    ContentPresenter? ColorOnlyContentPresendter = VisualTreeHelper.GetParent(gridColorOnlyColorAndNameParent) as ContentPresenter;
                    if (ColorOnlyContentPresendter != null)
                    {
                        Debug.Assert(true);
                        ColorOnlyContentPresendter.VerticalAlignment = VerticalAlignment.Stretch;
                    }
                    else
                    {
                        Debug.Assert(false);
                    }
                    if (this.DisplayColorAndName == true && borderColorAndName != null)
                    {
                    }
                }
                else { Debug.Assert(false); }

                if (borderColorAndName != null && this.ShowColorAndNameStreched == true && this.DisplayColorAndName == true)
                {
                    StackPanel? parentStackPanel = DepUtil.GetFirstChildrenOfType<StackPanel>(borderColorAndName);
                    if (parentStackPanel != null)
                    {
                        ContentControl? backgroundControl = DepUtil.GetFirstChildrenOfType<ContentControl>(parentStackPanel);
                        lblColorText = DepUtil.GetFirstChildrenOfType<TextBlock>(parentStackPanel);
                        if (backgroundControl != null && lblColorText != null)
                        {
                            // 查找父容器的索引
                            //Border parent = parentPanel.Parent as Border; it's ColorAndName
                            Grid gridPanel = new Grid();

                            // 将 Border 的 Child（StackPanel）替换为新的 Grid
                            borderColorAndName.Child = gridPanel;
                            parentStackPanel.Children.Remove(backgroundControl);
                            parentStackPanel.Children.Remove(lblColorText);

                            // 将控件添加到 Grid 中
                            gridPanel.Children.Add(backgroundControl);
                            gridPanel.Children.Add(lblColorText);

                            // 动态调整控件的布局，确保重叠
                            backgroundControl.Margin = new Thickness(0);
                            backgroundControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                            backgroundControl.VerticalAlignment = VerticalAlignment.Stretch;
                            backgroundControl.Width = double.NaN;
                            backgroundControl.Height = double.NaN;
                            lblColorText.HorizontalAlignment = HorizontalAlignment.Center;
                            lblColorText.VerticalAlignment = VerticalAlignment.Center;
                            lblColorText.Foreground = this.TextForeground;

                            // 设置 ZIndex，确保 TextBlock 在前面
                            Grid.SetZIndex(backgroundControl, 0);
                            Grid.SetZIndex(lblColorText, 1);
                        }
                        else { Debug.Assert(false); }

                    }
                    else { Debug.Assert(false); }
                }
            }
            else{ 
                //Debug.Assert(false); 
            }

            Color newSelectedColor = (Color)this.SelectedColor;
            if (newSelectedColor != null)
            {
                Color newForeGroundColor = CtrlUtil.GetWhiteBlackForeGroundColor(newSelectedColor);
                if (lblColorText != null)
                {
                    lblColorText.Foreground = new SolidColorBrush(newForeGroundColor);
                    lblColorText.GetBindingExpression(TextBlock.ForegroundProperty)?.UpdateTarget();
                }
            }
        }

        public BindingExpression? ColorBindingExpression()
        {
            return GetBindingExpression(CustomizedColorPicker.SelectedColorProperty);
        }
       
    }
}
