using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace TimeSheetApp.Controls
{
    /// <summary>
    /// Interaction logic for HintTextBox.xaml
    /// </summary>
    public partial class HintTextBox : UserControl, INotifyPropertyChanged
    {
        public delegate void TextHandler();
        public TextBox textField;
        public event TextHandler TextChangedEvent;
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable<string>), typeof(HintTextBox));
        
        public bool IgnoreTextChange
        {
            get { return (bool)GetValue(IgnoreTextChangeProperty); }
            set { SetValue(IgnoreTextChangeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IgnoreTextChange.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IgnoreTextChangeProperty =
            DependencyProperty.Register("IgnoreTextChange", typeof(bool), typeof(HintTextBox), new PropertyMetadata(true));


        private delegate void closeDropDownDelegate();
        Timer CloseTimer;
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(HintTextBox), new UIPropertyMetadata(string.Empty, new PropertyChangedCallback(TextChanged)), new ValidateValueCallback(TextValidate));

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HintTextBox control = (HintTextBox)d;
            if (e.NewValue != null)
                control.UserInputTextBox.Text = e.NewValue.ToString();
        }

        private static bool TextValidate(object value)
        {
            return true;
        }

        public IEnumerable<string> ItemsSource
        {
            get { return (IEnumerable<string>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        private readonly ObservableCollection<string> suggestList = new ObservableCollection<string>();
        private bool userUpdate = true;
        private bool expanded = false;


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public HintTextBox()
        {
            InitializeComponent();
            textField = UserInputTextBox;
            UserInputTextBox.TextChanged += TextInputChanged;
            UserInputTextBox.PreviewKeyDown += KeyDownMethod;
            InputCacheComboBox.PreviewKeyDown += KeyDownComboBoxMethod;
            InputCacheComboBox.SelectionChanged += SelectionChangedMethod;
            UserInputTextBox.MouseEnter += UserInputTextBox_GotFocus;
            UserInputTextBox.MouseLeave += ComboboxMouseLeave;
            InputCacheComboBox.MouseLeave += ComboboxMouseLeave;
            InputCacheComboBox.DropDownClosed += InputCacheDropDownClosed;
            CloseTimer = new Timer(CloseDropDown, null, Timeout.Infinite, Timeout.Infinite);
            InputCacheComboBox.ItemsSource = suggestList;
        }


        private void ComboboxMouseLeave(object sender, MouseEventArgs e)
        {
            CloseTimer.Change(5000, Timeout.Infinite);
        }

        private void InputCacheDropDownClosed(object sender, EventArgs e)
        {
            expanded = false;
        }
        private void CloseDropDown(object obj)
        {
            Application.Current.Dispatcher.Invoke(()=>InputCacheComboBox.IsDropDownOpen = false);
            CloseTimer.Change(Timeout.Infinite, Timeout.Infinite);
            userUpdate = false;
        }

        private void UserInputTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            userUpdate = true;

            if (!expanded && string.IsNullOrWhiteSpace(UserInputTextBox.Text))
            {
                FilterItems(UserInputTextBox.Text);
                if (suggestList.Count > 0)
                {
                    InputCacheComboBox.IsDropDownOpen = true;
                    UserInputTextBox.Focus();
                    expanded = true;
                }

            }
        }

        private void SelectionChangedMethod(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedItem != null)
            {
                userUpdate = false;
                UserInputTextBox.Text = (sender as ComboBox).SelectedItem.ToString();
                UserInputTextBox.CaretIndex = UserInputTextBox.Text.Length;
                UserInputTextBox.Focus();
                InputCacheComboBox.IsDropDownOpen = false;
                userUpdate = true;
            }
        }

        private void KeyDownMethod(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                FilterItems(string.Empty);
                InputCacheComboBox.IsDropDownOpen = true;
                InputCacheComboBox.Focus();
            }
            else if (e.Key == Key.Escape)
            {
                UserInputTextBox.Focus();
                InputCacheComboBox.IsDropDownOpen = false;
            }
        }

        private void KeyDownComboBoxMethod(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                UserInputTextBox.Focus();
                InputCacheComboBox.IsDropDownOpen = false;
            }
            else if (e.Key != Key.Up && e.Key != Key.Down && e.Key != Key.Enter)
            {
                UserInputTextBox.Focus();
            }
        }
        private void TextInputChanged(object sender, TextChangedEventArgs e)
        {

            if (!IgnoreTextChange && userUpdate)
            {
                suggestList.Clear();
                FilterItems(UserInputTextBox.Text);
                if (suggestList.Count > 0)
                {
                    InputCacheComboBox.IsDropDownOpen = true;
                }
                else
                {
                    InputCacheComboBox.IsDropDownOpen = false;
                }
            }
            Text = UserInputTextBox.Text;
            TextChangedEvent?.Invoke();

        }
        private void FilterItems(string filterStr)
        {
            suggestList.Clear();
            if (ItemsSource != null)
            {
                foreach (string cacheStr in ItemsSource)
                {
                    if (cacheStr.ToLower().IndexOf(filterStr.ToLower()) > -1)
                    {
                        suggestList.Add(cacheStr);
                    }
                }
            }
        }
    }
}