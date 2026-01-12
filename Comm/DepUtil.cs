using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GcjUtil
{
    public partial class DepUtil
    {
        public static void ReflectSetProp(object? obj, string propName, object? objNewValue)
        {
            if (obj != null)
            {
                Type? type = obj?.GetType();
                if (type != null)
                {
                    //var fieldInfo = type.GetField("<MyProperty>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                    FieldInfo? fieldInfo = type?.GetField("<" + propName + ">k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                    while(fieldInfo == null && type != null)
                    {
                        type = type.BaseType;
                        fieldInfo = type?.GetField("<" + propName + ">k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                    }

                    if (fieldInfo != null)
                    {
                        // Set the backing field to a new value
                        fieldInfo.SetValue(obj, objNewValue);
                    }
                }
            }
        }

        public static void ReflectSetPropExact(object? obj, string propName, object? objNewValue)
        {
            if (obj != null)
            {
                Type? type = obj?.GetType();
                if (type != null)
                {
                    //var fieldInfo = type.GetField("<MyProperty>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                    Debug.WriteLine($"--  {type.FullName}");
                    foreach (FieldInfo? field in (type?.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)))
                    {
                        Debug.WriteLine($"      {field.Name}");
                    }
                    FieldInfo? fieldInfo = type?.GetField(propName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty | BindingFlags.Public);
                    while (fieldInfo == null && type != null && type.FullName!= "System.Object")
                    {
                        if(type.BaseType == null) Debug.Assert(true);
                        type = type.BaseType;
                        if (type != null)
                        {
                            foreach (FieldInfo? field in (type?.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)))
                            {
                                Debug.WriteLine($"      {field.Name}");
                            }
                            fieldInfo = type?.GetField(propName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty | BindingFlags.PutRefDispProperty | BindingFlags.Public);
                        }
                    }

                    if (fieldInfo != null)
                    {
                        // Set the backing field to a new value
                        object? value = fieldInfo.GetValue(obj);
                        fieldInfo.SetValue(obj, objNewValue);
                        Debug.WriteLine($"Change {obj?.GetType()?.FullName}<{obj?.ToString()}> , change from {value?.ToString()} => {objNewValue?.ToString()}");
                    }
                }
            }
        }
        public static object? ReflectGetPropExact(object? obj, string propName, object? objNewValue)
        {
            if (obj != null)
            {
                Type? type = obj?.GetType();
                if (type != null)
                {
                    //var fieldInfo = type.GetField("<MyProperty>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
                    FieldInfo? fieldInfo = type?.GetField(propName, BindingFlags.Instance | BindingFlags.NonPublic);
                    while (fieldInfo == null && type != null)
                    {
                        type = type.BaseType;
                        fieldInfo = type?.GetField(propName, BindingFlags.Instance | BindingFlags.NonPublic);
                    }

                    if (fieldInfo != null)
                    {
                        // Set the backing field to a new value
                        object? value = fieldInfo.GetValue(obj);
                        return value;
                    }
                }
            }
            return null;
        }

        public static bool IsDescendantOf(DependencyObject? child, DependencyObject? potentialParent, bool bIncludeSelf=true)
        {
            if (bIncludeSelf == false && child == potentialParent)
                return false;

            // 从子元素开始，沿视觉树向上查找
            while (child != null)
            {
                if (child == potentialParent)
                {
                    return true; // 找到匹配的父元素
                }

                // 移动到父元素
                child = VisualTreeHelper.GetParent(child);
            }

            // 到达根元素但没有找到匹配的父元素
            return false;
        }

        // 递归遍历 Visual Tree 递归获取所有指定类型的控件
        public static List<T> GetAllChildrenOfType<T>(DependencyObject parent) where T : DependencyObject
        {
            List<T> result = new List<T>();

            // 子控件数量
            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childCount; i++)
            {
                // 获取当前子控件
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                // 如果子控件是目标类型，添加到结果列表
                if (child is T)
                {
                    result.Add((T)child);
                }

                // 递归查找该子控件的子控件
                result.AddRange(GetAllChildrenOfType<T>(child));
            }

            return result;
        }

        // 递归遍历 Visual Tree 递归获取所有指定类型的控件
        public static List<T> GetDirectChildrenOfType<T>(DependencyObject parent) where T : DependencyObject
        {
            List<T> result = new List<T>();

            // 子控件数量
            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childCount; i++)
            {
                // 获取当前子控件
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                // 如果子控件是目标类型，添加到结果列表
                if (child is T)
                {
                    result.Add((T)child);
                }
            }

            return result;
        }

        public static List<UIElement> GetDirectChildrenExceptType<T>(DependencyObject parent) where T : DependencyObject
        {
            List<UIElement> result = new List<UIElement>();

            // 子控件数量
            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childCount; i++)
            {
                // 获取当前子控件
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                // 如果子控件是目标类型，添加到结果列表
                if (!(child is T))
                {
                    result.Add((UIElement)child);
                }
            }

            return result;
        }

        public static List<T> GetAllChildrenOfTypeInterface<T, TInterface>(DependencyObject parent) where T : DependencyObject
        {
            List<T> result = new List<T>();

            // 子控件数量
            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childCount; i++)
            {
                // 获取当前子控件
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                // 如果子控件是目标类型，添加到结果列表
                if (child is TInterface)
                {
                    result.Add((T)child);
                }

                // 递归查找该子控件的子控件
                result.AddRange(GetAllChildrenOfTypeInterface<T, TInterface>(child));
            }

            return result;
        }
        
        public static T? GetFirstChildrenOfType<T>(DependencyObject parent) where T : DependencyObject  //离得更近的孩子优先
        {
            T ? result = null;

            // 子控件数量
            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childCount; i++)
            {
                // 获取当前子控件
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                // 如果子控件是目标类型，添加到结果列表
                if (child is T)
                {
                    return (T)child;
                }

            }
            // 递归查找该子控件的子控件
            for (int i = 0; i < childCount; i++)
            {
                // 获取当前子控件
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                result = GetFirstChildrenOfType<T>(child);
                if (result!=null && result is T)
                {
                    return (T)result;
                }
            }
            return result;
        }

        public static T? FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            if (child == null)
                return (T?)null;
            // 获取父级对象
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            // 如果没有父级对象，返回 null
            if (parentObject == null) return null;

            // 如果父级对象是目标类型，返回该对象
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }

            // 递归查找上一级父对象
            return FindParent<T>(parentObject);
        }

        public static T? FindParent<T,T1,T2>(DependencyObject child) where T : DependencyObject //T1, T2 is subclass of T
        {
            // 获取父级对象
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            // 如果没有父级对象，返回 null
            if (parentObject == null) return null;

            // 如果父级对象是目标类型，返回该对象
            T parent = parentObject as T;
            if (parent != null && (parent is T1 || parent is T2))
            {
                return parent;
            }

            // 递归查找上一级父对象
            return FindParent<T,T1,T2>(parentObject);
        }

        public static List<T> GetAllChildrenOfType<T,T1,T2>(DependencyObject parent) where T : DependencyObject //T1, T2 is subclass of T.
        {
            List<T> result = new List<T>();

            // 子控件数量
            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childCount; i++)
            {
                // 获取当前子控件
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                // 如果子控件是目标类型，添加到结果列表
                if (child is T && (child is T1 || child is T2))
                {
                    result.Add((T)child);
                }

                // 递归查找该子控件的子控件
                result.AddRange(GetAllChildrenOfType<T,T1,T2>(child));
            }

            return result;
        }
        public static List<T> GetAllChildrenOfType<T, T1, T2,T3>(DependencyObject parent) where T : DependencyObject //T1, T2 is subclass of T.
        {
            List<T> result = new List<T>();

            // 子控件数量
            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childCount; i++)
            {
                // 获取当前子控件
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                // 如果子控件是目标类型，添加到结果列表
                if (child is T && (child is T1 || child is T2 || child is T3))
                {
                    result.Add((T)child);
                }

                // 递归查找该子控件的子控件
                result.AddRange(GetAllChildrenOfType<T, T1, T2>(child));
            }

            return result;
        }
        public static void DumpObjectStructure(DependencyObject obj, int nDepth = 0, int nDumpLevel=0)
        {
            if (obj == null)
                return;

            if (nDepth == 0)
                Debug.WriteLine("** dunmp object(" + obj.GetType().FullName + ", " + (obj is FrameworkElement ? (obj as FrameworkElement)?.Name : "#NoNM") + ") **");
            else
                Debug.WriteLine("".PadRight(nDepth << 2) + obj.GetType().FullName + ", " + (obj is FrameworkElement ? (obj as FrameworkElement)?.Name : "#NoNM"));

            if(nDumpLevel >0 ) GetPropAndEvents(obj, nDepth + 1, nDumpLevel);

            // 从子元素开始，沿视觉树向下查找
            int nChildCnt = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < nChildCnt; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                DumpObjectStructure(child, nDepth + 1, nDumpLevel);
            }
        }

        /** nDumpLevel: 0, 1, ... 9
         * 0x0100: all event handle module will be dumped
         */
        public enum DUMP_LEVEL
        {
            DUMP_NONE =0x0,
            DUMP_SIZE = 0x01,
            DUMP_POSITION = 0x02,
            DUMP_MARGIN = 0x04,
            DUMP_ALIGNMENT = 0x08,
            DUMP_ALIGNMENT_SIZE_POSITION = DUMP_ALIGNMENT + DUMP_SIZE + DUMP_POSITION,
            DUMP_ALIGNMENT_SIZE_POSITION_DUMP_MARGIN = DUMP_ALIGNMENT + DUMP_SIZE + DUMP_POSITION+ DUMP_MARGIN,
            DUMP_ALL_PROP = 0x080,
            DUMP_EVENTHANDLE = 0x100,

        }
        public static string GetPropAndEvents(DependencyObject obj, int nDepth, int nDumpLevel = 0)
        {
            if (obj == null)
                return "";
            //StringBuilder sb = new StringBuilder();
            //int nPropDumped = 0;
            foreach (PropertyInfo prop in obj.GetType().GetProperties()) { 
                if(prop.GetValue(obj) != null)
                {
                    //if (nPropDumped != 0)
                    //    sb.Append(", ");
                    //sb.Append("["+prop.Name+"="+ prop.GetValue(obj)?.ToString()+"]");
                    //nPropDumped++;
                    bool bPrintProp = false;
                    if ((nDumpLevel & (int)DUMP_LEVEL.DUMP_ALL_PROP) == (int)DUMP_LEVEL.DUMP_ALL_PROP)
                        bPrintProp = true;
                    if ((nDumpLevel & (int)DUMP_LEVEL.DUMP_ALIGNMENT) == (int)DUMP_LEVEL.DUMP_ALIGNMENT && prop.Name.Contains("Alignment"))
                        bPrintProp = true;
                    if ((nDumpLevel & (int)DUMP_LEVEL.DUMP_POSITION) == (int)DUMP_LEVEL.DUMP_POSITION && (
                        prop.Name.Equals("Top",StringComparison.CurrentCultureIgnoreCase) || prop.Name.Equals("Left", StringComparison.CurrentCultureIgnoreCase)
                        || prop.Name.Equals("Bottom", StringComparison.CurrentCultureIgnoreCase) || prop.Name.Equals("Right", StringComparison.CurrentCultureIgnoreCase)
                        || prop.Name.Equals("Row", StringComparison.CurrentCultureIgnoreCase) || prop.Name.Equals("Column", StringComparison.CurrentCultureIgnoreCase)
                        || prop.Name.Equals("RowSpan", StringComparison.CurrentCultureIgnoreCase) || prop.Name.Equals("ColumnSpan", StringComparison.CurrentCultureIgnoreCase)))
                        bPrintProp = true;
                    if ((nDumpLevel & (int)DUMP_LEVEL.DUMP_SIZE) == (int)DUMP_LEVEL.DUMP_SIZE && (
                        prop.Name.Equals("Width", StringComparison.CurrentCultureIgnoreCase) || prop.Name.Equals("Height", StringComparison.CurrentCultureIgnoreCase)
                        || prop.Name.Equals("ColumnDefinitions", StringComparison.CurrentCultureIgnoreCase) || prop.Name.Equals("RowDefinitions", StringComparison.CurrentCultureIgnoreCase)))
                        bPrintProp = true;
                    if ((nDumpLevel & (int)DUMP_LEVEL.DUMP_MARGIN) == (int)DUMP_LEVEL.DUMP_MARGIN && prop.Name.Contains("Margin"))
                        bPrintProp = true;
                    if (bPrintProp)
                    {
                        string ?sPropValue = null;
                        if (prop.Name.Equals("ColumnDefinitions", StringComparison.CurrentCultureIgnoreCase))
                            sPropValue = (prop.GetValue(obj) as ColumnDefinitionCollection)?.Count().ToString();
                        else if (prop.Name.Equals("RowDefinitions", StringComparison.CurrentCultureIgnoreCase))
                            sPropValue = (prop.GetValue(obj) as RowDefinitionCollection)?.Count().ToString();
                        else 
                            sPropValue  = prop.GetValue(obj)?.ToString();
                        Debug.WriteLine("".PadRight(nDepth << 2) + "-> " + "[" + prop.Name + "=" + sPropValue + "]");
                    }
                }
            }
            Type type = obj.GetType();

#if false
            foreach (EventInfo evt in type.GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                Debug.WriteLine($"Event: {evt.Name}");

                // 获取事件的添加方法
                MethodInfo addMethod = evt.GetAddMethod();
                if (addMethod != null)
                {
                    // 获取事件的委托实例
                    Delegate[] eventDelegate = (Delegate[])addMethod.Invoke(obj, null);

                    if (eventDelegate != null)
                    {
                        foreach (Delegate handler in eventDelegate)
                        {
                            // 获取处理程序方法的声明类和名称
                            MethodInfo method = handler.Method;

                            if (nPropDumped == 0)
                                sb.Append("[" + evt.Name + "=" + (handler.Target + "!" + method.Module.Name + "." + method.Name) + "]");
                            else
                                sb.Append(",").Append("[" + evt.Name + "=" + (handler.Target + "!" + method.Module.Name + "." + method.Name) + "]");
                            nPropDumped++;

                            //Console.WriteLine($"Handler Method: {method.Name}");
                            //Console.WriteLine($"Module: {method.Module.Name}");
                            //Console.WriteLine($"Target: {handler.Target}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No handlers attached.");
                    }
                }
            }
#endif
#if false
            // 查找存储事件处理程序的方法
            MethodInfo getEventHandlersMethod = typeof(UIElement).GetMethod("GetRoutedEventHandlers", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.SetProperty );

            if (getEventHandlersMethod == null)
            {
                Debug.WriteLine("Unable to find GetRoutedEventHandlers method.");
            }

            foreach (RoutedEvent routedEvent in EventManager.GetRoutedEvents())
            {
                // 调用 GetRoutedEventHandlers 方法，获取所有处理程序
                var handlers = (RoutedEventHandlerInfo[])getEventHandlersMethod.Invoke(obj, new object[] { routedEvent });

                if (handlers != null && handlers.Length > 0)
                {
                    Console.WriteLine($"RoutedEvent: {routedEvent.Name}");

                    foreach (var handlerInfo in handlers)
                    {
                        var handler = handlerInfo.Handler;

                        Console.WriteLine($"Handler: {handler.Method.Name}, Target: {handler.Target}, Module: {handler.Method.Module.Name}");
                    }
                }
            }
#endif

#if true

            /* //https://referencesource.microsoft.com/#PresentationCore/Core/CSharp/system/windows/EventHandlersStore.cs,30
             * public RoutedEventHandlerInfo[] GetRoutedEventHandlers(RoutedEvent routedEvent)
             * internal FrugalObjectList<RoutedEventHandlerInfo> this[RoutedEvent key]
             * public void Remove(EventPrivateKey key, Delegate handler)
             * internal Delegate this[EventPrivateKey key]
             * public Delegate Get(EventPrivateKey key)
             * private FrugalMap _entries; // Map of EventPrivateKey/RoutedEvent to Delegate/FrugalObjectList<RoutedEventHandlerInfo> (respectively)
             **/
            if(EventHandlersStoreType==null)
                EventHandlersStoreType = type.GetProperty("EventHandlersStore", BindingFlags.Instance | BindingFlags.NonPublic);
            
            var eventHandlersStore = EventHandlersStoreType?.GetValue(obj, null);

            if(eventHandlersStore!= null)
            {
                // Get the store's type ...
                if (storeType == null) storeType = eventHandlersStore?.GetType();

                //get private property _entries
                if (_entriesField == null)
                    _entriesField = storeType?.GetField("_entries", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                var _entries = _entriesField?.GetValue(eventHandlersStore);
                #region FrugalMapImpl
                /**
                 * 
                    internal struct FrugalMap
                    {
                        public object this[int key]
                        {
                            get
                            {
                                // If no entry, DependencyProperty.UnsetValue is returned
                                if (null != _mapStore)
                                {
                                    return _mapStore.Search(key);
                                }
                                return DependencyProperty.UnsetValue;
                            }
     
                            set
                            {
                                if (value != DependencyProperty.UnsetValue)
                                {
                                    // If not unset value, ensure write success
                                    if (null != _mapStore)
                                    {
                                        // This is done because forward branches
                                        // default prediction is not to be taken
                                        // making this a CPU win because set is
                                        // a common operation.
                                    }
                                    else
                                    {
                                        _mapStore = new SingleObjectMap();
                                    }
     
                                    FrugalMapStoreState myState = _mapStore.InsertEntry(key, value);
                                    if (FrugalMapStoreState.Success == myState)
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        // Need to move to a more complex storage
                                        FrugalMapBase newStore;
     
                                        if (FrugalMapStoreState.ThreeObjectMap == myState)
                                        {
                                            newStore = new ThreeObjectMap();
                                        }
                                        else if (FrugalMapStoreState.SixObjectMap == myState)
                                        {
                                            newStore = new SixObjectMap();
                                        }
                                        else if (FrugalMapStoreState.Array == myState)
                                        {
                                            newStore = new ArrayObjectMap();
                                        }
                                        else if (FrugalMapStoreState.SortedArray == myState)
                                        {
                                            newStore = new SortedObjectMap();
                                        }
                                        else if (FrugalMapStoreState.Hashtable == myState)
                                        {
                                            newStore = new HashObjectMap();
                                        }
                                        else
                                        {
                                            throw new InvalidOperationException(SR.Get(SRID.FrugalMap_CannotPromoteBeyondHashtable));
                                        }
     
                                        // Extract the values from the old store and insert them into the new store
                                        _mapStore.Promote(newStore);
     
                                        // Insert the new value
                                        _mapStore = newStore;
                                        _mapStore.InsertEntry(key, value);
                                    }
                                }
                                else
                                {
                                    // DependencyProperty.UnsetValue means remove the value
                                    if (null != _mapStore)
                                    {
                                        _mapStore.RemoveEntry(key);
                                        if (_mapStore.Count == 0)
                                        {
                                            // Map Store is now empty ... throw it away
                                            _mapStore = null;
                                        }
                                    }
                                }
                            }
                        }
     
                        public void Sort()
                        {
                            if (null != _mapStore)
                            {
                                _mapStore.Sort();
                            }
                        }
     
                        public void GetKeyValuePair(int index, out int key, out Object value)
                        {
                            if (null != _mapStore)
                            {
                                _mapStore.GetKeyValuePair(index, out key, out value);
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException("index");
                            }
                        }
            
                        public void Iterate(ArrayList list, FrugalMapIterationCallback callback)
                        {
                            if (null != callback)
                            {
                                if (null != list)
                                {
                                    if (_mapStore != null)
                                    {
                                        _mapStore.Iterate(list, callback);
                                    }
                                }
                                else
                                {
                                    throw new ArgumentNullException("list");
                                }
                            }
                            else
                            {
                                throw new ArgumentNullException("callback");
                            }
                        }
     
                        public int Count
                        {
                            get
                            {
                                if (null != _mapStore)
                                {
                                    return _mapStore.Count;
                                }
                                return 0;
                            }
                        }
     
                        internal FrugalMapBase _mapStore;
                    }
     
                 * 
                 **/
                #endregion FrugalMapImpl
                //public void GetKeyValuePair(int index, out int key, out Object value)
                //public int Count
                if (_entries != null)
                {
                    if (_FrugalMapCountField == null)
                        _FrugalMapCountField = _entries.GetType().GetProperty("Count", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    int _FrugalMapCount = (int)_FrugalMapCountField.GetValue(_entries);
                    if (GetKeyValuePairMethod == null)
                        GetKeyValuePairMethod = _entries.GetType().GetMethod("GetKeyValuePair", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    for (int i = 0; i < _FrugalMapCount; i++)
                    {
                        object?[] parameters = new object[] { i, 0, null };
                        GetKeyValuePairMethod.Invoke(_entries, parameters);
                        int key = (int)parameters[1];
                        object _FrugalObjectList = parameters[2];
                        if (_FrugalObjectList != null && _FrugalObjectList.GetType().FullName.StartsWith("MS.Utility.FrugalObjectList"))
                        {
                            if (_FrugalObjectListCountField == null)
                            {
                                _FrugalObjectListCountField = _FrugalObjectList.GetType().GetProperty("Count", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                            }
                            int? _FrugalObjectListCount = (int?)_FrugalObjectListCountField?.GetValue(_FrugalObjectList);
                            if (_ItemMethod == null)
                            {
                                _ItemMethod = _FrugalObjectList.GetType().GetProperty("Item", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            }
                            for (int j = 0; j < (_FrugalObjectListCount ?? 0); j++)
                            {
                                object? eventHandler = _ItemMethod.GetValue(_FrugalObjectList, new object[] { j });
                                string strFunc = (eventHandler as RoutedEventHandlerInfo?)?.Handler?.Method.ToString();
                                int nPos = strFunc.IndexOf(' ');
                                //if (nPropDumped != 0)
                                //    sb.Append(", ");
                                //sb.Append("[" + strFunc.Substring(0, nPos + 1) + (eventHandler as RoutedEventHandlerInfo?)?.Handler?.Method?.Module?.Name + "!" + strFunc.Substring(nPos + 1) + "]");
                                //nPropDumped++;
                                ////sb.Append("[" + (eventHandler as RoutedEventHandlerInfo?)?.Handler?.Method?.Module?.Name + "!" + (eventHandler as RoutedEventHandlerInfo?)?.Handler?.Method?.Name +
                                ////    "(" + ((eventHandler as RoutedEventHandlerInfo?)?.Handler?.Target as FrameworkElement)?.Name + "<" + (eventHandler as RoutedEventHandlerInfo?)?.Handler?.Target?.ToString() + ">, EventParamter)]");
                                if((nDumpLevel & (int)DUMP_LEVEL.DUMP_EVENTHANDLE) == (int)DUMP_LEVEL.DUMP_EVENTHANDLE)
                                    Debug.WriteLine("".PadRight(nDepth << 2) + "<- " + "[" + strFunc.Substring(0, nPos + 1) + (eventHandler as RoutedEventHandlerInfo?)?.Handler?.Method?.Module?.Name + "!" + strFunc.Substring(nPos + 1) + "]");
                            }
                        }else if (_FrugalObjectList != null && _FrugalObjectList.GetType().FullName == "System.Windows.DependencyPropertyChangedEventHandler")
                        {
                            string strFunc = ((System.Delegate)_FrugalObjectList).Method.ToString();
                            int nPos = strFunc.IndexOf(' ');
                            //if (nPropDumped != 0)
                            //    sb.Append(", ");
                            //sb.Append("[" + strFunc.Substring(0, nPos + 1) + ((System.Delegate)_FrugalObjectList).Method.Module.Name + "!" + strFunc.Substring(nPos + 1) + "]"); 
                            //nPropDumped++;
                            if ((nDumpLevel & (int)DUMP_LEVEL.DUMP_EVENTHANDLE) == (int)DUMP_LEVEL.DUMP_EVENTHANDLE) 
                                Debug.WriteLine("".PadRight(nDepth << 2) + "<- " + "[" + strFunc.Substring(0, nPos + 1) + ((System.Delegate)_FrugalObjectList).Method.Module.Name + "!" + strFunc.Substring(nPos + 1) + "]");
                        }
                        else if (_FrugalObjectList != null && _FrugalObjectList.GetType().FullName == "System.EventHandler")
                        {
                            string strFunc = ((System.EventHandler)_FrugalObjectList).Method.ToString();
                            int nPos = strFunc.IndexOf(' ');
                            //if (nPropDumped != 0)
                            //    sb.Append(", ");
                            //sb.Append("[" + strFunc.Substring(0, nPos + 1) + ((System.Delegate)_FrugalObjectList).Method.Module.Name + "!" + strFunc.Substring(nPos + 1) + "]");
                            //nPropDumped++;
                            if ((nDumpLevel & (int)DUMP_LEVEL.DUMP_EVENTHANDLE) == (int)DUMP_LEVEL.DUMP_EVENTHANDLE) 
                                Debug.WriteLine("".PadRight(nDepth << 2) + "<- " + "[" + strFunc.Substring(0, nPos + 1) + ((System.Delegate)_FrugalObjectList).Method.Module.Name + "!" + strFunc.Substring(nPos + 1) + "]");
                        }
                        else
                        {
                            Debug.Assert(false);
                        }
                    }
                }
            }

#endif

#if false
            foreach (EventInfo evt in type.GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                // 获取 AddAccessor
                MethodInfo addAccessor = evt.GetAddMethod();
                //FieldInfo fieldInfo = type.GetField(evt.Name+"Event", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                //PropertyInfo propInfo = type.GetProperty(evt.Name + "Event", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                FieldInfo fieldInfo = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Where(f => f.Name == "SelectedColorChangedEvent").FirstOrDefault();
                //PropertyInfo[] propInfos = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).Where(f => f.Name.Contains("SelectedColorChanged")).ToArray();

                // 调用 AddAccessor 获取委托列表
                //Delegate[] delegates = (Delegate[])addAccessor.Invoke(obj, new object[]{ null});

                Delegate? eventDelegate = fieldInfo.GetValue(obj) as Delegate;

                if (eventDelegate != null)
                {
                    // 遍历委托的调用列表
                    foreach (Delegate handler in eventDelegate.GetInvocationList())
                    {
                        Console.WriteLine($"Method: {handler.Method.Name}, Target: {handler.Target}");
                    }
                }
                else
                {
                    Console.WriteLine("No delegates attached to this event.");
                }
            }
#endif

#if false
            foreach (EventInfo evt in type.GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                Debug.WriteLine($"Event: {evt.Name}");

                // 获取事件的底层字段（通常以 "EventName" 命名）
                FieldInfo fieldInfo = type.GetField(evt.Name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                FieldInfo fieldInfo2 = type.GetField(evt.EventHandlerType.Name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                FieldInfo fieldInfo3 = type.GetField(evt.EventHandlerType.FullName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                FieldInfo fieldInfo4 = type.GetField(evt.EventHandlerType.AssemblyQualifiedName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                MethodInfo methodInfo = type.GetMethod(evt.Name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                MethodInfo methodInfo2 = type.GetMethod(evt.EventHandlerType.Name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                MethodInfo methodInfo3 = type.GetMethod(evt.Name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                MethodInfo methodInfo4 = type.GetMethod(evt.Name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                //MethodInfo methodInfo1 = type.getconst(evt.Name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                if (fieldInfo != null)
                {
                    // 获取事件的委托实例
                    Delegate eventDelegate = fieldInfo.GetValue(obj) as Delegate;

                    if (eventDelegate != null)
                    {
                        foreach (Delegate handler in eventDelegate.GetInvocationList())
                        {
                            // 获取处理程序方法的声明类和名称
                            MethodInfo method = handler.Method;

                            if (nPropDumped == 0)
                                sb.Append("[" + evt.Name + "=" + (handler.Target + "!" + method.Module.Name + "." + method.Name) + "]");
                            else
                                sb.Append(",").Append("[" + evt.Name + "=" + (handler.Target + "!" + method.Module.Name + "." + method.Name) + "]");
                            nPropDumped++;

                            //Console.WriteLine($"Handler Method: {method.Name}");
                            //Console.WriteLine($"Module: {method.Module.Name}");
                            //Console.WriteLine($"Target: {handler.Target}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No handlers attached.");
                    }
                }
            }
#endif
            //return sb.ToString();
            return "";
        }
        static PropertyInfo? _FrugalMapCountField = null;// _entries.GetType().GetProperty("Count", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        static MethodInfo? GetKeyValuePairMethod = null;
        static PropertyInfo? _FrugalObjectListCountField = null;
        static PropertyInfo? _ItemMethod = null;// _entries.GetType().PropertyInfo("GetKeyValuePair", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        static PropertyInfo? EventHandlersStoreType = null;
        static Type? storeType = null;
        static FieldInfo? _entriesField = null;

        //static MethodInfo? GetEventHandlersMethod = null;


    }
}
