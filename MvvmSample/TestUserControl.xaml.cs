using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;

namespace NV
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }
    }
    public class TestViewModel
    {
        public int MinInterval { get; set; }
        public int MinVolume { get; set; }

        public List<TestClass> TestClassList { get; set; }
        public ObservableCollection<TestClass> TestClassObservableCollection { get; set; }
        public List<TestClassWithNotifyPropertyChanged> TestClassListWithNotifyPropertyChanged { get; set; }
        public ObservableCollection<TestClassWithNotifyPropertyChanged> TestClassObservableCollectionWithNotifyPropertyChanged { get; set; }

        public DateTime SelectedDate { get; set; }
        public ICommand AddCommand { get; }
        public ICommand PropertyChangeCommand { get; }

        public TestViewModel()
        {
            AddCommand = new RelayCommand(OnAddClick);
            PropertyChangeCommand = new RelayCommand(OnPropertyChangeClick);

            MinInterval = 5;
            MinVolume = 30;
            SelectedDate = DateTime.Today;

            TestClassList = new List<TestClass>()
            {
                new TestClass() { Id = 1, Name = "William" },
                new TestClass() { Id = 2, Name = "John" },
                new TestClass() { Id = 3, Name = "Mary" }
            };

            TestClassObservableCollection = new ObservableCollection<TestClass>()
            {
                new TestClass() { Id = 1, Name = "William" },
                new TestClass() { Id = 2, Name = "John" },
                new TestClass() { Id = 3, Name = "Mary" }
            };

            TestClassListWithNotifyPropertyChanged = new List<TestClassWithNotifyPropertyChanged>()
            {
                new TestClassWithNotifyPropertyChanged() { Id = 1, Name = "William" },
                new TestClassWithNotifyPropertyChanged() { Id = 2, Name = "John" },
                new TestClassWithNotifyPropertyChanged() { Id = 3, Name = "Mary" }
            };

            TestClassObservableCollectionWithNotifyPropertyChanged = new ObservableCollection<TestClassWithNotifyPropertyChanged>()
            {
                new TestClassWithNotifyPropertyChanged() { Id = 1, Name = "William" },
                new TestClassWithNotifyPropertyChanged() { Id = 2, Name = "John" },
                new TestClassWithNotifyPropertyChanged() { Id = 3, Name = "Mary" }
            };

        }

        private void OnPropertyChangeClick()
        {
            TestClassList[0].Name = "Changed";
            TestClassObservableCollection[0].Name = "Changed";
            TestClassListWithNotifyPropertyChanged[0].Name = "Changed";
            TestClassObservableCollectionWithNotifyPropertyChanged[0].Name = "Changed";
        }

        private void OnAddClick()
        {
            var testClass = new TestClass() { Id = 4, Name = "TestAdded" };
            TestClassList.Add(testClass);
            TestClassObservableCollection.Add(testClass);

            var testClassWithNotifyPropertyChanged = new TestClassWithNotifyPropertyChanged() { Id = 4, Name = "TestAdded" };
            TestClassListWithNotifyPropertyChanged.Add(testClassWithNotifyPropertyChanged);
            TestClassObservableCollectionWithNotifyPropertyChanged.Add(testClassWithNotifyPropertyChanged);
        }
    }

    public class TestClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestClassWithNotifyPropertyChanged : INotifyPropertyChanged
    {
        private int _id;
        private string _name;

        public int Id
        {
            get => _id;
            set
            {
                if (value == _id) return;
                _id = value;
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
