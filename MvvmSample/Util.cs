using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NV
{
    public class RelayCommand : ICommand
    {
        private readonly Func<Task> _asyncExecute;
        private readonly Action _syncExecute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Func<Task> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Func<Task> asyncExecute, Func<bool> canExecute)
        {
            _asyncExecute = asyncExecute ?? throw new ArgumentNullException(nameof(asyncExecute));
            _canExecute = canExecute;
        }

        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action syncExecute, Func<bool> canExecute)
        {
            _syncExecute = syncExecute ?? throw new ArgumentNullException(nameof(syncExecute));
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke() ?? true;
        }

        public async void Execute(object parameter)
        {
            if (_asyncExecute != null)
            {
                await _asyncExecute();
            }
            else if (_syncExecute != null)
            {
                _syncExecute();
            }
        }
    }

    public class TickHelper
    {
        // 整合了TickUp和TickDn的功能
        public static decimal Tick(decimal price, bool isUp)
        {
            if (price >= 1000 || (!isUp && price > 1000))
                return isUp ? 5 : -5;
            else if (price >= 500 || (!isUp && price > 500))
                return isUp ? 1 : -1;
            else if (price >= 100 || (!isUp && price > 100))
                return isUp ? 0.5m : -0.5m;
            else if (price >= 50 || (!isUp && price > 50))
                return isUp ? 0.1m : -0.1m;
            else if (price >= 10 || (!isUp && price > 10))
                return isUp ? 0.05m : -0.05m;
            else
                return isUp ? 0.01m : -0.01m;
        }

        // 判斷了tick的值，避免了整數溢出
        public static decimal Tick(decimal price, int tick, bool isUp)
        {
            decimal new_price = price;
            if (tick > 0)
            {
                for (int i = 0; i < tick; i++)
                    new_price += Tick(new_price, isUp);
            }
            return new_price;
        }

        public static decimal AdjustPriceToTick(decimal price)
        {
            // 獲取較大的TickSize
            decimal tickSize = Tick(price, true);

            // 將價格調整到最接近的有效單位
            decimal adjustedPrice = Math.Ceiling(price / tickSize) * tickSize;

            return adjustedPrice;
        }
    }
}
