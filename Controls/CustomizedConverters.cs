using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using Point = System.Windows.Point;

namespace GcjUiCtrl.Control
{
    public class MarginCustomizedConverterForIsidTestControlWindow : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 3 || values[0] == DependencyProperty.UnsetValue)
                return new Thickness(0);

            // Extract the values
            double grdLeftWidth = (double)values[0];//grdLeft.Width
            double grdWebBrowserHeight = (double)values[1];//grdWebBrowser.Height
            double grdTitleHeight = (double)values[2];//grdWebBrowser.Height
            double nLeftShift = -10;// (double)values[3];
            double nHeight = 0;// (double)values[4];

            if (grdLeftWidth != 0 && grdWebBrowserHeight != 0 && grdTitleHeight != 0)
            {
                // Custom calculations for each side of the margin
                double left = -grdLeftWidth + nLeftShift;
                double top = -grdWebBrowserHeight - grdTitleHeight+nHeight;
                double right = grdLeftWidth - nLeftShift;
                double bottom = grdWebBrowserHeight + grdTitleHeight- nHeight;

                return new Thickness(left, top, right, bottom);
            }
            else
            {
                return new Thickness(0);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MarginCustomizedConverterForIsidTestControlWindowCtrl : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 3 || values[0] == DependencyProperty.UnsetValue)
                return new Thickness(0);

            // Extract the values
            double grdLeftWidth = (double)values[0];//grdLeft.Width
            double grdWebBrowserHeight = (double)values[1];//grdWebBrowser.Height
            double grdTitleHeight = (double)values[2];//grdWebBrowser.Height
            double nLeftShift = -10;// (double)values[3];
            double nHeight = 0;// (double)values[4];

            if (grdLeftWidth != 0 && grdWebBrowserHeight != 0 && grdTitleHeight != 0)
            {
                // Custom calculations for each side of the margin
                double left = -grdLeftWidth + nLeftShift;
                double top = -grdWebBrowserHeight - grdTitleHeight + nHeight;
                double right = grdLeftWidth - nLeftShift;
                double bottom = grdWebBrowserHeight + grdTitleHeight - nHeight;

                return new Thickness(left, top, right, bottom);
            }
            else
            {
                return new Thickness(0);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MarginConverter1Ctrl : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || !(values[0] is double width) || !(values[1] is double height))
                return new Thickness(0);

            // 计算每个边距的值
            double left = width * 0.1;   // 示例：左边距是宽度的 10%
            double top = height * 0.2;   // 示例：上边距是高度的 20%
            double right = width * 0.15; // 示例：右边距是宽度的 15%
            double bottom = height * 0.25; // 示例：下边距是高度的 25%

            return new Thickness(left, top, right, bottom);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MarginConverter2Ctrl : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 4 || values[0] == DependencyProperty.UnsetValue)
                return new Thickness(0);

            // Extract the values
            double control1Width = (double)values[0];
            double control1Height = (double)values[1];
            double control2Width = (double)values[2];
            double control2Height = (double)values[3];

            // Custom calculations for each side of the margin
            double left = CalculateLeftMargin(control1Width, control2Width);
            double top = CalculateTopMargin(control1Height, control2Height);
            double right = CalculateRightMargin(control1Width, control2Width);
            double bottom = CalculateBottomMargin(control1Height, control2Height);

            return new Thickness(left, top, right, bottom);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private double CalculateLeftMargin(double width1, double width2)
        {
            // Implement custom logic for left margin
            return width1 * 0.5 + width2 * 0.25;
        }

        private double CalculateTopMargin(double height1, double height2)
        {
            // Implement custom logic for top margin
            return height1 * 0.3 + height2 * 0.2;
        }

        private double CalculateRightMargin(double width1, double width2)
        {
            // Implement custom logic for right margin
            return width1 * 0.4 + width2 * 0.3;
        }

        private double CalculateBottomMargin(double height1, double height2)
        {
            // Implement custom logic for bottom margin
            return height1 * 0.5 + height2 * 0.1;
        }
    }

    public class RectConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] is double width && values[1] is double height)
            {
                // Create a Rect object based on the ActualWidth and ActualHeight
                return new Rect(0, 0, width, height);
            }
            return new Rect(0, 0, 0, 0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HalfValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double originalValue)
            {
                return originalValue / 2;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PointConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is double width && values[1] is double height)
            {
                return new Point(width / 2, height / 2); // 计算中心点
            }
            return new Point(0, 0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class GreaterThanZeroConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                return d > 0;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /*
    <YourControl.Visibility>
        <MultiBinding Converter="{StaticResource BooleanToVisibilityMultiConverter}">
            <Binding Path="FileRecentShowAll" Source="{StaticResource WindowContext}"/>
            <Binding Path="HasValue" RelativeSource="{RelativeSource Self}"/>
        </MultiBinding>
    </YourControl.Visibility>
    */

    public class BooleanToVisibilityMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is bool fileRecentShowAll && values[1] is bool hasValue)
            {
                // 当两个属性都为 true 时，显示控件
                return (fileRecentShowAll && hasValue) ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    /*
    <Grid Grid.Row="1" Visibility="{Binding Path=UserInfo,Mode=OneWay,Converter={StaticResource UserInfoToVisibilityConverter}}" >
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="40"/>
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>
        <local:CustomizedRoundImage Grid.Column="0" x:Name="imgUser" Height="40" Source="{Binding Path=UserImageSource, RelativeSource={RelativeSource AncestorType=UserControl}}" />
        <TextBlock Grid.Column="0" x:Name="txtUserInfo" Text="User Info..."/>
    </Grid>
     */
    public class UserInfoToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 检查 value 是否为字符串
            if (value is string userInfo)
            {
                // 判断字符串是否为有效的电子邮件地址 或者改为需要的check 逻辑
                if (!string.IsNullOrWhiteSpace(userInfo) && IsValidEmail(userInfo))
                {
                    return Visibility.Visible;
                }
            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        // 检查字符串是否为有效的电子邮件地址
        private bool IsValidEmail(string email)
        {
            // 使用正则表达式验证电子邮件地址
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
    }

    /*
    <YourControl>
        <YourControl.Visibility>
            <MultiBinding Converter="{StaticResource MultiConditionToVisibilityConverter}" ConverterParameter="1">
                <!-- 绑定到 WindowContext 的 FileRecentShowAll -->
                <Binding Path="FileRecentShowAll" Source="{StaticResource WindowContext}"/>
                <!-- 绑定到 Control 的 HasValue 属性 -->
                <Binding Path="HasValue" RelativeSource="{RelativeSource Self}"/>
            </MultiBinding>
        </YourControl.Visibility>
    </YourControl>

    <!-- 另一个控件，使用不同的 LogicIndex -->
    <YourControl>
        <YourControl.Visibility>
            <MultiBinding Converter="{StaticResource MultiConditionToVisibilityConverter}" ConverterParameter="2">
                <!-- 绑定到 WindowContext 的 FileRecentShowAll -->
                <Binding Path="FileRecentShowAll" Source="{StaticResource WindowContext}"/>
                <!-- 绑定到 Control 的 HasValue 属性 -->
                <Binding Path="HasValue" RelativeSource="{RelativeSource Self}"/>
            </MultiBinding>
        </YourControl.Visibility>
    </YourControl>
     */
    public class MultiConditionToVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // 第一个和第二个参数是两个 bool 类型的属性
            if (values.Length >= 3 && values[0] is bool fileRecentShowAll && values[1] is bool hasValue && values[2] is int logicIndex)
            {

                // 例如，如果两个布尔值都为 true 且 logicIndex 是某个特定值时，显示控件
                if (fileRecentShowAll && hasValue /*&& logicIndex == (int)parameter*/)
                {
                    return Visibility.Visible;
                }
            }
            else if (values.Length >= 2 && values[0] is bool fileRecentShowAll2 && values[1] is bool hasValue2 && parameter is string sParam)
            {

                // 例如，如果两个布尔值都为 true 且 logicIndex 是某个特定值时，显示控件
                if (hasValue2 )
                {
                    bool isInteger = int.TryParse(sParam, out int nConverterParameter);

                    if (isInteger && nConverterParameter >=1 && nConverterParameter <=4)
                    {
                        //Debug.WriteLine($"{sParam} 是一个整数，值为 {nConverterParameter}");
                        return Visibility.Visible;
                    }
                    else
                    {
                        //Debug.WriteLine($"{input} 不是一个有效的整数");
                    }
                }
            }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
