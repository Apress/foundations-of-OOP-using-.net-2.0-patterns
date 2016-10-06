using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Chap07.WinConsumerProducer {
    public partial class Form1: Form {
        public Form1() {
            InitializeComponent();
        }

        private int _counter;
        private void IncrementCounter() {
            txtMessage.Text = "Counter (" + _counter + ")";
            _counter++;
        }

        delegate void DelegateIncrementCounter();

        private void PeriodicIncrement() {
            while( 1 == 1) {
                Invoke(new DelegateIncrementCounter(IncrementCounter));
                Thread.Sleep(1000);
            }
        }
        Thread _thread;

        private void Form1_Load(object sender, EventArgs e) {
            _thread = new Thread(new ThreadStart(PeriodicIncrement));
            _thread.Start();
        }
    }
}

