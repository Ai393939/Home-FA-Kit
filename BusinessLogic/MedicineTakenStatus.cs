using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BusinessLayer
{
    public class MedicineTakenStatus : INotifyPropertyChanged
    {
        private bool _isTaken;

        public Medicine Medicine { get; set; }
        public TimeOnly Time { get; set; }

        public bool IsTaken
        {
            get { return _isTaken; }
            set
            {
                if (_isTaken != value)
                {
                    _isTaken = value;
                    OnPropertyChanged();
                }
            }
        }

        public MedicineTakenStatus(Medicine medicine, TimeOnly time)
        {
            Medicine = medicine;
            Time = time;
            IsTaken = false;
        }

        public void MarkAsTaken()
        {
            IsTaken = !IsTaken;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}