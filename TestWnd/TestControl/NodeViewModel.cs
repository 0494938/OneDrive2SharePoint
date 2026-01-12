using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IsidTestApp
{
    public class SrcNodeViewModel
    {
        private bool? _isChecked = false;
        private bool _isExpanded;

        public string Name { get; set; }
        public bool IsFolder { get; set; }
        public SrcNodeViewModel Parent { get; set; }
        public ObservableCollection<SrcNodeViewModel> Children { get; set; } = new();

        // 勾选状态逻辑
        public bool? IsChecked
        {
            get => _isChecked;
            set => SetIsChecked(value, true, true);
        }

        // 展开状态逻辑 (用于更换图标)
        public bool IsExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; OnPropertyChanged(); }
        }

        private void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == _isChecked) return;
            _isChecked = value;

            // 1. 向下通知：如果当前是文件夹，更新所有子节点
            if (updateChildren && _isChecked.HasValue)
            {
                foreach (var child in Children)
                    child.SetIsChecked(_isChecked, true, false);
            }

            // 2. 向上通知：通知父节点重新计算状态
            if (updateParent && Parent != null)
            {
                Parent.VerifyCheckState();
            }

            OnPropertyChanged(nameof(IsChecked));
        }

        private void VerifyCheckState()
        {
            bool? state = null;
            if (Children.All(c => c.IsChecked == true)) state = true;
            else if (Children.All(c => c.IsChecked == false)) state = false;

            SetIsChecked(state, false, true);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public enum ProcessStatus { Pending, Processing, Completed }

    public class DstNodeViewModel : INotifyPropertyChanged
    {
        private ProcessStatus _status;
        private bool _isExpanded;
        public string Name { get; set; }
        public bool IsFolder { get; set; } // 区分文件夹或文件
        public DstNodeViewModel Parent { get; set; }
        public ObservableCollection<DstNodeViewModel> Children { get; set; } = new();

        public bool IsExpanded
        {
            get => _isExpanded;
            set { _isExpanded = value; OnPropertyChanged(); }
        }
        public ProcessStatus Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
