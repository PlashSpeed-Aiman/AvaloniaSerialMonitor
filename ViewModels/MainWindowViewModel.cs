using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Ports;
using Avalonia.Threading;
using Avalonia1.Models;
using ReactiveUI;


namespace Avalonia1.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {

        private char[] charsToTrim = { '\n' };
        private string[] _portList;
        private SerialPort? _serialPort;

        public MainWindowViewModel()
        {
            _portList = SerialPort.GetPortNames();
            MonitorText = String.Empty;
            DataList = new ObservableCollection<SensorData>();
        }

        public ObservableCollection<SensorData> DataList
        {
            get;
            set;
        }
        public string Greeting => "Arduino Serial Monitor";
        public string[] PortList => _portList;
        public string MonitorText { get; private set; }
        public void ResetPorts()
        {
            if (_serialPort != null)
            {
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();
                _serialPort.DataReceived -= DataReceivedHandler;
                _serialPort.Close();
            }


        }
        public void LogData(string text)
        {
            LoggerAsync.SaveToTextFileAsync(text);
        }
        public void UpdatePorts()
        {
            _portList = SerialPort.GetPortNames();
            this.RaisePropertyChanged("PortList");

        }

        public void StartListening(string comPort)
        {
            if (_serialPort != null)
            {

                _serialPort.DataReceived -= DataReceivedHandler;
                _serialPort.Close();
                _serialPort = new SerialPort(comPort);
                _serialPort.Open();
                Trace.WriteLine("Old Connection");
            }
            else if (_serialPort == null)
            {
                _serialPort = new SerialPort(comPort);
                _serialPort.NewLine = "\r\n";
                _serialPort.Open();
                Trace.WriteLine("New Connection");
            }

            DispatcherTimer dt = new DispatcherTimer();
          
            _serialPort.DataReceived += DataReceivedHandler;
            dt.Tick += new EventHandler(saveData);
            dt.Interval = new TimeSpan(0, 30, 0);
            dt.Start();
        }

        private void saveData(object? sender, EventArgs e)
        {
            LoggerAsync.SaveToTextFileAsync(MonitorText);
        }

        public void ClearText()
        {
            MonitorText = String.Empty;
            this.RaisePropertyChanged("MonitorText");
        }
        //Violates MVVM (maybe?) and Single Responsibility Principle
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            var msg = String.Empty;
            var s = sender as SerialPort;
            try
            {
                msg = s.ReadLine();
            }
            catch (OperationCanceledException ex)
            {
                msg = String.Empty;
            }
            string[] snsr = msg.Split(" ");
            
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                MonitorText += msg + $" {DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}\n";
                this.RaisePropertyChanged("MonitorText");
            });
        }
    }
}
