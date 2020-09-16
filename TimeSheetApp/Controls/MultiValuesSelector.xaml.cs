using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LebedevControls
{
    public static class MyExtensions
    {
        public static void Press(this Button btn)
        {
            btn.Background = new SolidColorBrush(Color.FromRgb(46, 204, 113));
            btn.BorderBrush = new SolidColorBrush(Color.FromRgb(39, 174, 96));
        }

        public static void Unpress(this Button btn)
        {
            btn.Background = new SolidColorBrush(Color.FromRgb(149, 165, 166));
            btn.BorderBrush = new SolidColorBrush(Color.FromRgb(127, 140, 141));
        }
    }

    /// <summary>
    /// Interaction logic for MultiValuesSelector.xaml
    /// </summary>
    public partial class MultiValuesSelector : UserControl
    {
        #region DependencyProperties
        private static readonly DependencyProperty DisplayTipMemberProperty =
            DependencyProperty.Register(nameof(DisplayTipMember), typeof(string), typeof(MultiValuesSelector));

        private static readonly DependencyProperty DisplayMemberProperty =
    DependencyProperty.Register(nameof(DisplayMember), typeof(string), typeof(MultiValuesSelector));

        private static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                name: nameof(ItemsSource),
                propertyType: typeof(IEnumerable),
                ownerType: typeof(MultiValuesSelector),
                typeMetadata: new PropertyMetadata(defaultValue: null, propertyChangedCallback: new PropertyChangedCallback(OnItemsSourceChanged)));

        private static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(
                name: nameof(SelectedItems),
                propertyType: typeof(IList),
                ownerType: typeof(MultiValuesSelector),
                typeMetadata: new PropertyMetadata(defaultValue: new ObservableCollection<object>(), propertyChangedCallback: new PropertyChangedCallback(OnSelectedItemsChanged)));

        private static readonly DependencyProperty LastSelectedItemProperty =
            DependencyProperty.Register(nameof(LastSelectedItem), typeof(object), typeof(MultiValuesSelector), new PropertyMetadata(null, new PropertyChangedCallback(OnSelectedItemChanged)));

        public IList SelectedItems
        {
            get => (IList)GetValue(SelectedItemsProperty);
            set => SetValue(SelectedItemsProperty, value);
        }
        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        public object LastSelectedItem
        {
            get => GetValue(LastSelectedItemProperty);
            set => SetValue(LastSelectedItemProperty, value);
        }
        #endregion

        #region DependencyChangedEventsHandling
        private static void OnSelectedItemsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs propertyArgs)
        {
            MultiValuesSelector control = (MultiValuesSelector)obj;
            if (propertyArgs.OldValue != null)
            {
                if(propertyArgs.OldValue is INotifyCollectionChanged)
                {
                    ((INotifyCollectionChanged)propertyArgs.OldValue).CollectionChanged -= control.OnSelectedItemsChanged;
                }
            }
            if (propertyArgs.NewValue != null)
            {
                if (propertyArgs.NewValue is INotifyCollectionChanged)
                {
                    ((INotifyCollectionChanged)propertyArgs.NewValue).CollectionChanged += control.OnSelectedItemsChanged;
                }
                control.OnSelectedItemsChanged();
            }
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MultiValuesSelector currentControl = (MultiValuesSelector)d;

            if (currentControl.SelectedItems == null)
            {
                currentControl.SelectItem(e.NewValue);
            }
            if (e.NewValue != null && currentControl.SelectedItems != null)
            {
                if (currentControl.SelectedItems.Count != 0)
                {
                    currentControl.UnselectAll();
                }
                if (!currentControl.SelectedItems.Contains(e.NewValue))
                {
                    currentControl.SelectedItems.Add(e.NewValue);
                    currentControl.SelectItem(e.NewValue);
                }
            }

        }

        private void OnSelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnSelectedItemsChanged();
        }

        private void OnSelectedItemsChanged()
        {
            if (BtnObjPair.Count < 1) return;
            foreach (object obj in ItemsSource)
            {
                Button btn = BtnObjPair.FirstOrDefault(i => i.Value == obj).Key;
                if (SelectedItems.Contains(obj))
                {
                    btn?.Press();
                }
                else
                {
                    btn?.Unpress();
                }
            }
            if (ItemsCount == SelectedItems.Count)
            {
                SelectAllButton.Press();
            }
            else
            {
                SelectAllButton.Unpress();
            }
        }

        private static void OnItemsSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs propertyArgs)
        {
            MultiValuesSelector control = (MultiValuesSelector)obj;
            if (propertyArgs.NewValue != null)
            {
                control.Cleanup();
                control.GenerateButtons();
            }
        }
        #endregion

        #region public Values
        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public string DisplayMember
        {
            get => (string)GetValue(DisplayMemberProperty);
            set => SetValue(DisplayMemberProperty, value);
        }
        /// <summary>
        /// Отображаемые всплывающие подсказки
        /// </summary>
        public string DisplayTipMember
        {
            get => (string)GetValue(DisplayTipMemberProperty);
            set => SetValue(DisplayTipMemberProperty, value);
        }
        public int Rows { get; set; } = 1;
        /// <summary>
        /// Добавить кнопку выделить всё
        /// </summary>
        public bool AllSelectButton { get; set; }
        Dictionary<Button, object> BtnObjPair = new Dictionary<Button, object>();
        public bool AllowMultiSelect { get; set; }
        #endregion

        #region private controls
        private Button SelectAllButton { get; set; }
        private int ItemsCount
        {
            get
            {
                int count = 0;
                foreach (object item in ItemsSource)
                {
                    count++;
                }
                return count;
            }
        }
        #endregion

        private void Cleanup()
        {
            BtnObjPair.Clear();
            SelectedItems?.Clear();
            while (MainGrid.Children.Count != 0)
            {
                MainGrid.Children.RemoveAt(0);
            }
        }

        private void GenerateButtons()
        {
            double gridActualWidth = MainGrid.ActualWidth;
            double currentRightBorder = gridActualWidth;
            double blockWidth = currentRightBorder * Rows / (ItemsCount + (AllSelectButton ? 1 : 0));
            double blockWidthSum = 0;
            int currentColumn = 0;
            int currentRow = 0;
            if(Rows > 1)
            {
                for (int i = 0; i < Rows; i++)
                {
                    MainGrid.RowDefinitions.Add(new RowDefinition());
                }
            }

            if (AllSelectButton)
            {
                SelectAllButton = new Button
                {
                    Content = new TextBlock
                    {
                        Text="Выбрать всё", TextWrapping = TextWrapping.Wrap, TextAlignment = TextAlignment.Center
                    },
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Background = new SolidColorBrush(Color.FromRgb(149, 165, 166)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(127, 140, 141)),
                    BorderThickness = new Thickness(1),
                    Style = null
                };
                blockWidthSum += blockWidth;
                SelectAllButton.Click += SelectAllBtnClick;
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
                Grid.SetColumn(SelectAllButton, currentColumn++);
                MainGrid.Children.Add(SelectAllButton);
            }

            foreach (object obj in ItemsSource)
            {
                Button newButton = CreateBtn(obj);
                newButton.Click += OnButtonClick;
                blockWidthSum += blockWidth;
                if (blockWidthSum >= currentRightBorder && Rows>1)
                {
                    currentRow++;
                    currentColumn = 0;
                    currentRightBorder += gridActualWidth;
                }
                if (currentRow == 0) MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
                Grid.SetRow(newButton, currentRow);
                Grid.SetColumn(newButton, currentColumn++);
                BtnObjPair.Add(newButton, obj);
                MainGrid.Children.Add(newButton);
            }
        }

        private Button CreateBtn(object obj)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = obj.ToString();
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.TextAlignment = TextAlignment.Center;
            Button newButton = new Button
            {
                Content = textBlock,
                Background = new SolidColorBrush(Color.FromRgb(149, 165, 166)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(127, 140, 141)),
                BorderThickness = new Thickness(1),
                Style = null
            };
            if (!string.IsNullOrWhiteSpace(DisplayMember))
            {
                PropertyInfo propertyInfo = obj.GetType().GetProperty(DisplayMember);
                if (propertyInfo != null)
                {
                    textBlock.Text = propertyInfo.GetValue(obj)?.ToString();
                }
                else
                {
                    throw new Exception($"Cant find {DisplayMember} property on {obj.GetType()}");
                }
            }
            if (!string.IsNullOrWhiteSpace(DisplayTipMember))
            {
                PropertyInfo propertyInfo = obj.GetType().GetProperty(DisplayTipMember);
                if (propertyInfo != null)
                {
                    textBlock.ToolTip = propertyInfo.GetValue(obj)?.ToString();
                }
                else
                {
                    throw new Exception($"Cant find {DisplayMember} property on {obj.GetType()}");
                }
            }
            if(LastSelectedItem == obj || SelectedItems.Contains(obj))
            {
                newButton.Press();
            }
            return newButton;
        }

        private void SelectAllBtnClick(object sender, RoutedEventArgs e)
        {
            if (!AllowMultiSelect)
            {
                return;
            }
            if (ItemsCount == SelectedItems.Count)
            {
                UnselectAll();
            }
            else
            {
                SelectAll();
            }
        }

        private void SelectAll()
        {
            foreach (object obj in ItemsSource)
            {
                SelectItem(obj);
            }
        }

        private void UnselectAll()
        {
            if(ItemsSource != null)
            {
                foreach (object obj in ItemsSource)
                {
                    UnselectItem(obj);
                }
            }
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            Button selectedBtn = sender as Button;
            object selectedObject = BtnObjPair[selectedBtn];
            if (AllowMultiSelect)
            {
                if (!SelectedItems.Contains(selectedObject))
                {
                    SelectItem(selectedObject);
                }
                else
                {
                    UnselectItem(selectedObject);
                }
            }
            else
            {
                UnselectAll();
                SelectItem(selectedObject);
            }
        }

        private void SelectItem(object obj)
        {
            Button btn = BtnObjPair.FirstOrDefault(i => i.Value.ToString().Equals(obj.ToString())).Key;
            if (btn != null)
            {
                btn.Press();
            }
            bool contains = false;
            for (int i = 0; i < SelectedItems.Count; i++)
            {
                if (SelectedItems[i].ToString().Equals(obj.ToString()))
                {
                    contains = true;
                }
            }
            if (!contains && btn!=null)
            {
                SelectedItems.Add(obj);
            }
        }

        private void UnselectItem(object obj)
        {
            Button btn = BtnObjPair.FirstOrDefault(i => i.Value == obj).Key;
            for (int i = 0; i < SelectedItems.Count; i++)
            {
                if (SelectedItems[i].ToString().Equals(obj.ToString()))
                {
                    btn?.Unpress();
                    SelectedItems.Remove(SelectedItems[i]);
                }
            }
        }

        public MultiValuesSelector()
        {
            InitializeComponent();
        }
    }
}
